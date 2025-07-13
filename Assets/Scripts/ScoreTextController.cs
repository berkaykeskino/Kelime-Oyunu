using TMPro;
using UnityEngine;

public class ScoreTextController : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI _bestScore;
    [SerializeField] private TextMeshProUGUI _lastScore;

    void Awake()
    {
        int bestScore = PlayerPrefs.GetInt("bestScore");
        _bestScore.text = "Best Score: " + bestScore.ToString();

        int lastScore = PlayerPrefs.GetInt("lastScore");
        _lastScore.text = "Last Score: " + lastScore.ToString();
    }
}