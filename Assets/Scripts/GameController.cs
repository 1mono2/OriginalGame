using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviourPunCallbacks
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

    public bool isOnline = true;

    // Start is called before the first frame update
    void Start()
    {
        Connect();

        playerController = player1.GetComponent<PlayerController>();
        player2Controller = player2.GetComponent<Player2Controller>();
        spawnerScript = spawner.GetComponent<SpawnerScript>();
        isBattling = false;

        hiroyukiCat =  GameObject.Find("HiroyukiCat");
        hiroyukiCat.gameObject.SetActive(false);
    }

    public void Connect()
    {
        if (isOnline)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.OfflineMode = true;
        }
    }

    public void DisConnect()
    {
        if (isOnline)
        {
            PhotonNetwork.Disconnect();
        }
        else
        {
            PhotonNetwork.OfflineMode = false;
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();

    }

    public override void OnJoinedRoom()
    {

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) // cat
        {
            Vector3 playerPos = new Vector3(0, 8.5f, 0);
            PhotonNetwork.Instantiate("Player", playerPos, Quaternion.identity);
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2) // astronaut
        {
            Vector3 playerPos = new Vector3(0, 3f, 8);
            Quaternion rotate = new Quaternion(90, 0, 0, 0);
            PhotonNetwork.Instantiate("Player2", playerPos, rotate);
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
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
