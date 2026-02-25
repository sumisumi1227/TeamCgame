using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private AimRay _aimRay;
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private GameObject _bullet;

    //壁のトリガー内にいるときは打てない
    private bool _isInHideWall = false;

    private void Update()
    {
        // 左クリックかつ「外にいるとき」だけ発射
        if (!Input.GetMouseButtonDown(0) || _isInHideWall) return;

        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject bullet = Instantiate(_bullet, transform.position, Quaternion.identity);
        // プレイヤーと弾の衝突を無視（自分に当たって消えないように）
        Collider2D bulletCol = bullet.GetComponent<Collider2D>();
        Collider2D playerCol = GetComponent<Collider2D>();
        if (bulletCol != null && playerCol != null)
        Physics2D.IgnoreCollision(bulletCol, playerCol);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        // レイの終点まで飛ばす・マウス位置で消えるように設定
        bulletScript.Init(transform.position, _aimRay._endPos);
        bulletScript.SetDestroyAt(mousePos);

        Vector2 direction = (mousePos - (Vector2)transform.position);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * _speed;
    }

    // 打てなくなる
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HideWall"))
            _isInHideWall = true;
    }

    // 打てるようにする
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HideWall"))
            _isInHideWall = false;
    }
}