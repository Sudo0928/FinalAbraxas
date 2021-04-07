using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Moveable : MonoBehaviour
{
    NavMeshAgent agent;
    ShadowMonster sm;

    public bool arrive = true;

    Vector3 temp;

    [SerializeField]
    Transform target;

    [SerializeField]
    List<Transform> RandomTr = new List<Transform>();

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        sm = GetComponent<ShadowMonster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (arrive)
        {
            if (sm._InLight || sm._2InLight)
            {
                if (sm._2InLight)
                {
                    temp = RandomTr[Random.Range(0, RandomTr.Count)].position;
                    arrive = false;
                }
                else agent.SetDestination(this.transform.position);
            }
            else agent.SetDestination(target.position);
        }
        else
        {
            agent.SetDestination(temp);
            if(temp.x == this.transform.position.x && temp.y == this.transform.position.y)
            {
                arrive = true;
            }
        }
        
    }
}
