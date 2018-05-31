using HomeReval.Domain;
using HomeReval.Services;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Views;
using Windows.Kinect;

namespace Controllers
{
    public class PractiseController : MonoBehaviour
    {

        // GameObjects
        public GameObject playButton;
        public GameObject stopButton;

        // Kinect imports
        private KinectSensor _sensor;
        private BodyFrameReader _reader;

        // Get services
        private IExerciseService exerciseService = new ExerciseService();

        // Session singleton
        private HomeRevalSession homeRevalSession;

        // Get body drawer
        private IBodyDrawer bodyDrawer;

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

            // Set exercise for service temp
            string json = File.ReadAllText(@"C:\Users\Stefan\Documents\exercise.json");
            Exercise jsonExercise = JsonConvert.DeserializeObject<Exercise>(json);
            exerciseService.StartNewExercise(jsonExercise);
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

                                bodyDrawer.DrawSkeleton(_bodies[i]);
                                ExerciseScore exerciseScore = exerciseService.Check(exerciseService.Convert(_bodies[i]));

                                Debug.Log(exerciseScore.Check);

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
