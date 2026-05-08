using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [Header("Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    private void Start()
    {
        // Get FMOD buses
        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");

        // Load saved values (default = 1)
        float master = PlayerPrefs.GetFloat("MasterVolume", .5f);
        float music = PlayerPrefs.GetFloat("MusicVolume", .5f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", .5f);

        // Apply to sliders
        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;

        // Apply to FMOD buses
        ApplyMaster(master);
        ApplyMusic(music);
        ApplySFX(sfx);

        // Hook events
        masterSlider.onValueChanged.AddListener(ApplyMaster);
        musicSlider.onValueChanged.AddListener(ApplyMusic);
        sfxSlider.onValueChanged.AddListener(ApplySFX);
    }

    public void ApplyMaster(float value)
    {
        masterBus.setVolume(value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void ApplyMusic(float value)
    {
        musicBus.setVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void ApplySFX(float value)
    {
        sfxBus.setVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
