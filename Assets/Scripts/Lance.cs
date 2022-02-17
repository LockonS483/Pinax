using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : MonoBehaviour
{
   void OnTriggerEnter(Collider other){
       if(other.gameObject.tag == "EProj"){
           Destroy(gameObject);
       }
   }
}
