using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

/// <summary>
/// 
/// </summary>

public class GameController : MonoBehaviourPunCallbacks
{
 
    public GameObject player1;
    public GameObject player2;
    PlayerController playerController;
    Player2Controller player2Controller;
    public GameObject spawner;
    SpawnerMove spawnerMove;
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
    public bool readyAllplayers = false;
    Photon.Realtime.Player[] players;


   

    // Start is called before the first frame update
    void Start()
    {
        integratedManager = GameObject.Find("IntegratedManager").GetComponent<IntegratedManager>();
        Connect();

        playerController = player1.GetComponent<PlayerController>();
        player2Controller = player2.GetComponent<Player2Controller>();
        spawnerMove = spawner.GetComponent<SpawnerMove>();

        // hiroyuki Cat
        hiroyukiCat =  GameObject.Find("HiroyukiCat");
        hiroyukiCat.gameObject.SetActive(false);
        // �F���L
        UniCat = GameObject.Find("UniverseCat");
        UniCat.gameObject.SetActive(false);

        isBattling = false;



    }

    // Update is called once per frame
    void Update()
    {
        if (integratedManager.isOnline)
        {
            if (!readyAllplayers)
            {
                readyAllplayers = CheckReadyStateAllPlayers();
            }


            if (isBattling == true && readyAllplayers == true)
            {
                if (time <= 0)
                {
                    SetHiroCat();
                }
                else
                {
                    time -= Time.deltaTime;
                }
            }
        }
        else //offline
        {
            if (isBattling == true)
            {
                if (time <= 0)
                {
                    SetHiroCat();
                   
                }
                else
                {
                    time -= Time.deltaTime;
                }
            }
        }
    }

    // Photon
    public void Connect()
    {
        
        Debug.Log(integratedManager.isOnline);
        if (integratedManager.isOnline)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Online Now!");
        }
        else  //offline
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
                PhotonNetwork.Instantiate(player1.gameObject.name, playerPos, Quaternion.identity);
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 2) // astronaut
            {
                Vector3 playerPos = new Vector3(0, 3f, 8);
                Quaternion rotate = new Quaternion(90, 0, 0, 0);
                PhotonNetwork.Instantiate(player2.gameObject.name, playerPos, rotate);


                Vector3 spawnerPos = new Vector3(1.5f, 8.1f, 0f);
                PhotonNetwork.Instantiate(spawner.gameObject.name, spawnerPos, Quaternion.identity);
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }

        }
        else{  // Offline
            Vector3 player1Pos = new Vector3(0, 8.5f, 0);
            PhotonNetwork.Instantiate(player1.gameObject.name, player1Pos, Quaternion.identity);

            Vector3 player2Pos = new Vector3(0, 3f, 8);
            Quaternion rotate = new Quaternion(90, 0, 0, 0);
            PhotonNetwork.Instantiate(player2.gameObject.name, player2Pos, rotate);

            Vector3 spawnerPos = new Vector3(1.5f, 8.1f, 0f);
            PhotonNetwork.Instantiate(spawner.gameObject.name, spawnerPos, Quaternion.identity);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
    }

   // Sceane
    public IEnumerator LoadResultSeekerWin ()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(resultSceneSeeker);
    }

    public IEnumerator LoadResulEscapeeWin()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(resultSceneEscapee);
    }

    public void SetHiroCat()
    {
        hiroyukiCat.SetActive(true);
        StartCoroutine(LoadResulEscapeeWin());
        DisConnect();
    }

    public void SetUniCat()
    {
        UniCat.gameObject.SetActive(true);
        StartCoroutine(LoadResultSeekerWin());
        DisConnect();
    }

    // Pos moving
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
        spawnerMove.MoveUpDownPos(beforeScale, afterScale);
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

    // Particle
    public void FinishingParticle()
    {
        cameraParticle.gameObject.SetActive(false);
    }

    // Photon
    public void SetReadyStateToTrue()  //Refer to UI button
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

        if (PhotonNetwork.CurrentRoom.PlayerCount != 2){ return false; }

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
}
