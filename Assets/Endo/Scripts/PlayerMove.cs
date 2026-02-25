using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _centerX = 0f;           // 常にいる中心のX
    [SerializeField] private float _stickOutAmount = 2f;     // D/A押してる時にはみ出す距離
    [SerializeField] private float _moveSpeed = 8f;          // 中央⇔左右への戻り速さ

    private void Update()
    {
        float targetX = _centerX;

        if (Input.GetKey(KeyCode.D))
            targetX = _centerX + _stickOutAmount;
        else if (Input.GetKey(KeyCode.A))
            targetX = _centerX - _stickOutAmount;

        float newX = Mathf.MoveTowards(transform.position.x, targetX, _moveSpeed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
