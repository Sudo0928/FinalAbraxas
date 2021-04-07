using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour ,IManager
{
    public GameManager gameManager { get { return GameManager.gameManager; } }


    public FollowCamera followCamera { get; set; }
    public Camera MainCamera         { get; set; }
    public Transform Path            { get; set; }
    public CameraWalk camerawalk { get; set; }



private void Awake()
    {
        followCamera = GameObject.Find("camera").GetComponent<FollowCamera>();
        MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        Path = GameObject.Find("Path").transform;

    }

   
}



