using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float xStart = 45.0f;
    [SerializeField] private float _enemySpawnBuffer = 1.0f;
    [SerializeField] private float _nextWaveBuffer = 10.0f;

    [Header("Waves")]
    [SerializeField] private List<Waves> _waves = new List<Waves>();

    private bool _waveStart = true;

    private int _currentWave = 0;

    private GameObject _previousWave;

    private WaitForSeconds _enemySpawnTime;
    private WaitForSeconds _nextWaveTime;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _enemySpawnTime = new WaitForSeconds(_enemySpawnBuffer);
        _nextWaveTime = new WaitForSeconds(_nextWaveBuffer);

        if (GameManager.instance != null)
        {
            if (GameManager.instance.isHardModeOn)
            {
                _enemySpawnBuffer /= 2.0f;
            }
            else
            {
                _enemySpawnBuffer /= 1.0f;
            }
        }

        StartCoroutine(StartWaveRoutine());
    }

    private void Update()
    {
        //UpdateCurrentWaveNumber();
        if (_previousWave.transform.childCount < 1)
        {
            _currentWave++;
            GameManager.instance.SetNextWave();
        }
    }

    private void UpdateCurrentWaveNumber()
    {
        _currentWave = GameManager.instance.SendWaveNumber();
        Debug.Log("Current Wave: " + _currentWave);
    }

    private void OnEnable()
    {
        if (_waveStart)
        {
            return;
        }
        else
        {
            _currentWave = GameManager.instance.SendWaveNumber();
            StartCoroutine(StartWaveRoutine());
        }
    }

    IEnumerator StartWaveRoutine()
    {
        _waveStart = false;

        while (!GameManager.instance.isDead)
        {
            var currentWave = _waves[_currentWave].sequence;
            Debug.Log("Modified Current Wave: " + _currentWave);
            Debug.Log("Total # of Waves: " + _waves.Count);
            _previousWave = new GameObject("Previous Wave");
            
            foreach (var obj in currentWave)
            {
                Instantiate(obj, _previousWave.transform);
                yield return _enemySpawnTime;

            }   

            if (_currentWave >= _waves.Count)
            {
                Debug.Log("Done all Waves!");
                //Disp[lay win screen
                break;
            }

            yield break;
        }
    }

    public int GetCurrentWave()
    {
        return _currentWave;
    }
}
