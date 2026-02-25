using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	private const int SCORE_POINT = 100;

	private const string DEFAULT_TEXT = "点";

	[Header("スコアテキスト"),SerializeField] private GameObject scoretext;

	[Header("スコアテキスト"), SerializeField] private Text _scoreText;

	[SerializeField] private bool isDebug;

	// m_scoreのゲッターとセッターを作成
	public class Score
	{
		public static int score = 0;
	}

	// Start is called before the first frame update
	void Start()
	{
		if(SceneManager.GetActiveScene().name == "GameScene")
		{
			Score.score = 0;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && isDebug)
		{
			//SetScore();
			SceneManager.LoadScene("Result");
		}

		_scoreText.text = Score.score.ToString() + DEFAULT_TEXT;
	}

	public static int SetScore()
	{
		Score.score += SCORE_POINT;

		return Score.score;
	}
}
