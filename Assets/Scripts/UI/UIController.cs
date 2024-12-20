using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    GameObject leaveToMainScreenCheck;
    [SerializeField]
    GameObject gameOverScreen;
    [SerializeField]
    GameObject winScreen;
    [SerializeField]
    GameObject textAdditionalInfoBG;
    [SerializeField]
    TextMeshProUGUI textAdditionalInfo;
    float oldTimeScale = 1.0f; //If we will change time scale, like, for exampel, slow time down, this will protect that.
    //TODO: Maybe rewrite this mess if more ui screens will be added.

    void Update() 
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !gameOverScreen.activeSelf)
        {
            if(inventoryAndStatsScreen.activeSelf)
            {
                inventoryAndStatsScreen.SetActive(false);
                setCursorVisibility(false);
                setGamePause(false);

            }
            else if(leaveToMainScreenCheck.activeSelf)
            {
                pauseMenu.SetActive(true);
                leaveToMainScreenCheck.SetActive(false);
            }
            else
            {
                if (settingsMenu.activeSelf)
                {
                    settingsMenu.SetActive(false);
                    pauseMenu.SetActive(true);
                }
                else if (!pauseMenu.activeSelf)
                {
                    pauseMenu.SetActive(true);
                    setGamePause(true);
                    setCursorVisibility(true);
                }
                else if (pauseMenu.activeSelf)
                {
                    setGamePause(false);
                    pauseMenu.SetActive(false);
                    setCursorVisibility(false);
                }
            }
        }



        if(Input.GetKeyUp(KeyCode.I) && inventoryAndStatsScreen.activeSelf)
        {
            inventoryAndStatsScreen.SetActive(false);
            setCursorVisibility(false);
            setGamePause(false);

        }
        else if (Input.GetKeyUp(KeyCode.I) && !pauseMenu.activeSelf && !settingsMenu.activeSelf)
        {
            inventoryAndStatsScreen.SetActive(true);
            setCursorVisibility(true);
            setGamePause(true);
        }
    }
    void setGamePause(bool state)
    {
        if(state)
        {
            oldTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = oldTimeScale;
        }
    }
    void setCursorVisibility(bool visibility)
    {
        if(visibility) Cursor.lockState = CursorLockMode.Confined;
        else Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = visibility;
    }

    public void SettingsButtonClicked()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ResumeButtonClicked()
    {
        setGamePause(false);
        pauseMenu.SetActive(false);
        setCursorVisibility(false);
    }

    public void LeaveButtonClicked()
    {
        pauseMenu.SetActive(false);
        leaveToMainScreenCheck.SetActive(true);
    }

    public void CancelLeaveButtonClicked()
    {
        pauseMenu.SetActive(true);
        leaveToMainScreenCheck.SetActive(false);
    }

    public void AcceptLeaveButtonClicked()
    {
        SceneManager.LoadScene(0);
        setGamePause(false);
    }
    public void restartButtonPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void changeTooltipText(string text)
    {
        if(text == "" || text == null) textAdditionalInfoBG.SetActive(false);
        else
        {
            textAdditionalInfoBG.SetActive(true);
            textAdditionalInfo.text = text;
        }
    }

    public void gameWon()
    {
        winScreen.SetActive(true);
        setGamePause(true);
    }
}
