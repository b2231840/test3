using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    //SceneManagerスクリプトに次のものを追加
    // フィールドの追加
    public Text GameOverText;
    public Text ClearText;

    public Text ScoreText;
    int _currentScore = 0;
    //メソッドの追加
    private void Start()
    {
        GameOverText.gameObject.SetActive(false);
        ClearText.gameObject.SetActive(false);

        ScoreText.text = _currentScore.ToString();
    }
    public void ShowGameOver()
    {
        GameOverText.gameObject.SetActive(true);
    }

    public void ShowClear()
    {
        ClearText.gameObject.SetActive(true);
    }

    public void AddScore(int score)
    {
        _currentScore += score;
        ScoreText.text = _currentScore.ToString();
    }
}