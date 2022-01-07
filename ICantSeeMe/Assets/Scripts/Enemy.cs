using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class Enemy : Triggerable
{
    public Transform Target;
    private Transform jogTarget;
    public float speed_running = 4.0f;
    public float speed_jog = 2.0f;
    private float speed = 2.0f;
    public bool activated = false;
    public Animator animator;
    public bool oneTime = false;

    private GameObject[] path = null;
    private int path_counter = 0;
    private int path_maxCount;
    //private bool reached_jogTarget = false;
    private Range _range;
    private float _prevX;
    private int _direction;
    private bool _attacking = false;
    private Coroutine _coroutine;
    public const byte gameOverEvent = 1;
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;
    public bool startActive;

    void Start()
    {
        //activated = false;
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _range = GetComponentInChildren<Range>();
        _prevX = transform.position.x;
        path = GameObject.FindGameObjectsWithTag("EnemyPath");
        path_maxCount = path.Length - 1;
        if (path_maxCount != 0)
            jogTarget = path[0].transform;
        animator.SetBool("activated", activated);

    }

    void Update()
    {
        animator.SetFloat("speed", Mathf.Abs(speed));
        animator.SetInteger("direction", _direction);
        //if (startActive)
        //{
        //    activate();
        //    startActive = false;
        //}
    }

    void FixedUpdate()
    {
        if (!activated)
            return;

        if (_attacking)
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
        _direction = (_prevX < transform.position.x) ? 1 : -1;
        _range.setColliderOffset(_direction);
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
        if (jogTarget.position.x == transform.position.x)
            updateJogTarget();
    }

    private void updateJogTarget()
    {
        path_counter = (path_counter == path_maxCount) ? 0 : path_counter + 1;
        jogTarget = path[path_counter].transform;
    }

    public override void activate()
    {
        if (oneTime && activated)
            return;
        activated = !activated;
        animator.SetBool("activated", activated);
        if (!activated)
        {
            _rb.bodyType = RigidbodyType2D.Static;
            _bc.isTrigger = true;
            _direction = 0;
        }
        else
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _bc.isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && Target != null)
        {
            _attacking = true;
            animator.SetBool("attacking", _attacking);
            _coroutine = StartCoroutine(waiter(0.4f));
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && Target != null && !_attacking)
        {
            _attacking = true;
            animator.SetBool("attacking", _attacking);
            _coroutine = StartCoroutine(waiter(0.4f));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            _attacking = false;
            animator.SetBool("attacking", _attacking);
            StopCoroutine(_coroutine);
        }
    }

    IEnumerator waiter(float s)
    {
        yield return new WaitForSeconds(s);
        if (_attacking = true)
            gameOver();
    }

    private void gameOver()
    {
        // Send event to all players
        object[] content = null;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(gameOverEvent, content, raiseEventOptions, SendOptions.SendReliable);
    }
}
