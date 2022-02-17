using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float xBound = 10;
    public int hitpoints;
    public int direction;

    public bool isReady;
    public Renderer[] flashBody;
    Color oColor;
    public Color flashColor;
    public float flashTime;
    public GameObject explosion;

    public Transform[] firePoints;

    float fireCooldown;
    public float fireRate = 0;
    public GameObject projectile;
    public Vector2 movespeedRange;
    public Vector2 moveTimeRange;
    float nextDirectionChange;
    public float moveSpeed;

    public int ID;

    public EnemyWave parentWave;
    // Start is called before the first frame update
    void Start()
    {
        oColor = flashBody[0].material.color;
        direction = Random.Range(0,2);
        moveSpeed = Random.Range(movespeedRange.x, movespeedRange.y);
        nextDirectionChange = Time.time + Random.Range(moveTimeRange.x, moveTimeRange.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(isReady){
            enemyUpdate();
        }
    }

    void enemyUpdate(){
        if(fireCooldown > 0){
            fireCooldown -= fireRate * Time.deltaTime;
        }else{
            foreach(Transform fp in firePoints){
                Instantiate(projectile, fp.position, fp.rotation);
            }
            fireCooldown = 1;
        }

        //Movement
        Vector3 moveV = new Vector3(moveSpeed, 0, 0);
        if(direction == 0){
            moveV *= -1;
        }
        transform.Translate(moveV * Time.deltaTime);

        if(transform.position.x <= (-xBound) || transform.position.x >= (xBound)){
            changeDirection();
        }else if(Time.time >= nextDirectionChange){
            changeDirection();
        }
    }

    void changeDirection(){
        if(direction == 0)
            direction = 1;
        else
            direction = 0;

        moveSpeed = Random.Range(movespeedRange.x, movespeedRange.y);
        nextDirectionChange = Time.time + Random.Range(moveTimeRange.x, moveTimeRange.y);
    }

    void OnTriggerEnter(Collider other){
        if(isReady){
            if(other.gameObject.tag == "PProj"){
                //Debug.Log("HIT");
                takeDamage(other.gameObject.GetComponent<Projectile>());
            }
        }
    }

    void takeDamage(Projectile p){
        FlashColor(flashColor);
        hitpoints -= p.dmg;
        p.Hit();

        if(hitpoints <= 0){
            Instantiate(explosion, transform.position, transform.rotation);
            parentWave.enemyDeath(ID);
            Debug.Log("Death");
            Destroy(gameObject);
        }
        Invoke("stopFlash", flashTime);
    }

    void stopFlash(){
        FlashColor(oColor);
    }

    void FlashColor(Color col){
        foreach(Renderer r in flashBody){
            r.material.color = col;
        }
    }
}
