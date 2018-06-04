using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject loginScreen;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject exerciseMenu;
    public GameObject newExerciseMenu;
    public GameObject selectExerciseMenu;

    public void LoadRecordingScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + 1);
    }

    public void LoadPracticeScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + 2);
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

    public void HideNewExerciseMenu()
    {
        newExerciseMenu.SetActive(false);
    }

    public void HideSelectExerciseMenu()
    {
        selectExerciseMenu.SetActive(false);
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

    public void ShowNewExerciseMenu()
    {
        newExerciseMenu.SetActive(true);
    }

    public void ShowSelectExerciseMenu()
    {
        selectExerciseMenu.SetActive(true);
    }

}
