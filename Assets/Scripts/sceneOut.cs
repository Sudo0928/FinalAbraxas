using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class sceneOut : MonoBehaviour
{
    private Move Player;
    private UiManager uiManager;
    private bool playonce = true;
    [SerializeField] private GameObject Fade;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject scene;
    [SerializeField] private GameObject gameover;

    private void Start()
    {
        Player = GameManager.GetManagerClass<CharacterManager>().playerInstance;
        uiManager = GameManager.GetManagerClass<UiManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {

            scene.SetActive(true);
            cam.SetActive(false);

            Player.Info.bisAction = false;
            Player.bisSound = false;
            Player.gameObject.SetActive(false);
           // Player.CineMove(transform.position, true);
            uiManager.ShowAndHide(uiManager.Hp[0].gameObject, false);
            uiManager.ShowAndHide(uiManager.Hp[1].gameObject, false);
            uiManager.ShowAndHide(uiManager.Hp[2].gameObject, false);
            uiManager.ShowAndHide(uiManager.stoneUI_instance.gameObject, false);
            playonce = false;
            
            Invoke("THEEND", 4.0f);
        }
    }

    private void THEEND()
    {
        Fade.SetActive(true);
        Invoke("TheEnd", 2.0f);
        
    }


    private void TheEnd()
    {
        SceneManager.LoadScene("ThanksToPlay");

    }


    private void Update()
    {
        if(!playonce)
        {
            gameover.SetActive(false);
        }
    }
}
