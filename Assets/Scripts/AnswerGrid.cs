using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AnswerGrid : MonoBehaviour, IObserveGameManager {

    [SerializeField] private GameObject _answerBoxPrefab;
    private List<AnswerBox> _answerBoxes = new List<AnswerBox>();
    private List<int> _answerLengths = new List<int>();
    private float _answerGridSize;

    private void Awake()
    {
        _answerGridSize = gameObject.GetComponent<RectTransform>().rect.width;
        AddSelfToGameManagerObservers();
    }

    public void AddSelfToGameManagerObservers(){
        GameManager.GetInstance().AddObserver(this);
    }

    public void ReceiveLevelData(LevelData levelData)
    {
        foreach (LevelQuestion question in levelData.LevelQuestions)
        {
            _answerLengths.Add(question.value.Length);
        }
    }

    public void CreateAnswerBoxes(int questionID){
        for (int i = 0; i < _answerLengths[questionID - 1]; i++){
            AnswerBox answerBox = Instantiate(_answerBoxPrefab, gameObject.transform).GetComponent<AnswerBox>();
            answerBox.Initialize(' ', i, _answerGridSize);
            _answerBoxes.Add(answerBox);
        }
    }

    public void DestroyAnswerBoxes(){
        float destroyDelay = 0.2f;
        for(int i = 0; i < _answerBoxes.Count; i++){
            _answerBoxes[i].SelfDestroy(_answerGridSize, destroyDelay);
        }
        _answerBoxes = new List<AnswerBox>();
    }
    

    public void SetAnswerBoxes(string buffer){
        for(int i = 0; i < math.min(buffer.Length, _answerBoxes.Count); i++){
            _answerBoxes[i].SetLetter(buffer[i]);
        }
    }

    public void ClearAnswerBoxes(){
        for (int i = 0; i < _answerBoxes.Count; i++)
        {
            _answerBoxes[i].SetLetter(' ');
        }
    }

    public void OnJokerPressed(string currentAnswer){
        List<int> emptyIndexes = GetJokerIndexes();
        int showIndex = emptyIndexes[UnityEngine.Random.Range(0, emptyIndexes.Count)];
        _answerBoxes[showIndex].EnableJoker(currentAnswer[showIndex]);
        ClearAnswerBoxes();

    }

    private List<int> GetJokerIndexes(){
        List<int> emptyIndexes = new List<int>();
        Debug.Log("-----");
        for (int i = 0; i < _answerBoxes.Count; i++)
        {
            if (_answerBoxes[i].IsJokerEnabled() == false)
            {
                emptyIndexes.Add(i);
                Debug.Log(i);
            }
        }
        return emptyIndexes;
    }
}