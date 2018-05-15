using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Windows.Kinect;

public class SkeletonDrawer {
    /*// Get bonestructure from unity
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
    private GameObject boneFootRight;*/

    public void DrawSkeleton(Body body)
    {
        if (body == null) return;

        // Head
        DrawJoint(BoneHead, body.Joints[JointType.Head]);
        // Neck
        DrawJoint(BoneNeck, body.Joints[JointType.Neck]);
        //SpineShoulder
        DrawJoint(BoneSpineShoulder, body.Joints[JointType.SpineShoulder]);
        //SpineMid
        DrawJoint(BoneSpineMid, body.Joints[JointType.SpineMid]);
        //SpineBase
        DrawJoint(BoneSpineBase, body.Joints[JointType.SpineBase]);

        //ShoulderLeft
        DrawJoint(BoneShoulderLeft, body.Joints[JointType.ShoulderLeft]);
        //ElbowLeft
        DrawJoint(BoneElbowLeft, body.Joints[JointType.ElbowLeft]);
        //WristLeft
        DrawJoint(BoneWristLeft, body.Joints[JointType.WristLeft]);
        //HandLeft
        DrawJoint(BoneHandLeft, body.Joints[JointType.HandLeft]);
        //HandTipLeft
        DrawJoint(BoneHandTipLeft, body.Joints[JointType.HandTipLeft]);
        //ThumbLeft
        DrawJoint(BoneThumbLeft, body.Joints[JointType.ThumbLeft]);


        //ShoulderRight
        DrawJoint(BoneShoulderRight, body.Joints[JointType.ShoulderRight]);
        //ElbowRight
        DrawJoint(BoneElbowRight, body.Joints[JointType.ElbowRight]);
        //WristRight
        DrawJoint(BoneWristRight, body.Joints[JointType.WristRight]);
        //HandRight
        DrawJoint(BoneHandRight, body.Joints[JointType.HandRight]);
        //HandTipRight
        DrawJoint(BoneHandTipRight, body.Joints[JointType.HandTipRight]);
        //ThumbRight
        DrawJoint(BoneThumbRight, body.Joints[JointType.ThumbRight]);

        //HipLeft
        DrawJoint(BoneHipLeft, body.Joints[JointType.HipLeft]);
        //KneeLeft
        DrawJoint(BoneKneeLeft, body.Joints[JointType.KneeLeft]);
        //AnkleLeft
        DrawJoint(BoneAnkleLeft, body.Joints[JointType.AnkleLeft]);
        //FootLeft
        DrawJoint(BoneFootLeft, body.Joints[JointType.FootLeft]);

        //HipRight
        DrawJoint(BoneHipRight, body.Joints[JointType.HipRight]);
        //KneeRight
        DrawJoint(BoneKneeRight, body.Joints[JointType.KneeRight]);
        //AnkleRight
        DrawJoint(BoneAnkleRight, body.Joints[JointType.AnkleRight]);
        //FootRight
        DrawJoint(BoneFootRight, body.Joints[JointType.FootRight]);
    }

    private void DrawJoint(GameObject gameObject, Windows.Kinect.Joint joint)
    {
        gameObject.transform.position =
            new Vector3(
                joint.Position.X * 4,
                joint.Position.Y * 4,
                joint.Position.Z * 4);
    }

    // Getters and Setters
    public GameObject BoneHead { get; set; }
    public GameObject BoneNeck { get; set; }
    public GameObject BoneSpineShoulder { get; set; }
    public GameObject BoneSpineMid { get; set; }
    public GameObject BoneSpineBase { get; set; }

    // Left arm
    public GameObject BoneShoulderLeft { get; set; }
    public GameObject BoneElbowLeft { get; set; }
    public GameObject BoneWristLeft { get; set; }
    public GameObject BoneHandLeft { get; set; }
    public GameObject BoneHandTipLeft { get; set; }
    public GameObject BoneThumbLeft { get; set; }

    // Right arm
    public GameObject BoneShoulderRight { get; set; }
    public GameObject BoneElbowRight { get; set; }
    public GameObject BoneWristRight { get; set; }
    public GameObject BoneHandRight { get; set; }
    public GameObject BoneHandTipRight { get; set; }
    public GameObject BoneThumbRight { get; set; }

    // Left leg
    public GameObject BoneHipLeft { get; set; }
    public GameObject BoneKneeLeft { get; set; }
    public GameObject BoneAnkleLeft { get; set; }
    public GameObject BoneFootLeft { get; set; }

    // Right leg
    public GameObject BoneHipRight { get; set; }
    public GameObject BoneKneeRight { get; set; }
    public GameObject BoneAnkleRight { get; set; }
    public GameObject BoneFootRight { get; set; }
}
