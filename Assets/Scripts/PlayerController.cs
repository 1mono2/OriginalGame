using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class PlayerController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    protected float defaultMoveSeed = 5;
    protected float moveSpeed;
    private Vector3 moveDir;
    Rigidbody rb;

    public GameObject planet;
    public GameObject gameobjController;
    GameController gameController;
    IntegratedManager integratedManager;
    IntegratedManager.GameMode mode;

    [SerializeField]
    private GameObject cat;
    Animator animator;

    // constant
    protected const float MOVESPEED_RATE = 2;
    protected const float WAIT_SECONDS = 5.0f;

    private void Awake()
    {
        integratedManager = GameObject.Find("IntegratedManager").GetComponent<IntegratedManager>();
        mode = integratedManager.GetMode();
        PhotonNetwork.NickName = "Player1";
        Vector3 camPos = new Vector3(0, 6.75f, -0.05f) + this.gameObject.transform.position;
        Quaternion camRotate = Quaternion.Euler(90, 0, 0);
        if (photonView.IsMine && mode == IntegratedManager.GameMode.online)
        {
            GameObject onlineCameraPrefab = (GameObject)Resources.Load("OnlineCamera");
            GameObject onlineCamera = Instantiate(onlineCameraPrefab, camPos, camRotate);
            onlineCamera.transform.parent = this.gameObject.transform;
            
        }
        else if (mode == IntegratedManager.GameMode.offline)
        {
            GameObject CameraPlayer1Prefab = (GameObject)Resources.Load("CameraPlayer1");
            GameObject onlineCamera = Instantiate(CameraPlayer1Prefab, camPos, camRotate);
            onlineCamera.transform.parent = this.gameObject.transform;
        }
        else if (mode == IntegratedManager.GameMode.cpu)
        {
            GameObject onlineCameraPrefab = (GameObject)Resources.Load("OnlineCamera");
            GameObject onlineCamera = Instantiate(onlineCameraPrefab, camPos, camRotate);
            onlineCamera.transform.parent = this.gameObject.transform;
        }


    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        planet = GameObject.Find("Planet");
        gameobjController = GameObject.Find("GameController");
        gameController = gameobjController.GetComponent<GameController>();

        animator = cat.GetComponent<Animator>();

        moveSpeed = defaultMoveSeed;

    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.isBattling == true && photonView.IsMine )
        {
            float h = Input.GetAxisRaw("Player1Horizontal");
            float v = Input.GetAxisRaw("Player1Vertical");
            moveDir = new Vector3(h, 0, v).normalized;
            float deg_dir = Mathf.Atan2(h, v) * Mathf.Rad2Deg;

            animator.SetInteger("Walking", 0);
            if (h != 0 | v != 0)
            {
                animator.SetInteger("Walking", 1); //true
                cat.gameObject.transform.localEulerAngles = new Vector3(0, deg_dir, 0);
            }
        }
    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
    }

    public void SpeedUp()
    {
        moveSpeed = moveSpeed * MOVESPEED_RATE;
        gameController.FloatingParticle("P1");
        StartCoroutine(SetDefalutSpeed());
    }

    public IEnumerator SetDefalutSpeed()
    {
        yield return new WaitForSeconds(WAIT_SECONDS);
        moveSpeed = defaultMoveSeed;
        gameController.FinishingParticle();
    }

    public void Warp()
    {
        SphereCollider planetCollider = planet.GetComponent<SphereCollider>();
        Vector3 randomPos = GetPosition(Random.Range(0, 360), Random.Range(0, 360),
            planetCollider.radius * (planet.transform.localScale.x - 1));
        Debug.Log(randomPos);
        rb.MovePosition(randomPos);
    }

    public Vector3 GetPosition(float angle1, float angle2, float radius)
    {
        float x = radius * Mathf.Sin(angle1 * Mathf.Deg2Rad) * Mathf.Cos(angle2 * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(angle1 * Mathf.Deg2Rad) * Mathf.Sin(angle2 * Mathf.Deg2Rad);
        float z = radius * Mathf.Cos(angle1 * Mathf.Deg2Rad);
        return new Vector3(x, y, z);
    }


    public void MoveUpDownPos(float beforeScale, float afterScale)
    {
        Vector3 scaleUpPos = transform.position * (afterScale / beforeScale);
        rb.MovePosition(scaleUpPos);
    }


   
}
