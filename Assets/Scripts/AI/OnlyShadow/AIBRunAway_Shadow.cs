using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBRunAway_Shadow : AIBehaviorBase
{

   
    //private Vector3 RunAwayPos;
    private AILightDetect lightDetect;
    private Vector3 runDir;

    private float runAwaytime;
    private bool bisTurn = false;

    [SerializeField]private GameObject ui;


    protected override void Awake()
    {
        base.Awake();

    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        lightDetect = gameObject.GetComponent<AILightDetect>();
        behaviorBeginEvent += initializeRun;

    }


    private void initializeRun()
    {
        runAwaytime = Time.time;
        /*
        float x, z;
        lightDetect.RunVector(out x, out z);
        x *= Random.Range(1, 9);
        z *= Random.Range(1, 9);
        */

       
      
        runDir = transform.forward * -1;
       

        // runDir = new Vector3(x,0,z).normalized;



    }

    private void RotateToTarget()
    {
        if (bisTurn) return;
        Quaternion lookRotation = Quaternion.LookRotation(runDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * 540.0f);

        

    }

    private bool CheckDestination(Vector3 pos, out Vector3 result)
    {
        while (true)
        {
            Vector3 randPos = pos + Random.insideUnitSphere *3.0f;
            NavMeshHit hit;
            randPos.y = transform.position.y;
            if (NavMesh.SamplePosition(randPos, out hit, 0.5f, NavMesh.AllAreas))
            {


                //result = hit.position;
                result = (transform.position - hit.position).normalized;
                
                    return true;
                
            }


        }

    }


    private void DestinationChange()
    {
        // Quaternion Rotation = Quaternion.Euler(0, 70.0f, 0);

        runDir += new Vector3(0, 45.0f, 0).normalized;
    }




    public override void Run()
    {
        //runDir = transform.forward * -1;
        //Agent.isStopped = true;
        
        //Agent.enabled = false;
        bisTurn = false;
       
        Agent.speed = 2.0f;
        IEnumerator Behavior()
        {
            while (true)
            {
                // transform.Translate(transform.forward * Time.deltaTime * -2.0f);

                Agent.Move(runDir * Time.deltaTime *2);
                
                RotateToTarget();

               
               if(Time.time - runAwaytime > 4.0f)
                {
                    statenum = 0;
                    break;
                }


                yield return null;
            }
            behaviorFinished = true;
        }

        StartCoroutine(Behavior());
        ui.SetActive(false);
        bisTurn = false;
        //Agent.enabled = true;

    }
}
