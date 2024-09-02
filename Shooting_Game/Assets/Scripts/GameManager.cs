using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;

    public float maxSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public GameObject gameOverSet;

    public Text scoreText;
    public Image[] lifeImage;

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f); //RandomRange는 더 이상 사용하지 않는 함수임!
            curSpawnDelay = 0;
        }

        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score); //{0:n0}: 세자리 마다 쉼표로 나눠주는 숫자 양식
    }

    void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 9);
        GameObject enemy= Instantiate(enemyObjs[ranEnemy],
            spawnPoints[ranPoint].position,
            spawnPoints[ranPoint].rotation);

        Rigidbody2D rigid=enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic=enemy.GetComponent<Enemy>();
        enemyLogic.player = player;

        if(ranPoint == 5 || ranPoint==6)    //우측 스폰
        {
            enemy.transform.Rotate(Vector3.back * 90);      //Vector3에서 회전은 front or Back
            rigid.velocity = new Vector2(enemyLogic.speed*(-1), -1);
        }
        else if(ranPoint==7 || ranPoint==8) //좌측 스폰
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }
        else                               //정면 스폰   
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed*(-1));
        }

    }

    public void UpdateLifeIcon(int life)
    {
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index=0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerEXE", 2f);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    void RespawnPlayerEXE()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);

        Player playerLogic=player.GetComponent<Player>();
        playerLogic.isHit = false;
    }
}
