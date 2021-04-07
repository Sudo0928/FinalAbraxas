using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBPatrol_Shadow : AIBehaviorBase
{
    [Header("평소 이동 속도")]
    [SerializeField] private float _MoveSpeed = 1.3f;

    [Header("정찰 구역")]
    [SerializeField] private Transform PatrolDistrict = null;

    [Header("목표 정찰 지점")]
    [SerializeField] private int PatrolIndex = 0;


    [Header("목표 도착 거리")]
    [SerializeField] private float PathEndDis = 0;

    private AILightDetect lightDetect;

    //전에 로봇을 자극 시킨 소리 
    private float prevDV = 0;

    private Move _playerInstance;

    private float _PatrolStartTime;
    private bool bisListend = false;

    private bool bisAttracted = false;
    // 왜 만들었냐고? 실행 순서 문제다.
    private bool bIsTimeOver = false;

    private bool bisPlayerAttatch = false;

    private List<Vector3> PatrolAreas = new List<Vector3>();
    private UiManager uiManager;

    public void Listen(Vector3 soundPos, int dv)
    {


        //if (Time.time - _PatrolStartTime < 1.0f) return;
        //if (!bIsTimeOver) return;
        if (dv < 1) return;

        float SoundDis = Vector3.Distance(transform.position, soundPos);


        switch(dv)
        {
            case 1:
                if (SoundDis >3.0f) return;
                break;
            case 2:
                if (SoundDis > 1.5f) return;
                break;
            case 3:                
                if (SoundDis > 1.5f) return;
                break;

        }

       // if (SoundDis > 1) return;
        if (dv == 0) return;


        switch(bisAttracted)
        {
            case true:

                if (Time.time - _PatrolStartTime < 1.0f) return;
                if (bisListend) return;

                uiManager.SpawnUI(transform, Color.red, 0, 90.0f);
                
                bisListend = true;
                statenum = 1;
                break;


            case false:
                if (prevDV >= dv) return;
                prevDV = dv;

                Agent.SetDestination(soundPos);
                Agent.speed = 0.8f;
                _PatrolStartTime = Time.time;
                if (prevDV == 2) bisAttracted = true; 
                uiManager.SpawnUI(transform, Color.yellow, 0, 90.0f);
               
                statenum = 2;
                break;
              



        }

        

        
    }

    protected override void Awake()
    {
        base.Awake();

    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        foreach (Transform tr in PatrolDistrict) PatrolAreas.Add(tr.position);
        lightDetect = gameObject.GetComponent<AILightDetect>();
        _characterManager.Sound += Listen;
        _playerInstance = _characterManager.playerInstance;
        uiManager = GameManager.GetManagerClass<UiManager>();
     

    }

    


    public override void Run()
    {

        prevDV = 0;
        statenum = 0;
        bisListend = false;
        bisAttracted = false;
        //_PatrolStartTime = Time.time;
       

        Agent.SetDestination(PatrolAreas[PatrolIndex]);
        Agent.isStopped = false;
        Agent.speed = _MoveSpeed;
      
        IEnumerator Behavior()
        {

           


            while (true)
            {

                //if (Time.time - _PatrolStartTime < 0.6f) bIsTimeOver = true;
               /*
                if(lightDetect.smallLight)
                {
                    statenum = 3;
                    bisListend = false;
                    bisAttracted = false;
                    break;
                }
                */
                 if(lightDetect.bigLight)
                {
                    statenum = 4;
                    bisListend = false;
                    bisAttracted = false;
                    break;
                }

                else if (lightDetect.smallLight)
                {
                    statenum = 3;
                    bisListend = false;
                    bisAttracted = false;
                    break;
                }
                else if(bisListend)
                {
                    statenum = 1;
                    bisListend = false;
                    bisAttracted = false;
                    Debug.Log("cause by Sound");
                    break;
                }
                 /*
                else if(lightDetect.playerAttatch && _playerInstance.bisSound)
                {
                    statenum = 1;
                    bisListend = false;
                    bisAttracted = false;
                    lightDetect.playerAttatch = false;
                    Debug.Log("cause by Attatch");
                    break;
                }
                */
                else if(Vector3.Distance(_playerInstance.transform.position,transform.position) < 0.3f)
                {
                    if (_playerInstance.bisSound)
                    {
                        statenum = 1;
                        bisListend = false;
                        bisAttracted = false;
                        Debug.Log("cause by Attatch");
                        break;
                    }
                }
              
                //&& !lightDetect.smallLight
                if (Agent.remainingDistance < PathEndDis && !lightDetect.smallLight)
                {
                    //Agent.isStopped = true;
                    //Agent.velocity = Vector3.zero;
                    prevDV = 0;
                    if (statenum == 0) { PatrolIndex = PatrolIndex == PatrolAreas.Count -1 ?   0 : PatrolIndex + 1; Agent.SetDestination(PatrolAreas[PatrolIndex]); }
                    else { break; }

                    
                }

                yield return null;

            }

            bIsTimeOver = false;

            behaviorFinished = true;

        }

        StartCoroutine(Behavior());
        bisListend = false;
        bisAttracted = false;


    }
}
