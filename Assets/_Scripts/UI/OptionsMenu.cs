using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour {

    public MenuManager mm;

    public AudioMixer audioMixer;
    public TMP_Text volumePercenentage;
	public float storedVolume;
	public Slider volumeSlider;

	public void Awake(){
		storedVolume = PlayerPrefs.GetFloat ("volume",56);
		volumeSlider.value = storedVolume;
	}

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        float vol = volume / 20 * 100;
        volumePercenentage.text = vol.ToString("0") + "%";
		PlayerPrefs.SetFloat ("volume", volume);
    }

    public void ReturnButtonPressed()
    {
        mm.HideOptions();
        mm.ShowMainMenu();
    }
}
