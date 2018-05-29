using System;
using HomeReval.Domain;
using Windows.Kinect;
using HomeReval.Helpers;

namespace HomeReval.Services
{
    class ExerciseService : IExerciseService
    {
        public int Compare(ConvertedBody bodyJSON, ConvertedBody bodyLive)
        {
            throw new NotImplementedException();
        }

        public ConvertedBody Convert(Body body)
        {

            /*foreach (var item in Map.SpineMappings)
            {
                JointType currentType = (JointType)item.Key;
                JointType targetType = (JointType)item.Value;

                //body.Joints[currentType].;
                //body.Joints[targetType];
            }*/

            return new ConvertedBody(body);
        }
    }
}
