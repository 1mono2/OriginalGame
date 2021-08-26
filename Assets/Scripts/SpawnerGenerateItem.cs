using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnerGenerateItem : MonoBehaviourPunCallbacks
{
    private GameObject gameControllerObj;
    private GameController gameController;

    public GameObject[] items;
    public float itemInterval = 7f;

    // Start is called before the first frame update
    void Start()
    {
        gameControllerObj = GameObject.Find("GameController");
        gameController = gameControllerObj.GetComponent<GameController>();

        StartCoroutine(ItemSearchExistenceAndGenerate());
    }


    IEnumerator ItemSearchExistenceAndGenerate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);  // search items per 1seconds
           if (gameController.isBattling == true && photonView.IsMine)
             {
            
                GameObject item = GameObject.FindGameObjectWithTag("Item");
                if (item == null)
                {
                    yield return new WaitForSeconds(itemInterval);
                    // Instantiate(items[Random.Range(0, items.Length)], this.transform.position, Quaternion.identity);
                    PhotonNetwork.Instantiate(items[Random.Range(0, items.Length)].gameObject.name, this.transform.position, Quaternion.identity);
                }
            }

        }
    }
    
}
