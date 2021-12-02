using Photon.Pun;
using UnityEngine;


public class PlayerMovement : MonoBehaviourPun
{
    private Rigidbody2D _rb;
    private float _horizontalMovement;
    private BoxCollider2D _boxCollider;
    private SpriteRenderer _spr;
    private Transform _tr;

    private float _playerSkin = 0.05f;
    private float _scaleFactor;

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

    //Size of overlap boxes
    private Vector2 _groundBoxSize;
    private Vector2 _horizontalBoxSize;

    //Control bools
    private bool _jump = false;
    private bool _grounded = false;
    private bool _collidedLeft = false;
    private bool _collidedRight = false;

    private bool _facingRight = true;

    public float velocity = 10f;
    public float jumpForce = 5;
    public LayerMask mask;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _spr = GetComponent<SpriteRenderer>();
        _tr = GetComponent<Transform>();
        _scaleFactor = _tr.localScale.x;

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

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        
        _horizontalMovement = Input.GetAxisRaw("Horizontal");

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


        if (Input.GetButtonDown("Jump") && _grounded)
        {
            _jump = true;
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

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

        _originLeft = _rb.position + Vector2.left * _horizontalBoxOffset;
        _collidedLeft = Physics2D.OverlapBox(_originLeft, _horizontalBoxSize, 0f, mask);

        _originRight = _rb.position + Vector2.right * _horizontalBoxOffset;
        _collidedRight = Physics2D.OverlapBox(_originRight, _horizontalBoxSize, 0f, mask);
    }

    private void Flip()
    {
        //Toggle bool
        _facingRight = !_facingRight;

        //flipX is true when facing right
        _spr.flipX = _facingRight;
    }
}