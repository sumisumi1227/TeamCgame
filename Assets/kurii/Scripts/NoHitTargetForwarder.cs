using UnityEngine;

/// <summary>
/// コライダーが子オブジェクトにある場合用。このオブジェクトに当たったら、
/// 親にある NoHitTarget を探して OnHitByPlayer を発火させる。
/// 植木鉢など「親に NoHitTarget・子に Collider2D」の構成で使う。
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class NoHitTargetForwarder : MonoBehaviour
{
    private NoHitTarget _noHitTarget;

    private void Awake()
    {
        _noHitTarget = GetComponentInParent<NoHitTarget>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_noHitTarget == null) return;
        if (!collision.gameObject.CompareTag("PlayerBullet")) return;
        Destroy(collision.gameObject);
        NoHitTarget.OnHitByPlayer?.Invoke(_noHitTarget);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_noHitTarget == null) return;
        if (!other.CompareTag("PlayerBullet")) return;
        Destroy(other.gameObject);
        NoHitTarget.OnHitByPlayer?.Invoke(_noHitTarget);
    }
}
