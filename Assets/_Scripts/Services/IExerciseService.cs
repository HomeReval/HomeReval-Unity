using HomeReval.Domain;
using UnityEngine.UI;
using Windows.Kinect;

namespace HomeReval.Services
{
    interface IExerciseService
    {
        void StartNewExercise(Exercise exercise, Text text);
        ExerciseScore Check(ConvertedBody bodyLive);
        ConvertedBody Convert(Body body);
        void Stop();
    }
}
