using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Injection : MonoBehaviour, IRecyclableGameObject
{
    public bool isActive { get; set; }
    private float MoveSpeed = 5.0f;
    private Move _playerInstance;
    private AIBThrow Parent;

    public void initializeInjection(Move player, Vector3 pos, Vector3 foward, AIBThrow parent )
    {
        _playerInstance = player;
        Parent = parent;
        transform.position = pos;
        transform.eulerAngles = foward;
        transform.LookAt(player.transform.position);
    }

    private void RotateToTarget()
    {
        Quaternion lookRotation = Quaternion.LookRotation(_playerInstance.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime *200f);
    }

    private void MoveToTarget()
    {
        Vector3 pos = _playerInstance.transform.position;
        pos.y += 0.4f;
        transform.position = Vector3.MoveTowards(transform.position, pos, MoveSpeed * Time.deltaTime);
    }

  



    // Update is called once per frame
    void Update()
    {
        //RotateToTarget();
        MoveToTarget();
    }


    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            int direction = (Vector3.Distance(transform.position, _playerInstance.Front.position) - Vector3.Distance(transform.position, _playerInstance.Back.position)) > 0 ? 1 : -1;
            _playerInstance.HitAction(direction,false);
            Parent.bisHit = true;
            isActive = false;
            gameObject.SetActive(false);
        }
    }

}
