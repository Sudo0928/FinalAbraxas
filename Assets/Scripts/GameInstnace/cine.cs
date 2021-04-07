using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class cine : MonoBehaviour
{
    
    
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject Fade;
    [SerializeField] private List<float> times = new List<float>();
    [SerializeField] private Transform scene;
    

    private Animator anim;

    [SerializeField] private List<GameObject> scenes = new List<GameObject>();
   [SerializeField] private List<PlayableDirector> directors = new List<PlayableDirector>();

    
private int i = 0;


    public bool PlayOnce { get; set; } = true;
    private bool endOnce = true;



    private PlayableDirector director;
    private Move _playerInstance;
    private UiManager uIManager;


    private void Start()
    {
        
        _playerInstance = GameManager.GetManagerClass<CharacterManager>().playerInstance;
        uIManager = GameManager.GetManagerClass<UiManager>();
        anim = Fade.GetComponent<Animator>();
        
        foreach(Transform s in scene)
        {
            scenes.Add(s.gameObject);
        }


        for(int i=0;i<scenes.Count;i++)
        {
            scenes[i].SetActive(false);
            directors.Add(scenes[i].GetComponent<PlayableDirector>());
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("Player")  && PlayOnce)
        {
            _playerInstance.ChangeState(Move.CharacterState.cinemachine,0);
            _playerInstance.Info.bisAction = false;
            uIManager.ShowAndHide(uIManager.stoneUI_instance.gameObject,false);
            uIManager.ShowAndHide(uIManager.Hp[0].gameObject,false);
            uIManager.ShowAndHide(uIManager.Hp[1].gameObject,false);
            uIManager.ShowAndHide(uIManager.Hp[2].gameObject,false);
            cam.SetActive(false);
            Fade.SetActive(false);
            scenes[i].SetActive(true);
            Fade.SetActive(false);
            PlayOnce = false;

           

        }

    }


    private void StartScene()
    {
        scenes[i].SetActive(true);
        //i += 1;

    }


    public void FadeEnd()
    {
        //cam.SetActive(false);
        StartScene();
       
        
        
        
        endOnce = true;
        //Fade.SetActive(false);

    }

    private IEnumerator timer()
    {
        


        yield return new WaitForSeconds(2.4f);
        scenes[i-1].SetActive(false);
        StartScene();
        Fade.SetActive(false);
        //
        endOnce = true;

        // FadeEnd();
        // yield return new WaitForSeconds(0.7f);

    }


    private void Update()
    {
        

       if(directors[i].time >= times[i] && endOnce)
        {
            if (i >= scenes.Count - 1)
            {
                
                _playerInstance.ChangeState(Move.CharacterState.Idle, 0);
                cam.SetActive(true);
                endOnce = false;
            }
            else
            {
                //scenes[i].SetActive(false);
                //cam.SetActive(true);
                Fade.SetActive(true);
                anim.Play("fadeout",-1);
                
                endOnce = false;
                i += 1;
                StartCoroutine(timer());
            }
        }


    }




}
