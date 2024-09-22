using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    public TabGroup tabGroup;
    public GameObject tabsScreen;
    [SerializeField]
    Sprite pressedButton;
    [SerializeField]
    bool pressedByDefault = false;
    [SerializeField]
    Sprite spriteDefault;

    private void Start()
    {
        tabGroup.Subscribe(this);
        if(pressedByDefault) TabClicked();
    }

    public void TabClicked()
    {
        tabGroup.OnTabSelected(this);
        tabsScreen.SetActive(true);
        gameObject.GetComponent<Image>().sprite = pressedButton;
    }
    public void TabUnclick()
    {
        tabsScreen.SetActive(false);
        gameObject.GetComponent<Image>().sprite = spriteDefault;
    }
}
