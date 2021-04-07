using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect : MonoBehaviour
{

    //public Zombie parent { get; set; }
   // public doctor parent { get; set; }
    //public GameObject raystart;
    //private RaycastHit Out;
    public bool bist { get; set; }
    public Move _playerinstance { get; set; } = null;


    private ParticleManager pm;

    private void Start()
    {
        pm = GameManager.GetManagerClass<ParticleManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {

            Vector3 ransomVec = transform.forward - _playerinstance.transform.forward;

            //Debug.Log(transform.forward.ToAngle() +"적");
            //Debug.Log(_playerinstance.transform.forward.ToAngle() +"플레이어");
            //Debug.Log(ransomVec);
            //Debug.Log(Mathf.Atan2(ransomVec.z, ransomVec.x) * Mathf.Rad2Deg);
            bist = true;
            pm.SetEffect(transform.position, 1);
            if (Mathf.Abs(transform.forward.ToAngle() - _playerinstance.transform.forward.ToAngle()) < 20) _playerinstance.HitAction(1);
            else _playerinstance.HitAction(-
                1);

            /*
            if(Mathf.Atan2(ransomVec.z, ransomVec.x) * Mathf.Rad2Deg > 90) _playerinstance.HitAction(-1);
            else _playerinstance.HitAction(1);
            */
            bist = false;
        }
    }


}
