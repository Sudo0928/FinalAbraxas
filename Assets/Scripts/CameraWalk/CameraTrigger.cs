using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private CameraManager cam;


    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 rot;
    [SerializeField] private GameObject UI;

    private void Start()
    {
        cam = GameManager.GetManagerClass<CameraManager>();
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            cam.camerawalk.CameraOrder(pos,rot);

            if(UI)
            {
                UI.SetActive(false);
            }

        }
    }

}
