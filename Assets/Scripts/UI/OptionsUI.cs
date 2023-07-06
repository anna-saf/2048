using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance;

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Button closeButton;

    public ReactiveProperty<float> musicVolume { get; private set; }

    private void Awake()
    {
        Instance = this;

        musicVolumeSlider.onValueChanged.AddListener((float volume) =>
        {
            musicVolume.Value = volume;            
        });

        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void Start()
    {
        Hide();
        musicVolume = GlobalModel.Instance.musicVolume;
        musicVolumeSlider.value = musicVolume.Value;
    }

    private Slider.SliderEvent UpdateVolume()
    {
        throw new NotImplementedException();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
