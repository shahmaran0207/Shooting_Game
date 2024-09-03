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
            GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = player.transform.position - transform.position;

            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }

        else if (enemyName == "L")
        {
            GameObject bulletR = Instantiate(bulletObjB, transform.position+Vector3.right*0.3f, transform.rotation);
            GameObject bulletL = Instantiate(bulletObjB, transform.position + Vector3.left * 0.3f, transform.rotation);

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);

            rigidR.AddForce(dirVecR.normalized * 4, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 4, ForceMode2D.Impulse);
        }
        else if (enemyName == "EL")
        {
            GameObject bulletR = Instantiate(bulletObjC, transform.position + Vector3.right * 0.3f, transform.rotation);
            GameObject bulletC = Instantiate(bulletObjC, transform.position, transform.rotation);
            GameObject bulletL = Instantiate(bulletObjC, transform.position + Vector3.left * 0.3f, transform.rotation);

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
            else if (ran < 6) Instantiate(itemCoin, transform.position, itemCoin.transform.rotation);
            else if (ran < 8) Instantiate(itemBomb, transform.position, itemBomb.transform.rotation);
            else if (ran < 9) Instantiate(itemPower, transform.position, itemPower.transform.rotation);

            gameObject.SetActive(false);
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet") gameObject.SetActive(false);
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);

            gameObject.SetActive(false);
        }
    }
}