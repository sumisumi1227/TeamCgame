using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 雪玉ストック数を画像アイコンで表示する。
/// 最大ストック数分の Image を指定し、貯まっている数だけ「ある」色、残りは「ない」色で表示する。
/// </summary>
public class SnowballStockDisplay : MonoBehaviour
{
    [SerializeField] private Shoot _shoot;
    [Tooltip("ストック用アイコン（1個目・2個目・3個目…）。同じスプライトを並べてもOK")]
    [SerializeField] private Image[] _stockIcons;

    [Header("見た目")]
    [SerializeField] private Color _hasStockColor = Color.white;
    [SerializeField] private Color _emptyColor = new Color(1f, 1f, 1f, 0.35f);

    private void Update()
    {
        if (_shoot == null || _stockIcons == null) return;

        int stock = _shoot.SnowballStock;
        for (int i = 0; i < _stockIcons.Length; i++)
        {
            if (_stockIcons[i] == null) continue;
            _stockIcons[i].color = i < stock ? _hasStockColor : _emptyColor;
        }
    }
}
