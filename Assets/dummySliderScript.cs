using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class dummySliderScript : MonoBehaviour {

    public TMP_Text frameText;
    public Slider frameSlider;
    public int maxFrame = 80;

    private void Start()
    {
        frameSlider.maxValue = maxFrame;
        frameSlider.value = maxFrame;
    }

    public void OnSliderEdit(float input)
    {
        frameText.text = "frame " + input.ToString() + "/" + maxFrame.ToString();
    }
	
}
