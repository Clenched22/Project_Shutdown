using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;

    private void Awake()
    {
        AudioManager[] objs = FindObjectsOfType<AudioManager>();

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sound s in Sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
        }
    }
    public void Play(string Name)
    {
        Sound s = Array.Find(Sounds, Sound => Sound.Name == Name);
        if (s == null)
        {
            Debug.Log("Null");
        }
        Debug.Log(Name);
        s.Pitch = UnityEngine.Random.Range(s.Pitch - 0.2f, s.Pitch + 0.2f);
        s.Source.Play();
    }

    //FindObjectOfType<AudioManager>().Play("SOUNDNAME");
}
