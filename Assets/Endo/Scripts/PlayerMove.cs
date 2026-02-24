using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //プレイヤーの速度設定（変更可）
    public float movespeed = 5f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Translate(Vector2.right * movespeed);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Translate(Vector2.left * movespeed);
        }

        float Posx = Mathf.Clamp(transform.position.x, -movespeed, movespeed);

        transform.position = new Vector3(Posx, transform.position.y, transform.position.z);


        //GetAxisRawで移動を入力
        //float h = Input.GetAxisRaw("Horizontal");
        //transform.Translate(Vector2.right * h * movespeed * Time.deltaTime);
    }

}
