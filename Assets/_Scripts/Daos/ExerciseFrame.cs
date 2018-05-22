using UnityEngine;
using System.Collections;

using Windows.Kinect;
using System;

namespace HomeReval.Daos
{
    [System.Serializable]
    public class ExerciseFrame
    {
        public ExerciseFrame()
        {
            Time = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public Body Body { get; set; }

        public Int32 Time { get; private set; }
    }
}
