using UnityEngine;
using Photon.Pun;

public class BrickRoad : Triggerable
{
    public bool isUp;
    public float startY;
    public float endY;
    public float speed;

    private bool hasToMoveUp;
    private bool _isMoving;
    private float step;
    private Vector3 endPosition;
    private Vector3 startPosition;

    private void Awake()
    {
        _isMoving = false;
        hasToMoveUp = isUp;
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
            if (hasToMoveUp)
            {
                // Move our position a step closer to the target.
                transform.position = Vector3.MoveTowards(transform.position, endPosition, step);

                if (Vector3.Distance(transform.position, endPosition) < 0.01f)
                {
                    _isMoving = false;
                }
            }
            else
            {
                // Move our position a step closer to the target.
                transform.position = Vector3.MoveTowards(transform.position, startPosition, step);

                if (Vector3.Distance(transform.position, startPosition) < 0.01f)
                {
                    _isMoving = false;
                }
            }
        }
    }

    public override void activate()
    {
        hasToMoveUp = !hasToMoveUp;
        _isMoving = true;
    }
}
