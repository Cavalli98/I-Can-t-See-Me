using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ParchmentTrigger : Trigger
{
    public GameObject parchment;
    public List<UnityEngine.UI.Button> buttons;
    public List<GameObject> toActivateWrongAnswer;
    private bool _isActive = false;
    private GameObject player;
    private PlayerMovement playerMovement;

    [PunRPC]
    public override void trigger()
    {
        print("ParchmentTrigger");
        _isActive = true;
        playerMovement.setTalking(true);

        //Set panel for player 1
        //if (!PhotonNetwork.IsMasterClient)
        if (player.name == "Girl(Clone)")
        {
            print("Parchment trigger girl");
            parchment.SetActive(true);
            parchment.transform.Find("Text").gameObject.SetActive(false);
            foreach (UnityEngine.UI.Button b in buttons)
            {
                b.gameObject.SetActive(true);
                b.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            print("Parchment trigger boy");
            parchment.SetActive(true);
            foreach (UnityEngine.UI.Button b in buttons)
                b.interactable = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Parchment trigger Enter collision");
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            playerMovement = player.GetComponent<PlayerMovement>();
            photonView.TransferOwnership(PhotonView.Get(player).Owner);
            this.photonView.RPC("setPlayer", RpcTarget.Others, null);
        }
    }

    public void CheckAnswer(bool answer)
    {
        print("CheckAnswer BrickRoad.. "+answer);
        parchment.SetActive(false);
        foreach (UnityEngine.UI.Button b in buttons)
            b.gameObject.SetActive(false);
        playerMovement.setTalking(false);
        

        if(answer) {
            foreach (GameObject t in toActivate)
                t.GetComponent<Triggerable>().activate();
        }
        else {
            foreach (GameObject t in toActivateWrongAnswer)
                t.GetComponent<Triggerable>().activate();
        }
    }

    [PunRPC]
    public void setPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            print("name: "+p.name);
            if (player == null && p.name == "Boy(Clone)") {
                player = p;
                playerMovement = player.GetComponent<PlayerMovement>();
            }
        }
        print("In setPlayer, players name: "+player.name);
    }
}
