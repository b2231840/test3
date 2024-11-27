using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    //SceneManager�X�N���v�g�Ɏ��̂��̂�ǉ�
    // �t�B�[���h�̒ǉ�
    public Text GameOverText;
    public Text ClearText;

    public Text ScoreText;
    int _currentScore = 0;
    //���\�b�h�̒ǉ�
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