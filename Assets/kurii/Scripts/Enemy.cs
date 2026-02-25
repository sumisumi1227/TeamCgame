using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Collider2D _collider;
    private EnemySpawnManager _manager;
    private int _spawnIndex;
    private bool _isRight;  // 右用スポーンなら true

    [SerializeField] private GameObject _enemySnowPrefab;
    [SerializeField] private float _throwSpeed = 5f;
    [SerializeField] private bool _isTripleShot;  // true なら3方向に同時投げ
    [SerializeField] private float _comeOutDuration = 0.3f;
    [SerializeField] private float _outOffsetX = 2.0f;       // 左右に出る距離
    [SerializeField] private float _backToHiddenDuration = 0.25f;
    [SerializeField] private float _hiddenWaitMin = 0.5f;     // 隠れている時間（最小）
    [SerializeField] private float _hiddenWaitMax = 1.5f;
    [SerializeField] private float _dyingDuration = 2f;      // 倒されてから消えるまでの時間

    private float _spawnTime;
    private float _stayDuration;
    private int _throwsRemaining;
    private float _nextThrowTime;
    private const float MinThrowInterval = 0.5f;

    private Vector3 _hiddenPos;
    private Vector3 _outPos;
    private float _stateStartTime;
    private float _stateDuration;
    private bool _isOut;
    private bool _dyingLogDone;

    private enum State { Hidden, ComingOut, Out, GoingBack, Dying }
    private State _state;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        if (_collider != null)
            _collider.enabled = false;
    }

    public void SetVisible(bool visible)
    {
        if (_collider != null)
            _collider.enabled = visible;
    }

    public void InitSpawn(EnemySpawnManager manager, int spawnIndex, bool isRight)
    {
        _manager = manager;
        _spawnIndex = spawnIndex;
        _isRight = isRight;
    }

    /// <summary>
    /// 隠れ位置でスポーン済み。左右に出て→雪投げ→隠れるを繰り返す。
    /// </summary>
    public void BeginComeOut()
    {
        _hiddenPos = transform.position;
        float xOff = _isRight ? _outOffsetX : -_outOffsetX;
        _outPos = _hiddenPos + new Vector3(xOff, 0f, 0f);
        _state = State.ComingOut;
        _stateStartTime = Time.time;
        _stateDuration = _comeOutDuration;
        _isOut = false;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.ComingOut:
                float t = (Time.time - _stateStartTime) / _stateDuration;
                if (t >= 1f)
                {
                    transform.position = _outPos;
                    SetVisible(true);
                    StartStayOut();
                    _state = State.Out;
                    _isOut = true;
                }
                else
                    transform.position = Vector3.Lerp(_hiddenPos, _outPos, t);
                break;

            case State.Out:
                if (Time.time > _spawnTime + _stayDuration)
                {
                    SetVisible(false);
                    _state = State.GoingBack;
                    _stateStartTime = Time.time;
                    _stateDuration = _backToHiddenDuration;
                }
                else if (_throwsRemaining > 0 && Time.time >= _nextThrowTime && _enemySnowPrefab != null)
                {
                    ThrowSnow();
                    _throwsRemaining--;
                    _nextThrowTime = Time.time + MinThrowInterval + Random.Range(0f, 0.5f);
                }
                break;

            case State.GoingBack:
                t = (Time.time - _stateStartTime) / _stateDuration;
                if (t >= 1f)
                {
                    transform.position = _hiddenPos;
                    _state = State.Hidden;
                    _stateStartTime = Time.time;
                    _stateDuration = Random.Range(_hiddenWaitMin, _hiddenWaitMax);
                }
                else
                    transform.position = Vector3.Lerp(_outPos, _hiddenPos, t);
                break;

            case State.Hidden:
                if (Time.time - _stateStartTime >= _stateDuration)
                    BeginComeOut();
                break;

            case State.Dying:
                if (Time.time - _stateStartTime >= _dyingDuration)
                {
                    Debug.Log("[Enemy] 2秒経過で消滅");
                    Destroy(gameObject);
                }
                break;
        }
    }

    public void StartStayOut()
    {
        _spawnTime = Time.time;
        _stayDuration = Random.Range(2f, 4f);
        _throwsRemaining = Random.Range(0, 3);  // 0,1,2 高確率で投げるなら 1 or 2 が多めでも可
        _nextThrowTime = Time.time + Random.Range(0.2f, 0.8f);
    }

    private void ThrowSnow()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;
        Vector2 baseDir = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;

        if (_isTripleShot)
        {
            // 3方向に同時投げ（中央・左30°・右30°）
            FireOne(baseDir);
            FireOne(RotateVector2(baseDir, -30f));
            FireOne(RotateVector2(baseDir, 30f));
        }
        else
        {
            FireOne(baseDir);
        }
    }

    private void FireOne(Vector2 direction)
    {
        GameObject snow = Instantiate(_enemySnowPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = snow.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = direction.normalized * _throwSpeed;
    }

    private static Vector2 RotateVector2(Vector2 v, float degrees)
    {
        float r = degrees * Mathf.Deg2Rad;
        return new Vector2(v.x * Mathf.Cos(r) - v.y * Mathf.Sin(r), v.x * Mathf.Sin(r) + v.y * Mathf.Cos(r));
    }
    // プレイヤーの弾が当たった(衝突)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Debug.Log("[Enemy] プレイヤーの弾が当たった(衝突) → Dyingへ");
            Destroy(collision.gameObject);
            _state = State.Dying;
            _stateStartTime = Time.time;
            if (_collider != null)
                _collider.enabled = false;
        }
    }

    // プレイヤーの弾が当たった(トリガー)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Debug.Log("[Enemy] プレイヤーの弾が当たった → Dyingへ");
            Destroy(other.gameObject);
            // SetVisible(false); を外す場合は外す
            _state = State.Dying;
            _stateStartTime = Time.time;
            if (_collider != null)
                _collider.enabled = false;
        }
    }

    private void OnDestroy()
    {
        if (_manager != null)
            _manager.FreeSpawn(_spawnIndex);
    }
}