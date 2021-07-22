using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

/// <summary>
/// プレイヤーがReady状態か判定するのは上手くいったと思う。
/// 後は、ボタンに作って、Ready状態をいじれるようにする。
/// </summary>

public class GameController : MonoBehaviourPunCallbacks
{
 
    public GameObject player1;
    public GameObject player2;
    PlayerController playerController;
    Player2Controller player2Controller;
    public GameObject spawner;
    SpawnerScript spawnerScript;
    public Camera cameraParticle;
    IntegratedManager integratedManager;
    

    [SceneName]
    public string resultSceneSeeker;
    [SceneName]
    public string resultSceneEscapee;

    public float time = 60.0f;

    public bool isBattling;

    GameObject hiroyukiCat;
    GameObject UniCat;

    // Photon Instance
    bool readyAllplayers = false;
    Photon.Realtime.Player[] players;
    // temporary
    // public GameObject IntegratedManager;

   

    // Start is called before the first frame update
    void Start()
    {
        // temporary
        // Instantiate(IntegratedManager);

        integratedManager = GameObject.Find("IntegratedManager").GetComponent<IntegratedManager>();
        // 一時的にtrue Mainから起動するため
        // integratedManager.isOnline = true;
        Connect();

        playerController = player1.GetComponent<PlayerController>();
        player2Controller = player2.GetComponent<Player2Controller>();
        spawnerScript = spawner.GetComponent<SpawnerScript>();
        
        // SceneManager.MoveGameObjectToScene(IntegratedManager, SceneManager.GetActiveScene());
       
        isBattling = false;

        // hiroyuki Cat
        hiroyukiCat =  GameObject.Find("HiroyukiCat");
        hiroyukiCat.gameObject.SetActive(false);
        // 宇宙猫
        UniCat = GameObject.Find("UniverseCat");
        UniCat.gameObject.SetActive(false);

       
    }

    // Update is called once per frame
    void Update()
    {
        if (!readyAllplayers)
        {
            readyAllplayers = CheckReadyStateAllPlayers();
        }
        

        if (isBattling == true && readyAllplayers == true)
        {
            if (time <= 0)
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

    // Photon
    public void Connect()
    {
        if (integratedManager.isOnline)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Online Now!");
        }
        else
        {
            PhotonNetwork.OfflineMode = true;
            Debug.Log("Offline Now!");
        }
    }

    public void DisConnect()
    {
        if (integratedManager.isOnline)
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
        // PhotonNetwork.NetworkingClient.OpJoinRandomOrCreateRoom(null, null); RandomRoom or CreateRoom
    }

    public override void OnJoinedRoom()
    {
        if (integratedManager.isOnline == true) {
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
        else{  // Offline
            Vector3 player1Pos = new Vector3(0, 8.5f, 0);
            PhotonNetwork.Instantiate("Player", player1Pos, Quaternion.identity);

            Vector3 player2Pos = new Vector3(0, 3f, 8);
            Quaternion rotate = new Quaternion(90, 0, 0, 0);
            PhotonNetwork.Instantiate("Player2", player2Pos, rotate);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
    }

   // Sceane
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

    // Photon
    public void SetReadyStateToTrue()
    {
        PhotonNetwork.LocalPlayer.SetReadyStateToTrue();
        Debug.Log("Set Ready!");
    }

    public IEnumerator CoroutineCheckReadyStateAllPlayers()
    {
        while (!readyAllplayers)
        {
            readyAllplayers = CheckReadyStateAllPlayers();
        }
        Debug.Log("All Player Ready.");
        yield return null;
    }

    public bool CheckReadyStateAllPlayers()
    {
        if (!PhotonNetwork.InRoom) { return false; }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            players = PhotonNetwork.PlayerList;
            bool[] state = new bool[2];
            for (int i = 0; i < state.Length; i++)
            {
                state[i] = players[i].GetReadyState();
            }

            if (state.All(i => i == true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
