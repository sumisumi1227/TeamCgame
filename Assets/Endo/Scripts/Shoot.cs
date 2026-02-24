using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private GameObject _snowball;
    //クールタイム追加
    [SerializeField] private float _cooltime = 2f;
    private float _nextFireTime = 0f;
    //壁の範囲にいる間
    private bool _isInHideWall = false;

    private Vector2 mousePos;

    void Update()
    {
        mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && Time.time >= _nextFireTime && !_isInHideWall) // 左クリック
        {
            _nextFireTime = Time.time + _cooltime;　

            GameObject snowballIns = Instantiate(_snowball, transform.position, Quaternion.identity);

            //マウスカーソル方向決めれる
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

            snowballIns.GetComponent<Rigidbody2D>().velocity = direction * _speed;

            //弾を消す二秒後
            Destroy(snowballIns,1f);
        }

    }


    //壁の範囲にいる間は雪玉を投げれないようにする
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HideWall"))
        {
            _isInHideWall = true;
        }
        //Debug.Log("弾触れる");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HideWall"))
        {
            _isInHideWall = false;
        }
    }
}