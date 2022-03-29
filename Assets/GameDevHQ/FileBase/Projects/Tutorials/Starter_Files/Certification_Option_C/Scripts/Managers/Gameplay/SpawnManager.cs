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
    }

    private void Update()
    {
        CheckEndOfWave();
    }

    private void CheckEndOfWave()
    {
        if (_previousWave != null)
        {
            if (_previousWave.transform.childCount < 1)
            {
                if (_currentWave >= _waves.Count)
                {
                    Debug.Log("Done all Waves!");
                    GameManager.instance.levelComplete = true;
                    GameManager.instance.WinScreen();
                }
                else
                {
                    GameManager.instance.NextWave();
                    Destroy(_previousWave.gameObject);
                }
            }
        }
        else
        {
            return;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(StartWaveRoutine());
    }

    IEnumerator StartWaveRoutine()
    {
        while (!GameManager.instance.isDead)
        {
            var currentWave = _waves[_currentWave].sequence;
            _previousWave = new GameObject("Previous Wave");
            
            foreach (var obj in currentWave)
            {
                Instantiate(obj, _previousWave.transform);
                yield return _enemySpawnTime;
            }

            if (_currentWave < _waves.Count)
            {
                _currentWave++;
            }

            yield break;
        }
    }

    public int GetCurrentWave()
    {
        return _currentWave;
    }

    public void SetCurrentWave()
    {
        _currentWave = 8;
        GameManager.instance.NextWave();
        Debug.Log("wave after Checkpoint:" + _currentWave);
    }

    public void DestroyPreviousWave()
    {
        Destroy(_previousWave.gameObject);
    }
}
