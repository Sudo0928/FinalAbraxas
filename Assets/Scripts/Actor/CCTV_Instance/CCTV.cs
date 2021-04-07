using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{

    public CCTV_Instance parent_Instance { get; set; }
    private Move Player;

    private void OnTriggerStay(Collider other)
    {
        

        if(other.gameObject.CompareTag("Player"))
        {

            Debug.Log("TLqkf");
          //  if(Player== null) {Player = other.gameObject.GetComponent<Move>(); }
            //parent_Instance.Player = Player;
            parent_Instance.LaserStart();
           // Player.ChangeState(Move.CharacterState.Hit, 3);
            
        }
    }
}
