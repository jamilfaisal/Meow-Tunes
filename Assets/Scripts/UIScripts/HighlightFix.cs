using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
  
// Slight modification from: https://answers.unity.com/questions/1313950/unity-ui-mouse-keyboard-navigate-un-highlight-butt.html  
[RequireComponent(typeof(Selectable))]
public class HighlightFix : MonoBehaviour, IPointerEnterHandler, IDeselectHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!EventSystem.current.alreadySelecting)
            EventSystem.current.SetSelectedGameObject(gameObject);
    }
  
    public void OnDeselect(BaseEventData eventData)
    {
        GetComponent<Selectable>().OnPointerExit(null);
    }
}