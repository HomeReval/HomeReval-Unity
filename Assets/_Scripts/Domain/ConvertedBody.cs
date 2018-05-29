using HomeReval.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Kinect;

namespace HomeReval.Domain
{
    class ConvertedBody
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

                float angle = (float)Math.Acos(Math.Sqrt(currentJoint.Position.X  * currentJoint.Position.X + currentJoint.Position.Y * currentJoint.Position.Y + currentJoint.Position.Z * currentJoint.Position.Z) *
                    Math.Sqrt(targetJoint.Position.X * targetJoint.Position.X + targetJoint.Position.Y * targetJoint.Position.Y + targetJoint.Position.Z * targetJoint.Position.Z));

                if(currentType == JointType.ElbowLeft)
                    UnityEngine.Debug.Log(
                        "yaw: " + Math.Atan((targetJoint.Position.X - currentJoint.Position.X) / (targetJoint.Position.Z - currentJoint.Position.Z))+
                        "pitch: "+ Math.Atan((targetJoint.Position.Z - currentJoint.Position.Z) / (targetJoint.Position.Y - currentJoint.Position.Y))+
                        "roll: "+ Math.Atan((targetJoint.Position.Y - currentJoint.Position.Y) / (targetJoint.Position.X - currentJoint.Position.X)));
                

                JointResults.Add(new JointResult
                    {
                        CurrentJoint = currentJoint,
                        TargetJoint = targetJoint,
                        Distance = distance,
                        Angle = angle
                    }
                );
            }

            CheckJoints = body.Joints.Values.ToList();
            Time = DateTime.Now;
        }

        public List<Joint> CheckJoints{ get; set; }
        public List<JointResult> JointResults { get; set; }
        public DateTime Time { get; set; }
    }
}
