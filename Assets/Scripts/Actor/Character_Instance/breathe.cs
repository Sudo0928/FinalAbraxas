using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breathe : MonoBehaviour
{

    // 0.2초 마다 한 번 씩 숨 소리를 낸다
    private float BreatheStartTime = 0;
    private BreatheHoldUI ui;
    // 숨 참는 시간
    private float BreatheHoldTime =5.0f;

    private Move _playerinstance = null;
    private PlayerAnim anim;
    private CharacterManager _characterManager = null;
    private UiManager _uiManager = null;
    private bool bisGauge = true;
    private float _breatheOutTime;
    // 숨을 참고 있는가?
    public bool bHoldBreathe { get; set; } = false;
    public CharacterInfo Info;
    [SerializeField] float x;
    [SerializeField] float y;

    
    private void Start()
    {
        
        _characterManager = GameManager.GetManagerClass<CharacterManager>();
        _uiManager = GameManager.GetManagerClass<UiManager>();
        _playerinstance = _characterManager.playerInstance;
        anim = gameObject.GetComponentInChildren<PlayerAnim>();
        anim.bre = this;
        //ui.playerInstance = _playerinstance;

    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z) && !bHoldBreathe)
        {
            
            if (!(_playerinstance.CurState == Move.CharacterState.Idle || _playerinstance.CurState == Move.CharacterState.Walk)) return;
            if (!Info.bisAction) return;

            if(!_playerinstance.bIsStateChangeAble) return;
            if (!bisGauge) return;

            //ui.gameObject.SetActive(true);
            _uiManager.ShowAndHide(_uiManager.breatheHoldUI_instance.gameObject, true);
            _uiManager.breatheHoldUI_instance.HoldAnim(5 - BreatheHoldTime);
            _playerinstance.ChangeState(Move.CharacterState.breathe, 4); 
            bHoldBreathe = true;


        }
        if(Input.GetKeyUp(KeyCode.Z) && bHoldBreathe)
        {
            bHoldBreathe = false;
            _uiManager.ShowAndHide(_uiManager.breatheHoldUI_instance.gameObject, false);
            if (_playerinstance.CurState != Move.CharacterState.Hit)
            {
                if(_playerinstance.CurState != Move.CharacterState.Die)
                _playerinstance.ChangeState(Move.CharacterState.Idle, 0);
            }
        }

        if(!bisGauge)
        {
            if(Time.time - _breatheOutTime > 3.0f)
            {
                bisGauge = true;
            }
        }

        switch(bHoldBreathe)
        {

            case true:

                BreatheHoldTime = Mathf.Clamp(BreatheHoldTime -= Time.deltaTime, 0, 5.0f);
                if (BreatheHoldTime <= 0.5) { BreatheHoldTime = 0; bHoldBreathe = false; _playerinstance.ChangeState(Move.CharacterState.Idle, 0); _uiManager.ShowAndHide(_uiManager.breatheHoldUI_instance.gameObject, false); bisGauge = false; _uiManager.SpawnUI(transform,x,y); _breatheOutTime = Time.time; }

                break;

            case false:
                BreatheHoldTime = Mathf.Clamp(BreatheHoldTime += Time.deltaTime, 0, 5.0f);
                if ((Time.time - BreatheStartTime > 0.5f))
                {
                    if (_playerinstance.bisSound)
                    {
                        _characterManager.Sound(transform.position, 0);
                    }
                        BreatheStartTime = Time.time;

                   
                }
                break;

        }


       
    }
}
