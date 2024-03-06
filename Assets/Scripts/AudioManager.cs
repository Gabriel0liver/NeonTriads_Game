using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
   public Sound[] sounds;

   public static AudioManager instance;

void Awake()
{
    if (instance == null)
    {
        instance = this;
    }
    else
    {
        Destroy(gameObject);
        return;
    }

    foreach (Sound s in sounds)
    {
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
    }

    DontDestroyOnLoad(gameObject);

    SceneManager.sceneLoaded += OnSceneLoaded;
}

private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    int sceneId = scene.buildIndex;
    if (sceneId > 0 && sceneId < 4)
    {
        Stop("menu_background");
    }
    else
    {
        Play("menu_background");
    }
}

void Start()
{
    Play("menu_background");
}

    public void Play(string name)
    {
         Sound s = System.Array.Find(sounds, sound => sound.name == name);
         s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void Pause(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }

    public void UnPause(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        s.source.UnPause();
    }


}
