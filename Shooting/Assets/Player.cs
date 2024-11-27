using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform[] RoutePoints;

    [Range(0, 200)]
    public float Speed = 20f;

    [Range(0, 50)]
    public float MoveSpeed = 10f;
    public float MoveRange = 50f;

    public float _initialLife = 100;
    public float Life = 100;
    public Image LifeGage1;
    public Image LifeGame2;
    public Image LifeGame3;

    public int lifes = 0;

    bool _isHitRoutePoint;

    IEnumerator Move()
    {
        var prevPointPos = transform.position;
        var basePosition = transform.position;
        var movedPos = Vector2.zero;

        foreach (var nextPoint in RoutePoints)
        {
            _isHitRoutePoint = false; //必ずfalseにする

            while (!_isHitRoutePoint)
            {
                //進行方向の計算
                var vec = nextPoint.position - prevPointPos;
                vec.Normalize();

                // プレイヤーの移動
                basePosition += vec * Speed * Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Speed = Speed / 2;
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    Speed = Speed * 2;
                }

                //上下左右に移動する処理
                // 行列によるベクトルの変換に関係する知識を利用しています。
                movedPos.x += Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime;
                movedPos.y += Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime;
                movedPos = Vector2.ClampMagnitude(movedPos, MoveRange);
                if(movedPos.y < 0)
                {
                    movedPos.y = 0;
                }
                if(movedPos.y >= 20)
                {
                    movedPos.y = 19;
                }
                var worldMovedPos = Matrix4x4.Rotate(transform.rotation).MultiplyVector(movedPos);


                //ルート上の位置に上下左右の移動量を加えている
                transform.position = basePosition + worldMovedPos;

                //次の処理では進行方向を向くように計算している
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vec, Vector3.up), 0.5f);

                yield return null;
                //Speed = 20f;
            }
            prevPointPos = nextPoint.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RoutePoint")
        {
            other.gameObject.SetActive(false);
            _isHitRoutePoint = true;
        }
        else if (other.gameObject.tag == "Enemy")
        {
            Life -= 34f;
            if(lifes == 0)
            {
                LifeGage1.enabled = false;
                lifes = 1;
            }
            else if(lifes == 1)
            {
                LifeGame2.enabled = false;
                lifes = 2;
            }
            else if(lifes == 2)
            {
                LifeGame3.enabled = false;
            }

            other.gameObject.SetActive(false);
            Object.Destroy(other.gameObject); //当たった敵は削除する

            if (Life <= 0)
            {
                Camera.main.transform.SetParent(null);
                gameObject.SetActive(false);
                var sceneManager = Object.FindObjectOfType<SceneManager>();
                sceneManager.ShowGameOver();
            }
            else if (other.gameObject.tag == "ClearRoutePoint")
            {
                var sceneManager = Object.FindObjectOfType<SceneManager>();
                sceneManager.ShowClear();
                _isHitRoutePoint = true;
            }
        }
    }


    //Playerスクリプト
    // 次のフィールドを追加
    public Bullet BulletPrefab;
    // 次のメソッドを追加
    public void ShotBullet(Vector3 targetPos)
    {
        var bullet = Object.Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        bullet.Init(transform.position, targetPos);
    }

    //Playerスクリプト
    //OnCollisionEnterメソッドを追加
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BackGround")
        {
            Life = 0;
            LifeGage1.enabled = false;
            LifeGame2.enabled = false;
            LifeGame3.enabled = false;

            Camera.main.transform.SetParent(null);
            gameObject.SetActive(false);
            var sceneManager = Object.FindObjectOfType<SceneManager>();
            
            sceneManager.ShowGameOver();
        }
    }

    void Start()
    {
        StartCoroutine(Move());
    }
}