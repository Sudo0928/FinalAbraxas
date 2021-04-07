using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] private GameObject fgf;
    private RectTransform rect;
    private bool bisSelect;

  
    private Vector3 a = new Vector3(-12.0f, -15.0f,0);
    private Vector3 b = new Vector3(12.2f, -15.0f,0);
   [SerializeField]  private Move mo;


    private void Start()
    {
        rect = fgf.GetComponent<RectTransform>();
        
        gameObject.SetActive(false);
    }


    public void OnEnter1()
    {
        rect.localPosition = a;
        Debug.Log("???");
        bisSelect = true;

    }

    public void OnEnter2()
    {
        rect.localPosition = b;
        Debug.Log("!!!!");
        bisSelect = false;

    }

    public void OnClick1()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("StartMenu");
    }

    public void OnClick2()
    {if(mo)
        mo.bisEnd = false;
        Time.timeScale = 1.0f;
        
        gameObject.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) OnClick2();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) OnEnter1();
        else if (Input.GetKeyDown(KeyCode.RightArrow)) OnEnter2();

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (bisSelect) OnClick1();
            else OnClick2();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick2();
        }

    }


}
