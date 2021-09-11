using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{ 
    IntegratedManager integratedManager;
    IntegratedManager.GameMode mode;
    [SerializeField]
    Image explain;
    [SerializeField]
    Button twoPlayerBattleButton;
    [SerializeField]
    Button onlineBattleButton;
    [SerializeField]
    Text Heading;
    [SceneName]
    public string mainBattle;

    private bool isActivatedExplain = false; 

    // Start is called before the first frame update
    void Start()
    {
        integratedManager = GameObject.Find("IntegratedManager").GetComponent<IntegratedManager>();
        mode = integratedManager.GetMode();
    }

    private void Update()
    {
        if (isActivatedExplain)
        {
            if (Input.anyKeyDown)
            {
                FadeExplainPanel();
            }
        }
    }

    public void Player2Battle()
    {

        integratedManager.SetMode(IntegratedManager.GameMode.offline);
        SceneManager.LoadScene(mainBattle);
    }


    public void OnlineBattle()
    {
        integratedManager.SetMode(IntegratedManager.GameMode.online);
        SceneManager.LoadScene(mainBattle);

    }

    public void CPUBattle()
    {

        integratedManager.SetMode(IntegratedManager.GameMode.cpu);
        SceneManager.LoadScene(mainBattle);
    }

    public void ViewExplainPanel()
    {
        isActivatedExplain = true;
        explain.gameObject.SetActive(true);
        twoPlayerBattleButton.gameObject.SetActive(false);
        onlineBattleButton.gameObject.SetActive(false);
        Heading.gameObject.SetActive(false);
    }

    public void FadeExplainPanel()
    {
        isActivatedExplain = false;
        explain.gameObject.SetActive(false);
        twoPlayerBattleButton.gameObject.SetActive(true);
        onlineBattleButton.gameObject.SetActive(true);
        Heading.gameObject.SetActive(true);
    }
}
