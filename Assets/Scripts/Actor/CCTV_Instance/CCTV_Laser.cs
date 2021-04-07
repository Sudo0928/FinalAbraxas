using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV_Laser : MonoBehaviour
{
    public CCTV_Instance Parent { get; set; }
    private Animator anim;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void AnimOn()
    {
        anim.Play("Laser", -1);
    }

    public void LaserPlayer()
    {
        Parent.LaserDamage();
    }

    public void LaserEnd()
    {
        //Parent.bIsLazor = false;
        Parent.LaserEnd();
    }
}
