using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public sealed class AIBCharge_Robot : AIBehaviorBase
{
   
    private float _chargeStartTime;
    private bool bisCollide;
    private bool bisPlayerHit;
    private EnemyAnim Anim;
    private Move _playerIntance;
    private ParticleManager pm;
  

    protected override void Awake()
    {
        base.Awake();

    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _playerIntance = _characterManager.playerInstance;
        pm = GameManager.GetManagerClass<ParticleManager>();
      
        Anim = behaviorController.anim;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("Props"))
        {
          
            bisCollide = true;
        }
        else if(other.gameObject.CompareTag("Player"))
        {
            bisPlayerHit = true;
        }
    }
    
   


    public override void Run()
    {
        bisPlayerHit = false;
        bisCollide = false;
        Debug.Log("charge");
        Agent.isStopped = true;
        Agent.enabled = false;     
        _chargeStartTime = Time.time;
        Vector3 targetPos = behaviorController.SoundPos;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);
        Vector3 vec = transform.forward;
        vec.y = 0;
      

        IEnumerator Behavior()
        {

            bisPlayerHit = false;
            while (true)
            {
                transform.Translate(vec * Time.deltaTime * 4.0f, Space.World);
                
                if (bisPlayerHit)
                {
                   
                    statenum = 5;
                    Anim.anim.SetBool("Col", bisCollide);
                    pm.SetEffect(transform.position, 1);

                    int direction = (Vector3.Distance(transform.position, _playerIntance.Front.position) - Vector3.Distance(transform.position, _playerIntance.Back.position)) > 0 ? 1 : -1;
                    _playerIntance.HitAction(direction);


                  

                    _playerIntance.bisSound = false;
                    break;
                }
              
                else if (bisCollide)
                {
                  
                    statenum = 5;
                    Anim.anim.SetBool("Col", bisCollide);
                    bisCollide = false;
                 
                    break;
                }

                else if(Time.time - _chargeStartTime   > 2.0f)
                {
                    
                    statenum = 5;
                    bisCollide = false;
                    Anim.anim.SetBool("Col", bisCollide);

                    break;
                }

                yield return null;

            }

            behaviorFinished = true;

        }

        StartCoroutine(Behavior());



    }
}
