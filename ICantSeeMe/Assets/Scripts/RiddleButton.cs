using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RiddleButton : MonoBehaviourPun
{
    public ParchmentTrigger parchment;
    public bool isCorrect = false;

    public void OnClick()
    {
        photonView.RPC("Check", RpcTarget.All, null);
    }

    [PunRPC]
    public void Check()
    {
        parchment.CheckAnswer(isCorrect);
    }
}
