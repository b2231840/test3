using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Range(0, 100)]
    public float Speed = 10;
    public float DeadSecond = 10f;

    float _time;
    Player _player;
    void Start()
    {
        _time = 0f;
        _player = Object.FindObjectOfType<Player>();
    }

    //Enemyスクリプト
    // 次のメソッドを追加
    private void OnMouseUpAsButton()
    {
        _player.ShotBullet(transform.position);
    }

    //Enemyスクリプト
    // 次のフィールドを追加
    public float Life = 10;
    // 次のメソッドを追加
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Life -= 10;
            Object.Destroy(other.gameObject);

            if (Life <= 0)
            {
                Object.Destroy(gameObject);
                var sceneManager = Object.FindObjectOfType<SceneManager>();
                sceneManager.AddScore(1000);
            }
        }
    }

    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= DeadSecond)
        {
            Object.Destroy(gameObject);
        }
        else
        {
            var vec = _player.transform.position - transform.position;
            transform.position += vec.normalized * Speed * Time.deltaTime;
        }

    }
}