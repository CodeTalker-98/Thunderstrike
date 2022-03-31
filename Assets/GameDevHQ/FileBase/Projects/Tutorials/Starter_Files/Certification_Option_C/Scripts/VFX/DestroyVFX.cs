using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyVFX : MonoBehaviour
{
    [Header("Audio Clip")]
    [SerializeField] private AudioClip _sound;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public void PlaySound()
    {
        AudioManager.instance.PlaySound(_sound);
    }

    public void Destroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
