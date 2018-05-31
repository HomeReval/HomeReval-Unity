
using HomeReval.Domain;
using System;
using System.Collections.Generic;
using Windows.Kinect;

namespace HomeReval.Validator
{
    class ExerciseValidator
    {
        public enum ValidatorState { NotStarted, WaitingForNext, Checking, Done };

        private const double margin = 5.0;
        private const double middleMargin = 10.0;
        private const double errorMargin = 15.0;

        private List<ExerciseScore> exerciseScores;
        private ExerciseRecording currentExercise;
        private Exercise exercise;

        // State
        private ValidatorState state;
        private int frame;
        private int current;
        private DateTime latestValidatedCheck;

        public ExerciseValidator(Exercise exercise)
        {
            // Set default state
            state = ValidatorState.NotStarted;
            frame = 0;
            current = 0;

            this.exercise = exercise;
            exerciseScores = new List<ExerciseScore>();
        }

        public ExerciseScore Check(ConvertedBody bodyLive)
        {
            if (state == ValidatorState.Done) return null;

            // Get body from current frame
            ConvertedBody bodyJSON = exercise.ExerciseRecordings[0].ConvertedBodies[frame];
            ExerciseScore exerciseScore;

            // Check move
            exerciseScore = Validate(bodyLive, bodyJSON);

            // Wait for user to start
            if (frame == 0 && !exerciseScore.Check)
            {
                return exerciseScore;
            }

            UnityEngine.Debug.Log(frame);

            // Find end of exercise
            if (frame == exercise.ExerciseRecordings[0].ConvertedBodies.Count-1)
            {
                frame = 0;
                current++;

                if(current == exercise.Amount)
                {
                    state = ValidatorState.Done;
                }
                else
                {
                    state = ValidatorState.WaitingForNext;
                }

                return exerciseScore;
            }

            // Check if user missed checks for more then 1 second and cancel current exercise
            if ((frame != 0) && (latestValidatedCheck < DateTime.Now.AddSeconds(-1)))
            {
                state = ValidatorState.WaitingForNext;
                frame = 0;
                return exerciseScore;
            }

            if (exerciseScore.Check)
            {
                // Check completed
                frame++;
                latestValidatedCheck = DateTime.Now;
            }

            UnityEngine.Debug.Log(frame);

            state = ValidatorState.Checking;

            return exerciseScore;
        }

        private ExerciseScore Validate(ConvertedBody bodyLive, ConvertedBody bodyJSON)
        {
            // Create new score
            ExerciseScore exerciseScore = new ExerciseScore() { Check = true, Score = 4 };

            // Loop through all joints in Dict
            foreach (var item in Map.LeftArmMappings)
            {
                // Get current joints out of dict
                JointType currentType = (JointType)item.Key;
                JointType targetType = (JointType)item.Value;

                // Get angle difference between live and recorded date
                double angleDifference = (bodyJSON.JointResults[currentType].Angle - bodyLive.JointResults[currentType].Angle);

                // Check if difference is bigger then the smallest margin
                if (angleDifference <= (Math.Abs(margin) * -1) || angleDifference >= margin)
                {
                    // Set check on false if one is outside smallest margin
                    exerciseScore.Check = false;
                }

                // Find right score for difference
                if (angleDifference > (Math.Abs(margin) * -1) && angleDifference < margin)
                {
                    if (!(exerciseScore.Score < 4))
                        exerciseScore.Score = 4;
                }
                else if (angleDifference > (Math.Abs(middleMargin) * -1) && angleDifference < middleMargin)
                {
                    if (!(exerciseScore.Score < 3))
                        exerciseScore.Score = 3;
                }
                else if (angleDifference > (Math.Abs(errorMargin) * -1) && angleDifference < errorMargin)
                {
                    if (!(exerciseScore.Score < 2))
                        exerciseScore.Score = 2;
                }
                else
                {
                    if (!(exerciseScore.Score < 1))
                        exerciseScore.Score = 1;
                }
            }

            // Add score to exerciseScores
            exerciseScores.Add(exerciseScore);

            return exerciseScore;
        }

        public ExerciseScore LatestScore 
        {
            get {
                if (exerciseScores.Count == 0) return null;
                return exerciseScores[exerciseScores.Count - 1]; 
            }
        }
    }
}
