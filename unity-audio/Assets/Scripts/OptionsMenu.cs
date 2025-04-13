using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public Toggle invertYToggle;
    public Button applyButton;
    public Slider bgmSlider; // 🎚️ BGM volume slider
    public Slider sfxSlider; // 🎚️ SFX volume slider
    public AudioMixer audioMixer; // 🎧 Reference to your MasterMixer

    private string previousScene;
    private const string INVERT_KEY = "InvertY";
    private const string BGM_KEY = "BGMVolume";
    private const string SFX_KEY = "SFXVolume";

    void Start()
    {
        previousScene = PlayerPrefs.GetString("LastScene", "MainMenu");

        // 🔁 Load InvertY setting
        if (PlayerPrefs.HasKey(INVERT_KEY))
            invertYToggle.isOn = PlayerPrefs.GetInt(INVERT_KEY) == 1;

        // 🎵 Load and apply saved BGM volume
        float savedBGM = PlayerPrefs.GetFloat(BGM_KEY, 1f); // Default to full
        bgmSlider.value = savedBGM;
        SetBGMVolume(savedBGM);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);

        // 🔊 Load and apply saved SFX volume
        float savedSFX = PlayerPrefs.GetFloat(SFX_KEY, 1f); // Default to full
        sfxSlider.value = savedSFX;
        SetSFXVolume(savedSFX);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // Apply button functionality
        if (applyButton != null)
            applyButton.onClick.AddListener(Apply);
        else
            Debug.LogError("❌ ERROR: Apply Button not assigned in Inspector!");

        Debug.Log($"Loaded Previous Scene: {previousScene}");
        Debug.Log($"Invert Y: {invertYToggle.isOn}, BGM: {savedBGM}, SFX: {savedSFX}");
    }

    public void SetBGMVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("BGMVolume", dB);
        PlayerPrefs.SetFloat(BGM_KEY, sliderValue);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("SFXVolume", dB);
        PlayerPrefs.SetFloat(SFX_KEY, sliderValue);
        PlayerPrefs.Save();
    }

    public void Apply()
    {
        Debug.Log("Apply Button Clicked!");
        PlayerPrefs.SetInt(INVERT_KEY, invertYToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log($"Invert Y Saved: {invertYToggle.isOn}");

        SceneManager.LoadScene(previousScene);
    }

    public void Back()
    {
        Debug.Log("Back Button Clicked! Returning to previous scene.");
        SceneManager.LoadScene(previousScene);
    }
}
