using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ontrigger : MonoBehaviour
{

     private Move _playerInstance;
    private bool _playOnce = true;
    [SerializeField] private Vector3 pos;

    private void Start()
    {
        _playerInstance = GameManager.GetManagerClass<CharacterManager>().playerInstance;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _playOnce)
        {
            if (_playerInstance.bisLantern)
            {

                _playerInstance.CineMove(pos, true);
                _playOnce = false;
            }
        }
    }
}
