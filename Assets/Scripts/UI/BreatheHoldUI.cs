using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BreatheHoldUI : MonoBehaviour
{
 
    private Animator anim;

    private float ScreenX;
    private float ScreenY;               // 끼야야앗


    public float ScreenWidth;
    public float ScreenHeight;

   
    private RectTransform rect;
    private UiManager _UiManager;
    

    [SerializeField] float x;
    [SerializeField] float y;
     private Transform TargetPos;
     private Camera Cam;

    /*
    private void Awake()
    {
        
       
        
    }
    */
    private void Start()
    {
        

        _UiManager = GameManager.GetManagerClass<UiManager>();
        _UiManager.breatheHoldUI_instance = this;
        InitializeUI( GameManager.GetManagerClass<CharacterManager>().playerInstance.transform, _UiManager.MainCamera);


        anim = GetComponent<Animator>();
        rect = gameObject.GetComponent<RectTransform>();
        // 
        gameObject.SetActive(false);
    }

    public void HoldAnim(float time)
    {
        anim.Play("breath_anim",0,time);
    }

    public void InitializeUI(Transform target, Camera cam )
    {
        Cam = cam;
        TargetPos = target;
    }


    private void Update()
    {
        

        ScreenX = ScreenWidth * Cam.WorldToViewportPoint(TargetPos.position).x - x;
         ScreenY = ScreenHeight * Cam.WorldToViewportPoint(TargetPos.position).y - y;


        //transform. position = new Vector3(ScreenX, ScreenY);
        
         rect.position = new Vector3(ScreenX, ScreenY);
    }

}
