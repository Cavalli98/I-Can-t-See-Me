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
    public int _direction;
    private bool _attacking = false;
    private Coroutine _coroutine;
    public const byte gameOverEvent = 1;
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;

    public string attackSound;
    public string sleepSound;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _range = GetComponentInChildren<Range>();
        _prevX = transform.position.x;
        path = GameObject.FindGameObjectsWithTag("EnemyPath");
        path_maxCount = path.Length - 1;
        if (path_maxCount != 0)
            jogTarget = path[0].transform;
        animator.SetBool("activated", activated);
        if (!activated)
        {
            _rb.bodyType = RigidbodyType2D.Static;
            _bc.isTrigger = true;
            AudioManager.instance.RpcPlaySound(sleepSound);
            AudioManager.instance.RpcLoopSound(sleepSound);
        }
        else
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _bc.isTrigger = false;
        }
    }

    void Update()
    {
        animator.SetFloat("speed", Mathf.Abs(speed));
        animator.SetInteger("direction", _direction);
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

    public override void activate()
    {
        if (oneTime && activated)
            return;
        activated = !activated;
        animator.SetBool("activated", activated);
        if (!activated)
        {
            _rb.bodyType = RigidbodyType2D.Static;
            _direction = 0;
            AudioManager.instance.RpcPlaySound(sleepSound);
            AudioManager.instance.RpcLoopSound(sleepSound);
        }
        else
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _bc.isTrigger = false;
            AudioManager.instance.RpcStopLoopSound(sleepSound);
            AudioManager.instance.RpcStopSound(sleepSound);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!activated && collision.collider.tag == "Player" && Target != null)
        {
            _attacking = true;
            animator.SetBool("attacking", _attacking);
            AudioManager.instance.RpcPlaySound(attackSound);
            _coroutine = StartCoroutine(waiter(0.4f));
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!activated && collision.collider.tag == "Player" && Target != null && !_attacking)
        {
            _attacking = true;
            animator.SetBool("attacking", _attacking);
            _coroutine = StartCoroutine(waiter(0.4f));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!activated && collision.collider.tag == "Player")
        {
            _attacking = false;
            animator.SetBool("attacking", _attacking);
            StopCoroutine(_coroutine);
        }
    }

    IEnumerator waiter(float s)
    {
        yield return new WaitForSeconds(s);
        if (_attacking)
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
