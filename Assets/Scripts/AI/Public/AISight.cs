using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISight : MonoBehaviour
{
    // 시야 거리
    public float _WatchDist;
    [SerializeField] private float _WatchAngle;
    [SerializeField] private Transform head;

    private Move _playerinstance;
    private ParticleManager pm;

    public bool isDetected = false;

    public bool isWatchable { get; set; } = true;


    // Start is called before the first frame update
    void Start()
    {
        _playerinstance = GameManager.GetManagerClass<CharacterManager>().playerInstance;
        pm = GameManager.GetManagerClass<ParticleManager>();
        StartCoroutine(Watch());

    }
    public void StartSight()
    {
        StartCoroutine(Watch());      
    }

    public void StopSight()
    {
        StopCoroutine(Watch());
    }
  
    private IEnumerator Watch()
    {
        while(true)
        {

           // if (!isWatchable) break;
           
            
            Vector3 noyP = _playerinstance.transform.position ;
                Vector3 noyH = transform.position;
                Vector3 noyF = transform.forward;
                noyP.y = 0;
                noyH.y = 0;
                noyF.y = 0;
                //Debug.Log(Vector3.Angle((noyP - noyH).normalized, noyF));
                
                if (Vector3.Angle((noyP - noyH).normalized, noyF) < _WatchAngle)
                {
                
                    RaycastHit hit;
                    Vector3 temp = _playerinstance.transform.position;
                    temp.y += 0.4f;
                   
                   
                    if (Physics.Raycast(head.position, temp - head.position, out hit, _WatchDist))
                    {
                        if (hit.collider.gameObject.CompareTag("Player") && _playerinstance.bisSound )
                        {
                       // Debug.Log("Ray");
                        pm.SetEffect(head.position, 2);

                       
                            isDetected = true;
                             break;
                        }
                    }
                }
                
            

            yield return new WaitForSeconds(0.01f);
        }


        
        }

 }

// Update is called once per frame

        

