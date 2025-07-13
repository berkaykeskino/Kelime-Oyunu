using System.Collections.Generic;
using Unity.Mathematics;

public class AnswerChecker: IObserveGameManager{

    List<string> _answers = new List<string>();

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

    public bool CheckAnswer(string buffer, int questionID){
        buffer = buffer.Substring(0, math.min(GetAnswer(questionID).Length, buffer.Length));
        if (GetAnswer(questionID).ToLower().Equals(buffer.ToLower())){
            return true;
        }
        return false;
    }

    public string GetAnswer(int questionID){
        return _answers[questionID - 1];
    }
}