using HomeReval.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Kinect;

namespace HomeReval.Domain
{
    public class ConvertedBody
    {
        public ConvertedBody(Body body){
            JointResults = new List<JointResult>();

            foreach (var item in Map.LeftArmMappings)
            {
                JointType currentType = (JointType)item.Key;
                JointType targetType = (JointType)item.Value;

                Joint currentJoint = body.Joints[currentType];
                Joint targetJoint = body.Joints[targetType];

                // Calculate distance between 2 joints
                float deltaX = targetJoint.Position.X - currentJoint.Position.X;
                float deltaY = targetJoint.Position.Y - currentJoint.Position.Y;
                float deltaZ = targetJoint.Position.Z - currentJoint.Position.Z;
                float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);

                float angle = Angle(currentJoint.Position.X, currentJoint.Position.Y, targetJoint.Position.X, targetJoint.Position.Y);

                if (currentType == JointType.ElbowLeft)
                    UnityEngine.Debug.Log("Angle: " + angle + " Distance: " + distance);
                    /*UnityEngine.Debug.Log(
                        "yaw: " + (Math.Atan((targetJoint.Position.X - currentJoint.Position.X) / (targetJoint.Position.Z - currentJoint.Position.Z)) * (180.0 / Math.PI))+
                        "pitch: "+ (Math.Atan((targetJoint.Position.Z - currentJoint.Position.Z) / (targetJoint.Position.Y - currentJoint.Position.Y)) * (180.0 / Math.PI)) +
                        "roll: "+ (Math.Atan((targetJoint.Position.Y - currentJoint.Position.Y) / (targetJoint.Position.X - currentJoint.Position.X))) *(180.0 / Math.PI));*/


                JointResults.Add(new JointResult
                    {
                        CurrentJoint = currentJoint,
                        TargetJoint = targetJoint,
                        Distance = distance,
                        Angle = angle
                    }
                );
            }

            //CheckJoints = body.Joints.Values.ToList();
            Time = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        }

        private float Angle(float ax, float ay, float bx, float by)
        {
            float deltaY = (ay - by);
            float deltaX = (bx - ax);

            return (float)(Math.Atan2(deltaY, deltaX)*(180.0 / Math.PI));
        }

        //public List<Joint> CheckJoints{ get; set; }
        public List<JointResult> JointResults { get; set; }
        public Int64 Time { get; set; }
    }
}
