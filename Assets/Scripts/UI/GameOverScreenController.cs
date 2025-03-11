using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreenController : MonoBehaviour
{
    [SerializeField]
    XPSystem playerXPSystem;
    [SerializeField]
    TextMeshProUGUI points;

    private void OnEnable()
    {
        float totalXP = playerXPSystem.xp + playerXPSystem.playerLevel * playerXPSystem.xpToLevelUp;
        points.text = totalXP.RoundToInt().ToString();
    }
}
