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

public class BoneView : MonoBehaviour {

	private KinectSensor _sensor;
	private BodyFrameReader _reader;

    // Log file
    private const string path = "Assets/Resources/kinect.json";
    StreamWriter writer;

    // List of all detected bodies by kinect
    IList<Body> _bodies;
    List<Body> recording = new List<Body>();
    GameObject[] gameObjects = new GameObject[25];
    //static List<Body> trackedBodies = new List<Body>();


    void Start(){
		_sensor = KinectSensor.GetDefault();

		if (_sensor != null) {

			_reader = _sensor.BodyFrameSource.OpenReader();

			if (!_sensor.IsOpen) {
				_sensor.Open();
			}
		}

        writer = File.AppendText(path);

        // Create gameobjects to display body
        for(int i = 0; i<gameObjects.Length; i++)
        {
            gameObjects[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //gameObjects[i].AddComponent<Rigidbody>();
            gameObjects[i].transform.position = new Vector3(0, 0, 0);
        }
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

                if (_bodies[0].IsTracked)
                {
                    foreach (Windows.Kinect.Joint joint in _bodies[0].Joints.Values)
                    {
                        if (joint.TrackingState == TrackingState.NotTracked) continue;

                        gameObjects[(int)joint.JointType].transform.position = new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
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
	}

    void OnApplicationQuit()
    {
        if(writer != null)
        {
            // Convert complete recording to JSON
            // Compress recording and convert to base 64
            string rec = Convert.ToBase64String(Zip(JsonConvert.SerializeObject(recording)));

            string json = "{\"name\": \"test\", \"description\": \"test\", \"exerciseRecordings\": [{\"recording\": \"" + rec+ "\"}]}";

            Debug.Log(json);

            StartCoroutine(postRequest("http://localhost:8000/exercises", json));
            //writer.WriteLine(rec);
            writer.Close();
        }
    }

    private IEnumerator postRequest(string url, string json)
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
    }

}
