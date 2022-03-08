using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [Range(0f, 1f)] [SerializeField] private float volume;

    private AudioSource audioSource;

    private void OnEnable()
    {
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.volume = volume;
        audioSource.Play();

        Invoke(nameof(DisableAfterPlay),audioSource.clip.length);
    }

    private void DisableAfterPlay()
    {
        // Todo Disable GameObject to reuse it with pool system
        gameObject.SetActive(false);
    }
}
