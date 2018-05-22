using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extras : MonoBehaviour {

    public MenuManager mm;

    public void LogOut()
    {
        LoginScreen.loggedIn = false;
        mm.ShowLogin();
        mm.HideMainMenu();
    }

}
