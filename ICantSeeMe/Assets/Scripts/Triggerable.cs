using Photon.Pun;
using UnityEngine;

public class Triggerable : MonoBehaviourPun
{
    public GameObject fromTrigger;

    [PunRPC]
    public virtual void activate()
    {

    }
}
