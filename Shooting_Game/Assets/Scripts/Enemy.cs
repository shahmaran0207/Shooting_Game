using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public string enemyName;

    public int enemyscore;
    public int health;

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

    SpriteRenderer spriteRenderer;
    // Rigidbody2D rigid;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    /*      좌 우 에서 나타나는 적으로 인해 더 이상 사용 불가능한 로직
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.down * speed;      //velocity: 속력
    */
    }

    void OnEnable()     // onEnable(): 컴포넌트가 활성화 될 때 호출되는 생명주기 함수
    {
        switch (enemyName)
        {
            case "EL":
                health = 30;
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

    void Update()
    {
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
        else if (enemyName == "EL")
        {
            GameObject bulletR = objectManager.MakeObj("BulletEnemyC");
            GameObject bulletC = objectManager.MakeObj("BulletEnemyC");
            GameObject bulletL = objectManager.MakeObj("BulletEnemyC");

            bulletR.transform.position = transform.position + Vector3.right * 0.3f;
            bulletC.transform.position = transform.position;
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidC = bulletC.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirVecC = player.transform.position - transform.position;
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);

            rigidR.AddForce(dirVecR.normalized * 5, ForceMode2D.Impulse);
            rigidC.AddForce(dirVecC.normalized * 5, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 5, ForceMode2D.Impulse);
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
        spriteRenderer.sprite = sprites[1];

        //Invoke: 시간차를 두기 위해 사용
        //※ 단, 함수 이름을 아래처럼 문자열로 사용해야 함!
        Invoke("ReturnSprite", 0.1f);

        if (health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyscore;

            //아이템 드롭 -> 랜덤으로 결정
            
            int ran = Random.Range(0, 10);
            if (ran < 3) Debug.Log("Not Item");
            else if (ran < 6)
            {
                objectManager.MakeObj("ItemCoin");
                itemCoin.transform.position = transform.position;
                
                Rigidbody2D rigid = itemCoin.GetComponent<Rigidbody2D>();
                rigid = GetComponent<Rigidbody2D>();
                rigid.velocity = Vector2.down * 1.5f;
            }

            else if (ran < 8)
            {
                objectManager.MakeObj("ItemBomb");
                itemBomb.transform.position = transform.position;

                Rigidbody2D rigid = itemBomb.GetComponent<Rigidbody2D>();
                rigid = GetComponent<Rigidbody2D>();
                rigid.velocity = Vector2.down * 1.5f;
            }
            else if (ran < 9)
            {
                objectManager.MakeObj("ItemPower");
                itemPower.transform.position = transform.position;

                Rigidbody2D rigid = itemPower.GetComponent<Rigidbody2D>();
                rigid = GetComponent<Rigidbody2D>();
                rigid.velocity = Vector2.down * 1.5f;
            }

            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;       //Quaternion.identity: 기본 회전값 -> 0
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
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