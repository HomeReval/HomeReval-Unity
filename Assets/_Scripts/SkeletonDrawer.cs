using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Windows.Kinect;

public class SkeletonDrawer
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

    public SkeletonDrawer(GameObject skeleton)
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

    public void DrawSkeleton(Body body)
    {
        if (body == null) return;

        skeleton.SetActive(true);

        // Head
        DrawJoint(boneHead, body.Joints[JointType.Head]);
        // Neck
        DrawJoint(boneNeck, body.Joints[JointType.Neck]);
        //SpineShoulder
        DrawJoint(boneSpineShoulder, body.Joints[JointType.SpineShoulder]);
        //SpineMid
        DrawJoint(boneSpineMid, body.Joints[JointType.SpineMid]);
        //SpineBase
        DrawJoint(boneSpineBase, body.Joints[JointType.SpineBase]);

        //ShoulderLeft
        DrawJoint(boneShoulderLeft, body.Joints[JointType.ShoulderLeft]);
        //ElbowLeft
        DrawJoint(boneElbowLeft, body.Joints[JointType.ElbowLeft]);
        //WristLeft
        DrawJoint(boneWristLeft, body.Joints[JointType.WristLeft]);
        //HandLeft
        DrawJoint(boneHandLeft, body.Joints[JointType.HandLeft]);
        //HandTipLeft
        DrawJoint(boneHandTipLeft, body.Joints[JointType.HandTipLeft]);
        //ThumbLeft
        DrawJoint(boneThumbLeft, body.Joints[JointType.ThumbLeft]);


        //ShoulderRight
        DrawJoint(boneShoulderRight, body.Joints[JointType.ShoulderRight]);
        //ElbowRight
        DrawJoint(boneElbowRight, body.Joints[JointType.ElbowRight]);
        //WristRight
        DrawJoint(boneWristRight, body.Joints[JointType.WristRight]);
        //HandRight
        DrawJoint(boneHandRight, body.Joints[JointType.HandRight]);
        //HandTipRight
        DrawJoint(boneHandTipRight, body.Joints[JointType.HandTipRight]);
        //ThumbRight
        DrawJoint(boneThumbRight, body.Joints[JointType.ThumbRight]);

        //HipLeft
        DrawJoint(boneHipLeft, body.Joints[JointType.HipLeft]);
        //KneeLeft
        DrawJoint(boneKneeLeft, body.Joints[JointType.KneeLeft]);
        //AnkleLeft
        DrawJoint(boneAnkleLeft, body.Joints[JointType.AnkleLeft]);
        //FootLeft
        DrawJoint(boneFootLeft, body.Joints[JointType.FootLeft]);

        //HipRight
        DrawJoint(boneHipRight, body.Joints[JointType.HipRight]);
        //KneeRight
        DrawJoint(boneKneeRight, body.Joints[JointType.KneeRight]);
        //AnkleRight
        DrawJoint(boneAnkleRight, body.Joints[JointType.AnkleRight]);
        //FootRight
        DrawJoint(boneFootRight, body.Joints[JointType.FootRight]);
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
