using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public sealed class AIBStun : AIBehaviorBase
{
    private float _stunStartTime;
   

    // 소리를 들었나?
  
    protected override void Awake()
    {
        base.Awake();

    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
      

    }


    public override void Run()
    {
        
        _stunStartTime = Time.time;
        IEnumerator Behavior()
        {
            while (true)
            {
               

                if (Time.time - _stunStartTime > 3.0f)
                {
                    statenum = 0;
                    
                    Agent.enabled = true;
                    Agent.isStopped = false;
                    break;
                }
                yield return null;               
            }
            behaviorFinished = true;
        }

        StartCoroutine(Behavior());



    }
}
