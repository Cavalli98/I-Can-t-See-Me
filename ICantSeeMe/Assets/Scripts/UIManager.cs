using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;

    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;

    [SerializeField]
    private GameObject gameOptions;

    [SerializeField]
    private GameObject createPanel;

    [SerializeField]
    private GameObject joinPanel;

    // Start is called before the first frame update
    void Start()
    {
        // Deactivate all panels but control panel
        progressLabel.SetActive(false);
        gameOptions.SetActive(false);
        createPanel.SetActive(false);
        joinPanel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public void PlayButton()
    {
        controlPanel.SetActive(false);
        progressLabel.SetActive(true);
    }

    public void OnConnected()
    {
        progressLabel.SetActive(false);
        gameOptions.SetActive(true);
    }

    public void CreatePanelButton()
    {
        gameOptions.SetActive(false);
        createPanel.SetActive(true);
    }

    public void JoinPanelButton()
    {
        gameOptions.SetActive(false);
        joinPanel.SetActive(true);
    }

    public void CreateRoom()
    {
        createPanel.SetActive(false);
        progressLabel.SetActive(true);
    }

    public void JoinRoom()
    {
        joinPanel.SetActive(false);
        progressLabel.SetActive(true);
    }
}
