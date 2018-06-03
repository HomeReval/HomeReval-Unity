using HomeReval.Validator;
using System;
using System.Collections.Generic;
using Windows.Kinect;

namespace HomeReval.Domain
{
    public class ConvertedBody
    {
        public ConvertedBody()
        {
            JointResults = new Dictionary<JointType, JointResult>();
        }

        public ConvertedBody(Body body, List<Map.Mappings> jointMappings){
            JointResults = new Dictionary<JointType, JointResult>();

            foreach (Map.Mappings mapping in jointMappings)
            {
                foreach (var item in Map.DictMappings[mapping])
                {
                    JointType currentType = item.Key;
                    JointType targetType = item.Value;

                    Joint currentJoint = body.Joints[currentType];
                    Joint targetJoint = body.Joints[targetType];

                    // Calculate distance between 2 joints
                    float deltaX = targetJoint.Position.X - currentJoint.Position.X;
                    float deltaY = targetJoint.Position.Y - currentJoint.Position.Y;
                    float deltaZ = targetJoint.Position.Z - currentJoint.Position.Z;
                    float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);

                    float angle = Angle(currentJoint.Position.X, currentJoint.Position.Y, targetJoint.Position.X, targetJoint.Position.Y);

                    JointResults.Add(currentType, new JointResult
                    {
                        CurrentJoint = currentJoint,
                        TargetJoint = targetJoint,
                        Distance = distance,
                        Angle = angle
                    });
                }
            }

            CheckJoints = body.Joints;
            Time = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        }

        private float Angle(float ax, float ay, float bx, float by)
        {
            float deltaY = (ay - by);
            float deltaX = (bx - ax);

            return (float)(Math.Atan2(deltaY, deltaX)*(180.0 / Math.PI));
        }

        public Dictionary<JointType, Joint> CheckJoints{ get; set; }
        public Dictionary<JointType, JointResult> JointResults { get; set; }
        public Int64 Time { get; set; }
    }
}
