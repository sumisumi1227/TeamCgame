using UnityEngine;

/// <summary>
/// ランダムオブジェクトのプレハブに付ける。付いていればその上昇距離を使う（なければ RandomObject のデフォルト）。
/// </summary>
public class SpawnedObjectRiseSettings : MonoBehaviour
{
    [Tooltip("このオブジェクトだけの上昇距離（0以下ならスポーナーのデフォルトを使用）")]
    public float upDistance = 2f;
}
