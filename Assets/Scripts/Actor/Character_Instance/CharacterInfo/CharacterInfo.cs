using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfo", menuName = "Scriptable Object/Character Data", order = int.MaxValue)]

public class CharacterInfo : ScriptableObject
{
    public int StoneLeft = 4;
    public bool WallSignal = false;
    public bool bisAction = true;
    // public bool bisLantern = false;
    public float moveDir_x;
    public float moveDir_z;
    public float voluem;
    public int hp = 3;

    private void OnEnable()
    {
        StoneLeft = 4;
        WallSignal = false;
        moveDir_x = 0;
        moveDir_z = 0;
        hp = 3;
    }

}


	