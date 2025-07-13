using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joker : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.GetInstance().OnJokerPressed();
    }
}