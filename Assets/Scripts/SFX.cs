using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public GameManager gameManager { get { return GameManager.gameManager; } }
    private AudioSource source;

   private AudioClip StoneSound;
    private AudioClip walkSound;
    private AudioClip hitSound;
    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        StoneSound = Resources.Load<AudioClip>("sound/effect/stonefall");
        walkSound = Resources.Load<AudioClip>("sound/effect/footstep1");
        hitSound = Resources.Load<AudioClip>("sound/effect/hitSound");

        gameManager.soundOut += SoundOut;
        

    }


    public void SoundOut(string s)
    {
       

        switch (s)
        {
            case "stone":
                source.volume = 0.3f;
                source.clip = StoneSound;
               
                break;

            case "walk":
                source.volume = 0.15f;
                source.clip = walkSound;
              
                break;

            case "hit":
                source.volume = 0.45f;
                source.clip = hitSound;
                break;


        }
        source.Play();
    }


}
