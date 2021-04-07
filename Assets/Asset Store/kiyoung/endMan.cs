using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endMan : MonoBehaviour
{
    [SerializeField] private GameObject Endmenu;

    private bool end = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (end) Endmenu.SetActive(true);
            else Endmenu.SetActive(false);

        }


    }
}
