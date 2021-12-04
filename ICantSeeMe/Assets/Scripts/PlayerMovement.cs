using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public class PlayerMovement : MonoBehaviourPun
{
    float timePassed = 0f;

    private Rigidbody2D _rb;
    private float _horizontalMovement;
    private float _verticalMovement;
    private BoxCollider2D _boxCollider;
    private SpriteRenderer _spr;
    private Transform _tr;
    private GameManager _gameManager;

    private float _playerSkin = 0.05f;
    private float _scaleFactor;

    public float velocity = 10f;
    public float jumpForce = 2;
    public LayerMask mask;

    //Size of box collider
    private float _playerWidth;
    private float _playerHeight;

    //Offset of overlap boxes
    private float _groundBoxOffset;
    private float _horizontalBoxOffset;

    //Positions of overlap boxes
    private Vector2 _origin;
    private Vector2 _originLeft;
    private Vector2 _originRight;
    private Vector2 _originDown;

    //Size of overlap boxes
    private Vector2 _groundBoxSize;
    private Vector2 _horizontalBoxSize;

    //Control bools
    private bool _jump = false;
    private bool _grounded = false;
    private bool _collidedLeft = false;
    private bool _collidedRight = false;
    private bool _collidedDown = false;
    private bool _canClimb = false;
    private bool _facingRight = true;
    private bool _gameOver = false;

    // for climbing
    private bool _isCloseToLadder = false, 
                _climbHeld = false, 
                _hasStartedClimb = false;
    private Transform _ladder;
    public float climbSpeed = 2.0f;

    public const byte gameOverEvent = 1;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();

        if (!photonView.IsMine)
        {
            Destroy(_rb);
            Destroy(_boxCollider);
            return;
        }
        
        _spr = GetComponent<SpriteRenderer>();
        _tr = GetComponent<Transform>();
        _scaleFactor = _tr.localScale.x;
        _gameOver = false;

        _playerWidth = _boxCollider.size.x * _scaleFactor;
        _playerHeight = _boxCollider.size.y * _scaleFactor;
        _horizontalBoxOffset = _playerWidth * 0.5f + _playerSkin;
        _groundBoxOffset = _playerHeight * 0.5f + _playerSkin;
        
        _groundBoxSize = new Vector2(_playerWidth - _playerSkin, _playerSkin);
        _horizontalBoxSize = new Vector2(_playerSkin, _playerHeight - _playerSkin);

        //Initialize the player facing right
        _facingRight = true;
        _spr.flipX = true;
    }

    private void Start()
    {
        if (!photonView.IsMine)
            return;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        if (_gameOver) {
            return;
        }
        
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");
        
        //Player moves right
        if (_horizontalMovement > 0 && !_collidedRight)
        {
            _tr.position = _tr.position
                                + _horizontalMovement * _tr.right * velocity * Time.deltaTime;
            if (!_facingRight)
            {
                Flip();
            }
        }

        //Player moves left
        if (_horizontalMovement < 0 && !_collidedLeft)
        {
            _tr.position = _tr.position
                                    + _horizontalMovement * _tr.right * velocity * Time.deltaTime;
            if (_facingRight)
            {
                Flip();
            }
        }

        // Player jumps
        if (Input.GetButtonDown("Jump") && _grounded && !_canClimb)
        {
            _jump = true;
        }

        // Player climbs
        _climbHeld = (_isCloseToLadder && Input.GetButton("Vertical")) ? true : false;
        if (_climbHeld)
        {
            if (!_hasStartedClimb) _hasStartedClimb = true;
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        if (_gameOver) {
            return;
        }

        _originLeft = _rb.position + Vector2.left * _horizontalBoxOffset;
        _collidedLeft = Physics2D.OverlapBox(_originLeft, _horizontalBoxSize, 0f, mask);

        _originRight = _rb.position + Vector2.right * _horizontalBoxOffset;
        _collidedRight = Physics2D.OverlapBox(_originRight, _horizontalBoxSize, 0f, mask);
        
        _originDown = _rb.position + Vector2.down * _groundBoxOffset;
        _collidedDown = Physics2D.OverlapBox(_originDown, _groundBoxSize, 0f, mask);


        if (_jump)
        {
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _jump = false;
            _grounded = false;
        }
        else
        {
            _origin = _rb.position + Vector2.down * _groundBoxOffset;
            _grounded = Physics2D.OverlapBox(_origin, _groundBoxSize, 0f, mask);
        }

        // Climbing
        if(_hasStartedClimb && !_climbHeld)
        {
            if(_horizontalMovement > 0 || _horizontalMovement < 0) ResetClimbing();
        }
        else if(_hasStartedClimb && _climbHeld)
        {
            float halfHeight     = _ladder.GetComponent<BoxCollider2D>().size.y * 0.5f * _ladder.transform.localScale.y;
            float topHandlerY    = _ladder.transform.position.y + halfHeight;
            float bottomHandlerY = _ladder.transform.position.y - halfHeight;

            if (_originDown.y > topHandlerY || _originDown.y < bottomHandlerY)
            {
                ResetClimbing();
            }
            else if (_originDown.y <= topHandlerY && _originDown.y >= bottomHandlerY)
            {
                Climb();
            }
        }

    }


    private void Flip()
    {
        //Toggle bool
        _facingRight = !_facingRight;

        //flipX is true when facing right
        _spr.flipX = _facingRight;
    }


    private void Climb()
    {
        Debug.Log("climb");
        _rb.bodyType = RigidbodyType2D.Kinematic;
        if (!transform.position.x.Equals(_ladder.transform.position.x))
            transform.position = new Vector3(_ladder.transform.position.x,transform.position.y,transform.position.z);

        Vector3 newPos = Vector3.zero;
        if (_verticalMovement > 0)
            newPos = _tr.position + _verticalMovement * _tr.up * climbSpeed * Time.deltaTime;
        else if(_verticalMovement < 0)
            newPos = _tr.position + _verticalMovement * _tr.up * climbSpeed * Time.deltaTime;
        if (newPos != Vector3.zero) _rb.MovePosition(newPos);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Ladder"))
        {
            _isCloseToLadder = true;
            this._ladder = collision.transform;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Ladder"))
        {
            _isCloseToLadder = false;
            this._ladder = null;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        print("Collision "+collision.gameObject.tag);
        if (collision.gameObject.tag == "Dangerous")
        {
            // Send event to all players
            object[] content = null;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
            PhotonNetwork.RaiseEvent(gameOverEvent, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        print("arrivato un evento player game over");
        if (!photonView.IsMine)
            return;

        byte eventCode = photonEvent.Code;

        if (eventCode == gameOverEvent)
        {
            GameOver();
        }
    }

    private void ResetClimbing()
    {
        if(_hasStartedClimb)
        {
            _hasStartedClimb = false;
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    
    // to print stuff every s seconds
    private void PrintStuff(float s)
    {
        timePassed += Time.deltaTime;
        if(timePassed > s)
        {
            print("");
            timePassed=0f;
        } 
    }

    public void GameOver()
    {
        print("Player - GameOver");
        _gameOver = true;
        _gameManager.GameOver();
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}