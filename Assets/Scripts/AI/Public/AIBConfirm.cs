using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBConfirm : AIBehaviorBase
{

   

     private float _WatchStartTime;                    // 감시 시작 시간 
     private float _BreatheListendTime;                // 숨 소리 듣기 시작한 시간
    private Transform cam;
    private FieldOfView view;

    [SerializeField] private float _MaximumWatchTime;  // 언제 까지 감시할건가??

    [SerializeField] private watchUI ui; // 나중에 ui Manager에서 관리하게 바꾸거라

    [SerializeField] private float SoundDis;

   
    private bool bIsListened;  // 발 소리를 들었는가?
    private int SoundGauge;  //숨소리 : 5번 들으면 가즈아
    

    private Move _playerInstance = null;

    private int decivel; // 소리 세기 확인 1: 숨 2: 걷는거 3: ㅗ

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
      
        _playerInstance = _characterManager.playerInstance;
        cam = GameManager.GetManagerClass<CameraManager>().MainCamera.transform;
        ui.cam = cam;
        _characterManager.Sound += Listen;
        anim = gameObject.GetComponentInChildren<EnemyAnim>();
        view = gameObject.GetComponentInChildren<FieldOfView>();
    }

    public void Listen(Vector3 soundPos, int dv)
    {
        if (Vector3.Distance(transform.position, soundPos) < SoundDis)
        {  
            decivel = dv;
            switch(decivel)   // 들은 소리가
            {
                case 0:                     // 숨 소리라면 
                    SoundGauge += 1;
                    _BreatheListendTime = Time.time;
                    ui.bisSound = true;
                    ui.Add();
                    SoundGauge = Mathf.Clamp(SoundGauge, -1, 6);

                    break;

                case 2:                    // 발 소리라면
                    bIsListened = true;
                    //behaviorController.uiManager.SpawnUI(this.transform, Color.red);

                   statenum = 2;
                    ui.erase();
                    break;

               

            }            
        }
    }


   

    public override void Run()
    {

        SoundGauge = 0;
        
        bIsListened = false;
        Agent.isStopped = true;
        Agent.velocity = Vector3.zero;
        _WatchStartTime = Time.time;
      
        ui.gameObject.SetActive(true);
        ui.Reset();
        //anim.speedchanger(0);


        IEnumerator Behavior()
        {

            while (true)
            {
                Agent.isStopped = true;

                /*
                if(sight.isDetected)
                {
                    statenum = 2;

                    break;

                }
                */

                if(Time.time - _BreatheListendTime > 0.5f)
                {
                    ui.bisSound = false;
                    _BreatheListendTime = Time.time; 
                    SoundGauge -= 1;
                    SoundGauge = Mathf.Clamp(SoundGauge, -1, 6);
                }

                if(SoundGauge >=5)
                {                   
                    statenum = 2;
                    ui.bisExclamtionDisplay = true;

                    break;
                }


                if (bIsListened)
                {                   
                    bIsListened = false;  break;                   
                }

                if (Time.time - _WatchStartTime >= 2.0f && SoundGauge < 0 )
                 {
                    ui.erase();
                    statenum = 0; break;
                 }
                
               
                yield return null;
            }
            //anim.speedchanger(1);
            behaviorFinished = true;  
            
           
        }
        StartCoroutine(Behavior());

    }
}
