using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIBehaviorBase : MonoBehaviour
{
    [Header("행동 시작 / 끝 지연 시간")]
    [SerializeField] protected float m_BehaviorBeginDelay = 0.0f;
    [SerializeField] protected float m_BehaviorFinalDelay = 0.0f;

    public bool allowBehaviorStart { get; set; }

    public bool behaviorFinished { get; set; }

    public System.Action behaviorBeginEvent { get; set; }
    public System.Action behaviorFinalEvent { get; set; }

    public BehaviorController behaviorController { get; private set; }

    public float behaviorBeginDelay => m_BehaviorBeginDelay;
    public float behaviorFinalDelay => m_BehaviorFinalDelay;

    public EnemyAnim anim { get; set; }

    public NavMeshAgent Agent { get; set; }

    public CharacterManager _characterManager { get; set; } = null;

    public int statenum { get; set; } = 0;
    public AISight sight { get; set; }
    protected virtual void Awake()
    {
        behaviorController = GetComponent<BehaviorController>();
           
        behaviorBeginEvent = () => allowBehaviorStart = true;

    }

    protected virtual void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        sight = GetComponent<AISight>();
        _characterManager = GameManager.GetManagerClass<CharacterManager>();
    }


   

    public virtual void InitializeBehavior()
    {
        allowBehaviorStart = behaviorFinished = false;
    }

    public abstract void Run();



}
