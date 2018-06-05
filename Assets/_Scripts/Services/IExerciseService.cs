using HomeReval.Domain;
using HomeReval.Validator;
using System.Collections.Generic;
using UnityEngine.UI;
using Views;
using Windows.Kinect;

namespace HomeReval.Services
{
    interface IExerciseService
    {
        void StartNewExercise(Exercise exercise, IBodyDrawer exampleBodyDrawer);
        ExerciseScore Check(ConvertedBody bodyLive);
        ConvertedBody Convert(Body body, List<Map.Mappings> jointMappings);
        ExerciseValidator.ValidatorState State();
        void Stop();
        string Progression();
    }
} 
