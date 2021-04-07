using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBHWatch : AIBehaviorBase
{

    [Header("추적 감시 시간 (0 이라면 계속 추적합니다.)")]
    [SerializeField] private float _MaximumWatchTime;

    [Header("최대 시야 거리")]
    [SerializeField] private float _WatchDist;

    // 헤드 


    // 소리를 들었는지 확인
    private bool bIsListened = false;

    // 감시 시작 시간
    private float _WatchStartTime;



    private Move _playerinstance;

  
    private int decivel;



    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        _characterManager.Sound += Listen;
        _playerinstance = _characterManager.playerInstance;
    }



    public void Listen(Vector3 soundPos, int dv)
    {

        if (Vector3.Distance(transform.position, soundPos) < _WatchDist && !bIsListened)
        {
            if (Time.time - _WatchStartTime < 1.5f) return;

            decivel = dv;

            behaviorController.MemoryPos = soundPos;
            bIsListened = true;

        }

    }


    public void Rotate()
    {
        Quaternion lookRotation = Quaternion.LookRotation(_playerinstance.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 1.6f);
        //if (Mathf.Approximately(transform.rotation.eulerAngles.y,lookRotation.eulerAngles.y)) transform.LookAt(_playerinstance.transform.position);
    }

    public override void Run()
    {

        sight.isDetected = false;
        bIsListened = false;
        Agent.isStopped = true;
        _WatchStartTime = Time.time;


        IEnumerator Behavior()
        {


            while (true)
            {
                Agent.isStopped = true;
                Rotate();

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

                        switch (decivel)
                        {
                            case 0:
                                statenum = 3;
                                bIsListened = false;
                                break;

                            case 2:
                                behaviorController.uiManager.SpawnUI(this.transform, Color.red, 0f, 90.0f);
                                bIsListened = false;
                                statenum = 2;
                                break;
                        }
                        break;
                    }

                }
                if (Time.time - _WatchStartTime >= _MaximumWatchTime)
                { statenum = 0; break; }


                yield return null;

            }

            behaviorFinished = true;

        }

        StartCoroutine(Behavior());



    }
}
