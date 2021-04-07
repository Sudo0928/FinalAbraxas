using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;

public class ShadowMonster : MonoBehaviour
{
    public bool arrive = false;
    public bool _InLight = false;  // 연빛
    public bool _2InLight = false;  //진 빛
    public bool RunAway = false;
    public bool find = false;

    public float _2Range = 0;     

    public float opacity = 1;         

    public SkinnedMeshRenderer mesh;

    private MaterialPropertyBlock MPB;

    public List<Transform> RandomTrs = new List<Transform>();

    private Transform player;

    private Move _playerInstance;

    private Vector3 RandomTr;

    private NavMeshAgent agent;

    private Animator animator;

    float ina = 1;

    private Light[] lights;
    private Color color = Color.green;


    private CharacterManager _characterManager;

    #region FSM

    private enum MonsterState { Patrol, Detect, Stun  ,Chase, RunAway ,Rage }
    private MonsterState CurrentState;




    #endregion


    #region Var 


    [SerializeField] private float SoundDis; // 소리 감지 거리 

    private int prevDv;    // 현재 기억하고 있는 소리의 크기
    //private Vector3 memoryVector; // 

    private Vector3[] PatrolPos = new Vector3[4];
    private int patrolIndex = 0; // 정찰 위치 순회용 


    private bool bisListened; // 현재 소리를 들은 상태냐..?


    private float patrolStartTime = 0; // 탐지를 시작한 시간


    private Vector3 OriginVec;


    #endregion

    #region Component

    private EnemyAnim enemyAnim;


    #endregion 


    private void Awake()
    {
        MPB = new MaterialPropertyBlock();
        agent = GetComponent<NavMeshAgent>();
        //animator = this.transform.GetChild(0).GetComponent<Animator>();
        enemyAnim = gameObject.GetComponentInChildren<EnemyAnim>();
    }

    private void Start()
    {
        // lights = gameManager.lights;
        // player = gameManager.Player;

        _characterManager = GameManager.GetManagerClass<CharacterManager>();
        _playerInstance = _characterManager.playerInstance;
        RandomTr = RandomTrs[Random.Range(0, RandomTrs.Count)].position;
        OriginVec = transform.position;
    }

    private void ChangeState(MonsterState state, int num)
    {
        //animator.SetInteger("Val", num);
        CurrentState = state;
        enemyAnim.ChangeAnim(num);
    }


    #region StateMachine


    

    private void PatrolState()
    {
        if (agent.remainingDistance < 0.3f)
        {
            if (!bisListened)
            {
              patrolIndex = patrolIndex >= 3 ? 0 : patrolIndex += 1;
              agent.SetDestination(PatrolPos[patrolIndex]);
            }
        }
    }


    private void DetectState()
    {
        if (Time.time - patrolStartTime > 4.0f)
        {
            ChangeState(MonsterState.Patrol, 0);
            agent.SetDestination(OriginVec);
        }

        if (agent.remainingDistance < 0.3f)
        {
            if (!bisListened)
            {
                patrolIndex = patrolIndex >= 3 ? 0 : patrolIndex += 1;
                agent.SetDestination(PatrolPos[patrolIndex]);
            }
        }
    }


    private void ChaseState()
    {
        if (agent.remainingDistance < 0.3f)
        {
            animator.SetTrigger("Hit");
            
        }

        if(enemyAnim.KeyDictionary["RobotAttack"])
        {
            int direction = (Vector3.Distance(transform.position, _playerInstance.Front.position) - Vector3.Distance(transform.position, _playerInstance.Back.position)) > 0 ? 1 : -1;
            _playerInstance.HitAction(direction);
            enemyAnim.KeyDictionary["RobotAttack"] = false;
            ChangeState(MonsterState.Patrol, 0);
            agent.SetDestination(OriginVec);
            

        }

    }


    private void RunAwayState()
    {

    }


    private void StunState()
    {

    }



    private void RageState()
    {

    }


    #endregion

    private void Listen(Vector3 Pos, int Dv)
    {

        if (Vector3.Distance(Pos, transform.position) > SoundDis) return;
        if (prevDv >= Dv) return;

        prevDv = Dv;
        bisListened = true;

       switch(CurrentState)
        {
            case MonsterState.Patrol:
                bisListened = false;
                agent.SetDestination(Pos);
                ChangeState(MonsterState.Detect, 1);
                patrolStartTime = Time.time;
                break;

            case MonsterState.Detect:
                bisListened = false;
                agent.SetDestination(_playerInstance.transform.position);
                ChangeState(MonsterState.Chase, 2);
                break;


        }



    }






