using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setRespawn : MonoBehaviour
{
    private CharacterManager CharacterManager;

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager = GameManager.GetManagerClass<CharacterManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CharacterManager.SetPos(transform.position);
        }
    }
}
