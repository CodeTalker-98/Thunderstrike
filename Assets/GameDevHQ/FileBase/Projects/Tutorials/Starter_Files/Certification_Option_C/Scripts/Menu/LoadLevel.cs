using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Image _loadingBar;

    private int _sceneIndex;

    private WaitForEndOfFrame _frame;

    private void Start()
    {
        Init();
        StartCoroutine(LoadSceneAsync());
    }

    private void Init()
    {
        _sceneIndex = PlayerPrefs.GetInt("Scene Index", 2);

        _frame = new WaitForEndOfFrame();
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(_sceneIndex);

        while (!async.isDone)
        {
            _loadingBar.fillAmount = async.progress;
            yield return _frame;
        }
    }
}
