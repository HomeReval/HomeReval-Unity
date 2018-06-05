
using HomeReval.Domain;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Views;
using Windows.Kinect;

namespace HomeReval.Validator
{
    class ExerciseValidator
    {
        public enum ValidatorState { NotStarted, WaitingForNext, Checking, Done };

        private const double margin = 10.0;
        private const double middleMargin = 20.0;
        private const double errorMargin = 30.0;

        private List<ExerciseScore> exerciseScores;
        private ExerciseRecording currentExercise;
        private Exercise exercise;

        // State
        private ValidatorState state;
        private int frame;
        private int current;
        private DateTime latestValidatedCheck;

        // View
        private IBodyDrawer exampleBodyDrawer;

        // Temp
        private Text text;

        public ExerciseValidator(Exercise exercise, IBodyDrawer exampleBodyDrawer, Text text)
        {
            // Set default state
            state = ValidatorState.NotStarted;
            frame = 0;
            current = 0;

            this.exercise = exercise;
            exerciseScores = new List<ExerciseScore>();

            this.exampleBodyDrawer = exampleBodyDrawer;

            // Temp
            this.text = text;
        }

        public ExerciseScore Check(ConvertedBody bodyLive)
        {
            // Get body from current frame
            ConvertedBody bodyJSON = exercise.ExerciseRecording.ConvertedBodies[frame];

            // Check move
            ExerciseScore exerciseScore;
            exerciseScore = Validate(bodyLive, bodyJSON);

            text.text = "state: " + state+" frame: " +frame + " score: " + exerciseScore.Score + " current: " + current;

            // Draw red body with current frame
            exampleBodyDrawer.DrawSkeleton(bodyJSON.CheckJoints);

            switch (state)
            {
                case ValidatorState.NotStarted:

                    // Wait for user to start
                    if (frame == 0 && !exerciseScore.Check)
                    {
                        return exerciseScore;
                    }

                    state = ValidatorState.Checking;

                    return exerciseScore;

                case ValidatorState.WaitingForNext:

                    // Wait for user to start
                    if (frame == 0 && !exerciseScore.Check)
                    {
                        return exerciseScore;
                    }

                    state = ValidatorState.Checking;

                    return exerciseScore;

                case ValidatorState.Checking:
                    if (exerciseScore.Check)
                    {
                        // Check completed
                        frame++;
                        latestValidatedCheck = DateTime.Now;
                    }

                    // Find end of exercise
                    if (frame == exercise.ExerciseRecording.ConvertedBodies.Count - 1)
                    {
                        frame = 0;
                        current++;

                        UnityEngine.Debug.Log(current + "  -  " + exercise.Amount);

                        if (current == exercise.Amount)
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
                    UnityEngine.Debug.Log(latestValidatedCheck < DateTime.Now.AddSeconds(-5));
                    if (latestValidatedCheck < DateTime.Now.AddSeconds(-5))
                    {
                        state = ValidatorState.WaitingForNext;
                        frame = 0;
                        return exerciseScore;
                    }

                    UnityEngine.Debug.Log(frame);

                    state = ValidatorState.Checking;

                    return exerciseScore;

                case ValidatorState.Done:
                    return null;

                default:
                    return null;
            }
        }

        private ExerciseScore Validate(ConvertedBody bodyLive, ConvertedBody bodyJSON)
        {
            // Create new score
            ExerciseScore exerciseScore = new ExerciseScore() { Check = true, Score = 4 };

            // Loop through all joint mappings from exercise
            foreach(Map.Mappings mapping in exercise.ExerciseRecording.JointMappings)
            {
                // Loop through all joints in Dict
                foreach (var item in Map.DictMappings[mapping])
                {
                    // Get current joints out of dict
                    JointType currentType = (JointType)item.Key;
                    JointType targetType = (JointType)item.Value;

                    // Get angle difference between live and recorded date
                    double angleDifference = 180 - Math.Abs(Math.Abs(bodyJSON.JointResults[currentType].Angle - bodyLive.JointResults[currentType].Angle) - 180);
                    if(currentType == JointType.ShoulderLeft)
                        text.text = "angledifference: " + angleDifference + " jsonangle: "+ bodyJSON.JointResults[currentType].Angle + " Liveangle: " + bodyLive.JointResults[currentType].Angle;

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
            }

            // Add score to exerciseScores
            exerciseScores.Add(exerciseScore);

            return exerciseScore;
        }

        /*public ExerciseScore LatestScore 
        {
            get {
                if (exerciseScores.Count == 0) return null;
                return exerciseScores[exerciseScores.Count - 1]; 
            }
        }*/

        public ValidatorState State
        {
            get
            {
                return state;
            }
        }
    }
}
