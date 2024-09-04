using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;                     //파일 읽기 위한 라이브러리

public class GameManager : MonoBehaviour
{
    public ObjectManager objmanager;

    public Transform[] spawnPoints;

    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public GameObject gameOverSet;

    public string[] enemyObjs;

    public Text scoreText;

    public Image[] lifeImage;
    public Image[] BombImage;

    public List<Spawn> spawnList;

    public int spawnIndex;

    public bool spawnEnd;

    void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjs = new string[] {"EnemyL", "EnemyM", "EnemyS" };
        ReadSpawnFile();
    }

    void ReadSpawnFile()
    {
        //1. 변수 초기화
        spawnList.Clear();  //clear: 리스트 비우는 함수
        spawnIndex = 0;
        spawnEnd = false;

        //2. 리스폰 파일 읽기
        //TextAsset: 텍스트 파일 에셋 클래스
        TextAsset textFile = Resources.Load("Stage 0") as TextAsset;
        StringReader stringreader = new StringReader(textFile.text);

        while(stringreader != null)
        {
            string line=stringreader.ReadLine();
            Debug.Log(line);
            if(line==null) break;

            //3. 리스폰 데이터 생성
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        //4. 텍스트 파일 닫기
        stringreader.Close();

        nextSpawnDelay = spawnList[0].delay;
    }

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > nextSpawnDelay)
        {
            SpawnEnemy();
            nextSpawnDelay = Random.Range(0.5f, 3f); //RandomRange는 더 이상 사용하지 않는 함수임!
            curSpawnDelay = 0;
        }

        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score); //{0:n0}: 세자리 마다 쉼표로 나눠주는 숫자 양식
    }

    void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 9);
        GameObject enemy = objmanager.MakeObj(enemyObjs[ranEnemy]);
        enemy.transform.position = spawnPoints[ranPoint].position;

        Rigidbody2D rigid=enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic=enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objmanager;

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

    public void UpdateBombIcon(int bomb)
    {
        for (int index = 0; index < 3; index++)
        {
            BombImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < bomb; index++)
        {
            BombImage[index].color = new Color(1, 1, 1, 1);
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
