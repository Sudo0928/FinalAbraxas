using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lasetmp1 : MonoBehaviour
{

    public float o;
    float z;
    // Start is called before the first frame update
    void Start()
    {
        z = transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(1, o, 1);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z + o);
    }

    public  void Zero(float f)
    {

        o = f;

    }

}
