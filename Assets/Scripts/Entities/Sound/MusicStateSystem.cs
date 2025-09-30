using System.Collections.Generic;
using Systems;
using TriInspector;
using UnityEngine;

namespace Entities.Sound
{
    public class MusicStateSystem
    {
        [SerializeReference, Range(0.5f, 3f)] private float _crossFadeTime;
        [SerializeField] private AudioSource _source1;
        [SerializeField] private AudioSource _source2;
        [ShowInInspector] private readonly Dictionary<MusicState, List<AudioClip>> _clips = new();

        private MusicState _currentState;

        public async void ChangeMusicState(MusicState state)
        {
            var clip = _clips[state][Random.Range(0, _clips[state].Count)];

            _currentState = state;
            
            _source2.clip = clip;
            _source2.volume = 0;
            _source2.Play();
            await foreach (var (a, b) in CrossfadeSystem.CrossfadeTwins(_crossFadeTime))
            {
                _source1.volume = a;
                _source2.volume = b;
            }
            _source1.Stop();
            _source2.Stop();
            
            _source1.clip = clip;
            _source1.time = _crossFadeTime;
            _source1.Play();
        }
   
    }

    public enum MusicState
    {
        Normal, Battle
    }
}