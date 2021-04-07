 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{

    private Animator Anim;
    public Move Player { get; set; }
    public breathe bre { get; set; }
    public GameObject Lantern;
   
   
    private Renderer render;
   
    private string s;
    private float alpa;
    private float dir = -1;

    public bool bisStartColor { get; set; } = false;

    private bool PlayerInput = false;


    public void SetAnim(int num, bool bisLantern = true)
    {
        Anim.SetInteger("State",num);
        Anim.SetFloat("AnimationSpeed", 1.0f);
      
        if (num != 2) PlayerInput = false;
    }

    public void GetHit(float num)
    {
        if (num < 0) Anim.SetTrigger("Hit"); else Anim.SetTrigger("Hit2");
        bisStartColor = true;
       

    }

    public void StopForStone()
	{
       
        
        if (PlayerInput) { Anim.SetInteger("State",4); } //Anim.SetFloat("AnimationSpeed", 3.0f); }
        else { Anim.SetFloat("AnimationSpeed", 0.0f); }

        PlayerInput = false;

    }


    public void StopForBreathe()
    {
        Anim.SetFloat("AnimationSpeed", 0.0f);
        if(!bre.bHoldBreathe) Anim.SetFloat("AnimationSpeed", 0.0f);

    }

    public void StartAgain()
	{
        Anim.SetFloat("AnimationSpeed", 3.0f);
        PlayerInput = true;
    }


    public void ForceLive()
    {
        Anim.SetTrigger("Live");
    }

    public void GetDie(float num)
    {
        if (num < 0) Anim.SetTrigger("Die"); else Anim.SetTrigger("Die2");
       
    }


    public void ThrowAction()
	{
        Player.bIsActionOver = true;
        PlayerInput = false;

    }

    public void LanternAction(bool lantern)
    {
        
        Anim.SetBool("Lantern",lantern);
        Lantern.gameObject.SetActive(lantern);
        

    }

    private void Awake()
    {
        Anim = gameObject.GetComponent<Animator>();
        render = gameObject.GetComponentInChildren<Renderer>();


       Debug.Log(render.sharedMaterial.shader.FindPropertyIndex("_Opacity"));
        s = render.sharedMaterial.shader.GetPropertyName(13);
       // render.
        Debug.Log(s);

       // colorStart = render.sharedMaterial.GetColor(s);
       // colorEnd = Color.red;

    }

    /*
    private void Update()
    {
        if (bisStartColor)
        {
            if (alpa < 0.2f) dir = 1;
            else if (alpa > 0.9f) dir = -1;

            alpa += Time.deltaTime * 0.8f * dir;
            render.sharedMaterial.SetFloat(s, alpa);

            //float lerp = Mathf.PingPong(Time.time, 4.0f) / 4.0f;

            //alpa.SetColor(s, Color.Lerp(colorStart, colorEnd, lerp));
            //render.sharedMaterial.SetColor(s, Color.Lerp(colorStart, colorEnd, lerp));



            // Debug.Log(render.sharedMaterial.GetColor(s));

        }
        else
        {
            render.sharedMaterial.SetFloat(s, 1.0f);
            //render.sharedMaterial.SetColor(s, colorStart);
        }
       
    }
     */


}
