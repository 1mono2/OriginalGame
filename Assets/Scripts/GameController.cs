using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    PlayerController playerController;
    Player2Controller player2Controller;

    public Camera cameraParticle;
   

    [SceneName]
    public string resultSceneSeeker;
    [SceneName]
    public string resultSceneEscapee;

    public float time = 60.0f;

    public bool isBattling;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player1.GetComponent<PlayerController>();
        player2Controller = player2.GetComponent<Player2Controller>();
        isBattling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBattling == true) { 
        time -= Time.deltaTime;
        if(time <= 0)
        {
            SceneManager.LoadScene(resultSceneEscapee);
        }
        }
    }

    public IEnumerator LoadResult ()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(resultSceneSeeker);
    }

    public void P1MoveUpDownPos(float beforeScale, float afterScale)
    {
        playerController.MoveUpDownPos(beforeScale, afterScale);
    }

    public void P2MoveUpDownPos(float beforeScale, float afterScale)
    {
        player2Controller.MoveUpDownPos(beforeScale, afterScale);
        
    }

    public void FloatingParticle(string player)
    {
        if(player == "P1")
        {
            cameraParticle.gameObject.SetActive(true);
             cameraParticle.rect = new Rect(0, 0, 0.5f, 1);
        }
        else if(player == "P2")
        {
            cameraParticle.gameObject.SetActive(true);
            cameraParticle.rect = new Rect(0.5f, 0, 0.5f, 1);
        }
    }

    public void FinishingParticle()
    {
        cameraParticle.gameObject.SetActive(false);
    }

}
