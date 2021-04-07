using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnScreen : MonoBehaviour, IRecyclableGameObject
{
    public bool isActive { get; set; }
   
    public Transform target { get; set; }

    public float ScreenX { get; set; }
    public float ScreenY { get; set; }             // 끼야야앗
    public Camera cam { get; set; }

    public float ScreenWidth { get; set; } = 1920;
    public float ScreenHeight { get; set; } = 1024;
   
    public float _startTime { get; set; }
    public RectTransform rect { get; set; }

    public float adjust_x { get; set; }
    public float adjust_y { get; set; }
    [SerializeField] private float disTime;


    public virtual void initializeUI(Transform transform, Camera camera,float x, float y )
    {

        if(!rect) rect = gameObject.GetComponent<RectTransform>();
        
        this.cam = camera;
        target = transform;
        adjust_x = x;
        adjust_y = y;
        _startTime = Time.time;
    }

    public virtual void Erase()
    {
        if(Time.time - _startTime > disTime)
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void OnDisable()
    {
        isActive = false;
    }


    public virtual void Update()
    {
        ScreenX = (ScreenWidth * cam.WorldToViewportPoint(target.position).x - adjust_x);
        ScreenY = (ScreenHeight * cam.WorldToViewportPoint(target.position).y - adjust_y);
        rect.position = new Vector3(ScreenX - 10,  ScreenY + 180.0f ,0);
        Erase();
        
    }
}
