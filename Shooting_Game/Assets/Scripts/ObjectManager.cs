using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject EnemyLPrefab;
    public GameObject EnemyMPrefab;
    public GameObject EnemySPrefab;
    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBombPrefab;
    public GameObject bulletPlayerAPrefab;
    public GameObject bulletPlayerBPrefab;
    public GameObject bulletEnemyAPrefab;
    public GameObject bulletEnemyBPrefab;
    public GameObject bulletEnemyCPrefab;
    public GameObject bulletFollowerPrefab;

    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;

    GameObject[] itemCoin;
    GameObject[] itemBomb;
    GameObject[] itemPower;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;
    GameObject[] bulletEnemyC;
    GameObject[] bulletFollower;
    GameObject[] targetPool;

    void Awake()
    {

        enemyL = new GameObject[10];
        enemyM = new GameObject[20];
        enemyS = new GameObject[30];

        itemCoin = new GameObject[20];
        itemBomb = new GameObject[10];
        itemPower = new GameObject[10];

        bulletPlayerA = new GameObject[200];
        bulletPlayerB = new GameObject[200];
        bulletEnemyA = new GameObject[200];
        bulletEnemyB = new GameObject[200];
        bulletEnemyC = new GameObject[200];
        bulletFollower = new GameObject[200];

        Generate();
    }

    void Generate()
    {
        for (int index = 0; index < enemyL.Length; index++)
        {
            enemyL[index] = Instantiate(EnemyLPrefab);
            enemyL[index].SetActive(false);
        }

        for (int index = 0; index < enemyM.Length; index++)
        {
            enemyM[index] = Instantiate(EnemyMPrefab);
            enemyM[index].SetActive(false);
        }
        for (int index = 0; index < enemyS.Length; index++)
        {
            enemyS[index] = Instantiate(EnemySPrefab);
            enemyS[index].SetActive(false);
        }
        for (int index = 0; index < itemCoin.Length; index++)
        {
            itemCoin[index] = Instantiate(itemCoinPrefab);
            itemCoin[index].SetActive(false);
        }
        for (int index = 0; index < itemPower.Length; index++)
        {
            itemPower[index] = Instantiate(itemPowerPrefab);
            itemPower[index].SetActive(false);
        }
        for (int index = 0; index < itemBomb.Length; index++)
        {
            itemBomb[index] = Instantiate(itemBombPrefab);
            itemBomb[index].SetActive(false);
        }
        for (int index = 0; index < bulletPlayerA.Length; index++)
        {
            bulletPlayerA[index] = Instantiate(bulletPlayerAPrefab);
            bulletPlayerA[index].SetActive(false);
        }
        for (int index = 0; index < bulletPlayerB.Length; index++)
        {
            bulletPlayerB[index] = Instantiate(bulletPlayerBPrefab);
            bulletPlayerB[index].SetActive(false);
        }
        for (int index = 0; index < bulletEnemyA.Length; index++)
        {
            bulletEnemyA[index] = Instantiate(bulletEnemyAPrefab);
            bulletEnemyA[index].SetActive(false);
        }
        for (int index = 0; index < bulletEnemyB.Length; index++)
        {
            bulletEnemyB[index] = Instantiate(bulletEnemyBPrefab);
            bulletEnemyB[index].SetActive(false);
        }
        for (int index = 0; index < bulletEnemyC.Length; index++)
        {
            bulletEnemyC[index] = Instantiate(bulletEnemyCPrefab);
            bulletEnemyC[index].SetActive(false);
        }

        for (int index = 0; index < bulletFollower.Length; index++)
        {
            bulletFollower[index] = Instantiate(bulletFollowerPrefab);
            bulletFollower[index].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "EnemyL":
                targetPool = enemyL;
                break;

            case "EnemyM":
                targetPool = enemyM;
                break;

            case "EnemyS":
                targetPool = enemyS;
                break;

            case "ItemCoin":
                targetPool = itemCoin;
                break;

            case "ItemBomb":
                targetPool = itemBomb;
                break;

            case "ItemPower":
                targetPool = itemPower;
                break;

            case "BulletPlayerA":
                targetPool = bulletPlayerA;
                break;

            case "BulletPlayerB":
                targetPool = bulletPlayerB;
                break;

            case "BulletEnemyA":
                targetPool = bulletEnemyA;
                break;

            case "BulletEnemyB":
                targetPool = bulletEnemyB;
                break;

            case "BulletEnemyC":
                targetPool = bulletEnemyC;
                break;
                  
            case "BulletFollower":
                targetPool = bulletFollower;
                break;
        }

        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "ItemCoin":
                targetPool = itemCoin;
                break;
            case "ItemBomb":
                targetPool = itemBomb;
                break;
            case "ItemPower":
                targetPool = itemPower;
                break;
            case "BulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "BulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "BulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "BulletEnemyB":
                targetPool = bulletEnemyB;
                break;
            case "BulletEnemyC":
                targetPool = bulletEnemyC;
                break;
            case "BulletFollower":
                targetPool = bulletFollower;
                break;
        }

        return targetPool;
    }
}
