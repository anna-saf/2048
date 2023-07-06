using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;

public class GlobalModel : MonoBehaviour
{
    public readonly string pathToConfig = Path.Combine(Directory.GetCurrentDirectory(), "Data\\Config.txt");

    public class Config
    {
        public Dictionary<string, string> playerPrefsKeyValue;
    }

    public Config config { get; private set; }

    public ReactiveProperty<int> record { get; private set; } = new ReactiveProperty<int>();

    public ReactiveProperty<float> musicVolume { get; private set; } = new ReactiveProperty<float>();

    public static GlobalModel Instance { get; private set; }

    private string playerPrefsRecordKey;
    private string playerPrefsMusicVolumeKey;

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        ReadConfig();
        SetDataFromConfig();
        GetValueInPlayerPrefs();
    }

    private void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        musicVolume.Subscribe(_ => SetFloatValueInPlayerPrefs(playerPrefsMusicVolumeKey, _));
        record.Subscribe(_ => SetIntValueInPlayerPrefs(playerPrefsRecordKey, _));
    }

    private void ReadConfig()
    {
        using (StreamReader streamReader = new StreamReader(pathToConfig))
        {
            string str = streamReader.ReadToEnd();
            config = JsonConvert.DeserializeObject<Config>(str);
        }
    }

    private void SetDataFromConfig()
    {
        playerPrefsMusicVolumeKey = config.playerPrefsKeyValue.GetValueOrDefault("PLAYER_PREFS_MUSIC_VOLUME");
        playerPrefsRecordKey = config.playerPrefsKeyValue.GetValueOrDefault("PLAYER_PREFS_RECORD");
    }

    private void GetValueInPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(playerPrefsMusicVolumeKey))
        {
            musicVolume.Value = PlayerPrefs.GetFloat(playerPrefsMusicVolumeKey);
        }
        else
        {
            SetFloatValueInPlayerPrefs(playerPrefsMusicVolumeKey, 100f);
            musicVolume.Value = PlayerPrefs.GetFloat(playerPrefsMusicVolumeKey);
        }
        record.Value = PlayerPrefs.GetInt(playerPrefsRecordKey);
    }

    private void SetFloatValueInPlayerPrefs(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    private void SetIntValueInPlayerPrefs(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
}
