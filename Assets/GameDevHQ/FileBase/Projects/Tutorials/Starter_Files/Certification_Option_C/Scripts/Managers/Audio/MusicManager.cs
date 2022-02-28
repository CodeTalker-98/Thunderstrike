using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MusicManager : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _fade = 2.0f;

    [Header("Music")]
    [SerializeField] private AudioClip[] _music;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        AudioManager.instance.PlayMusic(_music[0], _fade);
    }

    public void ChangeMusic()
    {
        AudioManager.instance.PlayMusic(_music[1], _fade);
    }
}
