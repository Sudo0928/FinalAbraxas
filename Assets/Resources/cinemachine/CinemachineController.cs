using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CinemachineController : MonoBehaviour
{
    private PlayableDirector playableDirector;


    private bool _playOnce = true;
    private Move _playerinstance;
    private SoundManager sound;
    private UiManager uimaanger;
    [SerializeField]private GameObject Scene;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject Lantern;
    [SerializeField] private GameObject rab;
    [SerializeField] private float Time;
    [SerializeField] private GameObject etc;
   [SerializeField] private bool bisEnd;
    [SerializeField] private bool UIOut;
    [SerializeField] private bool bisCinemove;

    private void Awake()
    {
        playableDirector = gameObject.GetComponentInChildren<PlayableDirector>();
        Scene.SetActive(false);   

    }
    private void Start()
    {
        _playerinstance = GameManager.GetManagerClass<CharacterManager>().playerInstance;
        sound = GameManager.GetManagerClass<SoundManager>();
        uimaanger = GameManager.GetManagerClass<UiManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && _playOnce)
        {
            

           
            Scene.SetActive(true);
            playableDirector.Play();

            _playerinstance.Info.bisAction = false;
            _playOnce = false;
            _playerinstance.ChangeState(Move.CharacterState.cinemachine, 0);
           // _playerinstance.transform.position = transform.position;
            uimaanger.ShowAndHide(uimaanger.stoneUI_instance.gameObject, false);
            uimaanger.ShowAndHide(uimaanger.Hp[0].gameObject, false);
            uimaanger.ShowAndHide(uimaanger.Hp[1].gameObject, false);
            uimaanger.ShowAndHide(uimaanger.Hp[2].gameObject, false);

            if (bisCinemove) { _playerinstance.CineMove(transform.position, true); }
            if (Lantern) Lantern.SetActive(false);
           
            if(UI) UI.SetActive(false);
            if (rab) rab.SetActive(false);
        }
    }

    private IEnumerator timer()
    {
       
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("newhos");
    }

    private void Update()
    {


        if (playableDirector.time >= Time)
        {
           
           
            _playerinstance.ChangeState(Move.CharacterState.Idle, 0);
            
            if (!bisEnd)
            {
                Scene.SetActive(false);
                gameObject.SetActive(false);

                if (!UIOut)
                {
                    uimaanger.ShowAndHide(uimaanger.stoneUI_instance.gameObject, true);
                    uimaanger.ShowAndHide(uimaanger.Hp[0].gameObject, true);
                    uimaanger.ShowAndHide(uimaanger.Hp[1].gameObject, true);
                    uimaanger.ShowAndHide(uimaanger.Hp[2].gameObject, true);
                }
                if (Lantern) Lantern.SetActive(true);
                if (UI) UI.SetActive(true);
                if (rab) {  rab.SetActive(true); _playerinstance.Info.bisAction = true; }

            }
           
          

           
            if(etc)
            {
                if (bisEnd) { etc.SetActive(true); StartCoroutine(timer()); sound.Volume(false,true); }
                else etc.SetActive(false);
            }
        }
    }
}
