using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _highScoreText;
    [SerializeField] private Text _weaponNameText;
    [SerializeField] private string[] _weapons;
    [SerializeField] private Image _healthBarFill;

    [Header("Screen Elements")]
    [SerializeField] private Text _waveNumberText;
    [SerializeField] private Text _waveInfoText;

    private void OnEnable()
    {
        UpdateWaveInfo();
    }

    public void UpdateWaveInfo()
    {
        if (GameManager.instance != null)
        {
            int waveNumber = GameManager.instance.SendWaveNumber();
            _waveNumberText.text = "Wave: " + waveNumber.ToString();
            _waveInfoText.text = GameManager.instance.SendWaveInfo(waveNumber);
        }
    }

    public void DisplayScore(int value)
    {
        _scoreText.text = value.ToString().PadLeft(7, '0');
    }

    public void DisplayHealth(float health, float maxHealth)
    {
        _healthBarFill.fillAmount = health / maxHealth;
    }

    public void DisplayWeaponName(int index)
    {
        if (index > 0)
        {
            _weaponNameText.text = _weapons[index - 1];
        }
    }
}
