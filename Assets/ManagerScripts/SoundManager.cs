using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    BGM,
    SFX,
    END
}

public class SoundManager : SingletonManager<SoundManager>
{
    private Dictionary<string, AudioClip> m_Sounds = new Dictionary<string, AudioClip>();
    public Dictionary<SoundType, AudioSource> AudioSources = new Dictionary<SoundType, AudioSource>();
    public Dictionary<SoundType, float> AudioVolumes = new Dictionary<SoundType, float>();

    protected override void Awake()
    {
        base.Awake();


        for (SoundType i = 0; i < SoundType.END; i++)
        {
            GameObject obj = new GameObject(i.ToString());
            AudioSources[i] = obj.AddComponent<AudioSource>();
            obj.transform.SetParent(gameObject.transform);
            AudioVolumes[i] = 0.5f;

            if (i == SoundType.BGM)
            {
                AudioSources[i].loop = true;
            }
        }

        AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Sounds/");
        foreach (AudioClip audioClip in audioClips)
        {
            m_Sounds[audioClip.name] = audioClip;
        }
    }
    public void SoundPlay(string clipName, SoundType soundType, float volume, float pitch)
    {
        if (soundType == SoundType.BGM)
        {
            AudioSources[soundType].clip = m_Sounds[clipName];
            AudioSources[soundType].volume = volume * AudioVolumes[soundType];
            AudioSources[soundType].Play();
        }
        else
        {
            AudioSources[soundType].PlayOneShot(m_Sounds[clipName], volume);
        }
        AudioSources[soundType].pitch = pitch;
    }
    public void SFX_VolumeSet(float value)
    {
        AudioVolumes[SoundType.SFX] = value;
    }
    public void BGM_VolumeSet(float value)
    {
        AudioVolumes[SoundType.BGM] = value;
    }
}

