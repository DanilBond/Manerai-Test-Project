using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioSource ambientAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    
    [SerializeField] private AudioClip[] punchClips;
    [SerializeField] private AudioClip dieClip;

    public void PlayPunchSoundAtLocation(Vector3 punchPosition)
    {
        AudioSource.PlayClipAtPoint(punchClips[Random.Range(0, punchClips.Length)], punchPosition);
    }

    public void PlayDieClip()
    {
        sfxAudioSource.PlayOneShot(dieClip);
    }
}
