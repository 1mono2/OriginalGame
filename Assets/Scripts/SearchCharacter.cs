using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchCharacter : MonoBehaviour
{
    ChaserController chaserController;
    //ChaserController.PurposeState purposeState;

    // Start is called before the first frame update
    void Start()
    {
        chaserController = GetComponentInParent<ChaserController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("escapee"))
        {
            Debug.Log("Player enter chase aria");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            chaserController.SetPurposeState(ChaserController.PurposeState.toGetItem, other.transform);
        }
        else if(other.CompareTag("escapee"))
        { 
            chaserController.SetPurposeState(ChaserController.PurposeState.chase, other.transform);
        }



    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("escapee"))
        {
            Debug.Log("Chaser lost player");
            chaserController.SetPurposeState(ChaserController.PurposeState.search);
        }
    }

    
}
