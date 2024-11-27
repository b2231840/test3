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
            _isHitRoutePoint = false; //�K��false�ɂ���

            while (!_isHitRoutePoint)
            {
                //�i�s�����̌v�Z
                var vec = nextPoint.position - prevPointPos;
                vec.Normalize();

                // �v���C���[�̈ړ�
                basePosition += vec * Speed * Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Speed = Speed / 2;
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    Speed = Speed * 2;
                }

                //�㉺���E�Ɉړ����鏈��
                // �s��ɂ��x�N�g���̕ϊ��Ɋ֌W����m���𗘗p���Ă��܂��B
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


                //���[�g��̈ʒu�ɏ㉺���E�̈ړ��ʂ������Ă���
                transform.position = basePosition + worldMovedPos;

                //���̏����ł͐i�s�����������悤�Ɍv�Z���Ă���
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
            Object.Destroy(other.gameObject); //���������G�͍폜����

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


    //Player�X�N���v�g
    // ���̃t�B�[���h��ǉ�
    public Bullet BulletPrefab;
    // ���̃��\�b�h��ǉ�
    public void ShotBullet(Vector3 targetPos)
    {
        var bullet = Object.Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        bullet.Init(transform.position, targetPos);
    }

    //Player�X�N���v�g
    //OnCollisionEnter���\�b�h��ǉ�
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