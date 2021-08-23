using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntegratedManager : MonoBehaviour
{
   [HideInInspector]
    public bool  isOnline; 
   

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

  
}
