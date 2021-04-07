using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour, IManager
{
    public GameManager gameManager { get { return GameManager.gameManager; } }
   
   
    private ObjectPool<OnScreen> _onScreenPool = null;
    private ObjectPool<OnScreen> _bubblePool = null;


    #region Class
    private StoneUI stoneUI;
    public BreatheHoldUI breatheHoldUI { get; set; }
    public stoneGague stoneGagueUI { get; set; }
    public Heart heartUI { get; set; }


    #endregion
    #region Component
    public Canvas MainCanvas { get; set; }
    public Camera MainCamera { get; set; }

    #endregion
    #region Instnace

    public StoneUI stoneUI_instance { get; set; }
    public BreatheHoldUI breatheHoldUI_instance { get; set; }
    public stoneGague    stoneGague_Instance { get; set; }
    public OnScreen onScreen_instance { get; set; }
    public OnScreen bubble_instance { get; set; }
    public Heart heart_instance;

    private Move _playerinstnace;
    public List<Heart> Hp = new List<Heart>();
    #endregion 
    private Sprite Exclamation;

    private void LoadUI()
    {
        stoneUI = Resources.Load<StoneUI>("UI/rock");
        breatheHoldUI = Resources.Load<BreatheHoldUI>("UI/breathe");
        stoneGagueUI = Resources.Load<stoneGague>("UI/StoneGauge");
        onScreen_instance = Resources.Load<OnScreen>("UI/Exclamation");
        Exclamation = Resources.Load<Sprite>("Sprite/Exclamation");
        bubble_instance = Resources.Load<OnScreen>("UI/drown");
        heartUI = Resources.Load<Heart>("UI/heart");

       
    }


    public T InitializeUI<T>(GameObject obj) where T : MonoBehaviour
    {
        GameObject sub = Instantiate(obj);
        sub.transform.parent = MainCanvas.transform;
        return sub.GetComponent<T>();


    }


    public void ShowAndHide(GameObject obj, bool bshow)
    {
        obj.SetActive(bshow);
    }

    private void Awake()
    {
        MainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        LoadUI();
        
        
        InitializeUI<StoneUI>(stoneUI.gameObject);
        InitializeUI<BreatheHoldUI>(breatheHoldUI.gameObject);
        InitializeUI<stoneGague>(stoneGagueUI.gameObject);
        
        
        
        
        for (int i = 0; i<3; i++)
        {
            heart_instance = InitializeUI<Heart>(heartUI.gameObject);

            //heart_instance.Initialize(-874.0f + 109 * i);
            Hp.Add(heart_instance);
            Hp[i].Initialize(100 + 109 * i);
        }
        
        //InitializeUI<BreatheHoldUI>(breatheHoldUI.gameObject, breatheHoldUI_instance);

    }


    private void Start()
    {
     
        _onScreenPool = new ObjectPool<OnScreen>();
        _bubblePool = new ObjectPool<OnScreen>();
       MainCamera =   GameManager.GetManagerClass<CameraManager>().MainCamera;
       
    }

   




    public void SpawnUI(Transform target , Color color,float x, float y)
    {
        OnScreen onScreen = _onScreenPool.GetRecyclableObject() ??
            _onScreenPool.RegisterRecyclableObject(Instantiate(onScreen_instance));


        onScreen.transform.parent = MainCanvas.transform;
        // Image image = onScreen.gameObject.AddComponent<Image>();
        //Image image = onScreen.gameObject.GetComponent<Image>();
        //image.sprite = Exclamation;
        //image.color = color;
        onScreen.initializeUI(target, MainCamera, x, y);

        if (onScreen.gameObject.activeSelf)
            onScreen.isActive = true;
        else
            onScreen.gameObject.SetActive(onScreen.isActive = true);

       

    }



    public  void SpawnUI(Transform target, float x, float y)
    {
        OnScreen bubble = _bubblePool.GetRecyclableObject() ??
          _bubblePool.RegisterRecyclableObject(Instantiate(bubble_instance));


        bubble.transform.parent = MainCanvas.transform;
        bubble.initializeUI(target, MainCamera, x, y);
       

        if (bubble.gameObject.activeSelf)
            bubble.isActive = true;
        else
            bubble.gameObject.SetActive(bubble.isActive = true);
    }

    public void HPDown(int i)
    {
        if (i <= 0) return;
        Hp[i-1].HpDown();
    }

    public void HPUp()
    {
        for(int j =0; j<3;j++)
        {
            Hp[j].HPUp();
        }
    }



}