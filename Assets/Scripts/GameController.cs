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

public class GameController : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    // resources in photon folder
    [SerializeField]
    public GameObject Player1Prefab;
    [SerializeField]
    public GameObject Player2Prefab;
    GameObject player1;
    GameObject player2;
    PlayerController playerController;
    Player2Controller player2Controller;
    [SerializeField]
    GameObject chaserPrefab;
    GameObject chaser;
    ChaserController chaserController;

    [SerializeField]
    public GameObject SpawnerPrefab;
    GameObject spawner;
    SpawnerMove spawnerMove;

    GameObject planet;
    Vector3 planetDefaultScale = new Vector3(15, 15, 15);
    Vector3 planetScaleUp = new Vector3(20, 20, 20);
    Vector3 planetScaleDown = new Vector3(10, 10, 10);

    [SerializeField]
    public GameObject NetworkGameControllerPrefab;
    GameObject NetworkGameControllerObj;
    NetworkGameController networkGameController;

    public Camera cameraParticle;
    IntegratedManager integratedManager;
    private IntegratedManager.GameMode mode;

    [SceneName]
    public string resultSceneSeeker;
    [SceneName]
    public string resultSceneEscapee;

    [HideInInspector]
    public float time;
    public float defaulTime = 60.0f;

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
        mode = integratedManager.GetMode();
        Connect();

        planet = GameObject.Find("Planet");

        // hiroyuki Cat
        hiroyukiCat = GameObject.Find("HiroyukiCat");
        hiroyukiCat.gameObject.SetActive(false);
        // 宇宙猫
        UniCat = GameObject.Find("UniverseCat");
        UniCat.gameObject.SetActive(false);

        isBattling = false;
        time = defaulTime;



    }

    // Update is called once per frame
    void Update()
    {
        
        if (mode == IntegratedManager.GameMode.online)
        {
            if (readyAllplayers == false || playerController == null || player2Controller == null || spawnerMove == null || networkGameController == null)
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

                NetworkGameControllerObj = GameObject.FindWithTag("NetworkGameController");
                if (NetworkGameControllerObj)
                {
                    networkGameController = NetworkGameControllerObj.GetComponent<NetworkGameController>();
                }
                
            }


            if (isBattling == true && readyAllplayers == true)
            {
                if(!PhotonNetwork.CurrentRoom.TryGetStartTime(out int timestamp)) { return; }

                float elapsedTime = Mathf.Max(0f, unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000f);
                time = defaulTime - elapsedTime;
                if (time <= 0)
                {
                    Debug.Log("Lose Seeker");
                    if (networkGameController.finishFlag)
                    {
                        photonView.RPC(nameof(SetHiroCat), RpcTarget.AllViaServer);
                    }
                }
            }
        }
        else if(mode == IntegratedManager.GameMode.offline || mode == IntegratedManager.GameMode.cpu)
        {
            if (isBattling == true)
            {
                if (time <= 0)
                {
                    if (networkGameController.finishFlag)
                    {
                        photonView.RPC(nameof(SetHiroCat), RpcTarget.AllViaServer);
                    }
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

        if (mode == IntegratedManager.GameMode.online)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Online battle Now!");
        }
        else if (mode == IntegratedManager.GameMode.offline)
        {
            PhotonNetwork.OfflineMode = true;
            Debug.Log("Offline battle Now!");
        }
        else if (mode == IntegratedManager.GameMode.cpu)
        {
            PhotonNetwork.OfflineMode = true;
            Debug.Log("CPU battle Now!");
        }
    }

    public void DisConnect()
    {
        if (mode == IntegratedManager.GameMode.online)
        {
            PhotonNetwork.Disconnect();
        }
        else  //offline cpu
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
        Vector3 playerPos = new Vector3(0, 8.5f, 0);
        Vector3 player2Pos = new Vector3(0, 3f, 8);
        Vector3 spawnerPos = new Vector3(1.5f, 8.1f, 0f);
        Quaternion rotate = Quaternion.Euler(0, 180, 0);
        if (mode == IntegratedManager.GameMode.online)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1) // cat
            {
                player1 = PhotonNetwork.Instantiate(Player1Prefab.gameObject.name, playerPos, Quaternion.identity);
                playerController = player1.GetComponent<PlayerController>();

                spawner = PhotonNetwork.Instantiate(SpawnerPrefab.gameObject.name, spawnerPos, Quaternion.identity);
                spawnerMove = spawner.GetComponent<SpawnerMove>();

                NetworkGameControllerObj = PhotonNetwork.Instantiate(NetworkGameControllerPrefab.gameObject.name, Vector3.zero , Quaternion.identity);
                networkGameController = NetworkGameControllerObj.GetComponent<NetworkGameController>();
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 2) // astronaut
            {
                player2 = PhotonNetwork.Instantiate(Player2Prefab.gameObject.name, player2Pos, rotate);
                player2Controller = player2.GetComponent<Player2Controller>();

                
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }

        }
        else if (mode == IntegratedManager.GameMode.offline)
        {  
           
            player1 = PhotonNetwork.Instantiate(Player1Prefab.gameObject.name, playerPos, Quaternion.identity);
            playerController = player1.GetComponent<PlayerController>();

            
            player2 = PhotonNetwork.Instantiate(Player2Prefab.gameObject.name, player2Pos, rotate);
            player2Controller =  player2.GetComponent<Player2Controller>();

            
            spawner = PhotonNetwork.Instantiate(SpawnerPrefab.gameObject.name, spawnerPos, Quaternion.identity);
            spawnerMove = spawner.GetComponent<SpawnerMove>();

            NetworkGameControllerObj = PhotonNetwork.Instantiate(NetworkGameControllerPrefab.gameObject.name, spawnerPos, Quaternion.identity);
            networkGameController = NetworkGameControllerObj.GetComponent<NetworkGameController>();
        }
        else if (mode == IntegratedManager.GameMode.cpu)
        {
            player1 = PhotonNetwork.Instantiate(Player1Prefab.gameObject.name, playerPos, Quaternion.identity);
            playerController = player1.GetComponent<PlayerController>();


            player2 = PhotonNetwork.Instantiate(chaserPrefab.gameObject.name, player2Pos, rotate);
            player2Controller = player2.GetComponent<ChaserController>();


            spawner = PhotonNetwork.Instantiate(SpawnerPrefab.gameObject.name, spawnerPos, Quaternion.identity);
            spawnerMove = spawner.GetComponent<SpawnerMove>();

            NetworkGameControllerObj = PhotonNetwork.Instantiate(NetworkGameControllerPrefab.gameObject.name, spawnerPos, Quaternion.identity);
            networkGameController = NetworkGameControllerObj.GetComponent<NetworkGameController>();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
    }


    // Time
    public void SetStartTime() {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }
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
    [PunRPC]
    public void SetHiroCat(PhotonMessageInfo info)
    {

        networkGameController.SetFinishFlag(false);
        hiroyukiCat.SetActive(true);
        StartCoroutine(LoadResulEscapeeWin());
        DisConnect();      
    }
    [PunRPC]
    public void SetUniCat(PhotonMessageInfo info)
    {
     
        networkGameController.SetFinishFlag(false);
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
            if (mode == IntegratedManager.GameMode.online || mode == IntegratedManager.GameMode.cpu)
            {
                cameraParticle.rect = new Rect(0, 0, 1, 1);
            }
            else if (mode == IntegratedManager.GameMode.offline)
            {
                cameraParticle.rect = new Rect(0, 0, 0.5f, 1);
            }
            
        }
        else if (player == "P2")
        {
            cameraParticle.gameObject.SetActive(true);
            if (mode == IntegratedManager.GameMode.online || mode == IntegratedManager.GameMode.cpu)
            {
                cameraParticle.rect = new Rect(0, 0, 1, 1);
            }
            else if (mode == IntegratedManager.GameMode.offline)
            {
                cameraParticle.rect = new Rect(0.5f, 0, 0.5f, 1);
            }
          
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

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // 勝敗が決した後に接続が切れたら？ NetworkGameControllerがないのでFlag確認できない
        // NickNameで判定するのは不安が残る。Seekerかescapeeで的確に分ける方法ないか？
        player1 = GameObject.FindWithTag("escapee");
        player2 = GameObject.FindWithTag("seeker");
        Debug.Log("Call OnPlayerLeftRoom");
        if (otherPlayer.NickName == "Player1")
        {
            Debug.Log("Player1 left this game");
            photonView.RPC(nameof(SetUniCat), RpcTarget.AllViaServer);
        }
        else if (otherPlayer.NickName == "Player2")
        {
            Debug.Log("Player1 left this game");
            photonView.RPC(nameof(SetHiroCat), RpcTarget.AllViaServer);
        }

    }

    
}