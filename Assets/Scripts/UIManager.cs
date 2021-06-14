using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameController gameController;
    public Text timeText;
    public Text countDownText;
    public Canvas explain;

    bool getKey;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GetComponent<GameController>();
        getKey = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.isBattling == false & getKey ==true)
        {
            if (Input.anyKeyDown)
            {
                explain.gameObject.SetActive(false);
                StartCoroutine(CountDown());
                getKey = false;
            }
        }

        timeText.text = gameController.time.ToString("f1");

        
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
    }
}
