using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class Stranger : MonoBehaviourPun
{
    private bool _hasSword = true;
    public Animator animator;

    public string[] Messages;
    [HideInInspector]
    public int CurrentMessageIndex = 0;  // Default to first
    public bool ShowMessages = false;
    public Text dialogueText;
    public GameObject panel;
    public GameObject image;
    private PlayerMovement playerMovement;
    private GameObject player = null;

    void Start()
    {
        // Load messages, if you did not make them in the Editor
        string mess1 = "Knight: Thanks for saving me, young lady. I am a knight of the kingdom who came here a long time ago to defeat the goddess, but she trapped me in here and I couldn't find a way to escape!";
        string mess2 = "Knight: But I can see that your doing better than me. Maybe you have the power and abilities to defeat the goddess.";
        string mess3 = "Knight: Here, take this. We can't help everyone, but everyone can help someone.";
        Messages = new string[] { mess1, mess2, mess3 };
        animator.SetBool("hasSword", _hasSword);
        panel.SetActive(false);
        image.SetActive(false);
    }

    void Update()
    {
        if(ShowMessages)
        {
            if (Input.GetKeyDown(KeyCode.E))// Space Click to move forward
            { 
                CurrentMessageIndex++;
            }

            // Turn off messages if we get to the last one.
            if (CurrentMessageIndex > Messages.GetUpperBound(0))
            {
                ShowMessages = false;
                panel.SetActive(false);
                image.SetActive(false);
                this.photonView.RPC("changeAnimation", RpcTarget.All, player.name);
            }
            else
                dialogueText.text = Messages[CurrentMessageIndex];

            // Bounds checking
            CurrentMessageIndex = Mathf.Clamp(CurrentMessageIndex, Messages.GetLowerBound(0), Messages.GetUpperBound(0));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && _hasSword)
        {
            player = collision.gameObject;
            _hasSword = false;
            playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            playerMovement.talkWithStranger(true);
            activateDialogue();
        }
    }

    [PunRPC]
    private void changeAnimation(string pName)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            if (player == null && p.name == pName)
                player = p;
        }
        print("In changeAnimation, players name: "+player.name);

        animator.SetBool("hasSword", false);
        player.GetComponent<PlayerMovement>().talkWithStranger(false);
    }

    public bool getHasSword()
    {
        return _hasSword;
    }

    private void activateDialogue()
    {
        panel.SetActive(true);
        image.SetActive(true);
        dialogueText.text = Messages[0];
        ShowMessages = true;
    }
}
