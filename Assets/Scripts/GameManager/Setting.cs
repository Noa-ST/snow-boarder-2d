using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [Header("Sliders")]
    public Slider musicSlider;           // Slider điều chỉnh âm lượng nhạc
    public Slider soundSlider;           // Slider điều chỉnh âm lượng hiệu ứng

    private void Start()
    {
        // Gán giá trị ban đầu từ PlayerPrefs
        LoadSettings();

        // Đăng ký sự kiện cho slider
        if (musicSlider != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.3f); // Giá trị mặc định 0.3
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        }
        if (soundSlider != null)
        {
            soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f); // Giá trị mặc định 1.0
            soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
        }

        ApplySettings();
    }

    private void LoadSettings()
    {
        if (musicSlider != null) musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
        if (soundSlider != null) soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f);
    }

    private void OnMusicSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        ApplySettings();
        Debug.Log("Music volume changed to: " + value);
    }

    private void OnSoundSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
        PlayerPrefs.Save();
        ApplySettings();
        Debug.Log("Sound volume changed to: " + value);
    }


    private void ApplySettings()
    {
        if (AudioController.Ins != null)
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
            float soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);
            bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
            bool soundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;

            AudioController.Ins.SetMusicVolume(musicVolume);
            AudioController.Ins.musicAus.mute = !musicEnabled;
            AudioController.Ins.sfxAus.volume = soundVolume;
            AudioController.Ins.sfxAus.mute = !soundEnabled;

            Debug.Log("Applied settings - Music: " + musicVolume + " (Enabled: " + musicEnabled + "), Sound: " + soundVolume + " (Enabled: " + soundEnabled + ")");
        }
    }
}

