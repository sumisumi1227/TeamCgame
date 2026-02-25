using UnityEngine;

public class AimRay : MonoBehaviour
{
    [System.NonSerialized] public Vector2 _endPos;

    [SerializeField] private LayerMask hitLayer;

    private Camera _camera;

    /// <summary> AまたはDを押しているときだけtrue（照準・カーソル表示用） </summary>
    public bool IsAiming => Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        Cursor.visible = false;
        _endPos = GetRayEndPosition();
        Debug.DrawLine(transform.position, _endPos, Color.red);
    }

    Vector2 GetRayEndPosition()
    {
        if (!IsAiming)
            return _endPos;

        Vector3 viewport = _camera.ScreenToViewportPoint(Input.mousePosition);
        if (Input.GetKey(KeyCode.A))
            viewport.x = Mathf.Clamp(viewport.x, 0f, 0.5f);
        else if (Input.GetKey(KeyCode.D))
            viewport.x = Mathf.Clamp(viewport.x, 0.5f, 1f);

        float dist = Mathf.Abs(_camera.transform.position.z);
        Vector3 targetWorld = _camera.ViewportToWorldPoint(new Vector3(viewport.x, viewport.y, dist));
        targetWorld.z = 0f;

        Vector2 direction = ((Vector2)targetWorld - (Vector2)transform.position).normalized;
        float maxDistance = 100f;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, hitLayer);

        if (hit.collider != null)
            return hit.point;
        return GetScreenEdgePosition(direction);
    }

    Vector2 GetScreenEdgePosition(Vector2 direction)
    {
        Vector3 screenMin = _camera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 screenMax = _camera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        float t = 100f;

        if (direction.x > 0)
            t = Mathf.Min(t, (screenMax.x - transform.position.x) / direction.x);
        else if (direction.x < 0)
            t = Mathf.Min(t, (screenMin.x - transform.position.x) / direction.x);

        if (direction.y > 0)
            t = Mathf.Min(t, (screenMax.y - transform.position.y) / direction.y);
        else if (direction.y < 0)
            t = Mathf.Min(t, (screenMin.y - transform.position.y) / direction.y);

        return (Vector2)transform.position + direction * t;
    }
}