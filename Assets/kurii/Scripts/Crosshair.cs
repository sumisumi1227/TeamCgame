using UnityEngine;

/// <summary>
/// 照準用クロスヘア。A/D押下時だけ表示し、マウスカーソル位置に追従する。
/// </summary>
public class Crosshair : MonoBehaviour
{
    [SerializeField] private AimRay _aimRay;
    [SerializeField] private Camera _camera;
    [Tooltip("省略時はこのオブジェクトのTransformを使用")]
    [SerializeField] private Transform _crosshairTransform;
    [Tooltip("省略時は子のSpriteRendererを自動取得")]
    [SerializeField] private SpriteRenderer _crosshairSprite;

    private void Reset()
    {
        if (_crosshairTransform == null) _crosshairTransform = transform;
        if (_crosshairSprite == null) _crosshairSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Transform target = _crosshairTransform != null ? _crosshairTransform : transform;
        bool show = _aimRay != null && _aimRay.IsAiming;

        if (_crosshairSprite != null)
            _crosshairSprite.enabled = show;
        else
            target.gameObject.SetActive(show);

        if (show)
        {
            Camera cam = _camera != null ? _camera : Camera.main;
            if (cam != null)
            {
                Vector3 p = cam.ScreenToWorldPoint(Input.mousePosition);
                p.z = 0f;
                target.position = p;
            }
        }
    }
}
