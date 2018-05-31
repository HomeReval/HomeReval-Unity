using System.Collections;
using UnityEngine;
using Windows.Kinect;
using Newtonsoft.Json;
using System;
using Views;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

using HomeReval.Helpers;
using HomeReval.Domain;
using HomeReval.Services;

namespace Controllers
{

    public class RecordController : MonoBehaviour
    {
        //textObject
        public TMP_Text leftHandStateText;
        public TMP_Text rightHandStateText;
        public TMP_Text TimerText;

        //Handstates
        bool leftHandClosed = false;
        bool rightHandClosed = false;

        // GameObjects
        public GameObject playButton;
        public GameObject stopButton;
        public GameObject scrollViewContent;

        // Kinect imports
        private KinectSensor _sensor;
        private BodyFrameReader _reader;

        // Session singleton
        private HomeRevalSession homeRevalSession;

        // Services
        private IRequestService requestService = new RequestService();
        private IExerciseService exerciseService = new ExerciseService();

        // BodyDrawer
        private IBodyDrawer bodyDrawer;

        // Replay
        private bool replay = false;
        private int current;
        private int end;
        private ExerciseRecording replayRecording;
        private System.Diagnostics.Stopwatch timer;

        // Temp
        private ConvertedBody convertedBody;


        // Local session
        private bool recording = false;
        private ExerciseRecording currentExerciseRecording;

        void Start()
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {

                _reader = _sensor.BodyFrameSource.OpenReader();

                if (!_sensor.IsOpen)
                {
                    _sensor.Open();
                }
            }

            // Create bodyDrawer and body from prefab
            GameObject body = (GameObject)Instantiate(Resources.Load("Prefabs/Body"));
            bodyDrawer = new BodyDrawer(body);

            // Get singleton session instance
            homeRevalSession = HomeRevalSession.Instance;

            if (homeRevalSession.CurrentRecording == null)
            {
                homeRevalSession.CurrentRecording = new Exercise
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(20),
                    Amount = 5,
                    Name = "test",
                    Description = "test desc"

                };
            }

