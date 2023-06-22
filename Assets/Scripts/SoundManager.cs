using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public static float sliderMValue = 1, sliderSfxValue = 1;

    public AudioSource musicSource, effectSource;

    void Awake() {
        if(Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }

    public void MusicManager(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
        musicSource.loop = true;
    }

    public void ChangeMusicVolume(float value) 
    {
        musicSource.volume = value;
        sliderMValue = value;
    }

    public void ChangeEffectVolume(float value)
    {
        effectSource.volume = value;
        sliderSfxValue = value;
    }
}
