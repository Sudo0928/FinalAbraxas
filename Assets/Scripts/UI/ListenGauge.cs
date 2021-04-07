using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListenGauge : MonoBehaviour
{
    public Image Exclamation;
    public Image Bar;

    private bool bisListend = false;
    private float _gaugeStartTime = 0;
    private float alpa =0;
    


    // Start is called before the first frame update
    public void Add()
    {
        _gaugeStartTime = Time.deltaTime;
        bisListend = true;
    }


   

    public void Reset()
    {
        bisListend = false;
        alpa = 0;
        Bar.fillAmount = 0;
        Bar.gameObject.SetActive(true);
        Exclamation.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.deltaTime - _gaugeStartTime > 0.5f) { bisListend = false;}

        alpa = bisListend ? alpa += Time.deltaTime * 0.15f : alpa -= Time.deltaTime * 0.15f;
        alpa = Mathf.Clamp(alpa, 0, 1);
        Bar.fillAmount = alpa;
        if (alpa > 0.95f)
        {
            Bar.gameObject.SetActive(false);
            Exclamation.gameObject.SetActive(true);
            Invoke("erase", 0.3f);

        }

    }
}
