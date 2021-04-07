using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorController : MonoBehaviour
{
    private List<AIBehaviorBase> _AIBehaviors;
    public UiManager uiManager { get; set; }
    public AIBehaviorBase nextBehavior;

    private IEnumerator _Behavior;

    //[SerializeField]private EnemyAnim anim;
    //public Animator anim { get; set; }

    public EnemyAnim anim { get; set; }

    private int statenum = 0;

    public Vector3 SoundPos { get; set; }


    public Vector3 MemoryPos { get; set; } = Vector3.zero;

    private void Awake()
    {
        _AIBehaviors = new List<AIBehaviorBase>(GetComponents<AIBehaviorBase>());

        foreach (AIBehaviorBase ai in _AIBehaviors) ai.enabled = false;

        //foreach (AIBehaviorBase ai in _AIBehaviors) ai.anim = anim;
        anim = GetComponentInChildren<EnemyAnim>();

    }

    private void Start()
    {
        uiManager = GameManager.GetManagerClass<UiManager>();
       
        StartBehavior();
    }


   
   

    private IEnumerator Behavior()
    {
        int nextBehaviorIndex = 0;
        nextBehavior = _AIBehaviors[nextBehaviorIndex];
        WaitUntil waitAllowBehaviorstarted = new WaitUntil(
            () => nextBehavior.allowBehaviorStart);

        WaitUntil waitBehaviorFinished = new WaitUntil(
            () => nextBehavior.behaviorFinished);

        while(true)
        {
            nextBehavior.enabled = false;
            nextBehaviorIndex = nextBehavior.statenum;
            nextBehavior = _AIBehaviors[nextBehaviorIndex];
            nextBehavior.enabled = true;

            anim.ChangeAnim(nextBehaviorIndex);
            // nextBehavior = _AIBehaviors[nextBehaviorIndex];

            // 다음 행동 순서로 변경합니다.
            /* nextBehaviorIndex = (nextBehaviorIndex == _AIBehaviors.Count - 1) ?
                 0 : ++nextBehaviorIndex;*/

           

            if (Mathf.Approximately(nextBehavior.behaviorBeginDelay, 0.0f))
                yield return new WaitForSeconds(nextBehavior.behaviorBeginDelay);

            nextBehavior.behaviorBeginEvent?.Invoke();

            yield return waitAllowBehaviorstarted;
                 
            nextBehavior.Run();
                   
            yield return waitBehaviorFinished;

            nextBehavior.behaviorFinalEvent?.Invoke();

            if(Mathf.Approximately(nextBehavior.behaviorFinalDelay, 0.0f))
                yield return  new WaitForSeconds(nextBehavior.behaviorFinalDelay);

            nextBehavior.InitializeBehavior();

        }
        


    }

    public void StartBehavior()
    {
        if (_Behavior != null)
        {
            // 행동을 멈춥니다.
            StopCoroutine(_Behavior);

            // 모든 행동을 초기화합니다.
            foreach (var behavior in _AIBehaviors)
                behavior.InitializeBehavior();
        }

        // 행등올 시작합니다.
        StartCoroutine(_Behavior = Behavior());
    }


   

}
