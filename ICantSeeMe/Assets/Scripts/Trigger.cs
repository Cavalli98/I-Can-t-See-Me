using Photon.Pun;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject toActivate;

    [PunRPC]
    public void activate()
    {
        
    }
}
