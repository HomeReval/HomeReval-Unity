using UnityEngine;
using TMPro;
using Helpers;
using Newtonsoft.Json.Linq;

public class LoginScreen : MonoBehaviour {

    public MenuManager mm;

    public TMP_Text messageText;

    string username = "nickwindt@hotmail.nl";
    string password = "password";

    public static bool loggedIn = false;

    private Request request = new Request();

    void Awake()
    {
        request.AddPostCompletedHandler(response => Debug.Log(response));
        request.AddPostErrorHandler(response => Debug.Log(response));
    }

    public void UserTextChanged(string input)
    {
        username = input;
    }

    public void PassTextChanged(string input)
    {
        password = input;
        Debug.Log(password);
    }

    public void Login()
    {



        //windowmanagement
        //mm.HideLogin();
        //mm.ShowMainMenu();

        Debug.Log("{ \"username\" : \""+username+ "\", \"password\" : \"" + password + "\" }");

        //APICALL
        StartCoroutine(request.Post("/user/login","{ \"username\" : \"" + username + "\", \"password\" : \"" + password + "\" }", 
            success => {
                Debug.Log(success);
                JObject response = JObject.Parse(success);
                HomeRevalSession hrs = HomeRevalSession.Instance;
                hrs.Token = response.GetValue("accessToken").ToString();
                hrs.RefreshToken = response.GetValue("refreshToken").ToString();

                // Go to menu
                mm.HideLogin();
                mm.ShowMainMenu();
            }, 
            error => {
                Debug.Log(error);
            }
         ));      
    }

}
