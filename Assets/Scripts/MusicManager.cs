
using UniRx;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        audioSource = GetComponent<AudioSource>();

    }

    private void Start()
    {        
        GameObject.DontDestroyOnLoad(gameObject);
        ChangeVolume(GlobalModel.Instance.musicVolume.Value);
        GlobalModel.Instance.musicVolume.Subscribe(_ => ChangeVolume(_));
    }

    private void ChangeVolume(float volume)
    {
        audioSource.volume = volume/100;
    }

    /*public float GetVolume()
    {
        return volume;
    }*/
}
