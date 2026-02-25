using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 雪玉作成の溜まり具合をスライダーで表示。デバフ中は「効率ダウン」アイコンをゲージの前に表示する。
/// レイアウト例: [デバフアイコン] [====スライダー====]
/// </summary>
public class SnowballCreateGauge : MonoBehaviour
{
    [SerializeField] private Shoot _shoot;
    [Tooltip("ゲージ（Slider の Value または Image の Fill Amount で 0～1 を表示）")]
    [SerializeField] private Slider _slider;
    [Tooltip("Slider がない場合は Image の Fill Amount で表示")]
    [SerializeField] private Image _fillImage;
    [Tooltip("デバフ中だけ表示するイラスト（ゲージの左側などに配置）")]
    [SerializeField] private GameObject _debuffIcon;

    private void Update()
    {
        if (_shoot == null) return;

        float progress = _shoot.CreationProgress;

        if (_slider != null)
            _slider.value = progress;
        if (_fillImage != null)
            _fillImage.fillAmount = progress;

        if (_debuffIcon != null)
            _debuffIcon.SetActive(_shoot.IsSnowballCreateSlowed);
    }
}
