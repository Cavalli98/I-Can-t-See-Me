using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

public class GameOverScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public void Setup()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameOver");
        }
        //gameObject.SetActive(true);
    }


    //public void Restart()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        PhotonNetwork.LoadLevel("GameOver");
    //    }
    //}
}
