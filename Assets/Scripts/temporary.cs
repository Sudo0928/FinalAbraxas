using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temporary : MonoBehaviour
{


    private CharacterManager characgterManager;

    // Start is called before the first frame update
    void Start()
    {

        characgterManager = GameManager.GetManagerClass<CharacterManager>();
        characgterManager.Sound += Nothing;
    }

    public void Nothing(Vector3 pos, int a)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
