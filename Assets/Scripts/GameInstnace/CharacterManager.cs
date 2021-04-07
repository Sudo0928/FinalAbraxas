using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour , IManager
{

    public GameManager gameManager { get { return GameManager.gameManager; } }
    public Move playerInstance { get; set; }
    public bool bisRebirth = false;
    public CharacterInfo _characterInfo;
    [SerializeField] private GameObject fade;
    [SerializeField] private GameObject GameOver;

    public delegate void SoundDelegate(Vector3 vector, int decivel);   //소리 작동
    public SoundDelegate Sound;
    private UiManager uiManager;
    
    private Vector3 pos;




    private ObjectPool<Stone> _StonePool = null;
    public Stone stone_instance;

    public void Awake()
    {
       
        _characterInfo = Resources.Load<CharacterInfo>("CharacterInfo/CharacterInfo");

       
       

        _StonePool = new ObjectPool<Stone>();
    }

    private void Start()
    {
        uiManager = GameManager.GetManagerClass<UiManager>();
    }

    public void FallSound(string alpa)
    {
        gameManager.soundOut(alpa);
    }

    

    public Stone Throwstone()
    {
        Stone stone = _StonePool.GetRecyclableObject() ??
         _StonePool.RegisterRecyclableObject(Instantiate(stone_instance));

        if (stone.gameObject.activeSelf)
            stone.isActive = true;
        else
            stone.gameObject.SetActive(stone.isActive = true);

        return stone;

    }

    public void Die()
    {
        
       
        playerInstance.ChangeState(Move.CharacterState.Die, 7);

        

        uiManager.ShowAndHide(uiManager.Hp[0].gameObject, false);
        uiManager.ShowAndHide(uiManager.Hp[1].gameObject, false);
        uiManager.ShowAndHide(uiManager.Hp[2].gameObject, false);
        uiManager.ShowAndHide(uiManager.stoneUI_instance.gameObject, false);
        Invoke("Fadeout", 2.0f);

    }

    public void SetPos(Vector3 pos)
    {
        this.pos = pos;
    }



    public void fadein()
    {
       
        uiManager.ShowAndHide(uiManager.Hp[0].gameObject, true);
        uiManager.ShowAndHide(uiManager.Hp[1].gameObject, true);
        uiManager.ShowAndHide(uiManager.Hp[2].gameObject, true);
        uiManager.ShowAndHide(uiManager.stoneUI_instance.gameObject, true);
        _characterInfo.hp = 3;
        _characterInfo.bisAction = true;
        playerInstance.bisSound = true;
        uiManager.HPUp();
        
        
        playerInstance.ForceLivve();
        playerInstance.ChangeState(Move.CharacterState.Idle, 0);
        playerInstance.ForceLivve();
        fade.SetActive(false);
        GameOver.SetActive(false);
        bisRebirth = false;
    }


    public void Rebirth()
    {
        playerInstance.ChangeState(Move.CharacterState.cinemachine, 0);
        playerInstance.transform.position = pos;
        GameOver.SetActive(true);




        Invoke("wait", 6.0f);

      
    }

    public void wait()
    {
        bisRebirth = true;
    }


    public void Fadeout()
    {

        fade.SetActive(true);

        Invoke("Rebirth", 2.0f);

    }


}
