using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private int _maxHP = 3;
    private int _currentHP;
    [SerializeField] private Image[] _hearts;

    void Start()
    {
        //スタートは今のHPはMAXHPです
        _currentHP = _maxHP;
        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        _currentHP -= damage;

        if (_currentHP < 0)
            _currentHP = 0;

        UpdateHearts();

        if (_currentHP == 0)
        {
            Debug.Log("Game Over");
        }
    }

     void UpdateHearts()
    {
        for (int i = 0; i < _hearts.Length; i++)
        {
            _hearts[i].enabled = i < _currentHP;
        }
    }
}
