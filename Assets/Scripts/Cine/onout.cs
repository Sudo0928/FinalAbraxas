using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onout : MonoBehaviour
{
    private Move _playerInstance;
    private bool _playOnce = true;
    

    private void Start()
    {
        _playerInstance = GameManager.GetManagerClass<CharacterManager>().playerInstance;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _playOnce)
        {
            _playerInstance.ChangeState(Move.CharacterState.Idle, 0);
            _playOnce = false;
        }
    }
}
