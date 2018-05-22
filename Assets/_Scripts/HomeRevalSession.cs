using HomeReval.Daos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Windows.Kinect;

public sealed class HomeRevalSession
{
    private static volatile HomeRevalSession instance;
    private static object syncRoot = new object();



    private HomeRevalSession()
    {

    }

    public static HomeRevalSession Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new HomeRevalSession();
                }
            }

            return instance;
        }
    }

    public List<Exercise> Exercises { get; set; }
}
