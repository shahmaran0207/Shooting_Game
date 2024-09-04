using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float maxShotDelay;
    public float curShotDelay;

    public ObjectManager objectManager;

    public Vector3 followPos;

    public Transform parent;

    public Queue<Vector3> parentPos;

    public int followDelay;

    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    void Awake()
    {
        //FIFO: First in First out
        parentPos = new Queue<Vector3>();
    }

    void Watch()
    {
        //Input Pos
        parentPos.Enqueue(parent.position);

        if (parentPos.Count > followDelay)
        {
            //Output Pos
            followPos = parentPos.Dequeue();
        }
    }
    void Follow()
    {
        transform.position = followPos;
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1")) return;

        if (curShotDelay < maxShotDelay) return;
        
        GameObject bullet = objectManager.MakeObj("BulletFollower");
        bullet.transform.position = transform.position;
        
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);


        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
}
