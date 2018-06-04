using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnButtonService : MonoBehaviour {

    public MenuManager mm;

    public void ReturnButtonPressed()
    {
        this.transform.parent.gameObject.SetActive(false);
        mm.ShowMainMenu();
    }
}
