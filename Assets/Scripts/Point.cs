using TMPro;
using UnityEngine;

public class Point : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _pointText;
    private int _currentPoint = 0;

    public void AddPoint(int point){
        _currentPoint += point;
        _pointText.text = _currentPoint.ToString();
    }

    public int GetCurrentPoint(){
        return _currentPoint;
    }   
}