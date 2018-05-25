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
using HomeReval.Daos;

using Views;
using Helpers;

namespace Controllers
{

    public class RecordController : MonoBehaviour
    {
        // GameObjects
        public GameObject playButton;
        public GameObject stopButton;

        // Kinect imports
        private KinectSensor _sensor;
        private BodyFrameReader _reader;

        // Session singleton
        private HomeRevalSession homeRevalSession;

        // Http requests
        private Request request = new Request();

        // BodyDrawer
        private IBodyDrawer bodyDrawer;

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
            bodyDrawer = new SkeletonDrawer(body);

            // Get singleton session instance
            homeRevalSession = HomeRevalSession.Instance;
        }

        void FixedUpdate()
        {
            if (_reader != null)
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
                            bodyDrawer.Untracked();
                        }
                    }

                    // Clear frame to get a new one
                    frame.Dispose();
                }
            }

        }

        // Events
        public void OnBtnStartRecording()
        {
            // Create new recording
            currentExerciseRecording = new ExerciseRecording();

            recording = true;
            stopButton.SetActive(true);
            playButton.SetActive(false);
        }

        public void OnBtnStopRecording()
        {
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
        }

        public void OnBtnSaveRecording()
        {
            string exerciseRecording = Convert.ToBase64String(Gzip.Compress(JsonConvert.SerializeObject(homeRevalSession.CurrentRecording.ExerciseRecordings)));
            //homeRevalSession.KinectRecording = data;

            string json = "{\"name\": \"" + homeRevalSession.CurrentRecording.Name + "\", \"description\": \"" + homeRevalSession.CurrentRecording.Description + "\", \"" + homeRevalSession.CurrentRecording.Amount + "\" \"exerciseRecordings\": \"" + exerciseRecording + "\"}";

            System.IO.File.WriteAllText(@"C:\Users\Stefan\Documents\School\ProjectB\exercise.json", json);
            Debug.Log(json);
            //request.Post("homereval.ga:5000/exercise", json);

        }



    }
}
