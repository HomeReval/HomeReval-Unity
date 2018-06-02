using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views;
using Windows.Kinect;

using HomeReval.Helpers;
using HomeReval.Domain;
using HomeReval.Services;
using Newtonsoft.Json.Linq;

namespace Controllers
{

    public class RecordController : MonoBehaviour
    {
        public enum RecordState { KinectDisplaying, KinectRecording, ReplayPlaying, ReplayPaused };

        //textObject
        public TMP_Text leftHandStateText;
        public TMP_Text rightHandStateText;
        public TMP_Text TimerText;
        public TMP_Text TimerCountdownText;

        // Slider
        public TMP_Text frameText;
        public Slider frameSlider;

        //Colors
        Color original = Color.red;
        Color countdown = Color.cyan;

        //Handstates
        public bool leftHandClosed = false;
        public bool rightHandClosed = false;

        // GameObjects
        public GameObject startRecordingButton;
        public GameObject stopRecordingButton;
        public GameObject playReplayButton;
        public GameObject pauseReplayButton;
        public GameObject stopReplayButton;
        public GameObject removeButton;
        public GameObject cutButton;
        //public GameObject scrollViewContent;
        public Slider replaySlider;

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
        private int frame;
        private int end;
        private System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();


        // Local session
        private RecordState state = RecordState.KinectDisplaying;
        //private bool recording = false;
        //private List<ExerciseFrame> exerciseRecordingFrames; // = new List<ExerciseFrame>();
        private List<ConvertedBody> exerciseConvertedBodies;

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


            // Temp for creating recording
            if (homeRevalSession.CurrentRecording == null)
            {
                homeRevalSession.CurrentRecording = new Exercise
                {
                    /*StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(20),*/
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
                switch(state)
                {
                    case RecordState.KinectDisplaying: case RecordState.KinectRecording:
                        var kframe = _reader.AcquireLatestFrame();

                        if (kframe != null)
                        {
                            IList<Body> _bodies = new Body[kframe.BodyFrameSource.BodyCount];

                            kframe.GetAndRefreshBodyData(_bodies);

                            // Display only first active body
                            for (int i = 0; i < kframe.BodyFrameSource.BodyCount; i++)
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
                                        bodyDrawer.DrawSkeleton(_bodies[i].Joints);
                                        if (state == RecordState.KinectRecording)
                                        {
                                            exerciseConvertedBodies.Add(exerciseService.Convert(_bodies[i]));
                                            /*exerciseRecordingFrames.Add(new ExerciseFrame
                                            {
                                                Body = _bodies[i]
                                            });*/
                                        }

                                        // Exit after first tracked body is found
                                        break;
                                    }
                                }
                            }

                            // Disable untracked body
                            for (int i = 0; i < kframe.BodyFrameSource.BodyCount; i++)
                            {
                                if (!_bodies[i].IsTracked && bodyDrawer.Tracked)
                                {
                                    //bodyDrawer.Untracked();
                                }
                            }

                            // Clear frame to get a new one
                            kframe.Dispose();
                        }

                        break;

                    case RecordState.ReplayPlaying:

                        bodyDrawer.DrawSkeleton(homeRevalSession.CurrentRecording.ConvertedBodies[frame].CheckJoints);
                        if (timer.ElapsedMilliseconds > 33)
                        {
                            frame++;

                            if (frame >= homeRevalSession.CurrentRecording.ConvertedBodies.Count - 1)
                            {
                                frame = 0;
                                state = RecordState.ReplayPaused;

                                // Update view
                                pauseReplayButton.SetActive(false);
                                playReplayButton.SetActive(true);
                            }


                            timer.Reset();
                            timer.Start();

                            // Update view
                            frameText.text = "frame " + (frame + 1).ToString() + "/" + (homeRevalSession.CurrentRecording.ConvertedBodies.Count);
                            replaySlider.value = frame;
                        }

                        break;

