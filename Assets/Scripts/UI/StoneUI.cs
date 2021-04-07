using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StoneUI : MonoBehaviour
{
   
    private Dictionary<string, Image> UIDictionary = new Dictionary<string, Image>();
    private RectTransform rect;

    private Text StoneText;

    private bool bSpinUI;
    private float dt;
   

   public CharacterInfo info { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.GetManagerClass<UiManager>().stoneUI_instance = this;
        info = GameManager.GetManagerClass<CharacterManager>()._characterInfo;
        rect = gameObject.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(-853,-412);
        foreach(Transform t in transform)
        {
            if (t.name == "RedRound") UIDictionary.Add("Round", t.GetComponent<Image>());
            else if (t.name == "Rock_BackGround") UIDictionary.Add("BackGround", t.GetComponent<Image>());
           // else if (t.name == "Rock") UIDictionary.Add("Rock", t.GetComponent<Image>());
            else if (t.name == "Text") StoneText = t.GetComponent<Text>();

        }
       

      
    }

    public void UISet()
    {
      
        UIDictionary["Round"].fillAmount = 0;
        UIDictionary["BackGround"].color = Color.gray;
        //UIDictionary["Rock"].enabled = false;
        bSpinUI = true;
        StoneText.text = info.StoneLeft.ToString(); ;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (bSpinUI)
        {

            dt += Time.deltaTime;
            UIDictionary["Round"].fillAmount = dt*0.2f;

            if(dt>=5.0f)
            {
               // UIDictionary["Rock"].enabled = true;
                UIDictionary["BackGround"].color = Color.white;
                dt = 0;
                StoneText.text = (info.StoneLeft += 1).ToString();
                info.StoneLeft = Mathf.Clamp(info.StoneLeft,0, 4);
                if(info.StoneLeft >=4)bSpinUI = false;
            }

        }
    }
}
