using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public sealed class AIBPatrol_Robot : AIBehaviorBase
{
    [Header("평소 이동 속도")]
    [SerializeField] private float _MoveSpeed = 1.3f;

    [Header("정찰 구역")]
    [SerializeField] private Transform PatrolDistrict = null;

    [Header("목표 정찰 지점")]
    [SerializeField] private int PatrolIndex = 0;

  
    [Header("목표 도착 거리")]
    [SerializeField] private float PathEndDis = 0;

    //전에 로봇을 자극 시킨 소리 
    private float prevDV = 0;

    private float _PatrolStartTime;

    // 왜 만들었냐고? 실행 순서 문제다.
    private bool bIsTimeOver = false;

    // 소리를 들었나?
    private bool bIsListened = false;
    private List<Vector3> PatrolAreas = new List<Vector3>();
    private FieldOfView fov;
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
        fov = gameObject.GetComponentInChildren<FieldOfView>();
        fov.gameObject.SetActive(false);
        _characterManager.Sound += Listen;
        Anim = behaviorController.anim;


    }
    private void WalkToArea(Vector3 pos)
    {
        Agent.speed = 1.7f;
        behaviorController.uiManager.SpawnUI(this.transform, Color.red,0,0);
        statenum = 3;
        Agent.SetDestination(pos);

    }

    private void WalktoDestroy(Vector3 pos)
    {
        Agent.speed = 1.2f;
        behaviorController.uiManager.SpawnUI(this.transform, Color.red, 0, 0);
        statenum = 4;
        Agent.SetDestination(pos);


    }



    public void Listen(Vector3 soundPos, int dv)
    {
       

        //if (Time.time - _PatrolStartTime < 1.0f) return;
        if (!bIsTimeOver) return;
        if (dv < 1) return;

        float SoundDis = Vector3.Distance(transform.position, soundPos);

        if (SoundDis > 3) return;

        if (prevDV >= dv) return;
        prevDV = dv;

       

        switch (dv)
        {
            case 1:
                if (SoundDis > 2 && SoundDis < 2.5)
                {
                  
                    behaviorController.uiManager.SpawnUI(this.transform, Color.blue,0,0);
                    bIsListened = true;
                    behaviorController.SoundPos = soundPos;
                }
                else
                {
                   
                    WalkToArea(soundPos);
                }
                break;

            case 2:
                
                WalkToArea(soundPos);
                break;

            case 3:
                if (SoundDis > 2 && SoundDis < 2.5)
                {
                    
                    behaviorController.uiManager.SpawnUI(this.transform, Color.blue,0,0);
                    bIsListened = true;
                    behaviorController.SoundPos = soundPos;
                }
                else
                {
                    WalktoDestroy(soundPos);
                }
                break;
        }


    }
 
    private void setFree()
    {
        fov.gameObject.SetActive(false);
    }
    
    public void viewout()
    {
        fov.Forcechange(1.1f);
        fov.gameObject.SetActive(false);
    }
    
    public override void Run()
    {

        prevDV = 0;
        statenum = 0;
        sight.isDetected = false;
        _PatrolStartTime = Time.time;
        Agent.SetDestination(PatrolAreas[PatrolIndex]);
        Agent.isStopped = false;
        Agent.speed = _MoveSpeed;
        fov.ChangeState("Reduction", 3);
        Invoke("setFree",1.0f);

        IEnumerator Behavior()
        {
            bIsListened = false;

          

            while (true)
            {

                if (Time.time - _PatrolStartTime < 0.6f)  bIsTimeOver = true; 



                if (sight.isDetected)
                {                   
                    sight.isDetected = false;
                    statenum = 2;
                    break;
                }
                else
                {
                    if (bIsListened && !sight.isDetected)
                    {
                        bIsListened = false;
                        statenum = 4;
                        break;
                    }
                }

              

                if (Agent.remainingDistance < PathEndDis && !sight.isDetected)
                {
                    Agent.isStopped = true;
                    Agent.velocity = Vector3.zero;
                    prevDV = 0;
                    if (statenum == 0) { PatrolIndex = PatrolIndex == PatrolAreas.Count - 1 ? 0 : PatrolIndex + 1; Agent.SetDestination(PatrolAreas[PatrolIndex]); Agent.SetDestination(PatrolAreas[PatrolIndex]); statenum = 1; }
                    else if(statenum == 3) { fov.gameObject.SetActive(true); fov.ChangeState("Expand", 3); }
                    else { Anim.anim.SetTrigger("Hit"); statenum = 1; }
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
