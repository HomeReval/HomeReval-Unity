using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public MenuManager mm;

    public void PlayButtonPressed()
    {
        mm.HideMainMenu();
        mm.ShowExercise();
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
