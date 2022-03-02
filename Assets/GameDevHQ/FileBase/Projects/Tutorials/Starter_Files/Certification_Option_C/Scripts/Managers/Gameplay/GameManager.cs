using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private int _waveNumber = 0;
    [SerializeField] private float _brightness;

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
}
