using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoundManager : MonoBehaviour
{
    private AudioSource sound;
    public AudioClip[] bgm;
    [SerializeField] string[] bgm_name;
    [SerializeField] TMP_Text t_bgm;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        PlayBGM(0);
    }

    public void PlayBGM(int i)
    { sound.clip = bgm[i]; t_bgm.text = bgm_name[i]; sound.Play(); }

    public void MuteBtn()
    { sound.mute = !sound.mute; }

    public void SetVolume(float v)
    { sound.volume = v; }

}
