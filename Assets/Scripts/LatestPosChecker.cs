using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatestPosChecker : MonoBehaviour
{
    GameController gameController;
    ChaserController chaserController;
    Vector3 latestPos;
    // Start is called before the first frame update
    void Start()
    {

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        chaserController = GetComponentInParent<ChaserController>();
        latestPos = this.transform.position;
        StartCoroutine(CheckPosAndResetDir());
    }

  

    IEnumerator CheckPosAndResetDir()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (gameController.isBattling)
            {
                if (Vector3.Distance(latestPos, this.transform.position) < 0.9f)
                {
                    chaserController.SetDestination();
                }
                latestPos = this.transform.position;
            }
            
        }
    }
}
