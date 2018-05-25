using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Helpers;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine.Networking;

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
    }

    public void Login()
    {
        
        //windowmanagement
        //mm.HideLogin();
        //mm.ShowMainMenu();

        //APICALL
        Debug.Log("{\"username\":\"" + username + "\" \"password\":\"" + password + "\"}");
        Debug.Log(request.Post("/user/login", "{\"username\":\""+username+ "\" \"password\":\"" + password + "\"}"));


        

    }

}
