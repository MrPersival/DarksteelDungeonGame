using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButtonUI> tabButtons;

    public void Subscribe(TabButtonUI button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButtonUI>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButtonUI button)
    {

    }

    public void OnTabExit(TabButtonUI button)
    {

    }

    public void OnTabSelected(TabButtonUI button)
    {
        foreach (TabButtonUI tabButton in tabButtons) tabButton.TabUnclick();
    }
}
