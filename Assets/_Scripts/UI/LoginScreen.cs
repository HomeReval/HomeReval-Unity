using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginScreen : MonoBehaviour {

    public MenuManager mm;

    public TMP_Text messageText;

    string username = "";
    string password = "";

    string correctUsername = "henk";
    string correctPassword = "wachtwoord123";

    public static bool loggedIn = false;

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
        loggedIn = true;

        //windowmanagement
        mm.HideLogin();
        mm.ShowMainMenu();
        //APICALL
    }

}
