using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Sprite activated;
    public Sprite deactivated;
    private SpriteRenderer state;
    private bool isColliding = false;
    private bool isActive = false;

    private void Awake()
    {
        state = GetComponent<SpriteRenderer>();
        isActive = false;
        isColliding = false;
        state.sprite = deactivated;
    }
    // Start is called before the first frame update
    void Start()
    {
    
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding )
        {
            if (Input.GetKeyDown(KeyCode.E) && !isActive)
            {
                GetComponent<SpriteRenderer>().sprite = activated;
                isActive = true;
            }
            else if (Input.GetKeyDown(KeyCode.E) && isActive)
            {
                GetComponent<SpriteRenderer>().sprite = deactivated;
                isActive = false;
            }
        }
    }

    private void OnTriggerEnter2D (Collider2D Player)
    {        
        if (Player.tag != "Player")
            return;
        isColliding = true;
    }
    private void OnTriggerExit2D(Collider2D Player)
    {
        if (Player.tag != "Player")
            return;
        isColliding = false;
    }
}
