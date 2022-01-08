using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class MultiButton : Trigger
{
    public MultiButton other;
    public int seconds;
    public TextMeshProUGUI text;
    private bool _isTriggered { get; set; }
    public string Sound;
    private void Awake()
    {
        _isTriggered = false;
    }

    private void Update()
    {
        // Non fare controllo in update
        // Usare eventi sarebbe meglio
        if (_isTriggered && other._isTriggered)
        {
            foreach (GameObject t in toActivate)
                t.GetComponent<Triggerable>().activate();
            _isTriggered = false;
            other._isTriggered = false;
        }
    }

    [PunRPC]
    public override void trigger()
    {
        if (!_isTriggered)
        {
            AudioManager.instance.RpcPlaySound(Sound);
            AudioManager.instance.RpcLoopSound(Sound);
            _isTriggered = true;
            //print("Cliccato");
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        for (int i = seconds; i > 0; i--)
        {
            text.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        text.text = "";
        _isTriggered = false;
        AudioManager.instance.RpcStopLoopSound(Sound);
        AudioManager.instance.RpcStopSound(Sound);
        //print("Fine timer");
    }
}
