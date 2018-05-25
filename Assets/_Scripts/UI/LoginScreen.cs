using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Helpers;
using Newtonsoft.Json.Linq;
using System;

public class LoginScreen : MonoBehaviour {

    public MenuManager mm;

    public TMP_Text messageText;

    string username = "";
    string password = "";

    public static bool loggedIn = false;

    private Request request = new Request();

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

        //APICALL
        var jsonObject = new JObject();
        jsonObject.Add("Username", username);
        jsonObject.Add("Password", password);

        Debug.Log(jsonObject);
        Debug.Log(jsonObject.ToString());

        //Debug.Log(request.Post("/user/login", jsonObject.ToString()));
        

    }

}
