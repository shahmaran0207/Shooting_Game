using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    
    public Sprite[] sprites;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.down * speed;      //velocity: 속력
    }

    void OnHit(int damage)
    {
        health -= damage;
        spriteRenderer.sprite = sprites[1];

        //Invoke: 시간차를 두기 위해 사용
        //※ 단, 함수 이름을 아래처럼 문자열로 사용해야 함!
        Invoke("ReturnSprite", 0.1f);       

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet") Destroy(gameObject);
        else if (collision.gameObject.tag == "PlayerBullet") Destroy(gameObject);
    }
}