    private void Update()
    {/*
        StartCoroutine(MonsterAI());
        */
        LightDetect();
        switch (CurrentState)
        {
            case MonsterState.Patrol:
                PatrolState();
                break;
            case MonsterState.Detect:
                DetectState();
                break;
            case MonsterState.Chase:
                ChaseState();
                break;
            case MonsterState.RunAway:
                RunAwayState();
                break;

            case MonsterState.Stun:
                StunState();
                break;
            case MonsterState.Rage:
                RageState();
                break;

        }

    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
        //find가 감지했을 떄 true
            find = true;

            agent.speed = 1.3f;
            animator.SetInteger("Val", 1);
            agent.SetDestination(player.position);
        }
    }
    */
    bool CheckInLight(Light light, Vector3 dir)
    {
        if (light.type.Equals(LightType.Spot))
        {
            return CheckViewAngle(light.transform.forward, dir, light.spotAngle);
        }
        else if (light.type.Equals(LightType.Point))
        {
            return true;
        }
        return false;
    }

    void LightInfluenceRange(Light light, LayerMask layerMask)
    {
        Vector3 dir = (transform.position - light.transform.position).normalized;
        Ray ray = new Ray(light.transform.position, dir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, light.range - 2, layerMask) && hit.transform.Equals(transform))
        {
            _2InLight |= CheckInLight(light, dir);
            return;
        }
        else if(Physics.Raycast(ray, out hit, light.range, layerMask) && hit.transform.Equals(transform))
        {
            _InLight |= CheckInLight(light, dir);
            return;
        }
        _InLight |= false;
        _2InLight |= false;
    }

    public bool CheckViewAngle(Vector3 from, Vector3 to, float angle)
    {
        if(Vector3.Angle(from, to) < angle * 0.5f)
        {
            return true;
        }
        return false;
    }

    public void LightDetect()
    {
        LayerMask layerMask = -1 - (1 << LayerMask.NameToLayer("Player"));

        _InLight = false;
        _2InLight = false;
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i].gameObject.activeSelf)
            {
                LightInfluenceRange(lights[i], layerMask);
            }
        }

        if(_2InLight) SetOpacity(0.6f, true);
        else if(_InLight) SetOpacity(0.9f, true);
        else SetOpacity(1f, true);
    }

    #region SetOpacity

    public void SetOpacity(float val, bool lerp)
    {
        MPB.SetFloat(Shader.PropertyToID("_Opacity"), opacity = lerp ? opacity = Mathf.Lerp(opacity, val, Time.deltaTime) : opacity = val);
        mesh.SetPropertyBlock(MPB);
    }

    public void SetOpacity(float val, bool lerp, float lerpspeed)
    {
        MPB.SetFloat(Shader.PropertyToID("_Opacity"), opacity = lerp ? opacity = Mathf.Lerp(opacity, val, Time.deltaTime * lerpspeed) : opacity = val);
        mesh.SetPropertyBlock(MPB);
    }

    #endregion

    public IEnumerator MonsterAI()
    {
        LightDetect();
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude / 3);
        if (!RunAway) // RunAway -> 빛에 닿으면 활성화(랜턴 빛)
        {
            if(_2InLight)  
            {
                RandomTr = RandomTrs[Random.Range(0, RandomTrs.Count)].position;
                RunAway = true;
                agent.isStopped = false;
            }
            else if(_InLight)
            {
                animator.SetInteger("Val", 0);
                agent.isStopped = true;
            }
            else if(find)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
                if (Physics.Raycast(new Ray(transform.position, transform.forward), 1f, 1 << LayerMask.NameToLayer("Player")))
                {
                    agent.isStopped = true;
                    animator.SetInteger("Val", 2);
                    if(animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    {
                        find = false;
                        agent.isStopped = false;
                    }
                }
            }
            else if(arrive) // 도망 갈 떄 설정
            {
                RandomTr = RandomTrs[Random.Range(0, RandomTrs.Count)].position;
                arrive = false;
            }
            else
            {
                agent.isStopped = false;
                animator.SetInteger("Val", 1);
                agent.SetDestination(RandomTr);
                if (RandomTr.x == this.transform.position.x && RandomTr.z == this.transform.position.z)
                {
                    arrive = true;
                    agent.speed = 1f;
                }
            }
        }
        else
        {
            animator.SetInteger("Val", 1);
            agent.SetDestination(RandomTr);
            if (RandomTr.x == transform.position.x && RandomTr.z == transform.position.z)
            {
                RunAway = false;
            }
        }
        yield return new WaitForSeconds(0.1f);
    }
}
