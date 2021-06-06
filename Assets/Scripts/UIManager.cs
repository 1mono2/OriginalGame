using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameController gameController;
    public Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = gameController.time.ToString("f1");
    }
}
