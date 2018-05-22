using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KinectStatus : MonoBehaviour {

    public bool isConnected = false;
    public TMP_Text state;

    // Update is called once per frame
    void Update () {
        if (isConnected)
        {
            state.text = "Verbonden";
			state.color = Color.green;
        }
        if (!isConnected)
        {
            state.text = "Niet Verbonden";
			state.color = Color.red;
        }

	}
}
