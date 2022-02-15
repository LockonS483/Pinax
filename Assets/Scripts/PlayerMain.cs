using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    public float lifeTime;
    public float speed;

    float dieTime;
    
    // Start is called before the first frame update
    void Start()
    {
        dieTime = Time.time + lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > dieTime){
            Destroy(gameObject);
        }

        Vector3 mv = new Vector3(0, 0, speed * Time.deltaTime);
        transform.Translate(mv);
    }
}
