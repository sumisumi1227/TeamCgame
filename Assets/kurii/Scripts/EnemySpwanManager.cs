using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPointsLeft;
    [SerializeField] private Transform[] _spawnPointsRight;
    [SerializeField] private GameObject _enemyPrefab;           // 通常（1方向）
    [SerializeField] private GameObject _enemyTripleShotPrefab; // 3方向同時

    private bool[] _occupied;          
    private float[] _respawnBlockedUntil; // 倒されてから3秒間はその枠でリスポーンしない
    private float _nextSpawnTime;
    private float _gameStartTime;

    private void Start()
    {
        if (_spawnPointsLeft == null || _spawnPointsLeft.Length != 3 ||
            _spawnPointsRight == null || _spawnPointsRight.Length != 3)
        {
            Debug.LogWarning("EnemySpawnManager: 左3・右3のスポーン位置を設定してください。");
            return;
        }
        _occupied = new bool[6];
        _respawnBlockedUntil = new float[6];
        _gameStartTime = Time.time;
        _nextSpawnTime = Time.time + Random.Range(3f, 5f);
    }

    private void Update()
    {
        GameObject prefab = ChooseEnemyPrefab();
        if (prefab == null) return;
        if (Time.time < _nextSpawnTime) return;

        int? slot = GetRandomFreeSlot();
        if (!slot.HasValue) return;

        int i = slot.Value;
        _occupied[i] = true;

        int wall = i / 2;
        bool isRight = (i % 2) == 1;
        Transform spawnT = isRight ? _spawnPointsRight[wall] : _spawnPointsLeft[wall];
        Vector3 pos = spawnT.position;

        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
        Enemy enemy = go.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.InitSpawn(this, i, isRight);
            enemy.BeginComeOut();
        }

        _nextSpawnTime = Time.time + GetCurrentInterval();
    }

    private GameObject ChooseEnemyPrefab()
    {
        if (_enemyPrefab == null) return _enemyTripleShotPrefab;
        if (_enemyTripleShotPrefab == null) return _enemyPrefab;
        // 最初は敵1が70%・敵2が30%、60秒で50:50に近づく
        float elapsed = Time.time - _gameStartTime;
        float t = Mathf.Clamp01(elapsed / 60f);
        float normalChance = Mathf.Lerp(0.7f, 0.5f, t);
        return Random.value < normalChance ? _enemyPrefab : _enemyTripleShotPrefab;
    }

    private int? GetRandomFreeSlot()
    {
        var free = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            if (!_occupied[i] && Time.time >= _respawnBlockedUntil[i])
                free.Add(i);
        }
        if (free.Count == 0) return null;
        return free[Random.Range(0, free.Count)];
    }

    private float GetCurrentInterval()
    {
        float elapsed = Time.time - _gameStartTime;
        float t = Mathf.Clamp01(elapsed / 60f);
        return Mathf.Lerp(Random.Range(3f, 5f), 0.8f, t);
    }

    public void FreeSpawn(int index)
    {
        if (index < 0 || index >= 6) return;
        _occupied[index] = false;
        _respawnBlockedUntil[index] = Time.time + 3f;  // 同じ位置は3秒あけてリスポーン
    }
}