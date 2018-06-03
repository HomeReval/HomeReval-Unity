using HomeReval.Domain;
using System.Collections.Generic;

public sealed class HomeRevalSession
{
    private static volatile HomeRevalSession instance;
    private static object syncRoot = new object();



    private HomeRevalSession() { }

    public static HomeRevalSession Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new HomeRevalSession();
                }
            }

            return instance;
        }
    }

    // User Session
    public List<Exercise> Exercises { get; set; }
    public int currentExerciseIdx { get; set; }

    // Recording session
    public Exercise CurrentRecording { get; set; }

    public string Username { get; set; }
    public int UserID { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
