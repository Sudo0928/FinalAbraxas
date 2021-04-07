using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBPatrol_Nurse : AIBehaviorBase
{
    [Header("평소 이동 속도")]
    [SerializeField] private float _MoveSpeed = 1.3f;

    [Header("정찰 구역")]
    [SerializeField] private Transform PatrolDistrict = null;

    [Header("목표 정찰 지점")]
    [SerializeField] private int PatrolIndex = 0;


    [SerializeField] private float SoundDis;

    [Header("목표 도착 거리")]
    [SerializeField] private float PathEndDis = 0;

   
    private float _PatrolStartTime;

    // 왜 만들었냐고? 실행 순서 문제다.
    private bool bIsTimeOver = false;

    // 소리를 들었나?
    private bool bIsListened = false;
    private List<Vector3> PatrolAreas = new List<Vector3>();
   
    private EnemyAnim Anim;

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
        Anim = behaviorController.anim;


    }
   

  

    public void Listen(Vector3 soundPos, int dv)
    {


        if (Time.time - _PatrolStartTime < 1.0f) { return; }
        if (Vector3.Distance(transform.position, soundPos) > SoundDis) return;

            if (dv != 2) return;
        if (bIsListened) return ;
        behaviorController.uiManager.SpawnUI(this.transform, Color.red, 0f, 90.0f);

         bIsListened = true;  Agent.SetDestination(soundPos); Agent.speed = 0.6f; 
    }


    

   

    public override void Run()
    {

        statenum = 0;
        sight.isDetected = false;
        _PatrolStartTime = Time.time;
        Agent.SetDestination(PatrolAreas[PatrolIndex]);
        Agent.isStopped = false;
        Agent.speed = _MoveSpeed;
      
      
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
               


                if (Agent.remainingDistance < PathEndDis && !sight.isDetected)
                {
                    Agent.isStopped = true;
                    Agent.velocity = Vector3.zero;

                    PatrolIndex = PatrolIndex == PatrolAreas.Count - 1 ? 0 : PatrolIndex + 1; Agent.SetDestination(PatrolAreas[PatrolIndex]); statenum = 1;
                    break;
                }

                yield return null;

            }

            bIsTimeOver = false;

            behaviorFinished = true;

        }

        StartCoroutine(Behavior());



    }
}
