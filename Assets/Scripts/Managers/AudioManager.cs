using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;


    public bool playBGM;
    private int bgmIndex;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional if AudioManager should persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!playBGM)
            StopBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }

    public void PlaySFX(int _sfxIndex)
    {
        if(_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.75f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }
    public void PlayBGM(int _bgmIndex)
    {

        bgmIndex = _bgmIndex;
        StopBGM();

        bgm[bgmIndex].Play();
    }

    public void StopBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
