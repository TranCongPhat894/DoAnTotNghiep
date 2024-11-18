using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiomanagers : MonoBehaviour
{
    [Header("----audio source-----")]
    [SerializeField] AudioSource musicsource;
    [SerializeField] AudioSource SFXsource;
    [Header("-------audio clip-------")]
    public AudioClip background;
    private void Start()
    {
        musicsource.clip = background;
        musicsource.Play();
    }
}
