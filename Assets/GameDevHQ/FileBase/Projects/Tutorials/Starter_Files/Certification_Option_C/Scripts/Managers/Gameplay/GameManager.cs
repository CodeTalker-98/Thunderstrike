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
    [SerializeField] private int _currentWave;
    [SerializeField] private float _brightness;

    private bool _checkpointReached = false;

    private int _score;
    private int _highScore = 0;

    private Light _directionalLight;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;

    private WaitForSeconds _waveInfoScreenTime;

    public bool isHardModeOn { get; private set; }
    public bool levelComplete = false;
    public bool isDead = false;

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
            _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
            isDead = false;
        }
    }

    private void Start()
    {
        Init();
        NextWave();
    }

    private void Init()
    {
        LightSetup();
        _waveInfoScreenTime = new WaitForSeconds(_waveScreenTime);
        _spawnManager.GetCurrentWave();
    }

    private void LightSetup()
    {
        _directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();

        if (_directionalLight != null)
        {
            _directionalLight.intensity = _brightness;
        }
    }

    public void NextWave()
    {
        StartCoroutine(ShowWaveInfoScreen());
    }

    private void Update()
    {
        LevelStatus();
    }

    private void LevelStatus()
    {
        _currentWave = _spawnManager.GetCurrentWave(); 
    }

    public void GameOverScreen()
    {
        _gameOverScreen.SetActive(true);
    }

    public void WinScreen()
    {
        _gameOverScreen.SetActive(false);
        _winScreen.SetActive(true);
        SetScore();
    }

    private void SetScore()
    {
        if (_score > _highScore)
        {
            _highScore = _score;
            PlayerPrefs.SetInt("Highscore", _highScore);
            PlayerPrefs.Save();
        }



        _uiManager.DisplayFinalScore(_score, _highScore);
    }

    public void GetScore(int score)
    {
        _score = score;
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
        isDead = false;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void RetryCheckpoint()
    {
        if (_checkpointReached)
        {
            //Send wave to spawn manager to load it
            _score = PlayerPrefs.GetInt("Score", 0);
        }
    }

    public int SendWaveNumber()
    {
        return _currentWave;
    }

    public string SendWaveInfo(int index)
    {
        if (index >= 0 && index < 16)
        {
            return _waveInfo[index];
        }

        return null;
    }

    IEnumerator ShowWaveInfoScreen()
    {
        _spawnManager.enabled = false;
        _uiManager.UpdateWaveInfo();
        _waveInfoScreen.SetActive(true);
        yield return _waveInfoScreenTime;
        _waveInfoScreen.SetActive(false);
        _spawnManager.enabled = true;
    }
}