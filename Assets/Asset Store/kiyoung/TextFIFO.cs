using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFIFO : MonoBehaviour
{
    public bool FIFO = true;

    public float speed = 1;

    private TextMeshProUGUI tmp;

    private float time = 0;

    bool temp = true;

    // Start is called before the first frame update
    void Start()
    {
        tmp = this.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(FIFO)
        {
            if (Time.time - time <= 3 / speed)
            {
                if(temp)
                {
                    tmp.alpha = Mathf.Lerp(tmp.alpha, 0, Time.deltaTime * speed);
                }
                else tmp.alpha = Mathf.Lerp(tmp.alpha, 1, Time.deltaTime * speed);

            }
            else
            {
                temp = !temp;
                time = Time.time;
            }
        }
    }
}
