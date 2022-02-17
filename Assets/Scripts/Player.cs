using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playTime;
    public float moveSpeed = 5.0f;
    public float angleSpeed = 0;
    public Transform rotModel;

    public float fireRate;
    float fireCooldown = 0;

    public Transform[] firePoints;
    public Transform lancePoint;
    public GameObject projectile;
    public GameObject explosion;
    public GameObject lance;
    bool lanceFired = false;

    public float zenSpeed = 2f;
    float zenCharge = 0;
    public Transform zenCircle;
    Renderer zenRenderer;

    public AudioSource shoot;
    public AudioSource lanceSound;
    bool moving;
    public float firingWindow = 0.9f;
    float lastFire;

    public bool vuln;
    public EnemyManager em;

    // Start is called before the first frame update
    void Start()
    {
        zenRenderer = zenCircle.GetComponent<Renderer>();
        vuln = true;

        playTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //MOVEMENT
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 moveVector = new Vector3(moveX, 0, moveY);
        transform.position += (moveVector * moveSpeed * Time.deltaTime);
        if(vuln){
            Vector3 clamppos = transform.position;
            clamppos.x = Mathf.Clamp(clamppos.x, -9f, 9f);
            clamppos.z = Mathf.Clamp(clamppos.z, -8.5f, 20);
            transform.position = clamppos;
        }

        //Zen Skill
        if(moveVector.magnitude >= Mathf.Epsilon){
            moving = true;
            zenCharge = 0;
            lanceFired = false;
            lastFire = firingWindow;
            //zenRenderer.enabled = false;
        }else{
            moving = false;
            zenCharge += zenSpeed * Time.deltaTime;
            if(zenCharge >= 1 && lanceFired == false){
                Instantiate(lance, lancePoint.position, lancePoint.rotation);
                lanceFired = true;
                lanceSound.Play();
            }
            lastFire -= Time.deltaTime;
            //zenRenderer.enabled = true;
        }
        zenCharge = Mathf.Clamp(zenCharge, 0f, 1f);
        float zenScale = Mathf.Abs(zenCharge - 1f);
        Vector3 zenScaleV = new Vector3(zenScale, 1, zenScale);
        zenCircle.localScale = Vector3.Lerp(zenCircle.localScale, zenScaleV, 0.5f);
        float zenTransparency = zenCharge * 2.3f;
        //zen renderer
        Color zencolor = zenRenderer.material.color;
        zencolor.a = Mathf.Clamp(zenTransparency, 0f, 1f);
        zenRenderer.material.color = Color.Lerp(zenRenderer.material.color, zencolor, 1f);
        

        //rotation
        float rotmap = -moveX * angleSpeed;
        Vector3 rotV = new Vector3(0, 0, rotmap);
        Quaternion rotQ = Quaternion.Euler(rotV);
        rotModel.rotation = Quaternion.Lerp(rotModel.rotation, rotQ, 0.25f);
        
        
        if(fireCooldown > 0){
            fireCooldown -= fireRate * Time.deltaTime;
        }else{
            if(moving || lastFire > 0){
                foreach(Transform fp in firePoints){
                    Instantiate(projectile, fp.position, fp.rotation);
                    shoot.Play();
                }
                fireCooldown = 1;
            }
        }

        if(vuln){
            playTime += Time.deltaTime;
        }else{
            Vector3 npos = transform.position;
            npos.z += 40 * Time.deltaTime;
            transform.position = npos;
        }
    }

    void OnTriggerEnter(Collider other){
        if(vuln){
            if(other.gameObject.tag == "EProj"){
                Debug.Log("DEAD");
                Instantiate(explosion, transform.position, transform.rotation);
                em.PlayerDead();
                Destroy(gameObject);
            }
        }
    }
}
