using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    // Start is called before the first frame update
    public Enemy[] enemies;
    public int ecount;
    int ccount;
    public float enterSpeed;
    bool ready = false;

    public EnemyManager enemyManager;
    int[] enemyDead;
    void Start()
    {
        foreach(Enemy e in enemies){
            e.parentWave = this;
            e.isReady = false;
        }
        ready = false;
        ccount = ecount;
        enemyDead = new int[ecount];

        for(int i=0; i<ecount; i++){
            enemyDead[i] = 0;
            enemies[i].ID = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!ready && transform.position.z > 0){
            Vector3 pv = transform.position;
            pv.z -= enterSpeed * Time.deltaTime;
            transform.position = pv;
        }else{
            ready = true;
            foreach(Enemy e in enemies){
                e.isReady = true;
            }
        }
    }

    public void enemyDeath(int ID){
        if(enemyDead[ID] == 0){
            ccount-=1;
            enemyDead[ID] = 1;
        }

        if(ccount <= 0){
            enemyManager.WaveCleared();
            Destroy(gameObject);
        }
    }
}
