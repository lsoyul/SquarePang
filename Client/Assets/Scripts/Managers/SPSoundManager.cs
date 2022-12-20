using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DigitalRuby.SoundManagerNamespace;

using UnityEngine.UI;

public class SPSoundManager : MonoBehaviour
{
    public enum Sound_BGM
    {
        Title,
        Ingame,
    }

    public enum Sound_EFFECT
    {
        Error,
        MadeFinish1,
        MadeFinish2,
        MadeFinish3,
        MadeFinish4,
        MadeStart,
        MoveStart,
        StompBoard,
        StompBreaker,
        Tick,
        TouchFail,
        TouchStart,
    }

    public Slider Volume_Music;
    public Slider Volume_Sound;

    public GameObject MusicAudioSourcesRoot;
    public GameObject SoundAudioSourcesRoot;

    private static Dictionary<string, AudioSource> SoundAudioSources = new Dictionary<string, AudioSource>();
    private static Dictionary<string, AudioSource> MusicAudioSources = new Dictionary<string, AudioSource>();

    private static bool isInitializeSounds = false;


    private void Awake()
    {
        if (isInitializeSounds == false)
        {
            if (MusicAudioSourcesRoot != null)
            {
                foreach (Transform musics in MusicAudioSourcesRoot.transform)
                {
                    AudioSource newMusic = musics.gameObject.GetComponent<AudioSource>();

                    if (newMusic != null)
                    {
                        MusicAudioSources.Add(musics.name, newMusic);
                    }
                }
            }

            if (SoundAudioSourcesRoot != null)
            {
                foreach (Transform sounds in SoundAudioSourcesRoot.transform)
                {
                    AudioSource newSound = sounds.GetComponent<AudioSource>();

                    if (newSound != null)
                    {
                        SoundAudioSources.Add(sounds.name, newSound);
                    }
                }
            }

            if (Volume_Music != null) SoundManager.MusicVolume = Volume_Music.value;
            if (Volume_Sound != null) SoundManager.SoundVolume = Volume_Sound.value;
        }

        isInitializeSounds = true;
    }

    public static void PlayMusic(Sound_BGM bgm, bool isLoop = true)
    {
        string musicName = bgm.ToString();

        if (MusicAudioSources.ContainsKey(musicName))
        {
            if (MusicAudioSources[musicName].isPlaying == false)
            {
                //StopAllMusic();
                MusicAudioSources[musicName].PlayLoopingMusicManaged(1f, 1f, isLoop);
            }
        }
    }

    static void StopAllMusic()
    {
        foreach (KeyValuePair<string,AudioSource> audio in MusicAudioSources)
        {
            audio.Value.StopLoopingMusicManaged();
        }
    }

    public static void PlayEffect(Sound_EFFECT sound)
    {
        string soundName = sound.ToString();

        if (SoundAudioSources.ContainsKey(soundName))
        {
            SoundAudioSources[soundName].PlayOneShotSoundManaged(SoundAudioSources[soundName].clip, 1f);
        }
    }

    public void OnChangeMusicVolume()
    {
        SoundManager.MusicVolume = Volume_Music.value;
    }

    public void OnChangeSoundVolume()
    {
        SoundManager.SoundVolume = Volume_Sound.value;
    }
}
