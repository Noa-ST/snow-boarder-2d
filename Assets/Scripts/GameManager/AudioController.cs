using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : Singleton<AudioController>
{
    [Header("Main Settings:")]
    [Range(0, 1)]
    public float musicVolume = 0.3f;
    [Range(0, 1)]
    public float sfxVolume = 1f;

    public AudioSource musicAus;
    public AudioSource sfxAus;

    [Header("Game Sounds and Musics:")]
    public AudioClip[] backgroundMusics; // Nhạc nền ngẫu nhiên
    public AudioClip crashSound;         // Âm thanh va chạm (crash)
    public AudioClip gotCollectable;     // Âm thanh thu thập vật phẩm (sao)
    public AudioClip gameOverSound;      // Âm thanh khi thua (game over)
    public AudioClip gameWinSound;       // Âm thanh khi thắng (game win)
    public AudioClip buttonClickSound;   // Âm thanh khi nhấn nút (UI)
    public AudioClip pauseSound;         // Âm thanh khi tạm dừng

    public override void Awake()
    {
        MakeSingleton(true);
        UpdateAudioState(); // Cập nhật trạng thái âm thanh khi khởi động
    }

    public override void Start()
    {
        PlayBackgroundMusic();
    }

    /// <summary>
    /// Cập nhật trạng thái âm thanh dựa trên Pref
    /// </summary>
    private void UpdateAudioState()
    {
        if (musicAus) musicAus.mute = !Pref.MusicEnabled;
        if (sfxAus) sfxAus.mute = !Pref.SoundEnabled;
    }

    /// <summary>
    /// Play Sound Effect (mảng âm thanh)
    /// </summary>
    /// <param name="clips">Array of sounds</param>
    /// <param name="aus">Audio Source</param>
    public void PlaySound(AudioClip[] clips, AudioSource aus = null)
    {
        if (!aus) aus = sfxAus;
        if (clips != null && clips.Length > 0 && aus && Pref.SoundEnabled)
        {
            var randomIdx = Random.Range(0, clips.Length);
            aus.PlayOneShot(clips[randomIdx], sfxVolume);
        }
    }

    /// <summary>
    /// Play Sound Effect (âm thanh đơn)
    /// </summary>
    /// <param name="clip">Sound</param>
    /// <param name="aus">Audio Source</param>
    public void PlaySound(AudioClip clip, AudioSource aus = null)
    {
        if (!aus) aus = sfxAus;
        if (clip != null && aus && Pref.SoundEnabled)
        {
            aus.PlayOneShot(clip, sfxVolume);
        }
    }

    /// <summary>
    /// Play Music (mảng nhạc)
    /// </summary>
    /// <param name="musics">Array of musics</param>
    /// <param name="loop">Can Loop</param>
    public void PlayMusic(AudioClip[] musics, bool loop = true)
    {
        if (musicAus && musics != null && musics.Length > 0 && Pref.MusicEnabled)
        {
            var randomIdx = Random.Range(0, musics.Length);
            musicAus.clip = musics[randomIdx];
            musicAus.loop = loop;
            musicAus.volume = musicVolume;
            musicAus.Play();
        }
    }

    /// <summary>
    /// Play Music (nhạc đơn)
    /// </summary>
    /// <param name="music">Music</param>
    /// <param name="canLoop">Can Loop</param>
    public void PlayMusic(AudioClip music, bool canLoop)
    {
        if (musicAus && music != null && Pref.MusicEnabled)
        {
            musicAus.clip = music;
            musicAus.loop = canLoop;
            musicAus.volume = musicVolume;
            musicAus.Play();
        }
    }

    /// <summary>
    /// Set volume for music audio source
    /// </summary>
    /// <param name="vol">New Volume</param>
    public void SetMusicVolume(float vol)
    {
        if (musicAus) musicAus.volume = vol;
    }

    /// <summary>
    /// Stop playing music or sound effect
    /// </summary>
    public void StopPlayMusic()
    {
        if (musicAus) musicAus.Stop();
    }

    /// <summary>
    /// Play background music
    /// </summary>
    public void PlayBackgroundMusic()
    {
        PlayMusic(backgroundMusics, true);
    }

    /// <summary>
    /// Play crash sound
    /// </summary>
    public void PlayCrashSound()
    {
        PlaySound(crashSound);
    }

    /// <summary>
    /// Play collectable sound
    /// </summary>
    public void PlayCollectableSound()
    {
        PlaySound(gotCollectable);
    }

    /// <summary>
    /// Play game over sound
    /// </summary>
    public void PlayGameOverSound()
    {
        PlaySound(gameOverSound);
    }

    /// <summary>
    /// Play game win sound
    /// </summary>
    public void PlayGameWinSound()
    {
        PlaySound(gameWinSound);
    }

    /// <summary>
    /// Play button click sound
    /// </summary>
    public void PlayButtonClickSound()
    {
        PlaySound(buttonClickSound);
    }

    /// <summary>
    /// Play pause sound
    /// </summary>
    public void PlayPauseSound()
    {
        PlaySound(pauseSound);
    }
}