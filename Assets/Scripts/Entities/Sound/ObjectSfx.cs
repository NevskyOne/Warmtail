using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace Entities.Sound
{
    public class ObjectSfx : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [ShowInInspector] private readonly Dictionary<string, AudioClip> _clips = new();

        public void PlaySfx(string sfxName)
        {
            _source.PlayOneShot(_clips[sfxName]);
        }
    }
}