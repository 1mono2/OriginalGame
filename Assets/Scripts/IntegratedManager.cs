using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntegratedManager : MonoBehaviour
{
   [HideInInspector]
   public enum GameMode
    {
        online,
        offline,
        cpu,
    }
    private GameMode mode;
   

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public GameMode GetMode()
    {
        return mode;
    }

    public void SetMode(GameMode gameMode)
    {
        mode = gameMode;
    }

  
}
