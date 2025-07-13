using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LevelManager{

    public void CreateLevel(){
        TextAsset wordsFile;
        wordsFile = Resources.Load<TextAsset>("words");
        WordsData wordsData = JsonUtility.FromJson<WordsData>(wordsFile.text);
        List<LevelQuestion> selectedWords = new List<LevelQuestion>();
        selectedWords.AddRange(wordsData.length4.OrderBy(x => Random.value).Take(2));
        selectedWords.AddRange(wordsData.length5.OrderBy(x => Random.value).Take(2));
        selectedWords.AddRange(wordsData.length6.OrderBy(x => Random.value).Take(2));
        selectedWords.AddRange(wordsData.length7.OrderBy(x => Random.value).Take(2));
        selectedWords.AddRange(wordsData.length8.OrderBy(x => Random.value).Take(2));
        selectedWords.AddRange(wordsData.length9.OrderBy(x => Random.value).Take(2));
        selectedWords.AddRange(wordsData.length10.OrderBy(x => Random.value).Take(2));

        for(int i = 0; i< selectedWords.Count; i++){
            Debug.Log(selectedWords[i].value);
        }

        // Combine all unique letters from answers
        string letters = new string(
            selectedWords
            .SelectMany(entry => entry.value.ToUpper().ToCharArray())
            .Distinct()
            .OrderBy(c => c)
            .ToArray()
        );

        // Create level object
        LevelData newLevel = new LevelData
        {
            LevelNumber = 0,
            LevelTime = 240,
            LevelLetters = letters,
            LevelQuestions = selectedWords
        };

        // Serialize to JSON
        string levelJson = JsonUtility.ToJson(newLevel, true);

        // Write to persistent data path
        string outputPath = Path.Combine(Application.persistentDataPath, "level0.json");
        File.WriteAllText(outputPath, levelJson);

        Debug.Log("Level created at: " + outputPath);

    }

    public LevelData GetLevelData()
    {
        string path = Path.Combine(Application.persistentDataPath, "level0.json");
        string json = File.ReadAllText(path);
        LevelData currentLevelData = JsonUtility.FromJson<LevelData>(json);

        return currentLevelData;
    }

    private int GetCurrentLevel(){
        return PlayerPrefs.GetInt("level");
    }

    public void LevelUp(){
        PlayerPrefs.SetInt("level", GetCurrentLevel() + 1);
    }

    
}


internal struct WordsData
{
    public List<LevelQuestion> length4;
    public List<LevelQuestion> length5;
    public List<LevelQuestion> length6;
    public List<LevelQuestion> length7;
    public List<LevelQuestion> length8;
    public List<LevelQuestion> length9;
    public List<LevelQuestion> length10;
}
