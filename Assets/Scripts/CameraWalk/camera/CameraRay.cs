using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraRay : MonoBehaviour
{

    private RaycastHit Out;

    [SerializeField]private Transform PlayerRayEnd;
    [SerializeField] private CharacterInfo Info;

    // Start is called before the first frame update
   
    private void Start()
    {
        //var cameraData = transform.GetComponent<Camera>.GetUniversalAdditionalCameraData();


        
        

    }


    private void Update()
    {

        Debug.DrawRay(transform.position, PlayerRayEnd.position - transform.position, Color.green, 0.5f);

        if(Physics.Raycast(transform.position,PlayerRayEnd.position - transform.position, out Out, 15.0f))
        {
           
          
            if (Out.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Info.WallSignal = true;

               

            }
            else if (Out.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Info.WallSignal = false;
                
            }

        }

    }



}
