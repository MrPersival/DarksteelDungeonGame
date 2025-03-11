using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject settingsMenu;
    [SerializeField]
    GameObject creditsScreen;
    [SerializeField]
    GameObject mainMenu;
    [SerializeField]
    Animator settingsButtonAnim;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
            settingsButtonAnim.Rebind(); //requied to reset, otherwise will go to "Pressed" state and stay there. TODO: Find a better way to fix this.
            settingsButtonAnim.Update(0f);
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && creditsScreen.activeSelf)
        {
            creditsScreen.SetActive(false);
            mainMenu.SetActive(true);
            settingsButtonAnim.Rebind(); //requied to reset, otherwise will go to "Pressed" state and stay there. TODO: Find a better way to fix this.
            settingsButtonAnim.Update(0f);
        }
    }

    public void SettingsButtonPressed()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void cancelSettingsButtonPressed()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void newGameButtonPressed()
    {
        SceneManager.LoadScene(1);
    }
    public void quitButtonPressed()
    {
        Application.Quit();
    }
    public void creditsButtonPressed()
    {
        creditsScreen.SetActive(true);
        mainMenu.SetActive(false);
    }
}
