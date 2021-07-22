using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntegratedManager : MonoBehaviour
{
    public bool isOnline;
    [SceneName]
    public string mainBattle;
   

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Player2Battle()
    {
        isOnline = false;
        SceneManager.LoadScene(mainBattle);
    }

    public void OnlineBattle()
    {
        isOnline = true;
        SceneManager.LoadScene(mainBattle);
        
    }
}
