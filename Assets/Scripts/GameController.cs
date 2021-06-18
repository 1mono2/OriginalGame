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
    public GameObject spawner;
    SpawnerScript spawnerScript;
    public Camera cameraParticle;
   

    [SceneName]
    public string resultSceneSeeker;
    [SceneName]
    public string resultSceneEscapee;

    public float time = 60.0f;

    public bool isBattling;

    GameObject hiroyukiCat;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player1.GetComponent<PlayerController>();
        player2Controller = player2.GetComponent<Player2Controller>();
        spawnerScript = spawner.GetComponent<SpawnerScript>();
        isBattling = false;

        hiroyukiCat =  GameObject.Find("HiroyukiCat");
        hiroyukiCat.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void  Update()
    {
        if (isBattling == true)
        { 
            if(time <= 0)
            {
                
                hiroyukiCat.SetActive(true);
                StartCoroutine(LoadResulEscapeet());
            }
            else
            {
                time -= Time.deltaTime;
            }
        }
    }

    public IEnumerator LoadResult ()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(resultSceneSeeker);
    }

    public IEnumerator LoadResulEscapeet()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(resultSceneEscapee);
    }

    public void P1MoveUpDownPos(float beforeScale, float afterScale)
    {
        playerController.MoveUpDownPos(beforeScale, afterScale);
    }

    public void P2MoveUpDownPos(float beforeScale, float afterScale)
    {
        player2Controller.MoveUpDownPos(beforeScale, afterScale);
        
    }

    public void SpawnerMoveUpDownPos(float beforeScale, float afterScale)
    {
        spawnerScript.MoveUpDownPos(beforeScale, afterScale);
    }

    // speedUpItem
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
