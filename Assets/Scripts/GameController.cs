using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    PlayerController playerController;
    Player2Controller player2Controller;

    [SceneName]
    public string resultScene;

    public float time = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player1.GetComponent<PlayerController>();
        player2Controller = player2.GetComponent<Player2Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if(time <= 0)
        {
            SceneManager.LoadScene(resultScene);
        }
    }

    private IEnumerator LoadResult ()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(resultScene);
    }

    public void P1MoveUpDownPos(float beforeScale, float afterScale)
    {
        playerController.MoveUpDownPos(beforeScale, afterScale);
    }

    public void P2MoveUpDownPos(float beforeScale, float afterScale)
    {
        player2Controller.MoveUpDownPos(beforeScale, afterScale);
    }
}
