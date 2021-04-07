using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour, IRecyclableGameObject
{
    private CharacterManager characterManager = null;
    public GameObject prop;
    private GameObject prop_instnace;
   
    public bool isActive { get; set; }
    private bool bisOn;


    private Vector3 StartPos;
    private Vector3 DestPos;

   [SerializeField] private float Max_Y = 0.3f;
    [SerializeField] private float mht = 0.3f;

    private float g = 0;
    private float vx = 0;
    private float vy = 0;
    private float vz = 0;

    private float dat;


    private float t;
    // Start is called before the first frame update
    

    public void initializeStone(CharacterManager cm,Vector3 pos , Vector3 pos2)
    {
        transform.position = pos;
        t = 0;
        StartPos = pos;
        DestPos = pos2;
        characterManager = cm;
        bisOn = true;

        PreCalculate();

       // rigid.AddForce(force *5 ,ForceMode.VelocityChange);
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!bisOn) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default")) { characterManager.Sound(transform.position, 1);  }
        prop_instnace = Instantiate(prop, transform.position, prop.transform.rotation);
        //rigid.isKinematic = true;
        Disapear();
        bisOn = false;
        characterManager.FallSound("stone");
        
    }


    private void Update()
    {

        if( bisOn)
            transform.position = Move();
        
    }


    private void PreCalculate()
    {

         g = 2 * Max_Y / (mht * mht);

         vy = Mathf.Sqrt(2 * g * Max_Y);


        float a = g;
        float b = -2 * vy;

        dat = (-b + Mathf.Sqrt( b * b )) / (2 * a);
        vx = (StartPos.x - DestPos.x) / dat;
        
        vz = (StartPos.z - DestPos.z) / dat;

       


    }


    private Vector3 Move()
    {
        t += Time.deltaTime;

        

        float x = StartPos.x + vx * t;
        float y = StartPos.y + vy * t - 0.5f * g * t * t;
        float z = StartPos.z + vz * t;
        
        return new Vector3(x, y, z);
    }



    private void Disapear()
    {
        isActive = false;
       // Destroy(prop_instnace);
        gameObject.SetActive(false);
    }
   

}
