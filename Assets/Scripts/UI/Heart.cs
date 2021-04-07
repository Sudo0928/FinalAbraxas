using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private Animator anim;
    private RectTransform rect;
   


    public void Initialize(float x)
    {
        anim = gameObject.GetComponent<Animator>();
        rect = gameObject.GetComponent<RectTransform>();
        rect.position = new Vector2(x, 950);
    }

    public void HpDown()
    {
        anim.Play("hpdown");
    }

    public void HPUp()
    {
        anim.Play("HPUp");
    }

    public void stopAnim()
    {
        anim.SetFloat("speed",0);
    }

}
