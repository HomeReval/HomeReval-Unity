using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject loginScreen;
    public GameObject exerciseMenu;

    public void SwitchToNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + 1);
    }

    public void HideOptions()
    {
        optionsMenu.SetActive(false);
    }

    public void HideMainMenu()
    {
        mainMenu.SetActive(false);
    }

    public void HideLogin()
    {
        loginScreen.SetActive(false);
    }

    public void HideExercise()
    {
        exerciseMenu.SetActive(false);
    }



    public void ShowOptions()
    {
        optionsMenu.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
    }

    public void ShowLogin()
    {
        loginScreen.SetActive(true);
    }

    public void ShowExercise()
    {
        exerciseMenu.SetActive(true);
    }

}
