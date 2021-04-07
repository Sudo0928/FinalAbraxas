using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        if (anim == null) anim = gameObject.GetComponent<Animator>();
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                anim.Play("DoorOpen", -1);
            }
            // bIsIn = true;
           
        }
    }

    
}
