using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public List<AudioClip> clip;

    public void PlayClip(int num)
    {
        SoundManager.Instance.PlaySound(clip[num]);
    }

}
