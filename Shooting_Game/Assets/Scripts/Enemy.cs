using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public string enemyName;

    public int enemyscore;
    public int health;
    public int patternIndex;
    public int curPatternCount;

    public int[] maxPatternCount;

    public float speed;
    public float maxShotDelay;
    public float curShotDelay;

    public Sprite[] sprites;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject bulletObjC;
    public GameObject itemCoin;
    public GameObject itemPower;
    public GameObject itemBomb;
    public GameObject player;

    public ObjectManager objectManager;

    public GameManager gameManager;

    SpriteRenderer spriteRenderer;
    // Rigidbody2D rigid;

    Animator anim;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        /*      좌 우 에서 나타나는 적으로 인해 더 이상 사용 불가능한 로직
            rigid = GetComponent<Rigidbody2D>();
            rigid.velocity = Vector2.down * speed;      //velocity: 속력
        */

        if (enemyName == "B") anim = GetComponent<Animator>();
    }

    void OnEnable()     // onEnable(): 컴포넌트가 활성화 될 때 호출되는 생명주기 함수
    {
        switch (enemyName)
        {
            case "B":
                health = 10;
                Invoke("Stop", 2);
                break;

            case "L":
                health = 20;
                break;
            case "M":
                health = 10;
                break;
            case "S":
                health = 5;
                break;
        }
    }

    void Stop()
    {
        if (!gameObject.activeSelf) return;
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }

    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch (patternIndex)
        {
            case 0:
                FireFoward();
                break;

            case 1:
                FireShot();
                break;

            case 2:
                FireArc();
                break;

            case 3:
                FireAround();
                break;
        }
    }

    void FireFoward()
    {
        //4발 발사
        GameObject bulletRR = objectManager.MakeObj("BulletBossA");
        GameObject bulletR = objectManager.MakeObj("BulletBossA");
        GameObject bulletLL = objectManager.MakeObj("BulletBossA");
        GameObject bulletL = objectManager.MakeObj("BulletBossA");

        bulletR.transform.position = transform.position + Vector3.right * 0.45f;
        bulletRR.transform.position = transform.position + Vector3.right * 0.3f;
        bulletLL.transform.position = transform.position + Vector3.left * 0.3f;
        bulletL.transform.position = transform.position + Vector3.left * 0.45f;

        Rigidbody2D rigidRR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

        rigidR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) Invoke("FireFoward", 3.5f);
        else Invoke("Think", 1);
    }

    void FireShot()
    {
        for(int index=0; index<5; index++)
        {
            GameObject bullet = objectManager.MakeObj("BulletBossB");
            bullet.transform.position = transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) Invoke("FireShot", 1.5f);
        else Invoke("Think", 2);
    }

    void FireArc()
    {
        GameObject bullet = objectManager.MakeObj("BulletBossA");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        //Mathf.Sin(): 삼각함수 Sin
        Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 10 * curPatternCount / maxPatternCount[patternIndex]), -1);
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) Invoke("FireArc", 0.5f);
        else Invoke("Think", 2);
    }

    void FireAround()
    {
        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        for(int index=0; index< roundNum; index++)
        {
            GameObject bullet = objectManager.MakeObj("BulletBossB");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index/ roundNum),
                                         Mathf.Sin(Mathf.PI * 2 * index / roundNum));
            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 260 * index / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }


        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) Invoke("FireAround", 2.5f);
        else Invoke("Think", 2);
    }

    void Update()
    {
        if (enemyName == "B") return;
        Fire();
        Reload();
    }

    void Fire()
    {
        if (curShotDelay < maxShotDelay) return;

        if (enemyName == "S")
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyA");
            bullet.transform.position = transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }

        else if (enemyName == "L")
        {
            GameObject bulletR = objectManager.MakeObj("BulletEnemyB");
            GameObject bulletL = objectManager.MakeObj("BulletEnemyB");

            bulletR.transform.position = transform.position + Vector3.right * 0.3f;
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);

            rigidR.AddForce(dirVecR.normalized * 4, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 4, ForceMode2D.Impulse);
        }

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public void OnHit(int damage)
    {
        if (health <= 0) return;

        health -= damage;
        if (enemyName == "B")
        {
            anim.SetTrigger("OnHit");
        }
        else
        {
            spriteRenderer.sprite = sprites[1];

            //Invoke: 시간차를 두기 위해 사용
            //※ 단, 함수 이름을 아래처럼 문자열로 사용해야 함!
            Invoke("ReturnSprite", 0.1f);
        }

        if (health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyscore;

            //아이템 드롭 -> 랜덤으로 결정

            int ran = enemyName == "B" ? 0 : Random.Range(0, 10);
            if (ran < 3) Debug.Log("Not Item");
            else if (ran < 6)
            {
                GameObject itemCoin=objectManager.MakeObj("ItemCoin");
                itemCoin.transform.position = transform.position;
            }

            else if (ran < 8)
            {
                GameObject itemBomb = objectManager.MakeObj("ItemBomb");
                itemBomb.transform.position = transform.position;
            }
            else if (ran < 9)
            {
                GameObject itemPower=objectManager.MakeObj("ItemPower");
                itemPower.transform.position = transform.position;
            }

            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;       //Quaternion.identity: 기본 회전값 -> 0
            gameManager.callexplosion(transform.position, enemyName);

            //Boss Kill
            if (enemyName == "B")
            {
                gameManager.StageEnd();
            }
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet" && enemyName !="B")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }

        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            collision.gameObject.SetActive(false);
        }
    }
}