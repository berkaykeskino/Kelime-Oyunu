

using System.Collections.Generic;

public interface IObserveGameManager
{
    public void AddSelfToGameManagerObservers();
    public void ReceiveLevelData(LevelData levelData);
}

public struct LevelData
{
    public int LevelNumber;
    public float LevelTime;
    public string LevelLetters;
    public List<LevelQuestion> LevelQuestions;
}

[System.Serializable]
public struct LevelQuestion{
    public string key;
    public string value;
}