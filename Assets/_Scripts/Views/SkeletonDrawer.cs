using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Windows.Kinect;

namespace Views
{
    public class BodyDrawer: IBodyDrawer
    {

        private GameObject skeleton;

        private GameObject boneHead;
        private GameObject boneNeck;
        private GameObject boneSpineShoulder;
        private GameObject boneSpineMid;
        private GameObject boneSpineBase;

        // Left arm
        private GameObject boneShoulderLeft;
        private GameObject boneElbowLeft;
        private GameObject boneWristLeft;
        private GameObject boneHandLeft;
        private GameObject boneHandTipLeft;
        private GameObject boneThumbLeft;

        // Right arm
        private GameObject boneShoulderRight;
        private GameObject boneElbowRight;
        private GameObject boneWristRight;
        private GameObject boneHandRight;
        private GameObject boneHandTipRight;
        private GameObject boneThumbRight;

        // Left leg
        private GameObject boneHipLeft;
        private GameObject boneKneeLeft;
        private GameObject boneAnkleLeft;
        private GameObject boneFootLeft;

        // Right leg
        private GameObject boneHipRight;
        private GameObject boneKneeRight;
        private GameObject boneAnkleRight;
        private GameObject boneFootRight;

        public BodyDrawer(GameObject skeleton)
        {
            this.skeleton = skeleton;

            this.skeleton.SetActive(false);

            boneHead = this.skeleton.transform.Find("Head").gameObject;
            boneNeck = this.skeleton.transform.Find("Neck").gameObject;
            boneSpineShoulder = this.skeleton.transform.Find("SpineShoulder").gameObject;
            boneSpineMid = this.skeleton.transform.Find("SpineMid").gameObject;
            boneSpineBase = this.skeleton.transform.Find("SpineBase").gameObject;

            // Left arm
            boneShoulderLeft = this.skeleton.transform.Find("ShoulderLeft").gameObject;
            boneElbowLeft = this.skeleton.transform.Find("ElbowLeft").gameObject;
            boneWristLeft = this.skeleton.transform.Find("WristLeft").gameObject;
            boneHandLeft = this.skeleton.transform.Find("HandLeft").gameObject;
            boneHandTipLeft = this.skeleton.transform.Find("HandTipLeft").gameObject;
            boneThumbLeft = this.skeleton.transform.Find("ThumbLeft").gameObject;

            // Right arm
            boneShoulderRight = this.skeleton.transform.Find("ShoulderRight").gameObject;
            boneElbowRight = this.skeleton.transform.Find("ElbowRight").gameObject;
            boneWristRight = this.skeleton.transform.Find("WristRight").gameObject;
            boneHandRight = this.skeleton.transform.Find("HandRight").gameObject;
            boneHandTipRight = this.skeleton.transform.Find("HandTipRight").gameObject;
            boneThumbRight = this.skeleton.transform.Find("ThumbRight").gameObject;

            // Left leg
            boneHipLeft = this.skeleton.transform.Find("HipLeft").gameObject;
            boneKneeLeft = this.skeleton.transform.Find("KneeLeft").gameObject;
            boneAnkleLeft = this.skeleton.transform.Find("AnkleLeft").gameObject;
            boneFootLeft = this.skeleton.transform.Find("FootLeft").gameObject;

            // Right leg
            boneHipRight = this.skeleton.transform.Find("HipRight").gameObject;
            boneKneeRight = this.skeleton.transform.Find("KneeRight").gameObject;
            boneAnkleRight = this.skeleton.transform.Find("AnkleRight").gameObject;
            boneFootRight = this.skeleton.transform.Find("FootRight").gameObject;
        }

        public void DrawSkeleton(Dictionary<JointType, Windows.Kinect.Joint> joints)
        {
            skeleton.SetActive(true);

            // Head
            DrawJoint(boneHead, joints[JointType.Head]);
            // Neck
            DrawJoint(boneNeck, joints[JointType.Neck]);
            //SpineShoulder
            DrawJoint(boneSpineShoulder, joints[JointType.SpineShoulder]);
            //SpineMid
            DrawJoint(boneSpineMid, joints[JointType.SpineMid]);
            //SpineBase
            DrawJoint(boneSpineBase, joints[JointType.SpineBase]);

            //ShoulderLeft
            DrawJoint(boneShoulderLeft, joints[JointType.ShoulderLeft]);
            //ElbowLeft
            DrawJoint(boneElbowLeft, joints[JointType.ElbowLeft]);
            //WristLeft
            DrawJoint(boneWristLeft, joints[JointType.WristLeft]);
            //HandLeft
            DrawJoint(boneHandLeft, joints[JointType.HandLeft]);
            //HandTipLeft
            DrawJoint(boneHandTipLeft, joints[JointType.HandTipLeft]);
            //ThumbLeft
            DrawJoint(boneThumbLeft, joints[JointType.ThumbLeft]);


            //ShoulderRight
            DrawJoint(boneShoulderRight, joints[JointType.ShoulderRight]);
            //ElbowRight
            DrawJoint(boneElbowRight, joints[JointType.ElbowRight]);
            //WristRight
            DrawJoint(boneWristRight, joints[JointType.WristRight]);
            //HandRight
            DrawJoint(boneHandRight, joints[JointType.HandRight]);
            //HandTipRight
            DrawJoint(boneHandTipRight, joints[JointType.HandTipRight]);
            //ThumbRight
            DrawJoint(boneThumbRight, joints[JointType.ThumbRight]);

            //HipLeft
            DrawJoint(boneHipLeft, joints[JointType.HipLeft]);
            //KneeLeft
            DrawJoint(boneKneeLeft, joints[JointType.KneeLeft]);
            //AnkleLeft
            DrawJoint(boneAnkleLeft, joints[JointType.AnkleLeft]);
            //FootLeft
            DrawJoint(boneFootLeft, joints[JointType.FootLeft]);

            //HipRight
            DrawJoint(boneHipRight, joints[JointType.HipRight]);
            //KneeRight
            DrawJoint(boneKneeRight, joints[JointType.KneeRight]);
            //AnkleRight
            DrawJoint(boneAnkleRight, joints[JointType.AnkleRight]);
            //FootRight
            DrawJoint(boneFootRight, joints[JointType.FootRight]);
        }

        public void Untracked()
        {
            skeleton.SetActive(false);
        }

        private void DrawJoint(GameObject gameObject, Windows.Kinect.Joint joint)
        {
            gameObject.transform.position =
                new Vector3(
                    joint.Position.X * 4,
                    joint.Position.Y * 4,
                    joint.Position.Z * 4);
        }

        public bool Tracked
        {
            get
            {
                return skeleton.activeSelf;
            }
        }
    }
}
