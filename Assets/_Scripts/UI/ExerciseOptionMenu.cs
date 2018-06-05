using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseOptionMenu : MonoBehaviour {

    public MenuManager mm;

    public void NewExButtonPressed()
    {
        mm.HideExercise();
        mm.ShowNewExerciseMenu();
    }

    public void SelectExButtonPressed()
    {
        mm.HideExercise();
        mm.ShowSelectExerciseMenu();
    }


}
