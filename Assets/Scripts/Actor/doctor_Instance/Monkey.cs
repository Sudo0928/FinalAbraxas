using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    private EnemyAnim anim;
    private enum MonkeyState {Idle, Sound, Count, Break}
    private MonkeyState CurState;
    private float _soundStartTime = 0;
    private CharacterManager characterManager;
    private int _soundStateNum = 0;
   public Animator UI;
    public Animator noiseUI;
    public Animator Sleep;

    private void Start()
    {
        characterManager = GameManager.GetManagerClass<CharacterManager>();
        anim = gameObject.GetComponentInChildren<EnemyAnim>();
        UI.gameObject.SetActive(false);
        noiseUI.gameObject.SetActive(false);
        Sleep.gameObject.SetActive(false);
    }


    private void ChangeState(MonkeyState state, int num)
    {
        CurState = state;
        anim.ChangeAnim(num);
    }



    private void SoundState()
    {
        if(Time.time - _soundStartTime > 0.2f)
        {
            _soundStartTime = Time.time;
            _soundStateNum += 1;
            characterManager.Sound(transform.position,3);
        }
        
        if(_soundStateNum > 24)
        {
            _soundStartTime = Time.time;
            _soundStateNum = 0;
            noiseUI.gameObject.SetActive(false);
            Sleep.gameObject.SetActive(true);
            ChangeState(MonkeyState.Break, 3);
        }

    }

    private void CountState()
    {

        if (Time.time - _soundStartTime > 3.0f)
        {
            ChangeState(MonkeyState.Sound, 1);
            _soundStartTime = Time.time;
            UI.gameObject.SetActive(false);
            noiseUI.gameObject.SetActive(true);
        }



    }


    private void Dead()
    {
        ChangeState(MonkeyState.Break, 3);
        _soundStartTime = Time.time;
        _soundStateNum = 0;
        noiseUI.gameObject.SetActive(false);
        Sleep.gameObject.SetActive(true);
    }

    private void BreakState()
    {

        if (Time.time - _soundStartTime > 10.0f)
        {
            ChangeState(MonkeyState.Idle, 0);
            _soundStartTime = Time.time;
            anim.speedchanger(1.0f);
            Sleep.gameObject.SetActive(false);
        }

    }

    private void StateMachine()
    {
        switch(CurState)
        {
            case MonkeyState.Sound:
                SoundState();
                break;

            case MonkeyState.Count:
                CountState();
                break;

            case MonkeyState.Break:
                BreakState();
                break;

        }
    }


    private void Update()
    {
        StateMachine();
    }



    private void OnTriggerEnter(Collider other)
    {
       

        switch(CurState)
        {

            case MonkeyState.Idle:
                if (other.gameObject.CompareTag("Player"))
                {
                    ChangeState(MonkeyState.Count, 0);
                    UI.gameObject.SetActive(true);
                    UI.Play("listen",-1);
                    _soundStartTime = Time.time;
                }
                    break;

            case MonkeyState.Sound:
                if (other.gameObject.CompareTag("Enemy"))
                {

                    Invoke("Dead", 1.0f);
                }
                break;
        }


    }


}
