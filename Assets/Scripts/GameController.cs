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
    // resources in photon folder
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    GameObject player1;
    GameObject player2;
    PlayerController playerController;
    Player2Controller player2Controller;

    public GameObject spawnerPrefab;
    GameObject spawner;
    SpawnerMove spawnerMove;

    GameObject planet;
    Vector3 planetDefaultScale = new Vector3(15, 15, 15);
    Vector3 planetScaleUp = new Vector3(20, 20, 20);
    Vector3 planetScaleDown = new Vector3(10, 10, 10);

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

        planet = GameObject.Find("Planet");

        // hiroyuki Cat
        hiroyukiCat = GameObject.Find("HiroyukiCat");
        hiroyukiCat.gameObject.SetActive(false);
        // âFíàîL
        UniCat = GameObject.Find("UniverseCat");
        UniCat.gameObject.SetActive(false);

        isBattling = false;



    }

    // Update is called once per frame
    void Update()
    {
        if (integratedManager.isOnline)
        {
            // ÉvÉåÉCÉÑÅ[Ç™óéÇøÇΩÇ∆Ç´Ç«Ç§Ç∑ÇÈÇ©
            if (readyAllplayers == false || playerController == null || player2Controller == null || spawnerMove == null)
            {
                readyAllplayers = CheckReadyStateAllPlayers();
                player1 = GameObject.FindWithTag("escapee");
                if (player1)
                {
                    
                    playerController = player1.GetComponent<PlayerController>();
                    //Debug.Log(string.Format("{0}, is playerController", playerController));
                }
                player2 = GameObject.FindWithTag("seeker");
                if (player2)
                { 
                    player2Controller = player2.GetComponent<Player2Controller>();
                    //Debug.Log(string.Format("{0}, is player2Controller", player2Controller));
                }
                spawner = GameObject.FindWithTag("Respawn");
                
                if (spawner)
                {
                    spawnerMove = spawner.GetComponent<SpawnerMove>();
                    //Debug.Log(string.Format("{0}, is respawn", spawnerMove));
                }
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
        if (integratedManager.isOnline == true)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1) // cat
            {
                Vector3 playerPos = new Vector3(0, 8.5f, 0);
                player1 = PhotonNetwork.Instantiate(player1Prefab.gameObject.name, playerPos, Quaternion.identity);
                playerController = player1.GetComponent<PlayerController>();

                Vector3 spawnerPos = new Vector3(1.5f, 8.1f, 0f);
                spawner = PhotonNetwork.Instantiate(spawnerPrefab.gameObject.name, spawnerPos, Quaternion.identity);
                spawnerMove = spawner.GetComponent<SpawnerMove>();


            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 2) // astronaut
            {
                Vector3 player2Pos = new Vector3(0, 3f, 8);
                Quaternion rotate = new Quaternion(90, 0, 0, 0);
                player2 = PhotonNetwork.Instantiate(player2Prefab.gameObject.name, player2Pos, rotate);
                player2Controller = player2.GetComponent<Player2Controller>();


            }

            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }

        }
        else
        {  // Offline
            Vector3 playerPos = new Vector3(0, 8.5f, 0);
            player1 = PhotonNetwork.Instantiate(player1Prefab.gameObject.name, playerPos, Quaternion.identity);
            playerController = player1.GetComponent<PlayerController>();

            Vector3 player2Pos = new Vector3(0, 3f, 8);
            Quaternion rotate = new Quaternion(90, 0, 0, 0);
            player2 = PhotonNetwork.Instantiate(player2Prefab.gameObject.name, player2Pos, rotate);
            player2Controller = player2.GetComponent<Player2Controller>();

            Vector3 spawnerPos = new Vector3(1.5f, 8.1f, 0f);
            spawner = PhotonNetwork.Instantiate(spawnerPrefab.gameObject.name, spawnerPos, Quaternion.identity);
            spawnerMove = spawner.GetComponent<SpawnerMove>();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
    }


    // Sceane
    public IEnumerator LoadResultSeekerWin()
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

  
    // planet scale changed
    public void PlanetScaleUp()
    {
        planet.transform.localScale = planetScaleUp;
        P1MoveUpDownPos(planetDefaultScale.x, planetScaleUp.x);
        P2MoveUpDownPos(planetDefaultScale.x, planetScaleUp.x);
        SpawnerMoveUpDownPos(planetDefaultScale.x, planetScaleUp.x);
        StartCoroutine(SetDefaltPlanetScale());

    }

    public void PlanetScaleDown()
    {
        planet.transform.localScale = planetScaleDown;
        P1MoveUpDownPos(planetDefaultScale.x, planetScaleDown.x);
        P2MoveUpDownPos(planetDefaultScale.x, planetScaleDown.x);
        SpawnerMoveUpDownPos(planetDefaultScale.x, planetScaleDown.x);
        StartCoroutine(SetDefaltPlanetScale());
    }


    public IEnumerator SetDefaltPlanetScale()
    {
        yield return new WaitForSeconds(5.0f);
        float beforeScale = planet.transform.localScale.x;
        planet.transform.localScale = planetDefaultScale;
        P1MoveUpDownPos(beforeScale, planetDefaultScale.x);
        P2MoveUpDownPos(beforeScale, planetDefaultScale.x);
        SpawnerMoveUpDownPos(beforeScale, planetDefaultScale.x);
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
        spawnerMove.MoveUpDownPos(beforeScale, afterScale);
    }

    // speedUpItem
    public void P1SpeedUp()
    {
        playerController.SpeedUp();
    }

    public void P2SpeedUp()
    {
        player2Controller.SpeedUp();
    }

    public void FloatingParticle(string player)
    {
        if (player == "P1")
        {
            cameraParticle.gameObject.SetActive(true);
            cameraParticle.rect = new Rect(0, 0, 0.5f, 1);
        }
        else if (player == "P2")
        {
            cameraParticle.gameObject.SetActive(true);
            cameraParticle.rect = new Rect(0.5f, 0, 0.5f, 1);
        }
    }

    public void FinishingParticle()
    {
        cameraParticle.gameObject.SetActive(false);
    }

    // Warp Item
    public void P1Warp()
    {
        playerController.Warp();
    }

    public void P2Warp()
    {
        player2Controller.Warp();

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

        if (PhotonNetwork.CurrentRoom.PlayerCount != 2) { return false; }

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