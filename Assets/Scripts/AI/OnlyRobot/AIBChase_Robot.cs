using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBChase_Robot : AIBehaviorBase
{
    private Move _playerIntance = null;
    private ParticleManager pm;
   
    private EnemyAnim Anim;
    private bool bisDistance = false;
    private AIBPatrol_Robot patrol;
  
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        _playerIntance = _characterManager.playerInstance;
        pm = GameManager.GetManagerClass<ParticleManager>();
        patrol = gameObject.GetComponent<AIBPatrol_Robot>();
        Anim = behaviorController.anim;
    }

   

    public override void Run()
    {
        Debug.Log("chase");
      
        Agent.isStopped = false;
        Agent.speed = 2.0f;
        Agent.angularSpeed = 800f;
        sight.isWatchable = false;
        patrol.viewout();
        sight.StopSight();


        IEnumerator Behavior()
        {

            while (true)
            {
              
                Agent.SetDestination(_playerIntance.transform.position);
                Vector3 tmpVec = Agent.velocity.normalized;


                float xDir = tmpVec.x > 0.5f ? 1 : tmpVec.x < -0.5f ? -1 : 0;
                float zDir = tmpVec.z > 0.5f ? 1 : tmpVec.z < -0.5f ? -1 : -0;
                Vector3 adjVec = new Vector3(xDir, 0, zDir);

                Agent.Move(adjVec * Time.deltaTime);

                //if(!_playerIntance.bisSound) { statenum = 0; break; }     


              


                if (Vector3.Distance(transform.position, _playerIntance.transform.position) < 0.6f)
                {
                    if (!bisDistance)
                    {
                        bisDistance = true;

                        Agent.isStopped = true;
                      

                        Anim.anim.SetTrigger("Hit");
                        //Debug.Log(Vector3.Distance(transform.position, _playerIntance.transform.position));
                    }
                }


                if(Anim.KeyDictionary["RobotAttack"])
                {
                    statenum = 0;

                    pm.SetEffect(transform.position, 1);

                    int direction = (Vector3.Distance(transform.position, _playerIntance.Front.position) - Vector3.Distance(transform.position, _playerIntance.Back.position)) > 0 ? 1 : -1;
                    _playerIntance.HitAction(direction);



                    sight.isWatchable = true;
                   sight.isDetected = false;
                    _playerIntance.bisSound = false;
                    sight.StartSight();

                    Anim.KeyDictionary["RobotAttack"] = false;
                    bisDistance = false;
                    break;
                }

                


                yield return null;
            }
            behaviorFinished = true;


        }
        StartCoroutine(Behavior());

    }

}
