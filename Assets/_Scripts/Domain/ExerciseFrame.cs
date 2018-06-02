using UnityEngine;
using System.Collections;

using Windows.Kinect;
using System;

namespace HomeReval.Domain
{
    [System.Serializable]
    public class ExerciseFrame
    {
        public ExerciseFrame()
        {
            Time = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        }

        public Body Body { get; set; }

        public Int32 Time { get; private set; }
    }
}
