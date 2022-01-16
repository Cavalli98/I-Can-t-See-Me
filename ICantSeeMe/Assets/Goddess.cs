using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class Goddess : MonoBehaviourPun
{
    public Transform Target;
    private Transform jogTarget;
    public float speed_running = 4.0f;
    public float speed_jog = 2.0f;
    private float speed = 2.0f;
    private bool _died = false;
    private bool _done = false;
    private bool _talking = false;
    private bool _ended = false;

    private GameObject[] path = null;
    private int path_counter = 0;
    private int path_maxCount;
    private GoddessRange _range;
    private float _prevX;
    private int _direction;
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;
    
    public string[] Messages;
    private string[] MessagesBadEnd;
    private string[] MessagesGoodEnd;
    [HideInInspector]
    public int CurrentMessageIndex = 0;  // Default to first
    public bool ShowMessages = false;
    public bool ShowMessagesBadEnd = false;
    public bool ShowMessagesGoodEnd = false;
    public Text dialogueText;
    public GameObject panel;
    public GameObject image;
    private GameObject player = null;
    public const byte gameOverEvent = 1;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _range = GetComponentInChildren<GoddessRange>();
        _prevX = transform.position.x;
        path = GameObject.FindGameObjectsWithTag("EnemyPath");
        path_maxCount = path.Length - 1;
        if (path_maxCount != 0)
            jogTarget = path[0].transform;

        // Load messages, if you did not make them in the Editor
        string mess1 = "I have to congratulate, you managed to get here.";
        string mess2 = "But I know I will finish this game! I will end your life and then your beloved's one!";
        Messages = new string[] { mess1, mess2 };
        string messG1 = "Thanks to the sword the knight of the kingdom gave me, I managed to defeat the goddess.";
        string messG2 = "Now I can reunite with my partner!";
        MessagesGoodEnd = new string[] { messG1, messG2 };
        string messB1 = "Seeing that you were so selfish to not helping a man in need, you didn't get that sword, which could kill me.";
        string messB2 = "Now rest with your similar in hell!";
        MessagesBadEnd = new string[] { messB1, messB2 };
        panel.SetActive(false);
        image.SetActive(false);
    }

    void Update()
    {
        if (Target != null && !_done)
                startChatting(0);

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
                CurrentMessageIndex = 0;
                _done = true;
                panel.SetActive(false);
                image.SetActive(false);
                startAttacking();
            }
            else
                dialogueText.text = Messages[CurrentMessageIndex];

            // Bounds checking
            CurrentMessageIndex = Mathf.Clamp(CurrentMessageIndex, Messages.GetLowerBound(0), Messages.GetUpperBound(0));
        }
        if(ShowMessagesGoodEnd)
        {
            if (Input.GetKeyDown(KeyCode.E))// Space Click to move forward
            { 
                CurrentMessageIndex++;
            }

            // Turn off messages if we get to the last one.
            if (CurrentMessageIndex > MessagesGoodEnd.GetUpperBound(0))
            {
                ShowMessagesGoodEnd = false;
                CurrentMessageIndex = 0;
                _done = true;
                panel.SetActive(false);
                image.SetActive(false);
            }
            else
                dialogueText.text = MessagesGoodEnd[CurrentMessageIndex];

            // Bounds checking
            CurrentMessageIndex = Mathf.Clamp(CurrentMessageIndex, MessagesGoodEnd.GetLowerBound(0), MessagesGoodEnd.GetUpperBound(0));
        }
        if(ShowMessagesBadEnd)
        {
            if (Input.GetKeyDown(KeyCode.E))// Space Click to move forward
            { 
                CurrentMessageIndex++;
            }

            // Turn off messages if we get to the last one.
            if (CurrentMessageIndex > MessagesBadEnd.GetUpperBound(0))
            {
                ShowMessagesBadEnd = false;
                CurrentMessageIndex = 0;
                _done = true;
                panel.SetActive(false);
                image.SetActive(false);

                print("PUT WHAT MUST HAPPEN BAD END HERE");
                this.photonView.RPC("ending", RpcTarget.All, null);
            }
            else
                dialogueText.text = MessagesBadEnd[CurrentMessageIndex];

            // Bounds checking
            CurrentMessageIndex = Mathf.Clamp(CurrentMessageIndex, MessagesBadEnd.GetLowerBound(0), MessagesBadEnd.GetUpperBound(0));
        }
    }

    void FixedUpdate()
    {
        if (_died || _talking)
            return;


        setRange();

        if (Target != null)
        {
            followTarget();
        }
        else if (path != null && jogTarget != null)
        {
            jog();
        }
    }

    private void setRange()
    {
        if (Target == null) _direction = (jogTarget.position.x < transform.position.x) ? -1 : 1;
        else _direction = (_prevX < transform.position.x) ? 1 : -1;
        float newSize = (Mathf.Abs(transform.position.x - jogTarget.position.x) + 1.0f) / (_range.getXScale() * transform.localScale.x);
        _range.setOffsetAndSize(newSize, _direction);
        _prevX = transform.position.x;
    }

    private void followTarget()
    {
        speed = speed_running;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(Target.position.x, transform.position.y), speed * Time.deltaTime);
    }

    private void jog()
    {
        speed = speed_jog;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(jogTarget.position.x, transform.position.y), speed * Time.deltaTime);
        if (Mathf.Abs(jogTarget.position.x - transform.position.x) < speed * Time.deltaTime)
            updateJogTarget();
    }

    private void updateJogTarget()
    {
        path_counter = (path_counter == path_maxCount) ? 0 : path_counter + 1;
        jogTarget = path[path_counter].transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && !_died)
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().getHasMagicSword())
                PhotonView.Get(this).RPC("finalAttack", RpcTarget.All, true, collision.gameObject.name);
            else
                PhotonView.Get(this).RPC("finalAttack", RpcTarget.All, false, collision.gameObject.name);
        }
    }

    private void startChatting(int index)
    {
        _talking = true;
        if (player == null && Target != null)
            player = Target.gameObject;

        if (player != null) {
            player.GetComponent<PlayerMovement>().setTalking(true);
            panel.SetActive(true);
            image.SetActive(true);
            
            ShowMessages = (index == 0) ? true : false;
            ShowMessagesBadEnd = (index == 1) ? true : false;
            ShowMessagesGoodEnd = (index == 2) ? true : false;
        }
    }

    private void startAttacking()
    {
        _talking = false;
        player.GetComponent<PlayerMovement>().setTalking(false);
    }

    [PunRPC]
    void finalAttack(bool playerHasSword, string pName)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            if (player == null && p.name == pName)
                player = p;
        }
        _died = playerHasSword;

        if (_died)
        {
            _rb.bodyType = RigidbodyType2D.Static;
            _bc.isTrigger = true;
            startChatting(2);
        }
        else
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _bc.isTrigger = false;
            player.transform.Rotate(Vector3.forward*90);
            startChatting(1);
        }
    }

    [PunRPC]
    void ending()
    {
        if (!_ended && PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable entries = new ExitGames.Client.Photon.Hashtable { { "Level", "BadEnding" } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(entries);
            PhotonNetwork.LoadLevel("BadEnding");
            _ended = true;
        }
    }
}
