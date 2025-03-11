using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTooltipOnHoover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    TextMeshProUGUI textObj;
    [SerializeField]
    GameObject tooltipHolder;
    public string textToDisplay = "";

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(textToDisplay != "")
        {
            tooltipHolder.SetActive(true);
            textObj.text = textToDisplay;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipHolder.SetActive(false);
    }
}
