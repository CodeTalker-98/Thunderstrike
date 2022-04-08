using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetermineCheckpoint : MonoBehaviour
{
    private GameObject _retryButton;
    private GameObject _checkpointButton;

    private void Init()
    {
        _retryButton = GameObject.Find("/UI/Game Over Panel/Canvas/Retry Button Parent");
        _checkpointButton = GameObject.Find("/UI/Game Over Panel/Canvas/Checkpoint Button Parent");
    }

    private void OnEnable()
    {
        Init();
        DetermineVisibility();
    }

    private void DetermineVisibility()
    {
        if (GameManager.instance != null)
        {
            if (!GameManager.instance.checkpointReached)
            {
                _retryButton.SetActive(true);
                _checkpointButton.SetActive(false);
            }
            else
            {
                _retryButton.SetActive(false);
                _checkpointButton.SetActive(true);
            }
        }
    }
}
