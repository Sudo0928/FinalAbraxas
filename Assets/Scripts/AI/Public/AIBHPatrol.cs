using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public sealed class AIBHPatrol : AIBehaviorBase
{
    [Header("평소 이동 속도")]
    [SerializeField] private float _MoveSpeed = 3.0f;

   

    [Header("정찰 구역")]
    [SerializeField] private Transform PatrolDistrict = null;

    [Header("목표 정찰 지점")]
    [SerializeField] private int PatrolIndex = 0;

    [Header("소리 감지 거리")]
    [SerializeField] private float SoundDis = 0;

    [Header("목표 도착 거리")]
    [SerializeField] private float PathEndDis = 0;


    private float _PatrolStartTime;

    // 소리를 들었나?
    private bool bIsListened = false;
  

   

    #region Components
    //에이전트
    

    

    #endregion

    private List<Vector3> PatrolAreas = new List<Vector3>();


   
    


    protected override void Awake()
    {
        base.Awake();

    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
       
        foreach (Transform tr in PatrolDistrict) PatrolAreas.Add(tr.position);

       
       
        _characterManager.Sound += Listen;
      
       
    }
   
   
    public void Listen(Vector3 soundPos, int dv)
    {


        if (Time.time - _PatrolStartTime < 1.0f) {  return; }

        if (dv != 2) return;

        if (Vector3.Distance(transform.position, soundPos) < SoundDis) { bIsListened = true; }
       
        
        
    }


#region 의사 걷다가 멈추는 거
    public void StopWalk()
    {
        Agent.isStopped = true;
       
    }

    public void WalkAgain()
    {
        Agent.isStopped = false;
    }


#endregion


    public override void Run()
    {
       
       
        Agent.SetDestination(PatrolAreas[PatrolIndex]);
       
        Agent.isStopped = false;

      
       
        Agent.speed = _MoveSpeed;

        _PatrolStartTime = Time.time;
       
        IEnumerator Behavior()
        {

            bIsListened = false;
          
            while (true)
            {
                if (sight.isDetected)
                {
                  
                    sight.isDetected = false;
                    statenum = 2;
                   
                    break;
                }
                else
                {
                    if (bIsListened&&!sight.isDetected)
                    {
                        behaviorController.uiManager.SpawnUI(this.transform, Color.red, 0f,90.0f);

                       bIsListened = false; statenum = 1; break;

                    }
                }
               


                if(Vector3.Distance(transform.position,PatrolAreas[PatrolIndex]) < PathEndDis && !sight.isDetected) {
                    Agent.isStopped = true;
                    Agent.velocity = Vector3.zero;
                    statenum = 1;
                    PatrolIndex = PatrolIndex == PatrolAreas.Count - 1 ? 0 : PatrolIndex + 1; Agent.SetDestination(PatrolAreas[PatrolIndex]);
                    break;
                }

             
                yield return null; 

            }

            behaviorFinished = true;
            
        }

        StartCoroutine(Behavior());
        
        
        
    }
}
