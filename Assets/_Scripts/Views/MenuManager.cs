using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject loginScreen;

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

}
