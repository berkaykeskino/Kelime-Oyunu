using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _totalTimeInSeconds = 240f;
    private float _remainingTime;
    private Image _timerImage;

    public void InitializeTimer()
    {
        _timerImage = GetComponent<Image>();
        _remainingTime = _totalTimeInSeconds;
        _timerImage.fillAmount = 1f;
        StartCoroutine(DecreaseTime());
    }

    private IEnumerator DecreaseTime()
    {
        while (_remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            _remainingTime--;
            _timerImage.fillAmount = _remainingTime / _totalTimeInSeconds;
            if (_remainingTime == 0){
                OnTimeOut();
            }
        }
    }

    private void OnTimeOut(){
        GameManager.GetInstance().OnTimeOut();
    }
}
