using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Windows.Kinect;
using Newtonsoft.Json;
using System.IO;

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
                //recording.Add(_bodies[0]);

                Body body = _bodies[0];

                if (body.IsTracked)
                {
                    foreach (Windows.Kinect.Joint joint in body.Joints.Values)
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

                frame.Dispose();
            }
		}
	}

    void OnApplicationQuit()
    {
        if(writer != null)
        {
            string rec = JsonConvert.SerializeObject(recording);
            writer.WriteLine(rec);
            writer.Close();
        }
    }
}
