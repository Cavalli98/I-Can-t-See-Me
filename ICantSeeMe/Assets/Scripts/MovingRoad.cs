using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class MovingRoad : Triggerable
{
    //public float startX;
    //public float endX;
    public float speed;
    public CheckCollisionRoad leftObj;
    public CheckCollisionRoad rightObj;
    public Transform startObj;
    public Transform endObj;

    //private bool _hasToMove;
    private float _t;
    private Vector3 endPosition;
    private Vector3 startPosition;

    public bool rotating;
    public float angle;
    public bool RightPivot;
    private bool _hasRotated;
    private bool _hasToRotate;
    private Vector3 pivot;
    private float _rotation;
    private float _increment;

    private void Awake()
    {
        //_hasToMove = false;
        //startX = transform.position.x;
        //startPosition = new Vector3(startX, transform.position.y, transform.position.z);
        //endPosition = new Vector3(endX, transform.position.y, transform.position.z);
        startPosition = startObj.position;
        endPosition = endObj.position;
        _hasRotated = false;
        _hasToRotate = false;
        _increment = 0.1f;
        if (angle<0)
        {
            _increment = -_increment;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
   
        if(RightPivot)
        {
            pivot = rightObj.transform.position;
        }
        else
        {
            pivot = leftObj.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (_hasToMove)
        //{
        //    Move();
        //}

        if ((leftObj.collided && startPosition.x < endPosition.x) || (rightObj.collided && startPosition.x > endPosition.x))
        {
            Move();
        }
        if (rotating && _hasToRotate)
        {
            Rotate();
        }
    }

    [PunRPC]
    private void Rotate()
    {
        if (angle >= 0)
        {
            _rotation += _increment;
            if (_rotation < angle)
            {
                transform.RotateAround(pivot, Vector3.forward, _increment);
            }
            else
            {
                _hasToRotate = false;
                _rotation = 0;
                angle = -angle;
                _increment = -_increment;
            }
        }
        else
        {
            _rotation += _increment;
            if (_rotation > angle)
            {
                transform.RotateAround(pivot, Vector3.forward, _increment);
            }
            else
            {
                _hasToRotate = false;
                _rotation = 0;
                angle = -angle;
                _increment = -_increment;
            }
        }
    }
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        photonView.TransferOwnership(PhotonView.Get(collision.gameObject).Owner);
    //        if (collision.otherCollider == left)
    //        {
    //            Debug.Log("left collider");
    //            if (startPosition.x < endPosition.x)
    //                _hasToMove = true;
    //        }
    //        else if (collision.otherCollider == right)
    //        {
    //            Debug.Log("right collider");
    //            if (startPosition.x > endPosition.x)
    //                _hasToMove = true;
    //        }
    //        else
    //        {
    //            Debug.Log("other collider");
    //        }
    //    }
    //}

    public override void activate()
    {
        _hasToRotate = true;
    }

    [PunRPC]
    private void Move()
    {
        _t += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(startPosition, endPosition, _t);
        

        //Debug.Log("x: " + endPosition.x + " y: " + endPosition.y + " t: " + _t + " Lerp: " + Vector3.Lerp(startPosition, endPosition, _t));

        // Flip the points once it has reached the target
        if (_t >= 1)
        {
            //_hasToMove = false;
            var end = endPosition;
            var start = startPosition;
            startPosition = end;
            endPosition = start;
            _t = 0;
            leftObj.collided = false;
            rightObj.collided = false;
            //Debug.Log("Uscito check");
        }
    }
}
