using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject settingsMenu;
    [SerializeField]
    GameObject inventoryAndStatsScreen;
    [SerializeField]
    GameObject abilitiesScreen;
    bool isCursorActive = false;
    float oldTimeScale = 1.0f; //If we will change time scale, like, for exampel, slow time down, this will protect that.
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(settingsMenu.activeSelf)
            {
                settingsMenu.SetActive(false);
                pauseMenu.SetActive(true);
            }
            else if(!pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(true);
                oldTimeScale = Time.timeScale;
                Time.timeScale = 0;
                changeCursorState();
            }
            else if(pauseMenu.activeSelf)
            {
                Time.timeScale = oldTimeScale;
                pauseMenu.SetActive(false);
                changeCursorState();
            }

        }
    }

    void changeCursorState()
    {
        if(!isCursorActive)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            isCursorActive = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isCursorActive = false;
        }
    }

    public void SettingsButtonClicked()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
}
