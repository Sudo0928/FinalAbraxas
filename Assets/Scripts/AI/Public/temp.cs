using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{
    // Start is called before the first frame update

    private UiManager cm;

    private void Start()
    {
        cm = GameManager.GetManagerClass<UiManager>();
    }


    private void OnTriggerEnter(Collider other)
    {

       
        cm.SpawnUI(this.transform, Color.red, 0f, 90.0f);
    }
}
