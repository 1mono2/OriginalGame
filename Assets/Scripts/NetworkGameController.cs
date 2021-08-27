using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkGameController : MonoBehaviourPunCallbacks
{

    public bool finishFlag { get; private set; } = true;

   
    public void SetFinishFlag(bool flag)
    {
        finishFlag = flag;
    }
}
