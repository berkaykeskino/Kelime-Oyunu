

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionText: MonoBehaviour, IObserveGameManager {
    [SerializeField] private TextMeshProUGUI _questionText;
    private List<string> _questions = new List<string>();

    private void Awake() {
        AddSelfToGameManagerObservers();
    }

    public void AddSelfToGameManagerObservers(){
        GameManager.GetInstance().AddObserver(this);
    }

    public void ReceiveLevelData(LevelData levelData){
        foreach (LevelQuestion question in levelData.LevelQuestions){
            _questions.Add(question.key);
        }
    }

    public void SetQuestionText(int questionID){
        _questionText.text = _questions[questionID - 1];
    }

    public void ClearQuestionText(){
        _questionText.text = "";
    }

}