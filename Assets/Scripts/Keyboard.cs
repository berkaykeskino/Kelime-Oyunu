using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour, IObserveGameManager, IPointerExitHandler{
    [SerializeField] private Button _keyboardButtonPrefab;
    private List<KeyboardButton> _keyboardButtons = new List<KeyboardButton>();
    private string _buffer = "";
    private float _keyboardSize;
    List<string> _answers = new List<string>();

    private void Awake()
    {
        _keyboardSize = gameObject.GetComponent<RectTransform>().rect.width;
        AddSelfToGameManagerObservers();
    }

    public void AddSelfToGameManagerObservers()
    {
        GameManager.GetInstance().AddObserver(this);
    }

    public void ReceiveLevelData(LevelData levelData)
    {
        foreach (LevelQuestion question in levelData.LevelQuestions)
        {
            _answers.Add(question.value);
        }
    }

    public void OnKeyboardButtonEnter(char _letter){
        _buffer += _letter;
        GameManager.GetInstance().OnKeyboardButtonEnter(_buffer);
    }

    public void CreateKeyboardButtons(int questionID){
        string questionLetters = GetQuestionLetters(questionID);
        for (int i = 0; i < questionLetters.Length; i++)
        {
            KeyboardButton keyboardButton = Instantiate(_keyboardButtonPrefab, gameObject.transform).GetComponent<KeyboardButton>();
            keyboardButton.Initialize(questionLetters[i], this, questionLetters.Length, i, _keyboardSize / 2);
            _keyboardButtons.Add(keyboardButton);

        }
    }

    public void UpdateKeyboardButtons(int questionID){
        string questionLetters = GetQuestionLetters(questionID);
        for (int i = 0; i < _keyboardButtons.Count; i++){
            _keyboardButtons[i].SetLetter(questionLetters[i]);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.GetInstance().OnKeyboardExit(_buffer);
        _buffer = "";
    }

    private string GetQuestionLetters(int questionID)
    {
        string alphabet = "abcçdefgğhıijklmnoöprsştuüvyz";
        string answer = _answers[questionID - 1];

        while (answer.Length < 10)
        {
            int temp = Random.Range(0, alphabet.Length);
            char randomChar = alphabet[temp];

            if (!answer.Contains(randomChar))
            {
                answer += randomChar;
            }
        }

        // Shuffle the answer
        List<char> characters = answer.ToList();
        for (int i = 0; i < characters.Count; i++)
        {
            int randIndex = Random.Range(0, characters.Count);
            (characters[i], characters[randIndex]) = (characters[randIndex], characters[i]);
        }

        return new string(characters.ToArray());
    }

}