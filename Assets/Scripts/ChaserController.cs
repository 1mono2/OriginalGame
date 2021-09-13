using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChaserController : Player2Controller
{
 
    SphereCollider planetCollider;
    public enum PurposeState
    {
        chase,
        search,
        toGetItem,
    }
    private PurposeState purposeState = PurposeState.search;
    private Vector3 playerPos;
    private Vector3 destination;
    float planetMag;
    Vector3 arcticPos = new Vector3(0.1f, 7.49f, 0.1f);
    Vector3 latestChaserPos;
    GameObject sphere;
    protected override void Awake()
    {
        integratedManager = GameObject.Find("IntegratedManager").GetComponent<IntegratedManager>();
        mode = integratedManager.GetMode();
        
    }

    protected override void Start()
    {

        rb = GetComponent<Rigidbody>();
        planet = GameObject.FindWithTag("planet");
        planetCollider = planet.GetComponent<SphereCollider>();
        planetMag = planetCollider.radius * (planet.transform.localScale.x);
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        animator = astronaut.GetComponent<Animator>();

        destination = RandomDestination();
        Vector3 chaserOnPlanetPos = transform.position * (planetMag / transform.position.magnitude);
        moveDir = CalAzimuth(chaserOnPlanetPos, destination);
    }


    // Update is called once per frame
    protected override void Update()
    {
        if (gameController.isBattling == true)
        {

            transform.rotation = Quaternion.LookRotation(CalArcticDirection(this.transform.position), this.transform.position);
            if (purposeState == PurposeState.chase)
            {

                Vector3 playerOnPlanetPos = destination * (planetMag / destination.magnitude);
                Vector3 chaserOnPlanetPos = transform.position * (planetMag / this.transform.position.magnitude);
                moveDir = CalAzimuth(chaserOnPlanetPos, playerOnPlanetPos);

            }
            else if (purposeState == PurposeState.search)
            {
                if(Vector3.Distance(destination, this.transform.position) < 1f)
                {
                    
                   /* if (sphere)
                    {
                        Destroy(sphere.gameObject);
                    }*/
                    
                    destination = RandomDestination();
                }
                else
                {
                    Vector3 chaserOnPlanetPos = transform.position * (planetMag / transform.position.magnitude);
                    moveDir = CalAzimuth(chaserOnPlanetPos, destination);
                }

            }
            else if (purposeState == PurposeState.toGetItem)
            {
                Vector3 itdmOnPlanetPos = destination * (planetMag / destination.magnitude);
                Vector3 chaserOnPlanetPos = transform.position * (planetMag / transform.position.magnitude);
                moveDir = CalAzimuth(chaserOnPlanetPos, itdmOnPlanetPos);
            }

            float deg_dir = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            animator.SetInteger("AnimationPar", 0);
            if (moveDir.z != 0 | moveDir.x != 0)
            {
                animator.SetInteger("AnimationPar", 1);
                astronaut.gameObject.transform.localEulerAngles = new Vector3(0, deg_dir, 0);
            }

        }
    }


    internal void SetPurposeState(PurposeState tempState, Transform targetObj = null )
    {
        purposeState = tempState;
        if(tempState == PurposeState.chase)
        {
            destination = targetObj.position;
        }
        else if (tempState == PurposeState.search)
        {
            destination = RandomDestination();
        }
        else if(tempState == PurposeState.toGetItem)
        {
            destination = targetObj.position;
        }
    }

    internal IEnumerator SetPurposeState(PurposeState tempState)
    {
        yield return new WaitForSeconds(1.0f);
        purposeState = tempState;
       /* if (tempState == PurposeState.chase)
        {
            playerPos = targetObj.position;
        }
        else*/ 
        if (tempState == PurposeState.search)
        {
            destination = RandomDestination();
        }
        /*else if (tempState == PurposeState.toGetItem)
        {
            destination = targetObj.position;
        }*/
    }

    internal PurposeState GetPurposeState()
    {
        return purposeState;
    }

    private Vector3 RandomDestination()
    {
        
        Vector3 randomPos = GetPosition(Random.Range(0, 360), Random.Range(0, 360), planetCollider.radius * (planet.transform.localScale.x));
        Debug.Log("Chaser set next destionation to" + randomPos);
        //sphere= Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), randomPos, Quaternion.identity);
        return randomPos;
    }

    internal void SetDestination()
    {
        destination = RandomDestination();
        
    }
    
    Vector3 CalRelayPoint(Vector3 destination)
    {
        return destination;
    }

    Vector3 CalAzimuth(Vector3 from, Vector3 to)
    {
        float vlon = Mathf.Atan2(-from.x, from.z);
        float vlat = Mathf.Asin(from.y / planetMag);
        float wlon = Mathf.Atan2(-to.x, to.z);
        float wlat = Mathf.Asin(to.y / planetMag);
        float a1 = Mathf.Cos(vlat);
        float a2 = Mathf.Tan(wlat);
        float b1 = Mathf.Sin(vlat);
        float b2 = Mathf.Cos(wlon - vlon);
        float c1 = Mathf.Sin(wlon - vlon);
        float azimuth =  Mathf.PI/2 - Mathf.Atan2(a1 * a2 - b1 * b2, c1) ;
        float lon1_m = vlon * 180 / Mathf.PI;
        float lat1_m = vlat * 180 / Mathf.PI;
        float lon2_m = wlon * 180 / Mathf.PI;
        float lat2_m = wlat * 180 / Mathf.PI;
        float houi = azimuth * Mathf.Rad2Deg;
        Vector3 direction = new Vector3(Mathf.Sin(azimuth), 0, Mathf.Cos(azimuth)).normalized;
        /*float h = Input.GetAxisRaw("Player2Horizontal");
        float v = Input.GetAxisRaw("Player2Vertical");
        moveDir = new Vector3(h, 0, v).normalized;*/
        return direction;
    }

    Vector3 CalArcticDirection(Vector3 pos)
    {
        float a1 = Mathf.Pow(arcticPos.x, 2) + Mathf.Pow(arcticPos.y, 2) + Mathf.Pow(arcticPos.z, 2);
        float b1 = Mathf.Pow(pos.x, 2) + Mathf.Pow(pos.y, 2) + Mathf.Pow(pos.z, 2);
        float c1 = arcticPos.x * pos.x + arcticPos.y * pos.y + arcticPos.z * pos.z;
        float s = (a1 - c1) / (a1 - (Mathf.Pow(c1, 2) / b1));
        float t_1 = -(a1 * c1 - Mathf.Pow(c1, 2)) / (a1 * b1 - Mathf.Pow(c1, 2));

        Vector3 vectorBP = s * arcticPos + t_1 * pos;
        return vectorBP;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            // コルーチンで〇秒後にするのは不安定で使いたくない
            // しかし、DestroyしてもGetItemが代入されてしまうため
            StartCoroutine(SetPurposeState(PurposeState.search));
        }
    }

}
