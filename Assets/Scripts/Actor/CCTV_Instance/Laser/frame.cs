using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frame : MonoBehaviour
{

    float leftValue;
    float rightValue;

    public lasetmp left;
    public lasetmp right;


    public GameObject spark1;
    public GameObject spark2;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            leftValue = other.transform.position.z + 0.3f;
            rightValue = other.transform.position.z - 0.3f;

            left.Zero(leftValue);
            right.Zero(rightValue);

            spark1.SetActive(true);
            spark2.SetActive(true);


            spark1.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, leftValue+0.2f);
            spark2.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, rightValue-0.2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            left.Zero(0);
            right.Zero(0);

            spark1.SetActive(false);
            spark2.SetActive(false);
        }


    }


    // Update is called once per frame

}
