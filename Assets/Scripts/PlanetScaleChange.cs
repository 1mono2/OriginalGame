using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlanetScaleChange : MonoBehaviourPunCallbacks
{
    bool isAvailable;
    GameController gameController;

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

                    gameController.PlanetScaleUp();
                    if (photonView.IsMine)
                    {
                        Debug.Log("1p destroys item");
                        photonView.RPC(nameof(DestroyItemOnLocal), RpcTarget.AllViaServer);
                    }
                    else
                    {
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

                    gameController.PlanetScaleDown();
                    if (photonView.IsMine)
                    {
                        Debug.Log("2p has ownership and destroys item");
                        photonView.RPC(nameof(DestroyItemOnLocal), RpcTarget.AllViaServer);
                    }
                    else
                    {
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
