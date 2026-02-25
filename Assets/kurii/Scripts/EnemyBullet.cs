using UnityEngine;

/// <summary>
/// 敵が投げる雪。プレイヤーとHideWallにだけ当たる（レイヤーで制御）。
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float _lifetime = 2f;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Shoot shoot = other.GetComponent<Shoot>();
            if (shoot != null && shoot.IsInHideWall)
                return;  // 隠れ中は当たらない（弾は通過）

            Debug.Log("敵の雪がプレイヤーに当たった");
            Destroy(gameObject);
        }
        else if (other.CompareTag("HideWall"))
        {
            Debug.Log("敵の雪がHideWallに当たった");
            Destroy(gameObject);
        }
    }
}