            // Temp
            string json = "{\"CheckJoints\":{\"SpineBase\":{\"JointType\":0,\"Position\":{\"X\":0.1268492,\"Y\":0.0003032145,\"Z\":1.55446684},\"TrackingState\":2},\"SpineMid\":{\"JointType\":1,\"Position\":{\"X\":0.143317968,\"Y\":0.342211,\"Z\":1.580918},\"TrackingState\":2},\"Neck\":{\"JointType\":2,\"Position\":{\"X\":0.157608911,\"Y\":0.6636707,\"Z\":1.59252632},\"TrackingState\":2},\"Head\":{\"JointType\":3,\"Position\":{\"X\":0.144495845,\"Y\":0.795747161,\"Z\":1.58098686},\"TrackingState\":2},\"ShoulderLeft\":{\"JointType\":4,\"Position\":{\"X\":-0.0597087,\"Y\":0.5340935,\"Z\":1.63624334},\"TrackingState\":2},\"ElbowLeft\":{\"JointType\":5,\"Position\":{\"X\":-0.260287374,\"Y\":0.5430984,\"Z\":1.62317121},\"TrackingState\":2},\"WristLeft\":{\"JointType\":6,\"Position\":{\"X\":-0.270222068,\"Y\":0.7277846,\"Z\":1.55055237},\"TrackingState\":2},\"HandLeft\":{\"JointType\":7,\"Position\":{\"X\":-0.261549532,\"Y\":0.787828445,\"Z\":1.53379178},\"TrackingState\":2},\"ShoulderRight\":{\"JointType\":8,\"Position\":{\"X\":0.323077142,\"Y\":0.508788049,\"Z\":1.53460455},\"TrackingState\":2},\"ElbowRight\":{\"JointType\":9,\"Position\":{\"X\":0.4001329,\"Y\":0.263967842,\"Z\":1.56393611},\"TrackingState\":2},\"WristRight\":{\"JointType\":10,\"Position\":{\"X\":0.429431617,\"Y\":0.04392934,\"Z\":1.46486247},\"TrackingState\":2},\"HandRight\":{\"JointType\":11,\"Position\":{\"X\":0.420813948,\"Y\":-0.031162167,\"Z\":1.43006539},\"TrackingState\":2},\"HipLeft\":{\"JointType\":12,\"Position\":{\"X\":0.0357785337,\"Y\":-0.0004969903,\"Z\":1.53465486},\"TrackingState\":2},\"KneeLeft\":{\"JointType\":13,\"Position\":{\"X\":0.00541542936,\"Y\":-0.30539313,\"Z\":1.50017154},\"TrackingState\":2},\"AnkleLeft\":{\"JointType\":14,\"Position\":{\"X\":-0.0135584539,\"Y\":-0.6347466,\"Z\":1.545303},\"TrackingState\":1},\"FootLeft\":{\"JointType\":15,\"Position\":{\"X\":-0.01435886,\"Y\":-0.510895669,\"Z\":1.498815},\"TrackingState\":2},\"HipRight\":{\"JointType\":16,\"Position\":{\"X\":0.211398318,\"Y\":0.00111208786,\"Z\":1.49355745},\"TrackingState\":2},\"KneeRight\":{\"JointType\":17,\"Position\":{\"X\":0.279673278,\"Y\":-0.299809128,\"Z\":1.44880223},\"TrackingState\":2},\"AnkleRight\":{\"JointType\":18,\"Position\":{\"X\":0.36529997,\"Y\":-0.613705933,\"Z\":1.51094675},\"TrackingState\":1},\"FootRight\":{\"JointType\":19,\"Position\":{\"X\":0.298645556,\"Y\":-0.5092489,\"Z\":1.45322168},\"TrackingState\":2},\"SpineShoulder\":{\"JointType\":20,\"Position\":{\"X\":0.1542677,\"Y\":0.585875869,\"Z\":1.59178972},\"TrackingState\":2},\"HandTipLeft\":{\"JointType\":21,\"Position\":{\"X\":-0.2386434,\"Y\":0.834944844,\"Z\":1.510657},\"TrackingState\":2},\"ThumbLeft\":{\"JointType\":22,\"Position\":{\"X\":-0.234568238,\"Y\":0.804093659,\"Z\":1.56182182},\"TrackingState\":2},\"HandTipRight\":{\"JointType\":23,\"Position\":{\"X\":0.412473559,\"Y\":-0.09041451,\"Z\":1.40780449},\"TrackingState\":2},\"ThumbRight\":{\"JointType\":24,\"Position\":{\"X\":0.388567567,\"Y\":-0.0415960476,\"Z\":1.39583325},\"TrackingState\":2}},\"JointResults\":{\"ShoulderLeft\":{\"CurrentJoint\":{\"JointType\":4,\"Position\":{\"X\":-0.0597087,\"Y\":0.5340935,\"Z\":1.63624334},\"TrackingState\":2},\"TargetJoint\":{\"JointType\":5,\"Position\":{\"X\":-0.260287374,\"Y\":0.5430984,\"Z\":1.62317121},\"TrackingState\":2},\"Distance\":0.2012058,\"Angle\":-177.429459},\"ElbowLeft\":{\"CurrentJoint\":{\"JointType\":5,\"Position\":{\"X\":-0.260287374,\"Y\":0.5430984,\"Z\":1.62317121},\"TrackingState\":2},\"TargetJoint\":{\"JointType\":6,\"Position\":{\"X\":-0.270222068,\"Y\":0.7277846,\"Z\":1.55055237},\"TrackingState\":2},\"Distance\":0.198698714,\"Angle\":-93.0791},\"WristLeft\":{\"CurrentJoint\":{\"JointType\":6,\"Position\":{\"X\":-0.270222068,\"Y\":0.7277846,\"Z\":1.55055237},\"TrackingState\":2},\"TargetJoint\":{\"JointType\":7,\"Position\":{\"X\":-0.261549532,\"Y\":0.787828445,\"Z\":1.53379178},\"TrackingState\":2},\"Distance\":0.06293963,\"Angle\":-81.78123},\"HandLeft\":{\"CurrentJoint\":{\"JointType\":7,\"Position\":{\"X\":-0.261549532,\"Y\":0.787828445,\"Z\":1.53379178},\"TrackingState\":2},\"TargetJoint\":{\"JointType\":6,\"Position\":{\"X\":-0.270222068,\"Y\":0.7277846,\"Z\":1.55055237},\"TrackingState\":2},\"Distance\":0.06293963,\"Angle\":98.21877}},\"Time\":1527762164042}";
            convertedBody = JsonConvert.DeserializeObject<ConvertedBody>(json);
        }

        void FixedUpdate()
        {
            if (_reader != null)
            {
                if (!replay)
                {
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
                                    StartCoroutine(HandGesture());

                                    //Debug.Log("tracked : " + i);
                                    //skeletonDrawers[i].DrawSkeleton(_bodies[i]);
                                    bodyDrawer.DrawSkeleton(_bodies[i]);
                                    ExerciseScore exerciseScore = exerciseService.Compare(convertedBody, exerciseService.Convert(_bodies[i]));

                                    Debug.Log("Check: " + exerciseScore.Check + " Score: " + exerciseScore.Score);

                                    if (recording)
                                    {
                                        currentExerciseRecording.ConvertedBodies.Add(exerciseService.Convert(_bodies[i]));
                                    }

                                    // Exit after first tracked body is found
                                    break;
                                }
                            }
                        }

                        // Disable untracked body
                        for (int i = 0; i < frame.BodyFrameSource.BodyCount; i++)
                        {
                            if (!_bodies[i].IsTracked && bodyDrawer.Tracked)
                            {
                                //bodyDrawer.Untracked();
                            }
                        }

                        // Clear frame to get a new one
                        frame.Dispose();
                    }
                }
                else
                {
                    /*bodyDrawer.DrawSkeleton(replayRecording.ConvertedBodies[current].Body);
                    if (timer.ElapsedMilliseconds > 33)
                    {
                        current++;
                        timer.Reset();
                        timer.Start();
                    }

                    if (current == replayRecording.ExerciseFrames.Count)
                    {
                        replay = false;
                        replayRecording = null;
                        timer.Stop();
                        timer = null;
                    }*/
                }
            }

        }

        // Events
        public void OnBtnStartRecording()
        {
            // Create new recording
            currentExerciseRecording = new ExerciseRecording();
            Debug.Log("started recording button");
            recording = true;
            stopButton.SetActive(true);
            playButton.SetActive(false);
        }

        public void OnBtnStopRecording()
        {
            Debug.Log("stopped recording button");
            if (currentExerciseRecording != null)
            {

                Debug.Log(currentExerciseRecording);
                // Save current recording
                homeRevalSession
                    .CurrentRecording
                    .ExerciseRecordings
                    .Add(currentExerciseRecording);

                currentExerciseRecording = null;
            }

            recording = false;
            stopButton.SetActive(false);
            playButton.SetActive(true);

            // Update scroll view
            UpdateScrollView(homeRevalSession
                    .CurrentRecording
                    .ExerciseRecordings);
        }

        public void OnBtnSaveRecording()
        {
            string exerciseRecording = Convert.ToBase64String(Gzip.Compress(JsonConvert.SerializeObject(homeRevalSession.CurrentRecording.ExerciseRecordings)));
            //homeRevalSession.KinectRecording = data;

            string json = "{\"name\": \"" + homeRevalSession.CurrentRecording.Name + "\", \"description\": \"" + homeRevalSession.CurrentRecording.Description + "\", \"" + homeRevalSession.CurrentRecording.Amount + "\" \"exerciseRecordings\": \"" + exerciseRecording + "\"}";

            System.IO.File.WriteAllText(@"C:\Users\Stefan\Documents\exercise.json", JsonConvert.SerializeObject(homeRevalSession.CurrentRecording.ExerciseRecordings));
            Debug.Log(json);
            //requestService.Post("homereval.ga:5000/exercise", json);

        }

        public void OnBtnRemoveRecording(int idx)
        {
            // Remove recording from array
            homeRevalSession
                    .CurrentRecording
                    .ExerciseRecordings.RemoveAt(idx - 1);

            // Update scroll view with new array
            UpdateScrollView(homeRevalSession
                    .CurrentRecording
                    .ExerciseRecordings);
        }

        public void OnBtnReplayRecording(int idx)
        {
            /*if (homeRevalSession
                .CurrentRecording
                .ExerciseRecordings[idx - 1]
                .ExerciseFrames.Count
                ==
                0) return;

            replay = true;
            current = 0;
            timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            replayRecording = homeRevalSession
                    .CurrentRecording
                    .ExerciseRecordings[idx - 1];*/
        }

        private void UpdateScrollView(List<ExerciseRecording> exerciseRecordings)
        {
            foreach (Transform child in scrollViewContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            for (int i = 1; i < exerciseRecordings.Count + 1; i++)
            {
                // Get prefab and create recording
                GameObject rec = (GameObject)Instantiate(Resources.Load("Prefabs/Recording"));

                // Get and set text
                GameObject gName = rec.transform.Find("Name").gameObject;
                TMP_Text name = gName.GetComponent<TMP_Text>();
                name.text = "OPNAME: " + i;

                // Set position of recording
                RectTransform rectTransform = rec.GetComponent<RectTransform>();
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, (i * 500) - 200);
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 170);

                // Set button events
                GameObject replay = rec.transform.Find("ReplayButton").gameObject;
                int repIdx = i;
                replay.GetComponent<Button>().onClick.AddListener(() => OnBtnReplayRecording(repIdx));

                GameObject delete = rec.transform.Find("RemoveButton").gameObject;
                int remIdx = i;
                delete.GetComponent<Button>().onClick.AddListener(() => OnBtnRemoveRecording(remIdx));


                // Add to content window
                rec.transform.SetParent(scrollViewContent.transform, false);
            }

            if (exerciseRecordings.Count >= 5)
            {
                // Set width of content
                RectTransform rt = scrollViewContent.GetComponent<RectTransform>();
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, exerciseRecordings.Count * 251);
            }

        }

        IEnumerator HandGesture()
        {
            for (float interval = 3; interval < 0; interval -= 0.5f)
            {
                yield return new WaitForSeconds(0.5f);
                
                if (leftHandClosed && rightHandClosed)
                {
                    TimerText.text = interval.ToString() + "s";
                    if (interval >= 2.9f)
                    {
                        if (!recording)
                        {
                            TimerText.text = "Recording";
                            OnBtnStartRecording();
                            yield return new WaitForSeconds(5.0f);

                        }
                        else if (recording)
                        {
                            TimerText.text = "Stopped";
                            OnBtnStopRecording();
                            yield return new WaitForSeconds(5.0f);
                        }
                        interval = 3f;
                    }
                }
                else if (!leftHandClosed || !rightHandClosed)
                {
                    interval = 3f;
                    TimerText.text = "";
                }
            }

        }
    }
}
