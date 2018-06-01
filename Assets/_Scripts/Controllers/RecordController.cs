using System.Collections;
using UnityEngine;

using Windows.Kinect;
using Newtonsoft.Json;
using System;
using HomeReval.Daos;

using Views;
using Helpers;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Controllers
{

    public class RecordController : MonoBehaviour
    {
        //textObject
        public TMP_Text leftHandStateText;
        public TMP_Text rightHandStateText;
        public TMP_Text TimerText;
        public TMP_Text TimerCountdownText;

        //Colors
        Color original = Color.red;
        Color countdown = Color.cyan;

        //Handstates
        public bool leftHandClosed = false;
        public bool rightHandClosed = false;

        // GameObjects
        public GameObject playButton;
        public GameObject stopButton;
        public GameObject scrollViewContent;
        public Slider replaySlider;

        // Kinect imports
        private KinectSensor _sensor;
        private BodyFrameReader _reader;

        // Session singleton
        private HomeRevalSession homeRevalSession;

        // Http requests
        private Request request = new Request();

        // BodyDrawer
        private IBodyDrawer bodyDrawer;

        // Replay
        private bool replay = false;
        private int current;
        private int end;
        private ExerciseRecording replayRecording;
        private System.Diagnostics.Stopwatch timer;


        // Local session
        private bool recording = false;
        private ExerciseRecording currentExerciseRecording;

        void Start()
        {
            TimerText.text = "";
            TimerCountdownText.text = "";

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

            StartCoroutine(HandGesture());
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
                                    else //if (_bodies[i].HandLeftState == HandState.Open)
                                    {
                                        leftHandClosed = false;
                                        leftHandStateText.text = "Open";
                                    }
                                    if (_bodies[i].HandRightState == HandState.Closed)
                                    {
                                        rightHandClosed = true;
                                        rightHandStateText.text = "Closed";
                                    }
                                    else //if (_bodies[i].HandRightState == HandState.Open)
                                    {
                                        rightHandClosed = false;
                                        rightHandStateText.text = "Open";
                                    }
                                    

                                    //Debug.Log("tracked : " + i);
                                    //skeletonDrawers[i].DrawSkeleton(_bodies[i]);
                                    bodyDrawer.DrawSkeleton(_bodies[i]);
                                    if (recording)
                                    {
                                        currentExerciseRecording.ExerciseFrames.Add(new ExerciseFrame
                                        {
                                            Body = _bodies[i]
                                        });
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
                    bodyDrawer.DrawSkeleton(replayRecording.ExerciseFrames[current].Body);
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
                    }
                }
            }

        }

        // Events
        

        public void OnBtnStopRecording()
        {
            Debug.Log("stopped recording button");
            if (currentExerciseRecording != null)
            {

                Debug.Log(currentExerciseRecording);
                // Save current recording
                homeRevalSession
                    .CurrentRecording
                    .ExerciseFrames = currentExerciseRecording.ExerciseFrames;


                currentExerciseRecording = null;
            }

            recording = false;
            stopButton.SetActive(false);
            playButton.SetActive(true);

            // Update scroll view
            /*UpdateScrollView(homeRevalSession
                    .CurrentRecording
                    .ExerciseRecordings);*/

            UpdateReplayView(homeRevalSession.CurrentRecording);
        }

        public void OnBtnSaveRecording()
        {
            string exerciseRecording = Convert.ToBase64String(Gzip.Compress(JsonConvert.SerializeObject(homeRevalSession.CurrentRecording.ExerciseFrames)));
            //homeRevalSession.KinectRecording = data;

            string json = "{\"name\": \"" + homeRevalSession.CurrentRecording.Name + "\", \"description\": \"" + homeRevalSession.CurrentRecording.Description + "\", \"" + homeRevalSession.CurrentRecording.Amount + "\" \"exerciseRecordings\": \"" + exerciseRecording + "\"}";

            //request.Post("homereval.ga:5000/exercise", json);

        }

        /*public void OnBtnRemoveRecording(int idx)
        {
            // Remove recording from array
            homeRevalSession
                    .CurrentRecording
                    .ExerciseRecordings.RemoveAt(idx - 1);

            // Update scroll view with new array
            UpdateScrollView(homeRevalSession
                    .CurrentRecording
                    .ExerciseRecordings);
        }*/

        /*public void OnBtnReplayRecording(int idx)
        {
            replay = true;
            current = 0;
            timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            replayRecording = homeRevalSession
                    .CurrentRecording
                    .ExerciseRecordings[idx - 1];
        }*/

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
                //replay.GetComponent<Button>().onClick.AddListener(() => OnBtnReplayRecording(repIdx));

                GameObject delete = rec.transform.Find("RemoveButton").gameObject;
                int remIdx = i;
                //delete.GetComponent<Button>().onClick.AddListener(() => OnBtnRemoveRecording(remIdx));


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

        public void UpdateReplayView(Exercise exercise)
        {
            replaySlider.
        }

        IEnumerator HandGesture()
        {
            float interval = 3.0f;
            while (true)
            {
                //lower interval
                interval -= 0.5f;
                //wait for 0.5 seconds
                yield return new WaitForSeconds(0.5f);

                //both hands closed
                if (leftHandClosed && rightHandClosed)
                {
                    //set timer text to interval
                    TimerText.color = original;
                    TimerText.text = interval.ToString() + "s";
                    //if the interval drops below 0.5 continue
                    if (interval <= 0.5f)
                    {
                        if (!recording)
                        {
                            
                            TimerText.text = "START";
                            OnBtnStartRecording();
                        }
                        else if (recording)
                        {
                            TimerText.text = "Stopped";
                            OnBtnStopRecording();
                        }
                        interval = 3.0f;
                    }

                }
                else
                {
                    interval = 3.0f;
                    TimerText.text = "";
                }
            }
        }

        IEnumerator RecordCountdown()
        {
            float interval = 3.0f;
            while (interval > 0.5f)
            {
                //lower interval
                interval -= 0.5f;
                //wait for 0.5 seconds
                yield return new WaitForSeconds(0.5f);
                //set timer text to interval
                TimerCountdownText.color = countdown;
                TimerCountdownText.text = interval.ToString() + "s";
                //if the interval drops below 0.5 continue
                if (interval <= 0.5f)
                {
                    TimerCountdownText.text = "Recording";
                    currentExerciseRecording = new ExerciseRecording();
                    recording = true;
                    stopButton.SetActive(true);
                    playButton.SetActive(false);
                    yield return new WaitForSeconds(0.5f);
                    TimerCountdownText.text = "";
                }
            }
        }


        public void OnBtnStartRecording()
        {
            // Create new recording
            Debug.Log("started recording button");
            StartCoroutine(RecordCountdown());
        }
    }
}
