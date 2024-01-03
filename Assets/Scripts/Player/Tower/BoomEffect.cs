using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEffect : MonoBehaviour
{
    private AudioSource BoomSound;
    void Awake()
    {
        BoomSound = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        BoomSound.Play();
    }
    private void OnParticleSystemStopped()
    {
        Effect_Pool.ReturnObject(this);
    }
}
