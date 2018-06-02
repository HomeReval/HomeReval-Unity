using UnityEngine;
using TMPro;
using System;
using HomeReval.Domain;

public class ExerciseMenu : MonoBehaviour {

	public TMP_InputField startDateDay;
    public TMP_InputField startDateMonth;
    public TMP_InputField startDateYear;
    public TMP_InputField endDateDay;
    public TMP_InputField endDateMonth;
    public TMP_InputField endDateYear;

    public TMP_InputField exerciseName;
    public TMP_InputField exerciseDescription;
    public TMP_InputField amount;

    private HomeRevalSession hrs;
    public MenuManager mm;

    public void Awake()
    {
        hrs = HomeRevalSession.Instance;
    }

    public void Submit(DateTime start, DateTime end, string name, string desc, int amount)
    {
        hrs.CurrentRecording = new Exercise
        {
            /*StartDate = start,
            EndDate = end,*/
            Name = name,
            Description = desc,
            Amount = amount,
        };
    }

    public void CheckAndConvert()
    {
        DateTime startDate;
        DateTime endDate;

        DateTime.TryParse(startDateMonth.text + "/" + startDateDay.text + "/" + startDateYear.text, out startDate);
        DateTime.TryParse(endDateMonth.text + "/" + endDateDay.text + "/" + endDateYear.text, out endDate);

        //amount
        int amnt = StringToInt(amount.text);
        if (amnt <= 0)
        {
            amnt = 1;
        }

        if(startDate < endDate)
        {
            Submit(startDate, endDate, exerciseName.text, exerciseDescription.text, amnt);
            Debug.Log("succesfully submitted");
            mm.SwitchToNextScene();
        }
        else
        {
            Debug.Log("error during submit");
        }

    }

    int StringToInt(string stringInput)
    {
        int output;
        Int32.TryParse(stringInput, out output);
        return output;
    }

}
