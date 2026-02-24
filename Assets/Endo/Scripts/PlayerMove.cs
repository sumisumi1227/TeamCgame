using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //プレイヤーの速度設定（変更可）
    public float movespeed = 5f;

    private void Update()
    {
        //GetAxisRawで移動を入力
        float h = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector2.right * h * movespeed * Time.deltaTime);
    }

}
