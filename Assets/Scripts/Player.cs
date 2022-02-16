using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float angleSpeed = 0;
    public Transform rotModel;

    public float fireRate;
    float fireCooldown = 0;

    public Transform[] firePoints;
    public GameObject projectile;

    public float zenSpeed = 2f;
    float zenCharge = 0;
    public Transform zenCircle;
    Renderer zenRenderer;

    public AudioSource shoot;

    // Start is called before the first frame update
    void Start()
    {
        zenRenderer = zenCircle.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //MOVEMENT
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 moveVector = new Vector3(moveX, 0, moveY);
        transform.position += (moveVector * moveSpeed * Time.deltaTime);

        //Zen Skill
        if(moveVector.magnitude >= Mathf.Epsilon){
            zenCharge = 0;
            //zenRenderer.enabled = false;
        }else{
            zenCharge += zenSpeed * Time.deltaTime;
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
        zenRenderer.material.color = Color.Lerp(zenRenderer.material.color, zencolor, 0.6f);
        

        //rotation
        float rotmap = -moveX * angleSpeed;
        Vector3 rotV = new Vector3(0, 0, rotmap);
        Quaternion rotQ = Quaternion.Euler(rotV);
        rotModel.rotation = Quaternion.Lerp(rotModel.rotation, rotQ, 0.5f);

        if(fireCooldown > 0){
            fireCooldown -= fireRate * Time.deltaTime;
        }else{
            foreach(Transform fp in firePoints){
                Instantiate(projectile, fp.position, fp.rotation);
                shoot.Play();
            }
            fireCooldown = 1;
        }
    }
}
