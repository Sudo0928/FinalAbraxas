using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //public GameManager gameManager { get { return GameManager.gameManager; } }



    #region Variables


    [SerializeField]private float MoveSpeed = 100f;
    private float InputHorizon;
    private float InputVertical;

    private int TotlaStone = 4;
    private int CurrentStone = 4;
    
    public bool tempmove { get; set; } = true;

    private float pressed_time = 0;

 
    private bool bIspressedOver = false;
   
    public bool bIsActionOver { get; set; } = false;

    private Vector3 MoveDir;
   [SerializeField] private Vector3 TempDir;

    private GameObject Lantern;
    // 던지기 이전 상태값을 저장할 변수
    private bool prevThrowInput;

    // 던지기 키를 누르기 시작한 시간
    private float throwStartInputTime;

    // 누르고 있었던 시간
   private float throwInputTime;

    // 후 벽에 가려젔을 떄 이전 입력
    private bool prevSignal;

    private float HitStartTime =0;
    private float SoundMuteTime = 0;


    private float soundtime;
    public bool bisEnd { get; set; } = false;


    public bool bisSound { get; set; } = true;

    //랜턴을 들고 있냐?
    public bool bisLantern { get; set; } = false;
    private bool bispressedlan;

    private float HitDirection = 1;


    private Vector3 CinePos = Vector3.zero;
   [SerializeField] private bool bisCineMove = false;

   
    #endregion
    #region Component

    private CharacterController Controller;

    [SerializeField] private PlayerAnim Normal;
    [SerializeField] private PlayerAnim TransParent;

    private PlayerAnim Anim;
    public CharacterInfo Info;

    private CharacterManager CharacterManager = null;
    private UiManager uiManager = null;

    Transform path;
    [SerializeField] Stone stoneInstance;
    [SerializeField] Transform stoneshot;
    [SerializeField] EndMenu end;
    
    public Transform Front { get; set; }
    public Transform Back { get; set; }
    
    #endregion
    #region Function
    public void CineMove( Vector3 vec , bool go = false)
    {
        ChangeState(CharacterState.cinemachine, 1);
        CinePos = vec;
        bisCineMove = go;  

    }


    private void InputKey()
    {

        if (CurState == CharacterState.Die) return;

        Info.moveDir_z = InputHorizon = Input.GetAxisRaw("Horizontal");
        Info.moveDir_x = InputVertical = Input.GetAxisRaw("Vertical");

        // if(Info.StoneLeft > 0)Throw();  // 던질 돌이 남아있다면 던지기를 수행한다.

        if ((InputHorizon != 0 || InputVertical != 0) && bisSound)
        {
            CharacterManager.Sound(transform.position, 2);
            if (Controller.velocity.magnitude > 1.0f)
            {
                if (Time.time - soundtime > 0.35f)
                {
                    CharacterManager.FallSound("walk");
                    soundtime = Time.time;
                }
            }
        }


        // isSound가 true라면 걸을 때 마다 소리가 난다. 

        if (Input.GetKeyUp(KeyCode.C) && bispressedlan)
        {
            bisLantern = true;
            bispressedlan = false;
        }


        if (Input.GetKeyDown(KeyCode.C) && bisLantern)
        {
            if (!Info.bisAction) return;

            if (Lantern)
            {
                Lantern.transform.position = transform.position;
                Lantern.SetActive(true);
                Lantern = null;
            }
            bisLantern = false;
            Anim.LanternAction(bisLantern);

        }
        
        TempDir = path.transform.right * InputHorizon + path.transform.forward * InputVertical;
        TempDir = TempDir.normalized;
        // 이동 방향인 tempDir은 카매라의 자식 오브젝트인 path의 방향으로 결정된다. 

       
            MoveDir = TempDir;
        
       

    }



    

    #region Throw :: 돌 던지는 함수
    private void Throw()
	{
        if (!Info.bisAction) return;


        if (Input.GetKey(KeyCode.X))
        {
            
            if (!prevThrowInput && CurState != CharacterState.Walk)
            {
                

                    ChangeState(CharacterState.Throw, 2);
                    throwStartInputTime = Time.time;
                    prevThrowInput = true;
                    uiManager.ShowAndHide(uiManager.stoneGague_Instance.gameObject, true);

                Debug.Log(prevThrowInput);

            }

            throwInputTime = Time.time - throwStartInputTime;
        }
        if (prevThrowInput && Input.GetKeyUp(KeyCode.X) )
        {

            prevThrowInput = false;

            Debug.Log("왜 그래");
            throwInputTime = Mathf.Clamp(throwInputTime, 0.0f, 1.2f);
            // 누른 시간이 1.2 초를 초과할 수 없도록 합니다.
            Anim.StartAgain();
           
        }
        if (bIsActionOver)
        {   // 던지기 동작 실행
            uiManager.ShowAndHide(uiManager.stoneGague_Instance.gameObject, false);
            Stone stone= CharacterManager.Throwstone();
            uiManager.stoneUI_instance.UISet();
            stone.initializeStone(CharacterManager,stoneshot.position , stoneshot.position - transform.forward *2.0f * (throwInputTime +0.1f ));
           // stone.initializeStone(CharacterManager,transform.position , transform.position - transform.forward *2.0f * (throwInputTime +0.1f ));
            Info.StoneLeft -= 1;
            
            bIsActionOver = false;
            if (CurState != CharacterState.Hit)
            {
                if(CurState != CharacterState.Die)
                ChangeState(CharacterState.Idle, 0);
            }

        }



    }

    #endregion
    #region Rotate :: 항상 움직이는 방향으로 회전하게 만든다

    private void Rotate()
    {
        float yawAngle = MoveDir.ToAngle();
        Vector3 YawVector = new Vector3(0.0f, yawAngle , 0.0f);

        if (!Mathf.Approximately(MoveDir.sqrMagnitude, 0.0f))
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(YawVector), 0.4f);
               
    }
    #endregion
    #region SeeThroughtWall :: 벽 넘어서 보는 코루틴 
    private IEnumerator SeeThroughtWall()
    {
        while(true)
        {
            yield return new WaitUntil(() => prevSignal != Info.WallSignal);
            prevSignal = Info.WallSignal;
            if (Info.WallSignal)
            {
                TransParent.gameObject.SetActive(true);
                Normal.gameObject.SetActive(false);
                Anim = TransParent;
                Anim.LanternAction(bisLantern);
                
            }
            else
            {
                TransParent.gameObject.SetActive(false);
                Normal.gameObject.SetActive(true);
                Anim = Normal;
                Anim.LanternAction(bisLantern);
            }
        }
    }
    #endregion
    #region HitAction :: 처 맞았을 떄 호출 되는 함수다.

    public void HitAction(float hitdir , bool damage = true)
    {
        if (CurState == CharacterState.Hit) return;
        //bisSound = false;
       


        CharacterManager.FallSound("hit");

        if (bisLantern)
        {
            Lantern.transform.position = transform.position;
            Lantern.SetActive(true);
            bisLantern = false;
            Anim.LanternAction(bisLantern);

        }

        if(damage)
        {
            uiManager.HPDown(Info.hp);
            Info.hp -= 1;
           
        }

        uiManager.ShowAndHide(uiManager.stoneGague_Instance.gameObject, false);
        //prevThrowInput = false;
        HitDirection = hitdir;
        HitStartTime = Time.time; SoundMuteTime = Time.time;
        
        if (Info.hp > 0)
        {
            ChangeState(CharacterState.Hit, 3);
            Anim.GetHit(HitDirection);
        }
        else
        {
            
           ChangeState(CharacterState.Die, 7);
            CharacterManager.Die();
            bisSound = false;
            Info.bisAction = false;
            uiManager.ShowAndHide(uiManager.stoneGague_Instance.gameObject, false);
            Anim.GetDie(HitDirection);
            
        }
      
    }
    /*
    public void HitAction(float hitdir, int damage)
    {
        if (CurState == CharacterState.Hit) return;
        //bisSound = false;
        Info.hp -= damage;
        prevThrowInput = false;

        if (Lantern)
            {
                Lantern.transform.position = transform.position;
                Lantern.SetActive(true);
            bisLantern = false;
            Anim.LanternAction(bisLantern);

        }
            

        HitDirection = hitdir;
        HitStartTime = Time.time; SoundMuteTime = Time.time; Anim.GetHit(HitDirection);
        ChangeState(CharacterState.Hit, 3);


    }
    */


    #endregion

    #endregion
    #region State

    public enum CharacterState{Idle =0,Walk =1,Throw =2 ,breathe , Hit , cinemachine ,Die }
    public bool bIsStateChangeAble  { get; set; } = true;

    public CharacterState CurState;

    #endregion
    #region StateMachine

    private void UpdateState()
    {
        switch (CurState)
        {
            case CharacterState.Idle:
                IdleState();
                break;
            case CharacterState.Walk:
                WalkState();
                break;
            case CharacterState.Throw:
                ThrowState();
                break;
            case CharacterState.Hit:
                HitState();
                break;
            case CharacterState.cinemachine:

                break;
        }
    }



    #endregion

    #region StateFunction


    #region ChangeState
    public void ChangeState(CharacterState state,int a)
    {
        CurState = state;
       Anim.SetAnim(a,bisLantern);
       
    }



    public void ForceLivve()
    {
       
        Anim.ForceLive();
    }
    #endregion

    #region Idle 
    private void IdleState()
    {
        InputKey();
        bIsStateChangeAble = true;
        if (TempDir.magnitude > 0.2) ChangeState(CharacterState.Walk, 1);

    }
    #endregion
    #region Walk 
    private void WalkState()
    {
        InputKey();
        bIsStateChangeAble = true;
        Anim.SetAnim(1);
        
        if (TempDir.magnitude < 0.1) ChangeState(CharacterState.Idle, 0);

    }
    #endregion
    #region Throw
    private void ThrowState()
    {
        // if (TempDir.magnitude > 0.2) ChangeState(CharacterState.Walk, 1);
        //if (Info.StoneLeft > 0) Throw();


    }
    private void CinemachineState()
    {


    }


    #endregion
    #region Hit
    private void HitState()
    {
        bIsStateChangeAble = false;
        if (Time.time - HitStartTime > 1.0f) ChangeState(CharacterState.Idle, 0);
        Controller.Move(transform.forward* 0.75f * Time.deltaTime * HitDirection);
    }
    #endregion

    #endregion





    private void Awake()
    {

        Controller= gameObject.GetComponent<CharacterController>();
        CharacterManager = GameManager.GetManagerClass<CharacterManager>();
        CharacterManager.playerInstance = this;
        uiManager = GameManager.GetManagerClass<UiManager>();
        //Anim = gameObject.GetComponentInChildren<PlayerAnim>();
        Front = GameObject.Find("Front").transform;
        Back = GameObject.Find("Back").transform;

    }


   

   

    // Start is called before the first frame update
    void Start()
    {
        path = GameManager.GetManagerClass<CameraManager>().Path;
        Anim = TransParent;
        Anim.Player = this;
        bisCineMove = false;
        Anim.gameObject.SetActive(false);
        Anim = Normal;
        Anim.Player = this;
       

        //벽 뚫고 보는 코루틴을 실행한다.
        StartCoroutine(SeeThroughtWall());
    }

    // Update is called once per frame
    void Update()
    {if(Input.GetKeyDown(KeyCode.Escape ) && !bisEnd)
        {
            Time.timeScale = 0;
            end.gameObject.SetActive(true);
            bisEnd = true;
        }

        if (bisEnd) return;


        UpdateState();
        if (CurState == CharacterState.Hit) return;


        if (Input.anyKeyDown && CharacterManager.bisRebirth)
        {
            CharacterManager.bisRebirth = false;
            CharacterManager.fadein();

        }

        //InputKey();
        Rotate();
        //MoveDir.y -= 180.0f * Time.deltaTime;
        if (Info.StoneLeft > 0)
        {if(CurState != CharacterState.breathe)
            Throw();
        }
        if (!bisSound && Time.time - SoundMuteTime > 2.5f) {

            Anim.bisStartColor = false;
            if(Info.bisAction) bisSound = true;

        }

        if (CurState == CharacterState.Walk)
        {
            MoveDir.y -= 7.5f;
            Controller.Move(MoveDir * MoveSpeed * Time.deltaTime);

        }

        else if(CurState == CharacterState.Hit)
        {
            MoveDir.y -= 7.5f;
            Controller.Move(MoveDir * MoveSpeed * Time.deltaTime);
        }

        else if(CurState == CharacterState.cinemachine)
        {
            if(bisCineMove)
            {
                
                transform.position = Vector3.MoveTowards(transform.position,CinePos, 1.0f *Time.deltaTime);
                transform.LookAt(CinePos);
                if (Vector3.Distance(transform.position,CinePos) < 0.3f)
                {
                    if (Lantern)
                    {
                        Lantern.transform.position = transform.position;
                        Lantern.SetActive(true);
                        Lantern.GetComponent<BoxCollider>().enabled = false;
                       
                        bisLantern = false;
                        Anim.LanternAction(bisLantern);
                        ChangeState(CharacterState.Idle, 0);
                        Lantern = null;
                    }

                    Anim.SetAnim(0);
                    bisLantern = false;
                }
               

            }
        }
        


       

       
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Lantern"))
        {
           
            if(Input.GetKeyDown(KeyCode.C) && !bisLantern)
            {
               
                Lantern = other.gameObject;
               Lantern.SetActive(false);

               
                Anim.LanternAction(true);
                bispressedlan = true;
            }
        }
    }
}



public static class Vector3Extensions
{
    public static float ToAngle(this Vector3 directionVector)
    {
        directionVector.Normalize();
        return Mathf.Atan2(directionVector.x, directionVector.z) * Mathf.Rad2Deg;
    }

}
