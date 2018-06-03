using UnityEngine;
using TMPro;
using Newtonsoft.Json.Linq;
using HomeReval.Services;

public class LoginScreen : MonoBehaviour {

    public MenuManager mm;

    public TMP_Text messageText;

    string username = "nickwindt@hotmail.nl";
    string password = "password";

    public static bool loggedIn = false;

    private IRequestService requestService = new RequestService();

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
        StartCoroutine(requestService.Post("/user/login","{ \"username\" : \"" + username + "\", \"password\" : \"" + password + "\" }", 
            success => {
                Debug.Log(success);
                JObject response = JObject.Parse(success);
                HomeRevalSession hrs = HomeRevalSession.Instance;
                hrs.Token = response.GetValue("accessToken").ToString();
                hrs.RefreshToken = response.GetValue("refreshToken").ToString();

                StartCoroutine(requestService.Get("/user",
                successUser =>
                {
                    Debug.Log(successUser);
                    JObject responseUser = JObject.Parse(successUser);
                    hrs.UserID = (int)responseUser.GetValue("id");

                    // Go to menu
                    mm.HideLogin();
                    mm.ShowMainMenu();
                },
                errorUser =>
                {
                    Debug.Log(errorUser);
                }
                ));
            }, 
            error => {
                Debug.Log(error);
            }
         ));      
    }

}
