using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim : AnimBase
{
   public Animator anim { get; set; }
    public AIBHPatrol parent;

    public Dictionary<string, bool> KeyDictionary = new Dictionary<string, bool>();
   


    public override void ChangeAnim(int num)
    {
        anim.SetInteger("State", num);
     }

    public void speedchanger(float num)
    {
        anim.SetFloat("AnimSpeed", num);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        KeyDictionary.Add("RobotAttack", false);
       
       
    }

    public void StopMoment()
    {
        parent.StopWalk();

    }
    public void ReWalkMoment()
    {

        parent.WalkAgain();

    }

    public void ResetKey()
    {
       foreach(string b in KeyDictionary.Keys)
        {
            KeyDictionary[b] = false;
        }
    }


    public void Action(string bo)
    {
        KeyDictionary[bo] = true;
    }
    

 
}
