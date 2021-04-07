using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour,IManager
{
    public GameManager gameManager { get { return GameManager.gameManager; } }


    public GameObject effect1;
    public GameObject effect2;

    public void SetEffect(Vector3 pos ,int type )
    {
        switch(type)
        {
            case 1:
                Instantiate(effect1, pos, effect1.transform.rotation);
                break;

            case 2:
                Instantiate(effect2, pos, effect2.transform.rotation);
               
                break;
        }
    }
}
