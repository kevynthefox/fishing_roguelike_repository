using System;
using UnityEngine;

public class play_audio_on_collision : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnCollisionEnter(Collision other)
    {
        audioSource.Play();
    }
}
