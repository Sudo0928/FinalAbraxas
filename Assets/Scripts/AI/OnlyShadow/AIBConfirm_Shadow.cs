using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBConfirm_Shadow : AIBehaviorBase
{
    [Header("평소 이동 속도")]
    [SerializeField] private float _MoveSpeed = 1.3f;

    
    [Header("목표 도착 거리")]
    [SerializeField] private float PathEndDis = 0;

    private AILightDetect lightDetect;

   
    private float _PatrolStartTime;
    private bool bisListened = false;

    private Move _playerInstance;

    // 왜 만들었냐고? 실행 순서 문제다.
    private bool bIsTimeOver = false;

    private UiManager uiManager;

    private Vector3 TargetVec;
    private Vector3 CenterVec;

    public void Listen(Vector3 soundPos, int dv)
    {
        
        if (bisListened) return;

        float SoundDis = Vector3.Distance(transform.position, soundPos);

        switch (dv)
        {
            case 0:
                break;

            case 1:
                if (SoundDis > 1.5f) return;
                break;
            case 2:
                if (SoundDis > 2.0f) return;
                break;
            case 3:
                if (SoundDis > 2.0f) return;
                break;

        }

        if (dv == 2) {
            statenum = 1;
           // uiManager.SpawnUI(transform, Color.red, 0, 30);
            bisListened = true;
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

       
        lightDetect = gameObject.GetComponent<AILightDetect>();
        _characterManager.Sound += Listen;
        _playerInstance = _characterManager.playerInstance;
        uiManager = GameManager.GetManagerClass<UiManager>();

    }

    private bool CheckDestination(Vector3 pos, float range, out Vector3 result)
    {
        while (true)
        {
            Vector3 randPos = pos + Random.insideUnitSphere * range;
            NavMeshHit hit;
            randPos.y = transform.position.y;
            if (NavMesh.SamplePosition(randPos, out hit, 0.5f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }


        }

    }

    public override void Run()
    {

        
        statenum = 0;
        _PatrolStartTime = Time.time;
        bisListened = false;
        CenterVec = transform.position;
        CheckDestination(transform.position, 3, out TargetVec);
        Agent.SetDestination(TargetVec);
        Agent.isStopped = false;
        lightDetect.playerAttatch = false;
        Agent.speed = _MoveSpeed;

        IEnumerator Behavior()
        {
            



            while (true)
            {

                if (Time.time - _PatrolStartTime > 6.0f) bIsTimeOver = true;
                
              
                if (lightDetect.bigLight)
                {
                    statenum = 4;
                    break;
                }
               else if (lightDetect.smallLight)
                {
                    statenum = 3;
                    break;
                }


                else if(bisListened)
                {
                    statenum = 1;
                    bisListened = false;
                    Debug.Log("cause by Confirm_Sound");
                    break;
                }
                else if(lightDetect.playerAttatch &&   _playerInstance.bisSound)
                {
                    statenum = 1;
                    lightDetect.playerAttatch = false;
                    bisListened = false;
                    Debug.Log("cause by Confirm_Attatch");
                    break;
                  
                }
                else if(bIsTimeOver&& !bisListened)
                {
                    statenum = 0;
                    break;
                }

               else if(Agent.remainingDistance < 0.3f && !bisListened)
                {
                    CheckDestination(CenterVec, 3, out TargetVec);
                    Agent.SetDestination(TargetVec);
                }


             
                yield return null;

            }

            bIsTimeOver = false;

            behaviorFinished = true;

        }

        StartCoroutine(Behavior());



    }
}
