using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public EnemyWave[] waves;
    public int waveCount;
    public float startDelay;
    public float waveDelay;
    float nextSpawnTime;

    bool ready = true;
    int cWave = 0;

    public Text timeText;
    public GameObject textParent;
    public GameObject deathTextParent;

    public bool ended = false;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time + startDelay;
        ready = true;
        textParent.SetActive(false);
        deathTextParent.SetActive(false);
        ended = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!ended){
            if(ready && Time.time >= nextSpawnTime){
                EnemyWave w = Instantiate(waves[cWave], transform.position, transform.rotation);
                w.enemyManager = this;
                cWave++;
                ready = false;
            }
        }else{
            if(Input.GetKeyDown("space")){
                SceneManager.LoadScene("menu", LoadSceneMode.Single);
            }
        }
    }

    public void PlayerDead(){
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EProj");
        foreach(GameObject b in bullets){
            Destroy(b);
        }
        deathTextParent.SetActive(true);
        ended = true;
        return;
    }

    public void WaveCleared(){
        if(cWave >= waveCount){
            //win
            Debug.Log("winner");
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EProj");
            Player p = GameObject.FindWithTag("Player").GetComponent<Player>();
            
            foreach(GameObject b in bullets){
                Destroy(b);
            }
            p.vuln = false;

            timeText.text = p.playTime.ToString();
            textParent.SetActive(true);
            ended = true;
            return;
        }

        nextSpawnTime = Time.time + waveDelay;
        ready = true;
    }
}
