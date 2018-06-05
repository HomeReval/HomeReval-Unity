using HomeReval.Domain;
using HomeReval.Helpers;
using HomeReval.Services;
using HomeReval.Validator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Views;
using Windows.Kinect;

namespace Controllers
{
    public class PractiseController : MonoBehaviour
    {
        public enum PractiseState { KinectPaused, KinectChecking, KinectDone };

        // GameObjects
        public GameObject playButton;
        public GameObject stopButton;

        // State
        private PractiseState state;

        // Kinect imports
        private KinectSensor _sensor;
        private BodyFrameReader _reader;

        // Get services
        private IRequestService requestService = new RequestService();
        private IExerciseService exerciseService = new ExerciseService();

        // Session singleton
        private HomeRevalSession hrs;

        // Get body drawer
        private IBodyDrawer bodyDrawer;
        private IBodyDrawer exampleBodyDrawer;

        private List<ConvertedBody> exerciseResultRecording;
        private List<ExerciseScore> exerciseResultScores;

        private Exercise jsonExercise;

        // Score display
        string baseScoreText = "Klaar\nScore: ";
        public TMP_Text ScoreText;

        //Completed View
        public GameObject CompletedOverlay;

        //Progress display
        public TMP_Text ProgressText;

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

            state = PractiseState.KinectPaused;

            // Create bodyDrawer and body from prefab
            GameObject body = (GameObject)Instantiate(Resources.Load("Prefabs/Body"));
            bodyDrawer = new BodyDrawer(body);

            // Create bodyDrawer for exercise example
            GameObject bodyRed = (GameObject)Instantiate(Resources.Load("Prefabs/BodyRed"));
            exampleBodyDrawer = new BodyDrawer(bodyRed);

            // Get singleton session instance
            hrs = HomeRevalSession.Instance;            

            StartCoroutine(requestService.Get("/exercise/"+ hrs.Exercises[hrs.currentExerciseIdx].Id, 
                success =>
                {
                    // Decompress response and create ExerciseRecording
                    Debug.Log(success);
                    JObject response = JObject.Parse(success);

                    Debug.Log("Recording: " + response.GetValue("recording").ToString());

                    string exerciseRecordingJson = Gzip.DeCompress(Convert.FromBase64String(response.GetValue("recording").ToString()));

                    ExerciseRecording exerciseRecording = JsonConvert.DeserializeObject<ExerciseRecording>(exerciseRecordingJson);
                    
                    jsonExercise = new Exercise
                    {
                        Id = Int32.Parse(response.GetValue("id").ToString()),
                        Amount = hrs.Exercises[hrs.currentExerciseIdx].Amount,
                        ExerciseRecording = exerciseRecording,
                        Description = response.GetValue("description").ToString(),
                        Name = response.GetValue("name").ToString()
                    };

                    exerciseService.StartNewExercise(jsonExercise, exampleBodyDrawer);

                    exerciseResultRecording = new List<ConvertedBody>();
                    exerciseResultScores = new List<ExerciseScore>();
                },
                error =>
                {
                    Debug.Log(error);
                }
            ));
            Debug.Log(hrs.Exercises[hrs.currentExerciseIdx].PlanningId);
            hrs.currentPlanningId = hrs.Exercises[hrs.currentExerciseIdx].PlanningId;

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

                                if (state == PractiseState.KinectChecking)
                                {
                                    ConvertedBody convertedBody = exerciseService.Convert(_bodies[i],
                                        jsonExercise
                                            .ExerciseRecording
                                            .JointMappings);

                                    ProgressText.text = exerciseService.Progression();

                                    ExerciseScore score = exerciseService.Check(convertedBody);

                                    if (exerciseService.State() == ExerciseValidator.ValidatorState.Checking)
                                    {
                                        // Check body and add score to list
                                        exerciseResultScores.Add(score);

                                        // Add body to list for recording
                                        exerciseResultRecording.Add(convertedBody);
                                    }

                                    if (exerciseService.State() == ExerciseValidator.ValidatorState.Done)
                                    {
                                        CompletedOverlay.SetActive(true);

                                        int endScore = 0;
                                        int total = 0;
  

                                        for (int j = 0; j < exerciseResultScores.Count; j++)
                                        {
                                            total += exerciseResultScores[j].Score;
                                        }

                                        endScore = (int)Math.Round(((float)total / (float)exerciseResultScores.Count) * 25);

                                        string exerciseResultRecordingCompressed = Convert.ToBase64String(Gzip.Compress(JsonConvert.SerializeObject(exerciseResultRecording)));

                                        JObject resultJson = new JObject(
                                            new JProperty("duration", 0),
                                            new JProperty("score", endScore),
                                            new JProperty("exercisePlanning_ID", hrs.currentPlanningId),
                                            new JProperty("result", exerciseResultRecordingCompressed));

                                        ScoreText.text = baseScoreText + endScore + "%";

                                        StartCoroutine(
                                            requestService.Post("/exerciseresult", resultJson.ToString(), success =>
                                            {
                                                Debug.Log(success);
                                            },
                                            error =>
                                            {
                                                Debug.Log(error);
                                            }
                                            ));

                                        state = PractiseState.KinectDone;
                                    }
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

        }

        public void OnBtnStart()
        {
            state = PractiseState.KinectChecking;
        }

    }
}
