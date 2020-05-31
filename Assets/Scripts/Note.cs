using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public enum NoteState
//{
//    Off,
//    Attack,
//    Delay,
//    Sustain,
//    Release,
//}

public class Note : MonoBehaviour
{
    AudioSource audioSource;
    bool noteOn;
    float pitch;

    public float attackTime = 0.050f;
    public float decayTime = 0.100f;
    public float sustainLevel = 0.5f;
    public float releaseTime = 0.200f;

    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
        this.noteOn = false;
        StartCoroutine(PlayNoteCoroutine());
    }

    public float Pitch
    {
        get => this.pitch;
        set {
            this.pitch = value;
            if (audioSource != null) audioSource.pitch = pitch;
        }
    }

    public bool Press()
    {
        var ret = !this.noteOn;
        this.noteOn = true;
        return ret;
    }

    public bool Release()
    {
        var ret = this.noteOn;
        this.noteOn = false;
        return ret;
    }

    public bool IsNoteOn()
    {
        return this.noteOn;
    }

    public bool IsNoteOff()
    {
        return !this.noteOn;
    }

    IEnumerator PlayNoteCoroutine()
    {
        this.audioSource.volume = 0;
        this.audioSource.loop = true;
        this.audioSource.pitch = pitch;
        this.audioSource.Play();
        while (true)
        {
            yield return new WaitUntil(IsNoteOn);
            // Attack
            Debug.Log("attack");
            while (IsNoteOn() && this.audioSource.volume < 1.0f)
            {
                this.audioSource.volume += (1.0f / attackTime) * Time.deltaTime;
                yield return null;
            }
            // Decay
            Debug.Log("decay");
            while (IsNoteOn() && this.audioSource.volume > sustainLevel)
            {
                this.audioSource.volume -= ((1f - sustainLevel) / decayTime) * Time.deltaTime;
                yield return null;
            }
            // Sustain
            Debug.Log("sustain");
            if (IsNoteOn())
            {
                yield return new WaitUntil(IsNoteOff);
            }
            // Release
            Debug.Log("release");
            while (IsNoteOff() && this.audioSource.volume > 0)
            {
                this.audioSource.volume -= ((sustainLevel) / releaseTime) * Time.deltaTime;
                yield return null;
            }
        };
    }
}
