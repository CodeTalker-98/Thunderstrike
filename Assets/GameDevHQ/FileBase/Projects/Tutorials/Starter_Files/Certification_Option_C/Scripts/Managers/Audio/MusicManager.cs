using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _fade = 2.0f;

    [Header("Music")]
    [SerializeField] private AudioClip[] _music;

    public void MenuMusic()
    {
        AudioManager.instance.PlayMusic(_music[0], _fade);
    }

    public void GameplayMusic()
    {
        AudioManager.instance.PlayMusic(_music[1], _fade);
    }
}
