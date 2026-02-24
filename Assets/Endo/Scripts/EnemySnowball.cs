using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnowball : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //プレイヤーに当たったら
        if (other.CompareTag("Player"))
        {
            //プレイヤーをよんで１ダメージあたると１ダメージ削れる
            PlayerHP hp = other.GetComponent<PlayerHP>();
            hp.TakeDamage(1);

            Destroy(gameObject);
        }
    }
}
