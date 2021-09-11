using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameController gameController;
    IntegratedManager integratedManager;
    IntegratedManager.GameMode mode;
    public Text timeText;
    public Text countDownText;
    [SerializeField]
    private Canvas beforeStartCanvas;

    bool onetime;

  /*  void Main()
    {
        Vector3 v = new Vector3(0.1f, 0.1f, 0.1f).normalized;
        System.Console.Write(v);
    }*/
    // Start is called before the first frame update
    void Start()
    {
        gameController = GetComponent<GameController>();
        integratedManager = GameObject.Find("IntegratedManager").GetComponent<IntegratedManager>();
        mode = integratedManager.GetMode();
        onetime = true;

        if (mode != IntegratedManager.GameMode.online)
        {
            beforeStartCanvas.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (mode == IntegratedManager.GameMode.online)
        {
            if (gameController.isBattling == false & onetime == true & gameController.readyAllplayers == true)
            {
                StartCoroutine(CountDown());
                onetime = false;
                beforeStartCanvas.gameObject.SetActive(false);
            }

            }
        else if (mode == IntegratedManager.GameMode.offline || mode == IntegratedManager.GameMode.cpu)
        {
            if (gameController.isBattling == false & onetime == true)
            {
                if (Input.anyKeyDown) {
                    StartCoroutine(CountDown());
                    onetime = false;
                }
            }
        }

        timeText.text = gameController.time.ToString("f0");

        
    }

    IEnumerator CountDown()
    {
        countDownText.text = "3";
        yield return new WaitForSeconds(1);
        countDownText.text = "2";
        yield return new WaitForSeconds(1);
        countDownText.text = "1";
        yield return new WaitForSeconds(1);
        countDownText.text = "";
        gameController.isBattling = true;
        gameController.SetStartTime();
    }
}
