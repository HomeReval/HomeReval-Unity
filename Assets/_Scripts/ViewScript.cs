using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewScript : MonoBehaviour {

    // Drawer
    SkeletonDrawer skeletonDrawer;

    // Get bonestructure from unity
    public GameObject boneHead;
    public GameObject boneNeck;
    public GameObject boneSpineShoulder;
    public GameObject boneSpineMid;
    public GameObject boneSpineBase;

    // Left arm
    public GameObject boneShoulderLeft;
    public GameObject boneElbowLeft;
    public GameObject boneWristLeft;
    public GameObject boneHandLeft;
    public GameObject boneHandTipLeft;
    public GameObject boneThumbLeft;

    // Right arm
    public GameObject boneShoulderRight;
    public GameObject boneElbowRight;
    public GameObject boneWristRight;
    public GameObject boneHandRight;
    public GameObject boneHandTipRight;
    public GameObject boneThumbRight;

    // Left leg
    public GameObject boneHipLeft;
    public GameObject boneKneeLeft;
    public GameObject boneAnkleLeft;
    public GameObject boneFootLeft;

    // Right leg
    public GameObject boneHipRight;
    public GameObject boneKneeRight;
    public GameObject boneAnkleRight;
    public GameObject boneFootRight;

    // Use this for initialization
    void Start () {
        skeletonDrawer = new SkeletonDrawer((GameObject)Instantiate(Resources.Load("Prefabs/Body")));/*
        {
            BoneHead = boneHead,
            BoneNeck = boneNeck,
            BoneSpineShoulder = boneSpineShoulder,
            BoneSpineMid = boneSpineMid,
            BoneSpineBase = boneSpineBase,

            // Left arm
            BoneShoulderLeft = boneShoulderLeft,
            BoneElbowLeft = boneElbowLeft,
            BoneWristLeft = boneWristLeft,
            BoneHandLeft = boneHandLeft,
            BoneHandTipLeft = boneHandTipLeft,
            BoneThumbLeft = boneThumbLeft,

            // Right arm
            BoneShoulderRight = boneShoulderRight,
            BoneElbowRight = boneElbowRight,
            BoneWristRight = boneWristRight,
            BoneHandRight = boneHandRight,
            BoneHandTipRight = boneHandTipRight,
            BoneThumbRight = boneThumbRight,

            // Left leg
            BoneHipLeft = boneHipLeft,
            BoneKneeLeft = boneKneeLeft,
            BoneAnkleLeft = boneAnkleLeft,
            BoneFootLeft = boneFootLeft,

            // Right leg
            BoneHipRight = boneHipRight,
            BoneKneeRight = boneKneeRight,
            BoneAnkleRight = boneAnkleRight,
            BoneFootRight = boneFootRight
        };*/
        //Debug.Log(RecordingSession.Recording.Count);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
