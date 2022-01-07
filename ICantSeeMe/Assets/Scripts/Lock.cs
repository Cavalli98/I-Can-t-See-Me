using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Lock : Trigger
{
    public GameObject LockPanel;
    public int code;
    List<LockButton> buttons;
    private bool isActive;
    private bool locked;

    private void Awake()
    {
        isActive = false;
    }

    private void Start()
    {
        Transform lockZoom = LockPanel.transform.GetChild(0);
        // Number of buttons
        int count = lockZoom.childCount;

        buttons = new List<LockButton>();
        for (int i = 0; i < count; i++)
        {
            buttons.Add(lockZoom.GetChild(i).GetComponent<LockButton>());
        }

        //Set panel for player 1
        if (PhotonNetwork.IsMasterClient)
        {
            // Deactivate all text components
            foreach (LockButton button in buttons)
            {
                button.textUI.gameObject.SetActive(false);
            }
        }
        else
        {
            //Deactivate all buttons
            foreach (LockButton button in buttons)
            {
                button.buttonUI.interactable = false;
            }
        }
    }

    public void UpdateCode()
    {
        int number = 0;
        foreach (LockButton button in buttons)
        {
            number *= 10;
            number += button.number;
        }
        
        if (number == code)
        {
            LockPanel.SetActive(false);
            foreach (GameObject t in toActivate)
                t.GetComponent<Triggerable>().activate();
            Destroy(gameObject);
        }
    }

    [PunRPC]
    public override void trigger()
    {
        isActive = !isActive;
        LockPanel.SetActive(isActive);
    }
}
