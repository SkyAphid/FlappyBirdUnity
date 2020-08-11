using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{


    public static AudioManager Instance;

    public enum Sound
    {
        Fall,
        Flap,
        Hit,
        Point,
        Swoosh
    }

    [SerializeField] private AudioClip[] AudioClips = null;
    private AudioSource[] AudioSource;

    void Awake()
    {
        //Initialize Singleton
        if (Instance != null)
        {
            Debug.LogError("Multiple AudioManager instances found.");
        }

        Instance = this;

        //Initialize audio clips into audio sources
        AudioSource = new AudioSource[AudioClips.Length];

        for (int i = 0; i < AudioSource.Length; i++)
        {
            AudioSource[i] = gameObject.AddComponent<AudioSource>();
            AudioSource[i].clip = AudioClips[i];
        }
    }

    public static void PlaySFX(Sound sound)
    {
        Instance.AudioSource[(int)sound].Play();
    }
}
