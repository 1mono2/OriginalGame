using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2Controller : MonoBehaviour
{
    public float moveSpeed = 5;
    private Vector3 moveDir;
    Rigidbody rigidbody;
    public GameObject planet;
    public GameObject gameobjController;
    GameController gameController;

    GameObject astronaut;
    Animator animator;


    private void Start()
    {
        
        rigidbody = GetComponent<Rigidbody>();
        gameController = gameobjController.GetComponent<GameController>();

        astronaut = GameObject.Find("Player2/Astronaut");
        animator = astronaut.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.isBattling == true)
        {
            float h = Input.GetAxisRaw("Player2Horizontal");
            float v = Input.GetAxisRaw("Player2Vertical");
            moveDir = new Vector3(h, 0, v).normalized;

            animator.SetInteger("AnimationPar", 0);
            if (h != 0 | v != 0)
            {
                animator.SetInteger("AnimationPar", 1);

            }
            if (v > 0)
            {
                astronaut.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                if (h < 0)
                {
                    astronaut.gameObject.transform.localEulerAngles = new Vector3(0, -45, 0);
                }
                else if (h > 0)
                {
                    astronaut.gameObject.transform.localEulerAngles = new Vector3(0, 45, 0);
                }

            }
            else if (v < 0)
            {
                astronaut.gameObject.transform.localEulerAngles = new Vector3(0, 180, 0);
                if (h < 0)
                {
                    astronaut.gameObject.transform.localEulerAngles = new Vector3(0, -135, 0);
                }
                else if (h > 0)
                {
                    astronaut.gameObject.transform.localEulerAngles = new Vector3(0, 135, 0);
                }
            }
            else if (h < 0)
            {
                astronaut.gameObject.transform.localEulerAngles = new Vector3(0, -90, 0);
            }
            else if (h > 0)
            {
                astronaut.gameObject.transform.localEulerAngles = new Vector3(0, 90, 0);
            }
        }
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "escapee")
        {
            StartCoroutine(gameController.LoadResult());
        }
    }


    public void SpeedUp()
    {
        moveSpeed = 10;
        gameController.FloatingParticle("P2");
        StartCoroutine(SetDefalutSpeed());
    }

    public IEnumerator SetDefalutSpeed()
    {
        yield return new WaitForSeconds(5.0f);
        moveSpeed = 5;
        gameController.FinishingParticle();
    }

    public void Warp()
    {

        SphereCollider planetCollider = planet.GetComponent<SphereCollider>();
        Vector3 randomPos = GetPosition(Random.Range(0, 360), Random.Range(0, 360),
            planetCollider.radius * (planet.transform.localScale.x - 1));
        Debug.Log(randomPos);
        rigidbody.MovePosition(randomPos);
    }

    public Vector3 GetPosition(float angle1, float angle2, float radius)
    {
        float x = radius * Mathf.Sin(angle1 * Mathf.Deg2Rad) * Mathf.Cos(angle2 * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(angle1 * Mathf.Deg2Rad) * Mathf.Sin(angle2 * Mathf.Deg2Rad);
        float z = radius * Mathf.Cos(angle1 * Mathf.Deg2Rad);
        return new Vector3(x, y, z);
    }


    public void PlanetScaleDown()
    {
        planet.transform.localScale = new Vector3(10, 10, 10);
        SphereCollider planetCollider = planet.GetComponent<SphereCollider>();
        MoveUpDownPos(15.0f, 10.0f);
        gameController.P1MoveUpDownPos(15.0f, 10.0f);
        StartCoroutine(SetDefaltPlanetScale());
    }

    public IEnumerator SetDefaltPlanetScale()
    {
        yield return new WaitForSeconds(5.0f);
        planet.transform.localScale = new Vector3(15, 15, 15);
        MoveUpDownPos(10.0f, 15.0f);
        gameController.P1MoveUpDownPos(10.0f, 15.0f);
    }

    public void MoveUpDownPos(float beforeScale, float afterScale)
    {
        Vector3 scaleUpPos = transform.position * (afterScale / beforeScale);
        rigidbody.MovePosition(scaleUpPos);
    }
}
