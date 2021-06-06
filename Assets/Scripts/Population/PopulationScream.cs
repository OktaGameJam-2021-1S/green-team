using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PopulationScream : MonoBehaviour
{
    
    [SerializeField] private List<AudioClip> _sfx;
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _source.PlayOneShot(_sfx[Random.Range(0, _sfx.Count)]);
    }

}
