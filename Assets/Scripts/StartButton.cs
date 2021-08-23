using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{ 
    IntegratedManager integratedManager;

    [SceneName]
    public string mainBattle;

    // Start is called before the first frame update
    void Start()
    {
        integratedManager = GameObject.Find("IntegratedManager").GetComponent<IntegratedManager>();
    }

    public void Player2Battle()
    {
        integratedManager.isOnline = false;
        SceneManager.LoadScene(mainBattle);
    }


    public void OnlineBattle()
    {
        integratedManager.isOnline = true;
        SceneManager.LoadScene(mainBattle);

    }
}
