using UnityEngine;
using TMPro;
using Helpers;


public class LoginScreen : MonoBehaviour {

    public MenuManager mm;

    public TMP_Text messageText;

    string username = "";
    string password = "";

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
    }

    public void Login()
    {
        
        //windowmanagement
        //mm.HideLogin();
        //mm.ShowMainMenu();

        Debug.Log("{ \"username\" : \""+username+ "\", \"password\" : \"" + password + "\" }");

        //APICALL
        StartCoroutine(request.Post("/user/login","{ \"username\" : \"" + username + "\", \"password\" : \"" + password + "\" }", 
            success => Debug.Log("SUCCESS" + success), 
            error => Debug.Log("ERROR" + error)));        

    }

}
