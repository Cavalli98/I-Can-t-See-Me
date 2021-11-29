using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerTest : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _horizontalMovement;
    public float velocity = 10f;
    private bool _jump = false;
    public float jumpForce = 5;
    private BoxCollider2D _boxCollider;
    private float _playerWidth;
    private float _playerHeight;
    private float _groundBoxHeight;
    private float _horizontalBoxWidth;
    private Vector2 _groundBoxCheck;
    private Vector2 _horizontalBoxCheck;
    private float _playerSkin = 0.05f;
    private bool _grounded = false;
    private bool _collidedLeft = false;
    private bool _collidedRight = false;

    public LayerMask mask;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();

        _playerWidth = _boxCollider.size.x;
        _playerHeight = _boxCollider.size.y;
        _horizontalBoxWidth = _playerWidth * 0.5f + _playerSkin;
        _groundBoxHeight = _playerHeight * 0.5f + _playerSkin;
        
        _groundBoxCheck = new Vector2(_playerWidth - _playerSkin, _playerSkin);
        _horizontalBoxCheck = new Vector2(_playerSkin, _playerHeight - _playerSkin);
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        Debug.Log(_collidedRight);
        Debug.Log(_collidedLeft);

        if (_horizontalMovement > 0 && !_collidedRight)
        {
            transform.position = transform.position
                                + _horizontalMovement * transform.right * velocity * Time.deltaTime;
        }

        if (_horizontalMovement < 0 && !_collidedLeft)
        {
            transform.position = transform.position
                                    + _horizontalMovement * transform.right * velocity * Time.deltaTime;
        }


        if (Input.GetButtonDown("Jump") && _grounded)
        {
            _jump = true;
        }
    }

    private void FixedUpdate()
    {
        if (_jump)
        {
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _jump = false;
            _grounded = false;
        }
        else
        {
            Vector2 origin = _rb.position + Vector2.down * _groundBoxHeight;
            _grounded = Physics2D.OverlapBox(origin, _groundBoxCheck, 0f, mask);
        }

        Vector2 originLeft = _rb.position + Vector2.left * _horizontalBoxWidth;
        _collidedLeft = Physics2D.OverlapBox(originLeft, _horizontalBoxCheck, 0f, mask);

        Vector2 originRight = _rb.position + Vector2.right * _horizontalBoxWidth;
        _collidedRight = Physics2D.OverlapBox(originRight, _horizontalBoxCheck, 0f, mask);
    }
}