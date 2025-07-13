using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class AnswerBox : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _letter;
    private char _enableJoker = ' ';

    public void Initialize(char letter, int index, float answerGridWidth){
        SetLetter(letter);
        CreateAnimation(index, answerGridWidth);
    }

    public void SetLetter(char letter)
    {
        if (letter == ' '){
            _letter.text = char.ToString(_enableJoker);
        }else{
            _letter.text = char.ToString(letter);
        }
    }

    public bool IsJokerEnabled(){
        return _enableJoker != ' ';
    }

    public void EnableJoker(char answer){
        _enableJoker = answer;
        gameObject.transform.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360);
    }

    public Vector2 GetLocalPosition(int cellIndex, float answerGridWidth)
    {
        float localX = cellIndex * 70;
        return new Vector2(localX - answerGridWidth / 2, 220);
    }

    public void CreateAnimation(int index, float answerGridWidth){
        float createDelay = 0.2f;
        Vector2 firstBoxPosition = GetLocalPosition(0, answerGridWidth);
        gameObject.transform.localPosition = firstBoxPosition;
        gameObject.transform.DOLocalMoveX(GetLocalPosition(index, answerGridWidth).x, createDelay);
    }

    public void SelfDestroy(float answerGridWidth, float destroyDelay)
    {
        Vector2 firstBoxPosition = GetLocalPosition(0, answerGridWidth);
        gameObject.transform.DOLocalMoveX(firstBoxPosition.x, destroyDelay);
        StartCoroutine(DisableAnswerBox(destroyDelay));
        Destroy(gameObject, destroyDelay + 0.05f); //0.05 prevents dotween from accessing transform after it is destroyed
    }


    private IEnumerator DisableAnswerBox(float destroyDelay)
    {
        yield return new WaitForSeconds(destroyDelay);
        gameObject.SetActive(false);
    }
}