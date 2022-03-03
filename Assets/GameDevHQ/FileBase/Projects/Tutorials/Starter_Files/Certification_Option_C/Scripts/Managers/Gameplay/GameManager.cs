using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _waveInfoScreen;
  
    [Header("Debug")]
    [SerializeField] private int _waveNumber = 0;
    [SerializeField] private float _brightness;

    private bool _checkpointReached = false;
    private bool _levelComplete = false;

    private int _highScore = 0;

    private Light _directionalLight;

    public bool isHardModeOn { get; private set; }

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;

            isHardModeOn = (PlayerPrefs.GetInt("Hard Mode") == 1 ? true : false);
            _brightness = PlayerPrefs.GetFloat("Brightness Value", 1.0f);
            //get high score
        }
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        LightSetup();
    }

    private void LightSetup()
    {
        _directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();

        if (_directionalLight != null)
        {
            _directionalLight.intensity = _brightness;
        }
    }

    public void GameOverScreen()
    {
        _gameOverScreen.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        _pauseScreen.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        _pauseScreen.SetActive(false);
    }

    public void Retry()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void RetryCheckpoint()
    {
        if (_checkpointReached)
        {
            _waveNumber = 9;
        }
    }
}