                    case RecordState.ReplayPaused:
                        bodyDrawer.DrawSkeleton(homeRevalSession.CurrentRecording.ConvertedBodies[frame].CheckJoints);
                        break;
                }
            }
        }

        // Events
        public void UpdateReplayView()
        {
            replaySlider.maxValue = homeRevalSession.CurrentRecording.ConvertedBodies.Count-1;
            frameText.text = "frame 1/" + (homeRevalSession.CurrentRecording.ConvertedBodies.Count);
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
                        if (state == RecordState.KinectDisplaying)
                        {
                            
                            TimerText.text = "START";
                            OnBtnStartRecording();
                        }
                        else if (state == RecordState.KinectRecording)
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

                    exerciseConvertedBodies = new List<ConvertedBody>();
                    //exerciseRecordingFrames = new List<ExerciseFrame>();
                    state = RecordState.KinectRecording;
                    yield return new WaitForSeconds(0.5f);
                    TimerCountdownText.text = "";
                }
            }
        }


        public void OnBtnStartRecording()
        {
            // Create new recording
            stopRecordingButton.SetActive(true);
            startRecordingButton.SetActive(false);

            StartCoroutine(RecordCountdown());
        }

        public void OnBtnStopRecording()
        {
            if (exerciseConvertedBodies.Count > 0)
            {

                // Save current recording
                homeRevalSession
                    .CurrentRecording
                    .ConvertedBodies = exerciseConvertedBodies;


                // Update replay view
                UpdateReplayView();
            }

            exerciseConvertedBodies = null;

            state = RecordState.KinectDisplaying;
            RecordView();
        }

        // Save and send to API
        public void OnBtnSaveRecording()
        {
            string convertedBodiesCompressed = Convert.ToBase64String(Gzip.Compress(JsonConvert.SerializeObject(homeRevalSession.CurrentRecording.ConvertedBodies)));

            JObject exerciseJson = new JObject(
                new JProperty("name", homeRevalSession.CurrentRecording.Name),
                new JProperty("description", homeRevalSession.CurrentRecording.Description),
                new JProperty("recording", convertedBodiesCompressed));


            /*string exercisePlanning = "{\"startDate\":\""+ homeRevalSession.CurrentRecording.StartDate.ToLongDateString() + "\", " +
                "\"endDate\":\"" + homeRevalSession.CurrentRecording.EndDate.ToLongDateString() + "\"" +
                "\"description\":\"Geen idee hebben we nog niet.\"" +
                "\"amount\":" + homeRevalSession.CurrentRecording.Amount + "}";*/

            Debug.Log("request begint nu!");
            StartCoroutine(requestService.Post("/exercise", exerciseJson.ToString(), success => {
                Debug.Log(success);
                //JObject response = JObject.Parse(success);
                //Debug.Log(response.ToString());
            },
            error => {
                Debug.Log(error);
            }));

        }

        // Replay
        public void OnSliderUpdate(float frame)
        {
            this.frame = (int)frame;

            frameText.text = "frame " + (frame + 1).ToString() + "/" + (homeRevalSession.CurrentRecording.ConvertedBodies.Count);
            replaySlider.maxValue = homeRevalSession.CurrentRecording.ConvertedBodies.Count;
            replaySlider.value = frame;
        }

        public void OnBtnPlayReplay()
        {
            if (homeRevalSession.CurrentRecording.ConvertedBodies == null || homeRevalSession.CurrentRecording.ConvertedBodies.Count == 0) return;

            timer.Start();

            // Show right buttons
            state = RecordState.ReplayPlaying;
            ReplayView();
        }

        public void OnBtnPauseReplay()
        {
            state = RecordState.ReplayPaused;
            /*frame = 0;
            timer = new System.Diagnostics.Stopwatch();
            timer.Start();*/

            pauseReplayButton.SetActive(false);
            playReplayButton.SetActive(true);
        }

        public void OnBtnCutReplay()
        {
            if ((homeRevalSession.CurrentRecording.ConvertedBodies == null || homeRevalSession.CurrentRecording.ConvertedBodies.Count == 0) ||
                 (homeRevalSession.CurrentRecording.ConvertedBodies.Count <= frame)) return;

            homeRevalSession.CurrentRecording.ConvertedBodies = homeRevalSession.CurrentRecording.ConvertedBodies.GetRange(0, frame);
            frame = 0;

            // Update view
            frameText.text = "frame " + (frame + 1).ToString() + "/" + (homeRevalSession.CurrentRecording.ConvertedBodies.Count);
            replaySlider.maxValue = homeRevalSession.CurrentRecording.ConvertedBodies.Count;
            replaySlider.value = frame;

        }

        public void OnBtnStopReplay()
        {
            

            frame = 0;
            timer.Reset();
            timer.Stop();

            state = RecordState.KinectDisplaying;
            RecordView();
        }

        // View
        private void ReplayView()
        {
            startRecordingButton.SetActive(false);
            pauseReplayButton.SetActive(true);
            playReplayButton.SetActive(false);
            stopReplayButton.SetActive(true);
            //removeButton.SetActive(true);
            cutButton.SetActive(true);
        }

        private void RecordView() {
            startRecordingButton.SetActive(true);
            pauseReplayButton.SetActive(false);
            playReplayButton.SetActive(true);
            stopReplayButton.SetActive(false);
            startRecordingButton.SetActive(true);
            stopRecordingButton.SetActive(false);
            //removeButton.SetActive(false);
            cutButton.SetActive(false);
        }
        
        
    }
}
