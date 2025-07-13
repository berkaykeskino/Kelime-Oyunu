using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private List<IObserveGameManager> _observers = new List<IObserveGameManager>();
    private LevelData _levelData;
    private AnswerChecker _answerChecker;
    private int _questionID = 1;
    private int _questionCount = 0;
    private int _jokerCount = 0;

    [SerializeField] private Transform _canvasTransform;
    [SerializeField] private GameObject _timerPrefab;
    [SerializeField] private GameObject _questionBGPrefab;
    [SerializeField] private GameObject _questionTextPrefab;
    private QuestionText _questionText;
    [SerializeField] private GameObject _keyboardPrefab;
    private Keyboard _keyboard;
    [SerializeField] private GameObject _answerGridPrefab;
    private AnswerGrid _answerGrid;
    [SerializeField] private GameObject _pointPrefab;
    private Point _point;
    [SerializeField] private GameObject _jokerPrefab;
    private Joker _joker;

    void Awake()
    {
        _instance = this;

        LevelManager levelManager = new LevelManager();
        levelManager.CreateLevel();
        _levelData = levelManager.GetLevelData();

        _answerChecker = new AnswerChecker();
        _answerChecker.AddSelfToGameManagerObservers();

        Timer timer = Instantiate(_timerPrefab, _canvasTransform).GetComponent<Timer>();
        timer.InitializeTimer();

        Transform questionBGTransform = Instantiate(_questionBGPrefab, _canvasTransform).GetComponent<Transform>();
        _questionText = Instantiate(_questionTextPrefab, questionBGTransform).GetComponent<QuestionText>();
        _keyboard = Instantiate(_keyboardPrefab, _canvasTransform).GetComponent<Keyboard>();
        _answerGrid = Instantiate(_answerGridPrefab, _canvasTransform).GetComponent<AnswerGrid>();
        _point = Instantiate(_pointPrefab, _canvasTransform).GetComponent<Point>();
        _joker = Instantiate(_jokerPrefab, _canvasTransform).GetComponent<Joker>();
    }
    void Start()
    {
        NotifyObservers(_levelData);
        CreateKeyboardButtons();
        SetQuestionCount();

        SetQuestionText();
        CreateAnswerBoxes();
    }

    public static GameManager GetInstance(){
        return _instance;
    }

    public void AddObserver(IObserveGameManager observer){
        _observers.Add(observer);
    }


    private void PrintLevelData(){
        Debug.Log(_levelData.LevelNumber);
        Debug.Log(_levelData.LevelTime);
        Debug.Log(_levelData.LevelLetters);
        foreach (LevelQuestion levelQuestion in _levelData.LevelQuestions){
            Debug.Log(levelQuestion.key+" : "+  levelQuestion.value);
        }
    }

    private void SetQuestionCount(){
        _questionCount = _levelData.LevelQuestions.Count;
    }


    private void NotifyObservers(LevelData levelData){
        foreach (IObserveGameManager observer in _observers){
            observer.ReceiveLevelData(levelData);
        }
    }

    private void SetQuestionText(){
        _questionText.SetQuestionText(_questionID);
    }

    private void CreateKeyboardButtons()
    {
        _keyboard.CreateKeyboardButtons(_questionID);
    }

    private void CreateAnswerBoxes()
    {
        _answerGrid.CreateAnswerBoxes(_questionID);
    }

    public void OnKeyboardButtonEnter(string buffer)
    {
        Debug.Log(buffer+" is buffered");
        _answerGrid.SetAnswerBoxes(buffer);
    }

    public void OnJokerPressed(){
        string currentAnswer = _answerChecker.GetAnswer(_questionID);
        _answerGrid.OnJokerPressed(currentAnswer);
        _jokerCount++;
    }

    public void OnTimeOut(){
        SaveScore();
        LoadMainScene();
    }

    public void OnKeyboardExit(string buffer){
        if (_answerChecker.CheckAnswer(buffer, _questionID)){
            _point.AddPoint((buffer.Length - math.min(_jokerCount, buffer.Length)) * 100);
            _jokerCount = 0;
            _answerGrid.DestroyAnswerBoxes();
            _questionText.ClearQuestionText();
            _questionID++;
            if (_questionID > _questionCount){//all questions answered
                //set new level
                //LevelManager levelManager = new LevelManager();
                //levelManager.LevelUp();
                //go to main screen
                SaveScore();
                LoadMainScene();
            }else{    
                _keyboard.UpdateKeyboardButtons(_questionID);
                SetQuestionText();
                CreateAnswerBoxes();
            }
        }
        else{
            _answerGrid.ClearAnswerBoxes();
        }
    }

    public void LoadMainScene(){
        SceneManager.LoadScene(0);
    }

    private void SaveScore(){
        PlayerPrefs.SetInt("bestScore", math.max(PlayerPrefs.GetInt("bestScore"), _point.GetCurrentPoint()));
        PlayerPrefs.SetInt("lastScore", _point.GetCurrentPoint());
        PlayerPrefs.Save();
    }

    


}


