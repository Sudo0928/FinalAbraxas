using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textflop : MonoBehaviour
{
   [SerializeField] private Animator anim;

    // Start is called before the first frame update
    private void OnEnable()
    {
        anim.Play("GameOver",-1);
    }

    public void End()
    {
        anim.Play("press");
    }


}
