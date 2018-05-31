using System;
using HomeReval.Domain;
using Windows.Kinect;
using HomeReval.Helpers;
using System.Collections.Generic;

namespace HomeReval.Services
{
    class ExerciseService : IExerciseService
    {
        double marge = 5.0;
        double middleMarge = 10.0;
        double errorMarge = 15.0;

        public ExerciseScore Compare(ConvertedBody bodyJSON, ConvertedBody bodyLive)
        {
            ExerciseScore exerciseScore = new ExerciseScore() { Check = true, Score = 4 };

            foreach (var item in Map.LeftArmMappings)
            {
                JointType currentType = (JointType)item.Key;
                JointType targetType = (JointType)item.Value;

                double verschil = (bodyJSON.JointResults[currentType].Angle - bodyLive.JointResults[currentType].Angle);

                if (verschil <= (Math.Abs(marge) * -1) || verschil >= marge)
                {
                    exerciseScore.Check = false;
                }

                if (verschil > (Math.Abs(marge) * -1) && verschil < marge)
                {
                    if(!(exerciseScore.Score < 4))
                        exerciseScore.Score = 4;
                }
                else if (verschil > (Math.Abs(middleMarge) * -1) && verschil < middleMarge)
                {
                    if (!(exerciseScore.Score < 3))
                        exerciseScore.Score = 3;
                }
                else if (verschil > (Math.Abs(errorMarge) * -1) && verschil < errorMarge)
                {
                    if (!(exerciseScore.Score < 2))
                        exerciseScore.Score = 2;
                }
                else
                {
                    if (!(exerciseScore.Score < 1))
                        exerciseScore.Score = 1;

                    //break;
                }
            }

            //if (exerciseScore.Check)
                //UnityEngine.Debug.Log("GOED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            //UnityEngine.Debug.Log("live angle: " + bodyLive.JointResults[JointType.ElbowLeft].Angle + " json angle: " + bodyJSON.JointResults[JointType.ElbowLeft].Angle + " Verschil: " + (bodyJSON.JointResults[JointType.ElbowLeft].Angle - bodyLive.JointResults[JointType.ElbowLeft].Angle));        

            return exerciseScore;
        }

        public ConvertedBody Convert(Body body)
        {
            return new ConvertedBody(body);
        }
    }
}
