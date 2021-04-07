using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWalk : MonoBehaviour
{
    private CameraManager cameraManager;

    [SerializeField]private Vector3 CameraTargetPos;
    [SerializeField]private Vector3 CameraTrgetRot;

    //private Vector3 targetvec = Vector3.zero;
   
   
    private Vector3 OriginVector;
    //[SerializeField]private Quaternion CameraTargetRot;

	private void Start()
    {
        OriginVector = transform.localPosition;
        cameraManager = GameManager.GetManagerClass<CameraManager>();
        cameraManager.camerawalk = this;
       
	}

	public void CameraOrder(Vector3 Vector1 , Vector3 vector2)
	{
        CameraTargetPos = Vector1;
        CameraTrgetRot = vector2;

	}
    


    // Update is called once per frame
    void Update()
    {
       
        transform.localPosition = Vector3.Lerp(transform.localPosition, CameraTargetPos,  1.5f*Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(CameraTrgetRot), 2* Time.deltaTime);
    }
}
