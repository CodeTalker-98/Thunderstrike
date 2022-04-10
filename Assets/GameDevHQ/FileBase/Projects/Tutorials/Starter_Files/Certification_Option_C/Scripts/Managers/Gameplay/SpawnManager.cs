using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _enemySpawnBuffer = 1.0f;

    [Header("Waves")]
    [SerializeField] private List<Waves> _waves = new List<Waves>();

    private bool _canInitializeSpawn = false;
    private bool _waveOver = false;

    private int _currentWave = 0;

    private GameObject _previousWave;

    private WaitForSeconds _enemySpawnTime;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _enemySpawnTime = new WaitForSeconds(_enemySpawnBuffer);

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

        _canInitializeSpawn = true;
    }

    private void Update()
    {
        CheckEndOfWave();
    }

    private void CheckEndOfWave()
    {
        if (_previousWave != null)
        {
            if (_previousWave.transform.childCount < 1 && _waveOver)
            {
                if (_currentWave >= _waves.Count)
                {
                    GameManager.instance.levelComplete = true;
                    GameManager.instance.WinScreen();
                }
                else
                {
                    if (_currentWave == 8)
                    {
                        GameManager.instance.checkpointReached = true;
                    }

                    GameManager.instance.NextWave();
                    Destroy(_previousWave.gameObject);
                }

                _waveOver = false;
            }
        }
        else
        {
            return;
        }
    }

    private void OnEnable()
    {
        if (_canInitializeSpawn)
        {
            StartCoroutine(StartWaveRoutine());
        }
    }

    IEnumerator StartWaveRoutine()
    {
        while (!GameManager.instance.isDead)
        {
            var currentWave = _waves[_currentWave].sequence;
            _previousWave = new GameObject("Previous Wave");
            
            foreach (var obj in currentWave)
            {
                if (_previousWave != null)
                {
                    Instantiate(obj, _previousWave.transform);
                }
                else
                {
                    yield break;
                }

                yield return _enemySpawnTime;
            }

            if (_currentWave < _waves.Count)
            {
                _currentWave++;
            }

            _waveOver = true;
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
    }

    public void DestroyPreviousWave()
    {
        Destroy(_previousWave.gameObject);
    }

}
