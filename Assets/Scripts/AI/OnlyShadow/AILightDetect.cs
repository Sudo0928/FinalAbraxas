using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILightDetect : MonoBehaviour
{
    private Light[] lights;

    public SkinnedMeshRenderer mesh;

    private MaterialPropertyBlock MPB;

    private LightManager lightManager;

    private Move _playerInstace;
    private Transform lightTransform;

    private int Lightindex;
  
    public bool smallLight;
    public bool bigLight;
    public bool playerAttatch;
    public bool wallAttath;

    public float opacity = 1;


    private void Awake()
    {
        mesh = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        MPB = new MaterialPropertyBlock();
    }

    private void Start()
    {
        lightManager = GameManager.GetManagerClass<LightManager>();

        lights = lightManager.lights;
        _playerInstace = GameManager.GetManagerClass<CharacterManager>().playerInstance;
       /*
        StartCoroutine(detectLight());

        IEnumerator detectLight()
        {

            while(true)
            {
                LightDetect();


                yield return new WaitForSeconds(0.1f);
            }
        }
        */
       
    }



    public void SetOpacity(float val, bool lerp)
    {
        MPB.SetFloat(Shader.PropertyToID("_Opacity"), opacity = lerp ? opacity = Mathf.Lerp(opacity, val, Time.deltaTime) : opacity = val);
        mesh.SetPropertyBlock(MPB);
    }

    public void SetOpacity(float val, bool lerp, float lerpspeed)
    {
        MPB.SetFloat(Shader.PropertyToID("_Opacity"), opacity = lerp ? opacity = Mathf.Lerp(opacity, val, Time.deltaTime * lerpspeed) : opacity = val);
        mesh.SetPropertyBlock(MPB);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Light"))
        {
            bigLight = true;
            smallLight = false;
            SetOpacity(0.6f, true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if(other.gameObject.CompareTag("smallLight") && !bigLight)
        {
            smallLight = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Light"))
        {
            bigLight = false;
            SetOpacity(1f, true);
        }
        else if(other.gameObject.CompareTag("smallLight"))
        {
            smallLight = false;
        }
        else if(other.gameObject.CompareTag("Player") )
        {
            if (_playerInstace.bisSound)
            {
                playerAttatch = true;
            }
        }
        else if(other.gameObject.CompareTag("Untagged"))
        {
            wallAttath = true;
            
           
        }


    }


    


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("무..무슨");
    }


    private void LightDetect()
    {

        LayerMask layerMask = -1 - (1 << LayerMask.NameToLayer("Player"));
       

        smallLight = false;
        bigLight = false;


        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i].gameObject.activeSelf)
            {
                LightInfluenceRange(lights[i], layerMask);
                Lightindex = i;
                Debug.Log("바로 한 번");
            }
        }

        if (bigLight) SetOpacity(0.6f, true);
        else if (smallLight) SetOpacity(0.9f, true);
        else SetOpacity(1f, true);


    }

    private void LightInfluenceRange(Light light, LayerMask layerMask)
    {
        Vector3 dir = (transform.position - light.transform.position).normalized;
        Ray ray = new Ray(light.transform.position, dir);
        RaycastHit hit;
        Debug.DrawRay(light.transform.position, dir,Color.red);

        if(Vector3.Distance(light.transform.position, transform.position) <1f)
        {
            smallLight |= CheckInLight(light, dir);
            Debug.Log("스니 데이");
            return;
        }
        // if (Physics.Raycast(ray, out hit, light.range - 2, layerMask) && hit.transform.Equals(transform))
        else  if(Vector3.Distance(light.transform.position,transform.position) < 2f)
        {
            bigLight |= CheckInLight(light, dir);
            Debug.Log("조 져 보겠");
            return;
        }
        /*
        else if (Physics.Raycast(ray, out hit, light.range, layerMask) && hit.transform.Equals(transform))
        {
            smallLight |= CheckInLight(light, dir);
            Debug.Log("스니 데이");
            return;
        }
        */
        smallLight |= false;
        bigLight |= false;
    }

    bool CheckInLight(Light light, Vector3 dir)
    {
        if (light.type.Equals(LightType.Spot))
        {
            return CheckViewAngle(light.transform.forward, dir, light.spotAngle);
        }
        else if (light.type.Equals(LightType.Point))
        {
            return true;
        }
        return false;
    }

    public bool CheckViewAngle(Vector3 from, Vector3 to, float angle)
    {
        if (Vector3.Angle(from, to) < angle * 0.5f)
        {
            return true;
        }
        return false;
    }
    /*
    public void RunVector(out float x, out float z)
    {
        x = (transform.position.x - lights[Lightindex].transform.position.x) > 0 ? 0.1f : -0.1f;
        z = (transform.position.z - lights[Lightindex].transform.position.z) > 0 ? 0.1f : -0.1f;
       
    }
    */
   

}
