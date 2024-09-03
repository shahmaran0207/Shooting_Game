using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float maxShotDelay;
    public float curShotDelay;
    
    public int power;
    public int maxpower;
    public int maxbomb;
    public int life;
    public int score;
    public int bomb;

    public bool isHit;
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchLeft;
    public bool isTouchRight;
    public bool isBoomTime;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject BombEffect;

    public GameManager gameManager;

    public ObjectManager objmanager;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Fire();
        Reload();
        boom();
    }

    void boom()
    {
        if (!Input.GetButton("Fire2")) return;
        if (isBoomTime) return;
        if (bomb == 0) return;
        bomb--;
        isBoomTime = true;
        gameManager.UpdateBombIcon(bomb);

        BombEffect.SetActive(true);
        Invoke("OffBoomEffect", 5f);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int index = 0; index < enemies.Length; index++)
        {
            Enemy enemyLogic = enemies[index].GetComponent<Enemy>();
            enemyLogic.OnHit(3000);
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Enemy Bullet");

        for (int index = 0; index < enemies.Length; index++)
        {
            bullets[index].SetActive(false);
        }
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if ((isTouchRight && h == 1) ||(isTouchLeft &&h== -1)) h = 0;        

        float v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1)) v = 0;

        Vector3 curPos=transform.position;
        Vector3 nextPos=new Vector3(h, v, 0)*speed*Time.deltaTime;

        transform.position = curPos + nextPos;

        if(Input.GetButtonDown("Horizontal") ||Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Input", (int)h);
        }
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1")) return;

        if (curShotDelay < maxShotDelay) return;

        switch (power) {
            case 1:
                //제일 작은 총알
                //Instantiate: 매개변수 오브젝트를 생성하는 함수
                GameObject bullet = objmanager.MakeObj("BulletPlayerA");
                bullet.transform.position=transform.position;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

            case 2:
                GameObject bulletR = objmanager.MakeObj("BulletPlayerA");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;

                GameObject bulletL = objmanager.MakeObj("BulletPlayerA");
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

            case 3:
                GameObject bulletRR = objmanager.MakeObj("BulletPlayerA");
                GameObject bulletCC = objmanager.MakeObj("BulletPlayerB");
                GameObject bulletLL = objmanager.MakeObj("BulletPlayerA");

                bulletRR.transform.position = transform.position + Vector3.right * 0.35f;
                bulletCC.transform.position = transform.position;
                bulletLL.transform.position = transform.position + Vector3.left * 0.35f;

                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

        }

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch(collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        else if(collision.gameObject.tag=="Enemy" || collision.gameObject.tag == "Enemy Bullet")
        {
            if (isHit) return;
            isHit = true;

            life--;
            gameManager.UpdateLifeIcon(life);

            if (life == 0) gameManager.GameOver();
            else gameManager.RespawnPlayer();

            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }

        else if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();

            switch (item.type)
            {
                case "Coin":
                    score += 1000;
                    break;

                case "Power":
                    if (power == maxpower) score += 500;
                    else power++;
                    break;

                case "Bomb":
                    if (bomb == maxbomb) score += 500;
                    else
                    {
                        bomb++;
                        gameManager.UpdateBombIcon(bomb);
                    }

                    break;
            }

            collision.gameObject.SetActive(false);
        }
    }

    void OffBoomEffect()
    {
        BombEffect.SetActive(false);
        isBoomTime = false;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
