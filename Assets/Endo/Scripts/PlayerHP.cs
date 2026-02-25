using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private int _maxHP = 3;
    private int _currentHP;
    [SerializeField] private Image[] _hearts;
    [SerializeField] private SpriteRenderer _playerSprite;
    [Tooltip("?C???f?b?N?X = ????HP?B?v?f0: HP0??, 1: HP1?? ...")]
    [SerializeField] private Sprite[] _hpSprites;

    [Header("”í’eŽž–³“G")]
    [SerializeField] private float _invincibleDuration = 2f;
    private float _invincibleTimer;

    private bool _isDead;

    void Start()
    {
        //?X?^?[?g?????HP??MAXHP???
        _currentHP = _maxHP;
        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        if (_isDead || _invincibleTimer > 0f) return;

        _currentHP -= damage;
        if (_currentHP < 0)
            _currentHP = 0;

        UpdateHearts();

        // ‚±‚Ìƒqƒbƒg‚©‚çˆê’èŽžŠÔ–³“G
        _invincibleTimer = _invincibleDuration;

        if (_currentHP == 0)
        {
            _isDead = true;
            Debug.Log("Game Over");
            SceneManager.LoadScene("ResultScene");
        }
    }

    private void Update()
    {
        if (_invincibleTimer > 0f)
            _invincibleTimer -= Time.deltaTime;
    }

    void UpdateHearts()
    {
        for (int i = 0; i < _hearts.Length; i++)
        {
            _hearts[i].enabled = i < _currentHP;
        }

        if (_playerSprite != null && _hpSprites != null && _hpSprites.Length > 0)
        {
            int index = Mathf.Clamp(_currentHP, 0, _hpSprites.Length - 1);
            Sprite s = _hpSprites[index];
            if (s != null)
                _playerSprite.sprite = s;
        }
    }
}
