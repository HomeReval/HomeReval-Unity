using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Views;
using Windows.Kinect;

public class GestureController : MonoBehaviour {

    //textObject
    public TMP_Text leftHandStateText;
    public TMP_Text rightHandStateText;
    public TMP_Text TimerText;

    //Handstates
    bool leftHandClosed = false;
    bool rightHandClosed = false;

    // Kinect imports
    private KinectSensor _sensor;
    private BodyDrawer bodyDrawer;
    private BodyFrameReader _reader;

    // Use this for initialization
    void Start () {
        _sensor = KinectSensor.GetDefault();

        // Create bodyDrawer and body from prefab
        GameObject body = (GameObject)Instantiate(Resources.Load("Prefabs/Body"));
        bodyDrawer = new BodyDrawer(body);


        if (_sensor != null)
        {

            _reader = _sensor.BodyFrameSource.OpenReader();

            if (!_sensor.IsOpen)
            {
                _sensor.Open();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        var frame = _reader.AcquireLatestFrame();

        if (frame != null)
        {
            IList<Body> _bodies = new Body[frame.BodyFrameSource.BodyCount];

            frame.GetAndRefreshBodyData(_bodies);

            // Display only first active body
            for (int i = 0; i < frame.BodyFrameSource.BodyCount; i++)
            {
                if (_bodies[i] != null)
                {

                    if (_bodies[i].IsTracked)
                    {

                        if (_bodies[i].HandLeftState == HandState.Closed)
                        {
                            leftHandClosed = true;
                            leftHandStateText.text = "Closed";
                        }
                        else if (_bodies[i].HandLeftState == HandState.Open)
                        {
                            leftHandClosed = false;
                            leftHandStateText.text = "Open";
                        }
                        if (_bodies[i].HandRightState == HandState.Closed)
                        {
                            rightHandClosed = true;
                            rightHandStateText.text = "Closed";
                        }
                        else if (_bodies[i].HandRightState == HandState.Open)
                        {
                            rightHandClosed = false;
                            rightHandStateText.text = "Open";
                        }
                        //StartCoroutine(HandGesture());

                        // Exit after first tracked body is found
                        bodyDrawer.DrawSkeleton(_bodies[i].Joints);
                        break;
                    }
                }
            }

            // Clear frame to get a new one
            frame.Dispose();
        }
    }

    //IEnumerator HandGesture()
    //{
    //    for (float interval = 0; interval < 5; interval += 0.5f)
    //    {
    //        yield return new WaitForSeconds(interval);
    //        TimerText.text = interval.ToString();
    //        if (leftHandClosed && rightHandClosed)
    //        {
    //            if (interval == 5)
    //            {
    //                if (!recording)
    //                {
    //                    OnBtnStartRecording();
    //                }
    //                else if (recording)
    //                {
    //                    OnBtnStopRecording();
    //                }
    //            }
    //        }
    //        else
    //        {
    //            HandGesture().Reset();
    //        }
    //    }

    //}
}
