using UnityEngine;
using Photon.Pun;

public class BrickRoad : Triggerable
{
    public float speed;

    private bool _hasToMove;
    private float _t;
    public Transform startObj;
    public Transform endObj;
    private Vector3 endPosition;
    private Vector3 startPosition;


    private void Awake()
    {
        _hasToMove = false;
        startPosition = startObj.position;
        endPosition = endObj.position;
    }

    // Update is called once per frame
    private void Update()
    {
      
        if (_hasToMove)
        {
            // Moves the object to target position
            _t += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startPosition, endPosition, _t);

            // Flip the points once it has reached the target
            if (_t >= 1)
            {
                var end = endPosition;
                var start = startPosition;
                startPosition = end;
                endPosition = start;
                _t = 0;
                _hasToMove = false;
            }
        }
    }

    public override void activate()
    {
        _hasToMove = true;
    }
}
