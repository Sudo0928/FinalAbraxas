using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class stoneGague :MonoBehaviour
{
   

    private float ScreenX;
    private float ScreenY;               // 끼야야앗


    public float ScreenWidth;
    public float ScreenHeight;

    private float t;
    private RectTransform rect;
    private UiManager _UiManager;


    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] Image image;
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
        _UiManager.stoneGague_Instance = this;
        InitializeUI(GameManager.GetManagerClass<CharacterManager>().playerInstance.transform, _UiManager.MainCamera);


        
        rect = gameObject.GetComponent<RectTransform>();
        // 
        gameObject.SetActive(false);
    }

   

    public void InitializeUI(Transform target, Camera cam)
    {
        Cam = cam;
        TargetPos = target;
    }

    public void OnEnable()
    {
        t = 0;
        image.fillAmount = t;
    }

    

    private void Update()
    {


        ScreenX = ScreenWidth * Cam.WorldToViewportPoint(TargetPos.position).x - x;
        ScreenY = ScreenHeight * Cam.WorldToViewportPoint(TargetPos.position).y - y;

        t += Time.deltaTime * 0.83f;
        t = Mathf.Clamp(t,0, 1);
        image.fillAmount = t ;

        //transform. position = new Vector3(ScreenX, ScreenY);
      
        rect.position = new Vector3(ScreenX, ScreenY);
    }
}
