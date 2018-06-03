using UnityEngine;
using TMPro;
using System;
using HomeReval.Domain;
using UnityEngine.UI;
using Newtonsoft.Json;

public class ExerciseMenu : MonoBehaviour {

	public TMP_InputField startDateDay;
    public TMP_InputField startDateMonth;
    public TMP_InputField startDateYear;
    public TMP_InputField endDateDay;
    public TMP_InputField endDateMonth;
    public TMP_InputField endDateYear;

    public Toggle spineToggle;
    public Toggle leftArmToggle;
    public Toggle rightArmToggle;
    public Toggle leftLegToggle;
    public Toggle rightLegToggle;

    public TMP_InputField exerciseName;
    public TMP_InputField exerciseDescription;
    public TMP_InputField amount;

    private HomeRevalSession hrs;
    public MenuManager mm;

    public void Awake()
    {
        hrs = HomeRevalSession.Instance;
    }

    public void Submit(DateTime start, DateTime end, string name, string desc, int amount, bool spineCheckbox, bool leftArmCheckbox, bool rightArmCheckbox, bool leftLegCheckbox, bool rightLegCheckbox)
    {
        ExerciseRecording exerciseRecording = new ExerciseRecording();

        // Add right jointsmappings to recording
        if (spineCheckbox)
            exerciseRecording.JointMappings.Add(HomeReval.Validator.Map.Mappings.Spine);

        if (leftArmCheckbox)
            exerciseRecording.JointMappings.Add(HomeReval.Validator.Map.Mappings.LeftArm);

        if (rightArmCheckbox)
            exerciseRecording.JointMappings.Add(HomeReval.Validator.Map.Mappings.RightArm);

        if (leftLegCheckbox)
            exerciseRecording.JointMappings.Add(HomeReval.Validator.Map.Mappings.LeftLeg);

        if (rightLegCheckbox)
            exerciseRecording.JointMappings.Add(HomeReval.Validator.Map.Mappings.RightLeg);

        hrs.CurrentRecording = new Exercise
        {
            StartDate = start,
            EndDate = end,
            Name = name,
            Description = desc,
            Amount = amount,
            ExerciseRecording = exerciseRecording
        };
    }

    public void CheckAndConvert()
    {
        DateTime startDate;
        DateTime endDate;

        DateTime.TryParse(startDateMonth.text + "/" + startDateDay.text + "/" + startDateYear.text, out startDate);
        DateTime.TryParse(endDateMonth.text + "/" + endDateDay.text + "/" + endDateYear.text, out endDate);

        bool spineCheckbox = spineToggle.isOn;
        bool leftArmCheckbox = leftArmToggle.isOn;
        bool rightArmCheckbox = rightArmToggle.isOn;
        bool leftLegCheckbox = leftLegToggle.isOn;
        bool rightLegCheckbox = rightLegToggle.isOn;

        //amount
        int amnt = StringToInt(amount.text);
        if (amnt <= 0)
        {
            amnt = 1;
        }

        if(startDate < endDate)
        {
            Submit(startDate, endDate, exerciseName.text, exerciseDescription.text, amnt, spineCheckbox, leftArmCheckbox, rightArmCheckbox, leftLegCheckbox, rightLegCheckbox);
            //Debug.Log("succesfully submitted");
            mm.SwitchToNextScene();
        }
        else
        {
            //Debug.Log("error during submit");
        }

    }

    int StringToInt(string stringInput)
    {
        int output;
        Int32.TryParse(stringInput, out output);
        return output;
    }

}
