using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LockButton : MonoBehaviourPun
{
    public Text textUI;
    public UnityEngine.UI.Button buttonUI;
    public int number;
    public Lock lockObj;

    private void Awake()
    {
        buttonUI = GetComponent<UnityEngine.UI.Button>();
        number = 0;
    }

    public void OnClick()
    {
        this.photonView.RPC("UpdateNumber", RpcTarget.All, null);
    }

    [PunRPC]
    void UpdateNumber()
    {
        number = (number + 1) % 10;
        textUI.text = number.ToString();
        lockObj.UpdateCode();
    }
}
