using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public MenuManager mm;

    public void PlayButtonPressed()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + 1);
    }

    public void OptionsButtonPressed()
    {
        mm.ShowOptions();
        mm.HideMainMenu();
    }

    public void QuitButtonPressed()
    {
        Application.Quit();
    }


}
