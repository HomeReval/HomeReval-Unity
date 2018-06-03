using System;
using System.Collections.Generic;
using HomeReval.Domain;
using HomeReval.Validator;
using UnityEngine.UI;
using Views;
using Windows.Kinect;

namespace HomeReval.Services
{
    class ExerciseService : IExerciseService
    {
        private ExerciseValidator exerciseValidator;

        public void StartNewExercise(Exercise exercise, IBodyDrawer exampleBodyDrawer, Text text)
        {
            exerciseValidator = new ExerciseValidator(exercise, exampleBodyDrawer, text);
        }

        public ExerciseScore Check(ConvertedBody bodyLive)
        {
            return exerciseValidator.Check(bodyLive);
        }

        public void Stop()
        {
            exerciseValidator = null;
        }

        public ConvertedBody Convert(Body body, List<Map.Mappings> jointMappings)
        {
            return new ConvertedBody(body, jointMappings);
        }
    }
}
