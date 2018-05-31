using System;
using HomeReval.Domain;
using HomeReval.Validator;
using Windows.Kinect;

namespace HomeReval.Services
{
    class ExerciseService : IExerciseService
    {
        private ExerciseValidator exerciseValidator;

        public void StartNewExercise(Exercise exercise)
        {
            exerciseValidator = new ExerciseValidator(exercise);
        }

        public ExerciseScore Check(ConvertedBody bodyLive)
        {
            return exerciseValidator.Check(bodyLive);
        }

        public void Stop()
        {
            exerciseValidator = null;
        }

        public ConvertedBody Convert(Body body)
        {
            return new ConvertedBody(body);
        }
    }
}
