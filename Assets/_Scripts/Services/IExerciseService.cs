using HomeReval.Domain;
using Windows.Kinect;

namespace HomeReval.Services
{
    interface IExerciseService
    {
        void StartNewExercise(Exercise exercise);
        ExerciseScore Check(ConvertedBody bodyLive);
        ConvertedBody Convert(Body body);
        void Stop();
    }
}
