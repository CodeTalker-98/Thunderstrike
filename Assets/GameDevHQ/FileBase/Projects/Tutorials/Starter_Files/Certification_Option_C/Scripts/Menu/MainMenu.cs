using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _optionsMenu;

    [Header("Options Menu Components")]
    [SerializeField] private Toggle _hardModeToggle;
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private Slider[] _volumeSliders;

    private float _brightness;

    private MusicManager _musicManager;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        bool isHardModeOn = (PlayerPrefs.GetInt("Hard Mode") == 1) ? true : false;
        _hardModeToggle.isOn = isHardModeOn;

        _volumeSliders[0].value = AudioManager.instance.GetMasterVolumePercent();
        _volumeSliders[1].value = AudioManager.instance.GetMusicVolumePercent();
        _volumeSliders[2].value = AudioManager.instance.GetSFXVolumePercent();

        _brightness = PlayerPrefs.GetFloat("Brightness Value", 1.0f);
        _brightnessSlider.value = _brightness;

        _musicManager = GameObject.Find("Audio Manager").GetComponent<MusicManager>();

        _musicManager.MenuMusic();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
        _musicManager.GameplayMusic();
    }

    public void OptionsMenu()
    {
        _mainMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }

    public void Menu()
    {
        _optionsMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UpdateHardMode(bool isOn)
    {
        PlayerPrefs.SetInt("Hard Mode", (isOn) ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void UpdateMasterVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    public void UpdateMusicVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }

    public void UpdateSfxVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.SFX);
    }


    public void UpdateBrightness(float value)
    {
        PlayerPrefs.SetFloat("Brightness Value", value);
        PlayerPrefs.Save();
    }
}
