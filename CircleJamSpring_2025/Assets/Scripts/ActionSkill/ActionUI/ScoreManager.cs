using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    void Start()
    {
        UpdateScoreText();
    }

    /// <summary>
    /// スコアを加算するメソッド
    /// </summary>
    public void AddScore(int points)
    {
        score += points;
        Debug.Log(points + "pt 加算されました。現在のスコア: " + score);
        UpdateScoreText();
    }

    /// <summary>
    /// スコアのUIを更新するメソッド
    /// </summary>
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"SCORE: {score:D4}";
        }
        else
        {
            Debug.LogWarning("scoreTextが設定されていません！");
        }
    }

    /// <summary>
    /// 現在のスコアを取得するメソッド
    /// </summary>
    public int GetScore()
    {
        return score;
    }
}
