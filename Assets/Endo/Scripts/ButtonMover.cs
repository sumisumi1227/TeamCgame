using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonMover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Settings")]
    public Vector3 normalScale = Vector3.one;      // 通常サイズ
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1f); // ホバー時
    public float speed = 10f; // 拡大・縮小の速さ

    private Vector3 targetScale;

    void Start()
    {
        targetScale = normalScale;
    }

    void Update()
    {
        transform.localScale =
            Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
    }



    //ゲームジャムで使ったものを少し改良


    // カーソルが乗ったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = hoverScale;
    }

    // カーソルが離れたとき
    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = normalScale;
    }
}

