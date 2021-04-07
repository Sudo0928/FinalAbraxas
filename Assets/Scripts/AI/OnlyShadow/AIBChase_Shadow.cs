using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBChase_Shadow : AIBehaviorBase
{
    private AILightDetect lightDetect;
    private ParticleManager pm;
    private Move _playerIntance = null;
    private bool bisDistance = false;
    private EnemyAnim Anim;
   [SerializeField] private float _MoveSpeed;

    protected override void Awake()
    {
        base.Awake();

    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

       
        lightDetect = gameObject.GetComponent<AILightDetect>();
        _playerIntance = _characterManager.playerInstance;
        Anim = behaviorController.anim;
        pm = GameManager.GetManagerClass<ParticleManager>();
    }


    public override void Run()
    {

        Agent.isStopped = false;
        Agent.speed = _MoveSpeed;
             
        IEnumerator Behavior()
        {

            while (true)
            {

                Agent.SetDestination(_playerIntance.transform.position);
                Vector3 tmpVec = Agent.velocity.normalized;

                float xDir = tmpVec.x > 0.5f ? 1 : tmpVec.x < -0.5f ? -1 : 0;
                float zDir = tmpVec.z > 0.5f ? 1 : tmpVec.z < -0.5f ? -1 : -0;
                Vector3 adjVec = new Vector3(xDir, 0, zDir);

                Agent.Move(adjVec *_MoveSpeed * Time.deltaTime);

                
                


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

                else if(!_playerIntance.bisSound)
                {

                    statenum = 0;
                    break;
                }

                if (Vector3.Distance(transform.position, _playerIntance.transform.position) < 0.3f)
                {
                    if (!bisDistance)
                    {
                        bisDistance = true;

                        Agent.isStopped = true;


                        Anim.anim.SetTrigger("Hit");
                       
                    }
                }


                 if (Anim.KeyDictionary["RobotAttack"])
                {
                    statenum = 0;

                    pm.SetEffect(transform.position, 1);

                    int direction = (Vector3.Distance(transform.position, _playerIntance.Front.position) - Vector3.Distance(transform.position, _playerIntance.Back.position)) > 0 ? 1 : -1;
                    _playerIntance.HitAction(direction);

                    _playerIntance.bisSound = false;
                    Anim.KeyDictionary["RobotAttack"] = false;
                    bisDistance = false;
                    break;
                }


               

                yield return null;
            }
            behaviorFinished = true;


        }
        StartCoroutine(Behavior());
        lightDetect.playerAttatch = false;

    }
}


