using HomeReval.Domain;
using HomeReval.Helpers;
using HomeReval.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

        private Exercise jsonExercise;

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

            homeRevalSession.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOjQsImlzcyI6IkhvbWVSZXZhbCBBUEkiLCJpYXQiOjE1MjgxMDk5MDMsImV4cCI6MTUyODExMDgwM30.kMg3xPi6FDDuocxY3Gzcd6g_C8UBmw8o4H2XaupzHuk";


            StartCoroutine(requestService.Get("/exercise/13",//"/exercise/"+ homeRevalSession.Exercises[homeRevalSession.currentExerciseIdx].Id, 
                success =>
                {
                    // Decompress response and create ExerciseRecording
                    Debug.Log(success);
                    JObject response = JObject.Parse(success);

                    string exerciseRecordingJson = Gzip.DeCompress(Convert.FromBase64String(response.GetValue("recording").ToString()));

                    ExerciseRecording exerciseRecording = JsonConvert.DeserializeObject<ExerciseRecording>(exerciseRecordingJson);
                    
                    jsonExercise = new Exercise
                    {
                        Id = Int32.Parse(response.GetValue("id").ToString()),
                        Amount = 10,
                        ExerciseRecording = exerciseRecording,
                        Description = response.GetValue("description").ToString(),
                        Name = response.GetValue("name").ToString()
                    };

                    exerciseService.StartNewExercise(jsonExercise, exampleBodyDrawer, text);
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
                                    jsonExercise
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
