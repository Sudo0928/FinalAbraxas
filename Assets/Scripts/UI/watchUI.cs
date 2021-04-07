using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class watchUI : MonoBehaviour
{
    public Image Exclamation;
    public Image Bar;

    public bool bisSound { get; set; } = false;
    public bool bisExclamtionDisplay { get; set; }
    public Transform cam { get; set; }


    private float _gaugeStartTime = 0;
   [SerializeField] private float alpa;





    // Start is called before the first frame update
    public void Add()
    {
        _gaugeStartTime = Time.deltaTime;
        
    }

   
    public void erase()
    {
       
        alpa = 0;
        Bar.fillAmount = 0;
        Bar.gameObject.SetActive(true);
       // Exclamation.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void Reset()
    {
      
        alpa = 0;
        Bar.fillAmount = 0;
        Bar.gameObject.SetActive(true);
      //  Exclamation.gameObject.SetActive(false);
        bisExclamtionDisplay = false;
       
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        alpa = bisSound ? alpa += Time.deltaTime * 0.39f : alpa -= Time.deltaTime * 0.39f;
        alpa = Mathf.Clamp(alpa, 0, 1);
        Bar.fillAmount = alpa;
        if (alpa > 0.95f && bisExclamtionDisplay)
        {
            Bar.gameObject.SetActive(false);
           // Exclamation.gameObject.SetActive(true);
            Invoke("erase", 0.3f);

        }

    }
}
