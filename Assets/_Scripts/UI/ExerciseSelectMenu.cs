using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HomeReval.Domain;
using TMPro;
using HomeReval.Services;
using Newtonsoft.Json.Linq;
using System.Linq;

public class ExerciseSelectMenu : MonoBehaviour {

    public MenuManager mm;

    public TMP_Text beginDate;
    public TMP_Text endDate;
    public TMP_Text exName;
    public TMP_Text exDesc;
    public TMP_Text amount;

    //public List<Exercise> exList = new List<Exercise>();
    //int currentExerciseIndex = 0;
    int maxExerciseIndex = 999;

    HomeRevalSession hrs;

    // Services
    IRequestService requestService = new RequestService();

	// Use this for initialization
	void Start () {

        DateTime today = DateTime.Today;

        hrs = HomeRevalSession.Instance;

        Debug.Log("TODAY: " + today.ToString("yyyy-MM-dd"));

        // Get exercises
        StartCoroutine(requestService.Get("/exerciseplanning/date/"+today.ToString("yyyy-MM-dd"), success => 
        {
            Debug.Log(success);
            //JArray response = JArray.Parse(success);
            List<JObject> response = JArray.Parse(success).ToObject<List<JObject>>();

            foreach (JObject exercisePlanning in response.OrderByDescending(o => o.SelectToken("exercise.id")).ToList())
            {
                hrs.Exercises.Add(new Exercise
                {
                    Id = Convert.ToInt32(exercisePlanning.SelectToken("exercise.id").ToString()),
                    StartDate = DateTime.Parse(exercisePlanning.SelectToken("startDate").ToString()),
                    EndDate = DateTime.Parse(exercisePlanning.SelectToken("endDate").ToString()),
                    Name = exercisePlanning.SelectToken("exercise.name").ToString(),
                    Description = exercisePlanning.SelectToken("exercise.description").ToString(),
                    Amount = Convert.ToInt32(exercisePlanning.SelectToken("amount").ToString()),
                    PlanningId = Convert.ToInt32(exercisePlanning.SelectToken("id").ToString())
                });
            }

            // Switch exercise after receiving exercises
            SwitchExercise(hrs.currentExerciseIdx);
            maxExerciseIndex = hrs.Exercises.Count - 1;
        },
        error => {
            Debug.Log(error);
        }));

        /*Exercise ex1 = new Exercise {
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
        };*/

        /*exList.Add(ex1);
        exList.Add(ex2);
        exList.Add(ex3);*/
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void NextExerciseButton()
    {
        if (hrs.currentExerciseIdx < maxExerciseIndex)
        {
            hrs.currentExerciseIdx++;
            SwitchExercise(hrs.currentExerciseIdx);
        }
    }

    public void PreviousExerciseButton()
    {
        if(hrs.currentExerciseIdx > 0 )
        {
            hrs.currentExerciseIdx--;
            SwitchExercise(hrs.currentExerciseIdx);
        }  
    }


    void SwitchExercise(int i)
    {
        beginDate.text = hrs.Exercises[i].StartDate.ToString("dd/MM/yyyy");
        endDate.text = hrs.Exercises[i].EndDate.ToString("dd/MM/yyyy");
        exName.text = hrs.Exercises[i].Name;
        exDesc.text = hrs.Exercises[i].Description;
        amount.text = hrs.Exercises[i].Amount.ToString();
    }

    public void submit()
    {
        mm.LoadPracticeScene();
    }


}
