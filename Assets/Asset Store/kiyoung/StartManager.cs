using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartManager : MonoBehaviour
{
    public GameObject abraxas;
    public GameObject pak;
    public GameObject puw;
    public GameObject[] menu;
    public GameObject point;
    public GameObject brightness;
    public GameObject mainCanvas;
    public GameObject screen;
    public Image Ipoint;
    public Slider slider;
    public Slider slider2;
    public Volume v;
    public LiftGammaGain LGG;

    public TextMeshProUGUI GameStart;
    public TextMeshProUGUI GameSetting;
    public TextMeshProUGUI GameQuit;

    public RawImage mScreen;
    public VideoPlayer mVideoPlayer;

    public Material TMP_Material;
    public Material ChromaKey;

    public CharacterInfo Info;

    private bool PopUpWindow = false;
    private bool akd = false;
    private bool brightSetting = false;


    private bool bisenterUp = false;

    // Start is called before the first frame update
    void Start()
    {
        Ipoint = point.GetComponent<Image>();

        v.profile.TryGet(out LGG);

        if (mScreen != null && mVideoPlayer != null)
        {
            mScreen.material = ChromaKey;
            StartCoroutine(PrepareVideo());
        }

      
    }

    // Update is called once per frame
    void Update()
    {
        AKD();

        if (akd)
        {
            UDmenu();

            if (Input.GetKeyUp(KeyCode.Return)) bisenterUp = true;
            if(bisenterUp) EnterKeyDown(); 
            if(brightSetting)
            {
                LGG.gamma.value = new Vector4(1, 1, 1, slider.value);
                Info.voluem = slider2.value;
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    brightSetting = false;
                    brightness.SetActive(false);
                    mainCanvas.SetActive(true);
                }
            }
        }
    }

    // Any Key Down
    void AKD()
    {
        if (akd) return;

        if (Input.anyKeyDown)
        {
          

            akd = true;
            pak.SetActive(false);
            StartCoroutine(moveAnimation());
            StartCoroutine(FadeIn());
        }
    }
    /*
    public void OnPointerEnter(PointerEventData data)
    {
        if (data.pointerCurrentRaycast.gameObject.CompareTag("Slot"))
        {
            Debug.Log("no");
            string s = data.pointerCurrentRaycast.gameObject.name;

            switch(s)
            {
                case "Game start":
                    point.transform.SetParent(menu[0].transform);
                    point.transform.localPosition = Vector2.zero;
                    break;


                case "Game setting":
                    point.transform.SetParent(menu[1].transform);
                    point.transform.localPosition = Vector2.zero;
                    break;

            }
        }
    }

    */

    public void OnPoint(int a)
    {
        point.transform.SetParent(menu[a].transform);
        point.transform.localPosition = Vector2.zero;
    }


   

    void EnterKeyDown()
    {
        if (PopUpWindow) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            switch (point.transform.parent.name)
            {
                case "Game start":
                    ClickGameStart();
                    break;
                case "Game setting":
                    ClickGameSetting();
                    break;

                case "Game Quit":
                    EndGame();
                    break;

            }
        }
    }

    void UDmenu()
    {
        if (PopUpWindow) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            for(int i = 0; i < menu.Length; i++)
            {
                if (point.transform.parent.Equals(menu[i].transform) && i != 0)
                {
                    point.transform.SetParent(menu[i - 1].transform);
                    point.transform.localPosition = Vector2.zero;
                    return;
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            for(int i = 0; i < menu.Length; i++)
            {
                if (point.transform.parent.Equals(menu[i].transform) && i < menu.Length - 1)
                {
                    point.transform.SetParent(menu[i + 1].transform);
                    point.transform.localPosition = Vector2.zero;
                    return;
                }
            }
        }
    }

    public IEnumerator PrepareVideo()
    {
        mVideoPlayer.Prepare();

        while(!mVideoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(0.5f);
        }

        mScreen.texture = mVideoPlayer.texture;
        mScreen.material.SetTexture("_MainTex", mScreen.texture);

        StartCoroutine(PlayVideo());

        screen.SetActive(true);
    }

    public IEnumerator moveAnimation()
    {
        while(true)
        {
            abraxas.transform.position = new Vector2(abraxas.transform.position.x, Mathf.Lerp(abraxas.transform.position.y, 700, Time.deltaTime));
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        while(true)
        {
            GameStart.alpha = Mathf.Lerp(GameSetting.alpha, 1, Time.deltaTime);
            GameSetting.alpha = Mathf.Lerp(GameSetting.alpha, 1, Time.deltaTime);
            GameQuit.alpha = Mathf.Lerp(GameSetting.alpha, 1, Time.deltaTime);
            
            Ipoint.color = new Color(0.5f, 0.5f, 0.5f, Mathf.Lerp(Ipoint.color.a, 0.5f, Time.deltaTime));
            yield return null;
        }
    }

    public IEnumerator PlayVideo()
    {
        while (true)
        {
            if (mVideoPlayer != null && mVideoPlayer.isPrepared)
            {
                mVideoPlayer.Play();
                break;
            }
        }
        yield return null;
    }
    
    public IEnumerator StopVideo()
    {
        while (true)
        {
            if (mVideoPlayer != null && mVideoPlayer.isPrepared)
            {
                mVideoPlayer.Pause();
                break;
            }
        }
        yield return null;
    }

    public IEnumerator Softness(float a)
    {
        float time = Time.time;

        while(true)
        {
            if (Time.time - time <= 3f)
            {
                TMP_Material.SetFloat("_OutlineSoftness", Mathf.Lerp(TMP_Material.GetFloat("_OutlineSoftness"), a, Time.deltaTime * 3f));
            }
            else
            {
                TMP_Material.SetFloat("_OutlineSoftness", a);
                break;
            }
            yield return null;
        }
        yield return null;
    }

    public void ClickGameStart()
    {
        Debug.Log("GameStart");
        SceneManager.LoadScene("Forest");
    }
    
    public void ClickGameSetting()
    {
        StartCoroutine(Softness(1));
        PopUpWindow = true;
        puw.SetActive(true);
        menu[0].transform.parent.gameObject.SetActive(false);
    }

    public void EndGame()
    {
        Application.Quit();

    }


    public void SettingSave()
    {
        StartCoroutine(Softness(0));
        PopUpWindow = false;
        puw.SetActive(false);
        menu[0].transform.parent.gameObject.SetActive(true);
    }

    public void ClickBrightSetting()
    {
        brightSetting = true;
        brightness.SetActive(true);
        mainCanvas.SetActive(false);
    }
}
