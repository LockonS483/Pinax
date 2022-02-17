using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFire : MonoBehaviour
{
    public GameObject projectile;
    public Transform[] firePoints;
    public float startDelay;
    public int volleyCount;
    public float fireDelay;
    public float interval;
    public bool track;
    public Transform player;
    public float rotSpeed;

    int cVolleyCount;
    float nextVolley;
    float nextFire;

    public Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nextVolley = Time.time + startDelay;
        cVolleyCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(track){
            Vector3 targetpos = player.position;
            targetpos.y = transform.position.y;
            /*Vector3 direction = targetpos - transform.position;
            Quaternion toRot = Quaternion.FromToRotation(transform.forward, direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRot, rotSpeed * Time.time);*/
            transform.LookAt(targetpos, Vector3.up);
        }

        if(enemy.isReady){
            if(cVolleyCount > 0){
                if(Time.time > nextFire){
                    foreach(Transform fp in firePoints){
                        Instantiate(projectile, fp.position, fp.rotation);
                    }
                    nextFire = Time.time + fireDelay;
                    if(--cVolleyCount == 0){
                        nextVolley = Time.time + interval;
                    }
                }
            }else if(Time.time > nextVolley){
                cVolleyCount = volleyCount;
            }
        }
    }
}
