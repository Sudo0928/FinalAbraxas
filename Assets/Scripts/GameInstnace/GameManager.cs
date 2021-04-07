using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _GameManagerInstance = null;
    public List<IManager> _ManagerClass = null;

    public delegate void soundDel(string s);
    public soundDel soundOut;



    //public UI UIAction;

   




    public static GameManager gameManager {
        get {
            if (!_GameManagerInstance)
            {
                _GameManagerInstance = GameObject.Find("GameManager").GetComponent<GameManager>();
                _GameManagerInstance.InitializeGameManager();
            }

            return _GameManagerInstance;
        }
    }

    private void InitializeGameManager()
    {
        _ManagerClass = new List<IManager>();

        //  > 하위 매니저 클래스를 등록합니다.
        RegisterManagerClass<CharacterManager>();
        RegisterManagerClass<EnemyManager>();
        RegisterManagerClass<UiManager>();
        RegisterManagerClass<ParticleManager>();
        RegisterManagerClass<CameraManager>();
        RegisterManagerClass<LightManager>();
        RegisterManagerClass<SoundManager>();
    }

    private void RegisterManagerClass<T>() where T : IManager
    {
        _ManagerClass.Add(transform.GetComponentInChildren<T>());
    }

    public static T GetManagerClass<T>() where T : class, IManager
    {
        return gameManager._ManagerClass.Find(
            (IManager managerClass) => managerClass.GetType() == typeof(T)) as T;
    }


    private void Awake()
    {
        /*
        DontDestroyOnLoad(gameObject);
        //lights = FindObjectsOfType<Light>();
        
        if (_GameManagerInstance && _GameManagerInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        */

    }

}

