using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Player2Controller : MonoBehaviourPunCallbacks
{
    [SerializeField]
    protected float defaultMoveSeed = 5;
    protected float moveSpeed;
    protected Vector3 moveDir;
    protected Rigidbody rb;

    // Objects
    protected GameObject planet;
    protected GameController gameController;
    protected IntegratedManager integratedManager;
    protected IntegratedManager.GameMode mode;

    [SerializeField]
    protected GameObject astronaut;
    protected Animator animator;

    protected const float MOVESPEED_RATE = 2;
    protected const float WAIT_SECONDS = 5.0f;

    protected virtual void Awake()
    {
        integratedManager = GameObject.Find("IntegratedManager").GetComponent<IntegratedManager>();
        mode = integratedManager.GetMode();
        PhotonNetwork.NickName = "Player2";
        // Camera Inststance
        Vector3 camPos = new Vector3(0, 7.9f, -0.05f) + this.gameObject.transform.position;
        Quaternion camRotate = Quaternion.Euler(90, 180, 0);
        if (photonView.IsMine && mode == IntegratedManager.GameMode.online)
        {
            GameObject onlineCameraPrefab = (GameObject)Resources.Load("OnlineCamera");
            GameObject onlineCamera = Instantiate(onlineCameraPrefab, camPos, camRotate);
            onlineCamera.transform.parent = this.gameObject.transform;
           
        }else if(mode == IntegratedManager.GameMode.offline)
        {
            GameObject CameraPlayer2Prefab = (GameObject)Resources.Load("CameraPlayer2");
            GameObject onlineCamera = Instantiate(CameraPlayer2Prefab, camPos, camRotate);
            onlineCamera.transform.parent = this.gameObject.transform;
        }
    }

    protected virtual void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        planet = GameObject.FindWithTag("planet");
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        animator = astronaut.GetComponent<Animator>();

        moveSpeed = defaultMoveSeed;

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (gameController.isBattling == true && photonView.IsMine)
        {
            if(mode == IntegratedManager.GameMode.online || mode == IntegratedManager.GameMode.cpu)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                moveDir = new Vector3(h, 0, v).normalized;
            }
            else // mode == IntegratedManager.GameMode.offline
            {
                float h = Input.GetAxisRaw("Player2Horizontal");
                float v = Input.GetAxisRaw("Player2Vertical");
                moveDir = new Vector3(h, 0, v).normalized;
            }
            
            

            
         
        }
    }

    protected virtual void FixedUpdate()
    {
        if (gameController.isBattling == true)
        {
            rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);

            float deg_dir = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            animator.SetInteger("AnimationPar", 0);
            if (moveDir.x != 0 | moveDir.z != 0)
            {
                animator.SetInteger("AnimationPar", 1);
                astronaut.gameObject.transform.localEulerAngles = new Vector3(0, deg_dir, 0);
            }
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "escapee")
        {
            GameObject.Find("GameController").GetComponent<PhotonView>().RPC(nameof(gameController.SetUniCat), RpcTarget.AllViaServer);
        }
    }


    internal virtual void SpeedUp()
    {
        moveSpeed = moveSpeed * MOVESPEED_RATE;
        gameController.FloatingParticle("P2");
        StartCoroutine(SetDefalutSpeed());
    }

    internal IEnumerator SetDefalutSpeed()
    {
        yield return new WaitForSeconds(WAIT_SECONDS);
        moveSpeed = defaultMoveSeed;
        gameController.FinishingParticle();
    }

    internal void Warp()
    {
        SphereCollider planetCollider = planet.GetComponent<SphereCollider>();
        Vector3 randomPos = GetPosition(Random.Range(0, 360), Random.Range(0, 360),
            planetCollider.radius * (planet.transform.localScale.x - 1));
        Debug.Log(randomPos);
        rb.MovePosition(randomPos);
    }

    internal Vector3 GetPosition(float angle1, float angle2, float radius)
    {
        float x = radius * Mathf.Sin(angle1 * Mathf.Deg2Rad) * Mathf.Cos(angle2 * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(angle1 * Mathf.Deg2Rad) * Mathf.Sin(angle2 * Mathf.Deg2Rad);
        float z = radius * Mathf.Cos(angle1 * Mathf.Deg2Rad);
        return new Vector3(x, y, z);
    }

    internal void MoveUpDownPos(float beforeScale, float afterScale)
    {
        Vector3 scaleUpPos = transform.position * (afterScale / beforeScale);
        rb.MovePosition(scaleUpPos);
    }
}
