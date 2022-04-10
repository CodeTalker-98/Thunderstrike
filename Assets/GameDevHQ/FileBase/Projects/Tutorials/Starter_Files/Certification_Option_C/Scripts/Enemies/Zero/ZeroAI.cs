using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroAI : Enemy
{
    [SerializeField] private AudioClip _flybySound;

    private AudioSource _audio;

    public override void Init()
    {
        base.Init();

        _audio = GetComponent<AudioSource>();
        _audio.clip = _flybySound;
        _audio.Play();
    }
}
