using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect
{
    public static string Shot = "shot";
    public static string Boom = "boom";
    public static string Button1 = "button1";
    public static string Button2 = "button2";
    public static string BossDie = "bossDie";
    public static string BossApear = "bossApear";
    public static string Rayzer = "rayzer";
    public static string SelectApear = "selectApear";
    public static string Select = "select";
    public static string Skill2 = "skill2";
    public static string Skill1 = "skill1";
}
public class Bgm
{
    public static string Title = "title";
    public static string Ingame = "ingame";
}

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource BGM;
    private Dictionary<string, AudioClip> sfxs;
    private Dictionary<string, AudioClip> bgms;

    private void Awake()
    {
        SetInstance();
        sfxs = new Dictionary<string, AudioClip>();
        bgms = new Dictionary<string, AudioClip>();

        SetSoundManager();
    }

    private void SetSoundManager()
    {
        foreach (var sfx in Resources.LoadAll<AudioClip>("Sound/Sfx"))
        {
            sfxs.Add(sfx.name, sfx);
        }

        foreach (var bgm in Resources.LoadAll<AudioClip>("Sound/Bgm"))
        {
            bgms.Add(bgm.name, bgm);
        }

        this.gameObject.AddComponent<AudioSource>();
        BGM = GetComponent<AudioSource>();
    }

    public void PlaySfx(string key, float volume)
    {
        var sound = sfxs[key];

        var soundEffect = new GameObject("soundEffect").AddComponent<AudioSource>();
        soundEffect.volume = volume;
        soundEffect.clip = sound;
        soundEffect.PlayOneShot(soundEffect.clip);

        Destroy(soundEffect.gameObject, soundEffect.clip.length);
    }

    public void PlayBgm(string key, float volume)
    {
        var bgm = bgms[key];

        BGM.clip = bgm;
        BGM.volume = volume;
        BGM.loop = true;
        BGM.Play();
    }
}
