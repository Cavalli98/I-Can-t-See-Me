using UnityEngine;
using Photon;
using Photon.Pun;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    public bool music;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;


    public AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
    }

    public void Play()
    {
        source.volume = volume; /** (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));*/
        //source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.loop = false;
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void Loop()
    {
        //source.loop = true;
    }

    public void StopLoop()
    {
        //source.loop = false;
    }
}

public class AudioManager : MonoBehaviourPun
{
    public static AudioManager instance;

    [SerializeField]
    Sound[] sounds;
    public AudioSource musicSource;
    public AudioSource soundsSource;
    public AudioMixerGroup soundsMixer;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one AudioManager in the scene.");
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
            sounds[i].source.outputAudioMixerGroup = soundsMixer;
        }
    }

    [PunRPC]
    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                if (sounds[i].music)
                {
                    musicSource.clip = sounds[i].clip;
                    musicSource.Play();
                    return;
                }
                else
                {
                    //soundsSource.volume = sounds[i].volume * (1 + Random.Range(-sounds[i].randomVolume / 2f, sounds[i].randomVolume / 2f));
                    //soundsSource.pitch = sounds[i].pitch * (1 + Random.Range(-sounds[i].randomPitch / 2f, sounds[i].randomPitch / 2f));
                    sounds[i].Play();
                    return;
                }
            }
        }

        // no sound with _name
        Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    }

    public void RpcPlaySound(string _name)
    {
        photonView.RPC("PlaySound", RpcTarget.All, _name);
    }

    #region StopMethods
    [PunRPC]
    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }
        // no sound with _name
        Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    }

    public void RpcStopSound(string _name)
    {
        photonView.RPC("StopSound", RpcTarget.All, _name);
    }

    #endregion

    #region LoopMethods
    [PunRPC]
    public void LoopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Loop();
                return;
            }
        }
        // no sound with _name
        Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    }

    public void RpcLoopSound(string _name)
    {
        photonView.RPC("LoopSound", RpcTarget.All, _name);
    }

    #endregion

    #region StopLoopMethods
    [PunRPC]
    public void StopLoopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].StopLoop();
                return;
            }
        }
        // no sound with _name
        Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    }
    
    public void RpcStopLoopSound(string _name)
    {
        photonView.RPC("StopLoopSound", RpcTarget.All, _name);
    }
    #endregion
}
