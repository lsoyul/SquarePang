using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DigitalRuby.SoundManagerNamespace;

public class SPSoundManager : MonoBehaviour
{
    public enum Sound_BGM
    {
        Title,
    }

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
        }

        isInitializeSounds = true;
    }

    public static void PlayMusic(Sound_BGM bgm, bool isLoop = true)
    {
        string musicName = bgm.ToString();

        if (MusicAudioSources.ContainsKey(musicName))
        {
            MusicAudioSources[musicName].PlayLoopingMusicManaged(1f, 2f, isLoop);
        }
    }
}
