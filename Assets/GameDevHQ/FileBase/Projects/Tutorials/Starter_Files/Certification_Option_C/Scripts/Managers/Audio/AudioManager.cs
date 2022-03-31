using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private int _musicSourceAmount = 2;

    private int _activeMusicSourceIndex = 1;

    private float _masterVolumePercent = 1;
    private float _musicVolumePercent = 1;
    private float _sfxVolumePercent = 1;

    AudioSource[] _musicSources;

    private Transform _audioListener;
    private Transform _playerTransform;

    public enum AudioChannel {Master, Music, SFX}

    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);

            _musicSources = new AudioSource[_musicSourceAmount];

            for (int i = 0; i < _musicSources.Length; i++)
            {
                GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
                _musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                _musicSources[i].loop = true;
                newMusicSource.transform.parent = transform;                
            }

            _audioListener = GetComponent<AudioListener>().transform;

            if (FindObjectOfType<Player>() != null)
            {
                _playerTransform = FindObjectOfType<Player>().transform;
            }

            _masterVolumePercent = PlayerPrefs.GetFloat("Master Volume", 0.5f);
            _musicVolumePercent = PlayerPrefs.GetFloat("Music Volume", 0.5f);
            _sfxVolumePercent = PlayerPrefs.GetFloat("SFX Volume", 0.5f);
        }        
    }

    private void Update()
    {
        if (_playerTransform != null)
        {
            //_audioListener.position = _playerTransform.position;
        }
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.Master:
                _masterVolumePercent = volumePercent;
                break;
            case AudioChannel.Music:
                _musicVolumePercent = volumePercent;
                break;
            case AudioChannel.SFX:
                _sfxVolumePercent = volumePercent;
                break;
            default:
                break;
        }

        _musicSources[0].volume = _musicVolumePercent * _masterVolumePercent;

        PlayerPrefs.SetFloat("Master Volume", _masterVolumePercent);
        PlayerPrefs.SetFloat("Music Volume", _musicVolumePercent);
        PlayerPrefs.SetFloat("SFX Volume", _sfxVolumePercent);
        PlayerPrefs.Save();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, _audioListener.position, _sfxVolumePercent * _masterVolumePercent);
        }
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        _activeMusicSourceIndex = 1 - _activeMusicSourceIndex;
        _musicSources[_activeMusicSourceIndex].clip = clip;
        _musicSources[_activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }

    IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            _musicSources[_activeMusicSourceIndex].volume = Mathf.Lerp(0, _musicVolumePercent * _masterVolumePercent, percent);
            _musicSources[1 - _activeMusicSourceIndex].volume = Mathf.Lerp(_musicVolumePercent * _masterVolumePercent, 0, percent);
            yield return null;
        }
    }

    public float GetMasterVolumePercent()
    {
        return _masterVolumePercent;
    }

    public float GetMusicVolumePercent()
    {
        return _musicVolumePercent;
    }

    public float GetSFXVolumePercent()
    {
        return _sfxVolumePercent;
    }
}
