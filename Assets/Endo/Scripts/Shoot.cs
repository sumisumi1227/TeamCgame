using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private AimRay _aimRay;
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private GameObject _bullet;

    [Header("???X?g?b?N")]
    [SerializeField] private int _maxStock = 3;
    [Header("?????_???I?u?W?F?N?g??????f?o?t?i??????????j")]
    [SerializeField] private float _createDurationNormal = 2f;
    [SerializeField] private float _createDurationDebuffed = 4f;
    [SerializeField] private float _debuffDuration = 10f;

    private int _snowballStock;
    private bool _isInHideWall;
    private float _sKeyHoldTime;
    private float _debuffTimer;
    private float _creationProgress;

    public bool IsInHideWall => _isInHideWall;
    public int SnowballStock => _snowballStock;
    public int MaxStock => _maxStock;
    public bool IsSnowballCreateSlowed => _debuffTimer > 0f;
    public float CreationProgress => _creationProgress;

    private void OnEnable()
    {
        NoHitTarget.OnHitByPlayer += ApplySnowballSlowDebuff;
    }

    private void OnDisable()
    {
        NoHitTarget.OnHitByPlayer -= ApplySnowballSlowDebuff;
    }

    private void ApplySnowballSlowDebuff(NoHitTarget _)
    {
        _debuffTimer = _debuffDuration;
    }

    private void Update()
    {
        if (_debuffTimer > 0f)
            _debuffTimer -= Time.deltaTime;

        float createDuration = _debuffTimer > 0f ? _createDurationDebuffed : _createDurationNormal;

        if (_isInHideWall && Input.GetKey(KeyCode.S))
        {
            _sKeyHoldTime += Time.deltaTime;
            while (_sKeyHoldTime >= createDuration && _snowballStock < _maxStock)
            {
                _snowballStock++;
                _sKeyHoldTime -= createDuration;
                if (_debuffTimer > 0f)
                    _debuffTimer = 0f;
            }
        }
        else
        {
            _sKeyHoldTime = 0f;
        }

        if (_isInHideWall && Input.GetKey(KeyCode.S) && _snowballStock < _maxStock)
            _creationProgress = Mathf.Clamp01(_sKeyHoldTime / createDuration);
        else
            _creationProgress = 0f;

        //
        if (!Input.GetMouseButtonDown(0) || _isInHideWall || _snowballStock <= 0 || !_aimRay.IsAiming) return;

        _snowballStock--;

        Vector2 endPos = _aimRay._endPos;
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject bullet = Instantiate(_bullet, transform.position, Quaternion.identity);

        Collider2D bulletCol = bullet.GetComponent<Collider2D>();
        Collider2D playerCol = GetComponent<Collider2D>();
        if (bulletCol != null && playerCol != null)
            Physics2D.IgnoreCollision(bulletCol, playerCol);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Init(transform.position, endPos);
        bulletScript.SetDestroyAt(mousePos);

        Vector2 direction = (mousePos - (Vector2)transform.position);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * _speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HideWall"))
            _isInHideWall = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HideWall"))
            _isInHideWall = false;
    }
}
