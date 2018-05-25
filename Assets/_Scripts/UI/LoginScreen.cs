using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Helpers;

public class LoginScreen : MonoBehaviour {

    public MenuManager mm;

    public TMP_Text messageText;

    string username = "";
    string password = "";

    string correctUsername = "henk";
    string correctPassword = "wachtwoord123";

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
        request.Post("/user/login", "{json}");
        loggedIn = true;

        //windowmanagement
        mm.HideLogin();
        mm.ShowMainMenu();
        //APICALL
    }

}
