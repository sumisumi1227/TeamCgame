using UnityEngine;

/// <summary>
/// ランダムオブジェクト用。プレイヤーの弾（PlayerBullet）が当たったときだけ判定を取る。
/// このスクリプトを「当てたらダメ」のプレハブに付ける。
/// </summary>
public class NoHitTarget : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが当ててしまったときに呼ばれる（スコア減らすなどは別で購読）
    /// </summary>
    public static System.Action<NoHitTarget> OnHitByPlayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("PlayerBullet")) return;
        Destroy(collision.gameObject);
        OnHitByPlayer?.Invoke(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerBullet")) return;
        Destroy(other.gameObject);
        OnHitByPlayer?.Invoke(this);
    }
}
