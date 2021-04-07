using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour, IManager
{
    public GameManager gameManager { get { return GameManager.gameManager; } }

    public Light[] lights;

    /*
    private void Awake()
    {
     //   lights = FindObjectsOfType<Light>();
    }
    */

   
}
