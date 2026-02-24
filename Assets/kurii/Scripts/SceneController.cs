using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] float timeLimitSeconds = 0f;

    void Update()
    {
        if(timeLimitSeconds <= 0f)
        {
            return;
        }
        timeLimitSeconds -= Time.deltaTime;
        if(timeLimitSeconds <= 0f)
        {
            SceneManager.LoadScene("ResultScene");
        }
    }

    public void GoToGame() => SceneManager.LoadScene("GameScene");
    public void GoToResult() => SceneManager.LoadScene("ResultScene");
    public void GoToTitle() => SceneManager.LoadScene("TitleScene");
}