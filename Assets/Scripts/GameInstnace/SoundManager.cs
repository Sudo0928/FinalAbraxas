using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour,IManager
{
    public GameManager gameManager { get { return GameManager.gameManager; } }

    private AudioSource Audio;
    public CharacterInfo Info;
    private bool up = true;
    private bool soundMix = false;
    private float v =1;

    private void Awake()
    {

        Audio = gameObject.GetComponent<AudioSource>();
        Audio.volume = Info.voluem;
        DontDestroyOnLoad(this.gameObject);
    }



    public void Volume(bool up , bool sound)
    {
        this.up = up;
        soundMix = sound;

    }

    public void Update()
    {
        if (soundMix)
        {
            v = Audio.volume;

            switch (up)
            {
                case true:
                    v += Time.deltaTime * 0.25f;
                    v = Mathf.Clamp(v, 0, 1);
                    Audio.volume = v;
                    if (Mathf.Approximately(v, 0.9f)) soundMix = false;
                    break;

                case false:
                    v -= Time.deltaTime * 0.5f;
                    Debug.Log(v);
                    v = Mathf.Clamp(v, 0, 1);
                    Audio.volume = v;
                    if (Mathf.Approximately(v, 0.1f)) soundMix = false;
                    break;

            }
        }


    }




}
