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

    [Header("Wave Info")]
    [SerializeField] private string[] _waveInfo;
    [SerializeField] private float _waveScreenTime = 5.0f;


    [Header("Debug")]
    [SerializeField] private int _waveNumber = 0;
    [SerializeField] private float _brightness;

    private bool _nextWave = false;
    private bool _checkpointReached = false;
    private bool _levelComplete = false;

    private int _score;
    private int _highScore = 0;

    private Light _directionalLight;

    private UIManager _uiManager;

    private WaitForSeconds _waveInfoScreenTime;

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
            _highScore = PlayerPrefs.GetInt("Highscore", 0);
            _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
            _nextWave = true;
        }
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        LightSetup();
        _waveInfoScreenTime = new WaitForSeconds(_waveScreenTime);
    }

    private void LightSetup()
    {
        _directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();

        if (_directionalLight != null)
        {
            _directionalLight.intensity = _brightness;
        }
    }

    private void Update()
    {
        if (_nextWave)
        {
            NextWave();
        }

        if (_waveNumber == 9)
        {
            PlayerPrefs.SetInt("Score", _score);
            PlayerPrefs.Save();
        }

        if (_waveNumber == 16)
        {
            _levelComplete = true;
            _winScreen.SetActive(true);
        }

        if (_levelComplete)
        {
            if (_score > _highScore)
            {
                _highScore = _score;
                PlayerPrefs.SetInt("Highscore", _highScore);
                PlayerPrefs.Save();
            }

            _uiManager.DisplayFinalScore(_score, _highScore);

        }
    }

    public void GameOverScreen()
    {
        _gameOverScreen.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1.0f;
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
            //Send wave to spawn manager to load it
            _score = PlayerPrefs.GetInt("Score", 0);
        }
    }

    public void NextWave()
    {
        if (_waveNumber < 15)
        {
            //pause spawn manager
            _waveNumber++;
            _uiManager.UpdateWaveInfo();
            //_nextWave = true;

            if (_nextWave)
            {
                _nextWave = false;
                StartCoroutine(ShowWaveInfoScreen());
            }
        }
    }

    public int SendWaveNumber()
    {
        return _waveNumber;
    }

    public string SendWaveInfo(int index)
    {
        if (index > 0 && index < 16)
        {
            return _waveInfo[index - 1];
        }

        return null;
    }

    public void ManualWaveNumberIncrement()
    {
        _waveNumber++; // Call on final boss death
    }

    IEnumerator ShowWaveInfoScreen()
    {
        _waveInfoScreen.SetActive(true);
        yield return _waveInfoScreenTime;
        _waveInfoScreen.SetActive(false);
        //Resume Spawn Manager
    }
}