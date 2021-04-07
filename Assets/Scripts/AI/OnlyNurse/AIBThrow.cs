using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIBThrow : AIBehaviorBase
{
    private Move _playerInstance = null;

    //[SerializeField] Detect detect;
   
    [SerializeField] private Injection inject_instance;
    private ObjectPool<Injection> _InjectPool = null;
    public bool bisHit  { get; set; }



    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        bisHit = false;
       
        _playerInstance = _characterManager.playerInstance;
        _InjectPool = new ObjectPool<Injection>();
        
    }

    public void throwAction()
    {
        Injection injection = _InjectPool.GetRecyclableObject() ??
         _InjectPool.RegisterRecyclableObject(Instantiate(inject_instance));

        if (injection.gameObject.activeSelf)
            injection.isActive = true;
        else
            injection.gameObject.SetActive(injection.isActive = true);

        Vector3 throwpos = transform.position;
        throwpos.y += 0.5f;

        injection.initializeInjection(_playerInstance, throwpos,transform.forward,this);

      
    }







    public override void Run()
    {
        Agent.isStopped = true;
        throwAction();

        IEnumerator Behavior()
        {

            while (true)
            {

                if (bisHit)
                {
                    statenum = 0;
                    bisHit = false;
                    sight.isWatchable = true;
                    sight.isDetected = false;
                    _playerInstance.bisSound = false;
                    sight.StartSight();
                    break;
                }

                yield return null;
            }
            behaviorFinished = true;


        }
        StartCoroutine(Behavior());

    }

}
