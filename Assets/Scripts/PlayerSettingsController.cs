using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsController : MonoBehaviour
{
    //Will apply settings that cannot be applied in main menu

    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    PlayerController playerController;
    void Start()
    {
        mainCamera.fieldOfView = PlayerPrefs.GetFloat("FovSlider", mainCamera.fieldOfView);
        playerController.sensitivity = PlayerPrefs.GetFloat("MouseSensetivitySlider", playerController.sensitivity);
    }

}
