using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour ,IManager
{
    public GameManager gameManager { get { return GameManager.gameManager; } }

    public List<Monkey> MonKeyList = new List<Monkey>();




}
