using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Windows.Kinect;

public sealed class RecordingSession
{
    private static volatile RecordingSession instance;
    private static object syncRoot = new object();
    private static List<Body> recording;

    private RecordingSession()
    {

    }

    public static RecordingSession Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new RecordingSession();
                }
            }

            return instance;
        }
    }

    public static List<Body> Recording
    {
        get
        {
            return recording;
        }

        set
        {
            recording = value;
        }
    }
}
