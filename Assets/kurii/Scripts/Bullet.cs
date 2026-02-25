using UnityEngine;

public class Bullet : MonoBehaviour
{
	private const float LifetimeSec = 2f;
	// 発射直後の大きさ
	private const float StartScale = 0.25f;
	// 飛んだ先（目標付近）での最小の大きさ
	private const float EndScale = 0.05f;
	private const float DestroyDistanceThreshold = 0.1f;

	private Vector2 _targetPos;
	private float _totalDistance;
	private Vector2? _destroyAt;

	private void Start()
	{
		Destroy(gameObject, LifetimeSec);
	}

	/// <summary>
	/// 弾発射時の2点間距離(Rayの長さ)を設定する
	/// </summary>
	public void Init(Vector2 start, Vector2 target)
	{
		_targetPos = target;
		_totalDistance = Vector2.Distance(start, target);
	}

	public void SetDestroyAt(Vector2 worldPos)
	{
		_destroyAt = worldPos;
	}

	private void Update()
	{
		float currentDistance = Vector2.Distance(transform.position, _targetPos);
		float scaleRatio;
		if (_totalDistance < 0.001f)
		{
			// 距離がほぼ0のときは割り算しない（NaN防止）
			scaleRatio = StartScale;
		}
		else
		{
			// 飛ぶほど小さくなる（開始 0.25 目標付近で EndScale）
			scaleRatio = Mathf.Lerp(EndScale, StartScale, currentDistance / _totalDistance);
		}
		transform.localScale = Vector3.one * scaleRatio;

		if (_destroyAt is Vector2 target && Vector2.Distance(transform.position, target) < DestroyDistanceThreshold)
		{
			Destroy(gameObject);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// コライダーに当たったら必ず消滅
		Destroy(gameObject);
	}
}
