using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardButton : MonoBehaviour, IPointerEnterHandler{
    private Keyboard _keyboard;
    [SerializeField] private TextMeshProUGUI _letter;

    public void Initialize(char letter, Keyboard keyboard, int wordLength, int index, float distance)
    {
        SetLetter(letter);
        SetKeyboard(keyboard);
        gameObject.transform.localPosition = GetLocalPosition(index, wordLength, distance);
    }

    public Vector2 GetLocalPosition(int cellIndex, int wordLength, float distance)
    {
        float degree = (float)cellIndex / wordLength * 360;
        float radian = math.radians(degree);
        float offset = 45;
        float localX = math.sin(radian) * (distance - offset);
        float localY = math.cos(radian) * (distance - offset);

        return new Vector2(localX, localY);
    }

    private void OnButtonEnter(){
        _keyboard.OnKeyboardButtonEnter(char.Parse(_letter.text));
    }

    public void SetLetter(char letter){
        if (letter == 'ı'){
            _letter.text = "I";
        }
        else{
            _letter.text = char.ToString(letter).ToUpper();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnButtonEnter();
    }

    private void SetKeyboard(Keyboard keyboard){
        _keyboard = keyboard;
    }
}