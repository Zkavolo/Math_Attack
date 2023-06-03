using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public bool alt;

    void Start()
    {
        if(alt == true)
        {   
            slider.value = SoundManager.sliderMValue;
            SoundManager.Instance.ChangeMusicVolume(slider.value);
            slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMusicVolume(val));    
        } else 
        {
            slider.value = SoundManager.sliderSfxValue;
            SoundManager.Instance.ChangeEffectVolume(slider.value);
            slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeEffectVolume(val));
        }
    }

}
