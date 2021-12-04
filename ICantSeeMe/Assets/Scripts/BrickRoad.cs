using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BrickRoad : Triggerable
{
    public bool isUp;
    public float startY;
    public float endY;
    public float speed;
    private float _diff;
    private bool _isMoving;
    private float step;
    private Vector3 endPosition;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector3(transform.position.x, startY);
        endPosition = new Vector3(transform.position.x, endY);
        step = speed * Time.deltaTime; // calculate distance to move
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMoving)
        {
            // Se isUp prima era true
            if (!isUp)
            {
                // Move our position a step closer to the target.
                transform.position = Vector3.MoveTowards(transform.position, startPosition, step);

                if (Vector3.Distance(transform.position, startPosition) < 0.01f)
                {
                    _isMoving = false;
                }
            }
            else if (isUp)
            {
                // Move our position a step closer to the target.
                transform.position = Vector3.MoveTowards(transform.position, endPosition, step);

                if (Vector3.Distance(transform.position, endPosition) < 0.01f)
                {
                    _isMoving = false;
                }
            }
        }
    }

    [PunRPC]
    public override void activate()
    {
        if (isUp)
        {
            isUp = false;
        }
        else if (!isUp)
        {
            isUp = true;
        }
        _isMoving = true;
    }
}
