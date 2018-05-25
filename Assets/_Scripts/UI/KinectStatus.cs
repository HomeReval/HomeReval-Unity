using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Windows.Kinect;
using System;

public class KinectStatus : MonoBehaviour {

    public bool isConnected = false;
    public TMP_Text state;

    void Awake()
    {
        KinectSensor _sensor = KinectSensor.GetDefault();

        isConnected = _sensor.IsAvailable;
        _sensor.IsAvailableChanged += 
    }

    void Changed(EventArgs args)
    {

    }

    // Update is called once per frame
    void Update () {
        if (isConnected)
        {
            state.text = "Verbonden";
			state.color = Color.green;
        }
        if (!isConnected)
        {
            state.text = "Niet Verbonden";
			state.color = Color.red;
        }

	}
}
