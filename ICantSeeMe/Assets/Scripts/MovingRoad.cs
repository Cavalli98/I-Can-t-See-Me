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

    public float angle;
    public bool RightPivot;
    private bool _hasRotated;
    private bool _hasToRotate;
    private Vector3 pivot;
    private float _rotation;
    private float _increment;
    private Rigidbody2D _rb;

    private bool _ripartito;
    private Vector3 _tempEndPosition;
    private Vector3 _tempStartPosition;

    private void Awake()
    {
        //_hasToMove = false;
        //startX = transform.position.x;
        //startPosition = new Vector3(startX, transform.position.y, transform.position.z);
        //endPosition = new Vector3(endX, transform.position.y, transform.position.z);
        _rb = GetComponent<Rigidbody2D>();
        startPosition = startObj.position;
        endPosition = endObj.position;
        _hasRotated = false;
        _hasToRotate = false;
        _increment = 1f;
        _ripartito = false;
        if (angle < 0)
        {
            _increment = -_increment;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        if (RightPivot)
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
        //Debug.Log("" + startPosition + " " + endPosition + " " + transform.position); ;
        //Debug.Log("" + startPosition + " " + endPosition + " " + transform.position);
        if (_hasToRotate)
        {
            Rotate();
            leftObj.collided = false;
            rightObj.collided = false;
        }
        if ((leftObj.collided && startPosition.x < endPosition.x) || (rightObj.collided && startPosition.x > endPosition.x))
        {
            Move();
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


        //Debug.Log("Moving");

        if (((leftObj.ritorna && startPosition.x > endPosition.x) || (rightObj.ritorna && startPosition.x < endPosition.x)) && !_ripartito)
        {
            //Debug.Log("Dovrebbe Ripartire");
            _ripartito = true;
            _tempEndPosition = endPosition;
            _tempStartPosition = startPosition;

            endPosition = startPosition;
            startPosition = transform.position;

            leftObj.collided = !leftObj.collided;
            rightObj.collided = !rightObj.collided;
            _t = 0;
            //Debug.Log("" + (leftObj.collided) + (startPosition.x < endPosition.x) + (rightObj.collided) + (startPosition.x > endPosition.x));
        }
            

        // Flip the points once it has reached the target
        if (_t >= 1)
        {
            if (!leftObj.ritorna && !rightObj.ritorna)
            {
                //Debug.Log("Dobrebbe essere arrivato");
                var end = endPosition;
                var start = startPosition;
                startPosition = end;
                endPosition = start;

                //Debug.Log("Uscito check");
            }
            else
            {
                //Debug.Log("Dovrebbe essere ritornato");
                startPosition = _tempStartPosition;
                endPosition = _tempEndPosition;
                _ripartito = false;
                leftObj.ritorna = false;
                rightObj.ritorna = false;
            }
            _t = 0;
            leftObj.collided = false;
            rightObj.collided = false;
        }
    }
}
