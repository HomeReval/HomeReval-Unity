using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HomeReval.Domain;
using TMPro;

public class ExerciseSelectMenu : MonoBehaviour {

    public MenuManager mm;

    public TMP_Text beginDate;
    public TMP_Text endDate;
    public TMP_Text exName;
    public TMP_Text exDesc;
    public TMP_Text amount;

    public List<Exercise> exList = new List<Exercise>();
    int currentExerciseIndex = 0;
    int maxExerciseIndex = 999;

	// Use this for initialization
	void Start () {
        DateTime today = DateTime.Today;

        Exercise ex1 = new Exercise {
            Id = 0, 
            StartDate = today.AddDays(5), 
            EndDate = today.AddDays(10), 
            Name = "Steps 1 (1234567890123456789012345678901234567890)", 
            Description = "12345678901234657890123465789012346578901234657890", 
            ExerciseRecording = null, 
            Amount = 10 
            };
        Exercise ex2 = new Exercise
        {
            Id = 0,
            StartDate = today.AddDays(10),
            EndDate = today.AddDays(15),
            Name = "Steps 2 (1234567890123456789012345678901234567890)",
            Description = "12345678901234657890123465789012346578901234657890",
            ExerciseRecording = null,
            Amount = 900
        };
        Exercise ex3 = new Exercise
        {
            Id = 0,
            StartDate = today.AddDays(15),
            EndDate = today.AddDays(20),
            Name = "Steps 3 (1234567890123456789012345678901234567890)",
            Description = "(234567890123465789012346578901234657890123465789)",
            ExerciseRecording = null,
            Amount = 1000
        };

        exList.Add(ex1);
        exList.Add(ex2);
        exList.Add(ex3);

        maxExerciseIndex = exList.Count-1;
        SwitchExercise(currentExerciseIndex);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void NextExerciseButton()
    {
        if (currentExerciseIndex < maxExerciseIndex)
        {
            currentExerciseIndex++;
            SwitchExercise(currentExerciseIndex);
        }
    }

    public void PreviousExerciseButton()
    {
        if(currentExerciseIndex > 0 )
        {
            currentExerciseIndex--;
            SwitchExercise(currentExerciseIndex);
        }  
    }


    void SwitchExercise(int i)
    {
        beginDate.text = exList[i].StartDate.ToString();
        endDate.text = exList[i].EndDate.ToString();
        exName.text = exList[i].Name;
        exDesc.text = exList[i].Description;
        amount.text = exList[i].Amount.ToString();
    }

    public void submit()
    {
        mm.LoadPracticeScene();
    }


}
