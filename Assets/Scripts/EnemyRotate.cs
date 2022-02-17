using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotate : MonoBehaviour
{

    public float rotateSpeed;
    public Vector3 rot;
    public bool localRotate = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!localRotate){
            rot.y = Time.time * rotateSpeed;
            Quaternion q = Quaternion.Euler(rot);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, 0.9f);
        }else{
            transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
        }
    }
}
