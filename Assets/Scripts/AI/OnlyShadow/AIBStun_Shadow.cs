using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBStun_Shadow : AIBehaviorBase
{
    private float _stunStartTime;
    private AILightDetect lightDetect;
    private EnemyAnim Anim;
    [SerializeField] private GameObject ui;

    private Vector3 RunAwayPos;
    // 소리를 들었나?

    protected override void Awake()
    {
        base.Awake();

    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        lightDetect = gameObject.GetComponent<AILightDetect>();
        Anim = behaviorController.anim;

    }


    private bool CheckDestination(Vector3 pos , float range , out Vector3 result)
    {
        while (true)
        {
            Vector3 randPos = pos + Random.insideUnitSphere * range;
            NavMeshHit hit;
            randPos.y = 13.74f;
            if (NavMesh.SamplePosition(randPos, out hit, 0.5f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
           

        }
        
    }


    public override void Run()
    {
        Agent.isStopped = true;
        _stunStartTime = Time.time;
        ui.SetActive(true);
        Anim.speedchanger(0);
        IEnumerator Behavior()
        {
            while (true)
            {

                if (lightDetect.bigLight)
                {
                    statenum = 4;
                    break;

                }
                
                else if(!lightDetect.smallLight)

                {
                    statenum = 0;
                    break;
                }
                
                else if (Time.time - _stunStartTime > 3.0f)
                {
                    statenum = 4;
                    break;
                  
                }
                yield return null;
            }
            behaviorFinished = true;
        }

        StartCoroutine(Behavior());
        Anim.speedchanger(1);
        ui.SetActive(false);

    }
}
