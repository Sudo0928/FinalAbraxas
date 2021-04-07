using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV_Instance : MonoBehaviour
{
     private Transform underBox;
    private CCTV DetectRound;
   [SerializeField] private CCTV_Laser Laser_Instanace;

    private float CompareVar = 1;

    // 왕복할것인가?
     private bool bisReciprocation = false;
   

    private Transform RayStart;
    private GameObject Exclamation;

    

    private LineRenderer Laser;

    public Move Player;
    public bool bIsLazor = false;


   private float StopTime;
   private float ReStart;

    private void Start()
    {
       
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,0,transform.rotation.eulerAngles.z);


        foreach(Transform tr in transform)
        {
            if (tr.gameObject.name == "RayStart") RayStart = tr;
            //else if (tr.gameObject.name == "RayDirection") RayDir = tr;
            else if (tr.gameObject.name == "Watch") DetectRound = tr.GetComponentInChildren<CCTV>();
           // else if (tr.gameObject.name == "Exclamation") Exclamation = tr.gameObject;
            else if (tr.gameObject.name == "Laser")
            {
                Laser = tr.GetComponent<LineRenderer>();
                Laser_Instanace = tr.GetComponent<CCTV_Laser>();
            }
          }
          
        DetectRound.parent_Instance = this;
        Laser_Instanace.Parent = this;
        // Exclamation.SetActive(false);
        //Laser.enabled = false;
        Laser.gameObject.SetActive(false);
    }


    private void RoundRotation()
    {
        

        transform.Rotate(Vector3.up * 60 * Time.deltaTime, Space.World);
        if (CompareVar > 4) { CompareVar = 1; transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 1.0f, transform.rotation.eulerAngles.z); }
        if (transform.rotation.eulerAngles.y > 90 * CompareVar -5) { bisReciprocation = true; StopTime = Time.time; }
       
    }


    private void Reciprocation()
    {/*
        if (Count >= 4) { bisReciprocation = false; Count = 0; CompareVar += 1; return; }




        transform.Rotate(standardVector * Direction * 30 * Time.deltaTime, Space.Self);
       

        if (transform.rotation.eulerAngles.x > 320 && Direction == 1) { Direction = -1; Count += 1; }
        if (transform.rotation.eulerAngles.x < 290 && Direction == -1) { Direction = 1; Count += 1; }
       */


        ReStart =  Time.time - StopTime;
        if (ReStart > 3.0f) { bisReciprocation = false; CompareVar += 1; }


    }

   public void Lazor()
    {
       
        Laser.SetPosition(0, RayStart.position);
        Laser.SetPosition(1,Player.transform.position);
        Debug.Log("Tlqkfsusdk");

    }

    public void LaserEnd()
    {
       
        bIsLazor = false;
       // Exclamation.SetActive(false);
        Laser.gameObject.SetActive(false);
    }

    public void LaserStart()
    {
        if (!Player.bisSound) return;

        bIsLazor = true;
       // Exclamation.SetActive(true);
        Laser.gameObject.SetActive(true);
        Laser_Instanace.AnimOn();

    }

    public void LaserDamage()
    {
        // Player.ChangeState(Move.CharacterState.Hit, 3);
        int direction = (Vector3.Distance(transform.position, Player.Front.position) - Vector3.Distance(transform.position, Player.Back.position)) > 0 ? 1 : -1;
        Player.bisSound = false;
        Player.HitAction(direction);
    }


    private void Update()
    {
        if (bIsLazor) Lazor();

        if (bisReciprocation) Reciprocation();
        else RoundRotation();
       

        

    }



}
