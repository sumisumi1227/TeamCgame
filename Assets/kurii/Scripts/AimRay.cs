using UnityEngine;

public class AimRay : MonoBehaviour
{
    [System.NonSerialized] public Vector2 _endPos;

    [SerializeField] private LayerMask hitLayer;

    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        _endPos = GetRayEndPosition();
        Debug.DrawLine(transform.position, _endPos, Color.red);
    }

    Vector2 GetRayEndPosition()
    {
        Vector3 mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector2 direction = (mouseWorld - transform.position).normalized;
        float maxDistance = 100f;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, hitLayer);

        if (hit.collider != null)
            return hit.point;
        else
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