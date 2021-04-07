using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dispatch : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.CompareTag("Player"))
        {
           
           

                UI.SetActive(false);
          
        }
    }
}
