using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Windows.Kinect;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Text;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class BoneView : MonoBehaviour {

	private KinectSensor _sensor;
	private BodyFrameReader _reader;

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

    // List of all detected bodies by kinect
    IList<Body> _bodies;
    List<Body> recording = new List<Body>();

    void Start(){
        _sensor = KinectSensor.GetDefault();

		if (_sensor != null) {

			_reader = _sensor.BodyFrameSource.OpenReader();

			if (!_sensor.IsOpen) {
				_sensor.Open();
			}
		}

        skeletonDrawer = new SkeletonDrawer {
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
        };
    }

	void FixedUpdate(){
		if (_reader != null) {
			var frame = _reader.AcquireLatestFrame();

            if(frame != null)
            {
                _bodies = new Body[frame.BodyFrameSource.BodyCount];

                frame.GetAndRefreshBodyData(_bodies);

                // Add body frame to recording list
                recording.Add(_bodies[0]);

                foreach (var body in _bodies)
                {
                    if (body != null)
                    {
                        if (body.IsTracked)
                        {
                            skeletonDrawer.DrawSkeleton(body);
                            //DrawSkeleton(body);
                        }
                    }
                }


                /*foreach (var body in _bodies)
                {
                    if (body != null)
                    {
                        if (body.IsTracked)
                        {
                            string jsonBody = JsonConvert.SerializeObject(body);
                            Debug.Log(jsonBody);
                            //writer.WriteLine(jsonBody);
                            //Debug.Log(body.ToString());
                            //Debug.Log(body.HandLeftState.ToString());
                            //Debug.Log(body.Appearance.ToString());
                            //foreach (Windows.Kinect.Joint joint in body.Joints.Values)
                            //{
                                //Debug.Log("Type: " + joint.GetType() + " X: " + joint.Position.X + " Y: " + joint.Position.Y + " Z: " + joint.Position.Z);
                            //}

                            //Debug.Log(" X: " + body.Joints[JointType.Head].Position.X + " Y: " + body.Joints[JointType.Head].Position.Y + " Z: " + body.Joints[JointType.Head].Position.Z);
                        }
                    }
                }*/

                // Clear frame to get a new one
                frame.Dispose();
            }
		}

        if (Input.GetKeyDown("space"))
        {
            RecordingSession.Recording = recording;
            SceneManager.LoadScene(3);
        }


    }

    /*private void DrawSkeleton(Body body)
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

    private void DrawJoint( GameObject gameObject, Windows.Kinect.Joint joint)
    {
        gameObject.transform.position =
            new Vector3(
                joint.Position.X * 4,
                joint.Position.Y * 4,
                joint.Position.Z * 4);
    }*/

    void OnApplicationQuit()
    {
        /*if(writer != null)
        {
            // Convert complete recording to JSON
            // Compress recording and convert to base 64
            string rec = Convert.ToBase64String(Zip(JsonConvert.SerializeObject(recording)));

            string json = "{\"name\": \"test\", \"description\": \"test\", \"exerciseRecordings\": [{\"recording\": \"" + rec+ "\"}]}";

            Debug.Log(json);

            StartCoroutine(postRequest("http://localhost:8000/exercises", json));
            //writer.WriteLine(rec);
            writer.Close();
        }*/
    }

    /*private IEnumerator postRequest(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Authorization", "Bearer eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJhZG1pbiIsImV4cCI6MTUyNTM0NTIwMn0.LINprnMRZeaphdaYXB8e7BldpFs0FK7XcxheTdWwqP5q-wJ3BLMTZ_bgcbyKXhYwyq6d-gMtm2jtK0DYo_GjqQ");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }


    private void CopyTo(Stream src, Stream dest)
    {
        byte[] bytes = new byte[4096];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        {
            dest.Write(bytes, 0, cnt);
        }
    }

    private byte[] Zip(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);

        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                //msi.CopyTo(gs);
                CopyTo(msi, gs);
            }

            return mso.ToArray();
        }
    }

    private string Unzip(byte[] bytes)
    {
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                //gs.CopyTo(mso);
                CopyTo(gs, mso);
            }

            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }*/



}
