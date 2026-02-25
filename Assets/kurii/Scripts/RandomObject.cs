using System.Collections;
using UnityEngine;

public class RandomObject : MonoBehaviour
{
    [Header("出現するオブジェクト（複数）")]
    public GameObject[] spawnPrefabs;

    [Header("出現ポイント（複数）")]
    public Transform[] spawnPoints;

    [Header("最初の出現まで（秒）")]
    public float firstSpawnDelay = 10f;

    [Header("出現間隔（秒）")]
    public float spawnInterval = 6f;

    [Header("上昇する距離")]
    public float upDistance = 2f;

    [Header("上昇・下降のスピード")]
    public float moveSpeed = 2f;

    [Header("上昇しきってから下降するまでの待機時間")]
    public float stayTime = 1.5f;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), firstSpawnDelay, spawnInterval);
    }

    void Spawn()
    {
        Transform randomPoint = GetRandomSpawnPoint();
        GameObject randomPrefab = GetRandomPrefab();
        if (randomPoint == null || randomPrefab == null)
            return;

        GameObject obj = Instantiate(randomPrefab, randomPoint.position, Quaternion.identity);

        var col = obj.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        float rise = upDistance;
        var riseSettings = obj.GetComponent<SpawnedObjectRiseSettings>();
        if (riseSettings != null && riseSettings.upDistance > 0f)
            rise = riseSettings.upDistance;

        StartCoroutine(RiseStayAndReturn(obj, randomPoint.position, col, rise));
    }

    Transform GetRandomSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return null;
        for (int i = 0; i < 20; i++)
        {
            var t = spawnPoints[Random.Range(0, spawnPoints.Length)];
            if (t != null) return t;
        }
        return null;
    }

    GameObject GetRandomPrefab()
    {
        if (spawnPrefabs == null || spawnPrefabs.Length == 0) return null;
        for (int i = 0; i < 20; i++)
        {
            var p = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];
            if (p != null) return p;
        }
        return null;
    }

    IEnumerator RiseStayAndReturn(GameObject obj, Vector3 spawnPos, Collider2D col, float riseDistance)
    {
        if (obj == null) yield break;

        Vector3 topPos = spawnPos + new Vector3(0, riseDistance, 0);

        // 上昇
        while (obj != null && Vector3.Distance(obj.transform.position, topPos) > 0.01f)
        {
            obj.transform.position = Vector3.MoveTowards(
                obj.transform.position,
                topPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        if (obj == null) yield break;

        // 上昇しきったらコライダー有効
        if (col != null)
            col.enabled = true;

        // 下降タイミングまで待機
        yield return new WaitForSeconds(stayTime);

        if (obj == null) yield break;

        // 下降開始でコライダー無効
        if (col != null)
            col.enabled = false;

        // スポーン位置に戻る
        while (obj != null && Vector3.Distance(obj.transform.position, spawnPos) > 0.01f)
        {
            obj.transform.position = Vector3.MoveTowards(
                obj.transform.position,
                spawnPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        if (obj != null)
            Destroy(obj);
    }
}
