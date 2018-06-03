using HomeReval.Domain;
using HomeReval.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Views;
using Windows.Kinect;

namespace Controllers
{
    public class PractiseController : MonoBehaviour
    {

        // GameObjects
        public GameObject playButton;
        public GameObject stopButton;
        public Text text;

        // Kinect imports
        private KinectSensor _sensor;
        private BodyFrameReader _reader;

        // Get services
        private IRequestService requestService = new RequestService();
        private IExerciseService exerciseService = new ExerciseService();

        // Session singleton
        private HomeRevalSession homeRevalSession;

        // Get body drawer
        private IBodyDrawer bodyDrawer;
        private IBodyDrawer exampleBodyDrawer;

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

            // Create bodyDrawer for exercise example
            GameObject bodyRed = (GameObject)Instantiate(Resources.Load("Prefabs/BodyRed"));
            exampleBodyDrawer = new BodyDrawer(bodyRed);

            // Get singleton session instance
            homeRevalSession = HomeRevalSession.Instance;

            //homeRevalSession.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOjQsImlzcyI6IkhvbWVSZXZhbCBBUEkiLCJpYXQiOjE1Mjc5NzY0NDIsImV4cCI6MTUyNzk3NzM0Mn0.DiQBSQnSJl5OrGhk4bsyaFtK0QfKRnyWqx-UHzIcJyY";

            StartCoroutine(requestService.Get("/exercise/"+ homeRevalSession.Exercises[homeRevalSession.currentExerciseIdx].Id, 
                success =>
                {
                    // Decompress response and create ExerciseRecording
                    Debug.Log(success);
                },
                error =>
                {
                    Debug.Log(error);
                }
            ));

            // Maak exercise object met recording opgehaald vanuit database

            // Set exercise for service temp
            //string json = File.ReadAllText(@"C:\Users\Stefan\Documents\exercise.json");
            //Exercise jsonExercise = JsonConvert.DeserializeObject<Exercise>(json);
            //exerciseService.StartNewExercise(jsonExercise, exampleBodyDrawer, text);
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

                                bodyDrawer.DrawSkeleton(_bodies[i].Joints);
                                ExerciseScore exerciseScore = exerciseService.Check(exerciseService.Convert(_bodies[i], 
                                    homeRevalSession
                                        .Exercises[homeRevalSession.currentExerciseIdx]
                                        .ExerciseRecording
                                        .JointMappings));

                                Debug.Log(exerciseScore.Check + " score " + exerciseScore.Score);

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

        }
    }
}
