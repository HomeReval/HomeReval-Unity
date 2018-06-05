using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class EndDateValidator : MonoBehaviour {

    DateTime currenDate = DateTime.Today.AddDays(14);

    int dayMin = 1;
    int dayMax = 31;
    int monthMin = 1;
    int monthMax = 12;
    int yearMin = 0;
    int yearMax = DateTime.Today.Year + 10;
    
    public TMP_InputField day;
    public TMP_InputField month;
    public TMP_InputField year;

    private void Awake()
    {
        day.text = currenDate.Day.ToString();
        month.text = currenDate.Month.ToString();
        year.text = currenDate.Year.ToString();
    }

    public void OnDayChanged(string Sinput)
    {
        if(Sinput == "")
        {
            return;
        }
        int input;
        Int32.TryParse(Sinput, out input);
        if(input < dayMin)
        {
            day.text = ToString();
        } else if(input > dayMax)
        {
            day.text = dayMax.ToString();
        }
    }

    public void OnMonthChanged(string Sinput)
    {
        if (Sinput == "")
        {
            return;
        }
        int input;
        Int32.TryParse(Sinput, out input);
        if (input < monthMin)
        {
            month.text = monthMin.ToString();
        }
        else if (input > monthMax)
        {
            month.text = monthMax.ToString();
        }
    }

    public void OnYearChanged(string Sinput)
    {
        if (Sinput == "")
        {
            return;
        }
        int input;
        Int32.TryParse(Sinput, out input);
        if (input < yearMin)
        {
            year.text = yearMin.ToString();
        }
        else if (input > yearMax)
        {
            year.text = yearMax.ToString();
        }
    }

}
