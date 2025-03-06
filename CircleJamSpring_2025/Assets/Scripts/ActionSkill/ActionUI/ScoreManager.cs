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
    /// �X�R�A�����Z���郁�\�b�h
    /// </summary>
    public void AddScore(int points)
    {
        score += points;
        Debug.Log(points + "pt ���Z����܂����B���݂̃X�R�A: " + score);
        UpdateScoreText();
    }

    /// <summary>
    /// �X�R�A��UI���X�V���郁�\�b�h
    /// </summary>
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"SCORE: {score:D4}";
        }
        else
        {
            Debug.LogWarning("scoreText���ݒ肳��Ă��܂���I");
        }
    }

    /// <summary>
    /// ���݂̃X�R�A���擾���郁�\�b�h
    /// </summary>
    public int GetScore()
    {
        return score;
    }
}
