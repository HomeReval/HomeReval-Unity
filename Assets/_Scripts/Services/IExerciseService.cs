using HomeReval.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Kinect;

namespace HomeReval.Services
{
    interface IExerciseService
    {
        int Compare(ConvertedBody bodyJSON, ConvertedBody bodyLive);
        ConvertedBody Convert(Body body);
    }
}
