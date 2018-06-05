using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainButtonService : MonoBehaviour {

    public void ReturnToMainButton()
    {
        SceneManager.LoadScene(0);
    }
}
