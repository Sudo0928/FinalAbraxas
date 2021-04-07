using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubetemp : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) anim.Play("tran",-1);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) anim.Play("opaq", -1);
    }
}
