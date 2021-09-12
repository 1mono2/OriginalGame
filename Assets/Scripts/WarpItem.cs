using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class WarpItem : MonoBehaviourPunCallbacks
{
    GameController gameController;

    private bool isAvailable;


    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        isAvailable = true;
       
    }

 

    private void OnTriggerEnter(Collider collider)
    {
       
        string tag = collider.gameObject.tag;
        photonView.RPC(nameof(RPCCheckAvailable), RpcTarget.AllViaServer, tag);
        
    }

    [PunRPC]
    void RPCCheckAvailable(string tag, PhotonMessageInfo info)
    {
        if (tag == "escapee")
        {
            if (isAvailable)
            {
                isAvailable = false;
                if (info.Sender.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                {

                    gameController.P1Warp();
                    if (photonView.IsMine)
                    {
                        Debug.Log("1p destroys item");
                        photonView.RPC(nameof(DestroyItemOnLocal), RpcTarget.AllViaServer);
                        //PhotonNetwork.Destroy(this.gameObject);
                    }
                    else
                    {
                        //photonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
                        //photonView.RequestOwnership();
                        //Debug.Log("Ownership was transfferd to 2p");
                        //PhotonNetwork.Destroy(this.gameObject);

                        Debug.Log("1p doesnt have ownership");
                        photonView.RPC(nameof(DestroyItemOnLocal), RpcTarget.AllViaServer);
                    }

                }
            }
        }

        if (tag == "seeker")
        {
            if (isAvailable)
            {
                isAvailable = false;
                if (info.Sender.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                {

                    gameController.P2Warp();
                    if (photonView.IsMine)
                    {
                        Debug.Log("2p has ownership and destroys item");
                        
                        photonView.RPC(nameof(DestroyItemOnLocal), RpcTarget.AllViaServer);
                        //PhotonNetwork.Destroy(this.gameObject);
                    }
                    else
                    {
                        // 権限の移譲だけではオーナー以外は消せない
                        //photonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
                        //photonView.RequestOwnership();
                        //Debug.Log("Ownership was transfferd to 2p");
                        //string owner = photonView.Owner.UserId;
                        //int  controller = photonView.ControllerActorNr;
                        //Debug.Log(string.Format("owner is {0}, controller is {0}", owner, controller));
                        //PhotonNetwork.Destroy(this.gameObject);
                        Debug.Log("2p doesnt have ownership.");
                        
                        photonView.RPC(nameof(DestroyItemOnLocal), RpcTarget.AllViaServer);
                        
                    }
                }
            }   
        }
    }

    [PunRPC]
    public void DestroyItemOnLocal()
    {
        Destroy(this.gameObject);
    }
}
