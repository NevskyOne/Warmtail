using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Cysharp.Threading.Tasks;
using Systems.Effects;
using TriInspector;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Entities.Sound
{
    public class MusicStateSystem : MonoBehaviour
    {
        [SerializeReference, Range(0.5f, 8f)] private float _crossFadeTime;
        [SerializeField] private AudioSource _source1;
        [SerializeField] private AudioSource _source2;

        [SerializeField, ShowInInspector, SerializedDictionary("Music State", "Audio Clips")]
        private SerializedDictionary<MusicState, List<AudioClip>> _clips = new();

        private MusicState _currentState;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
             var clip = _clips[_currentState][Random.Range(0, _clips[_currentState].Count)];
             
             _source1.clip = clip;
             _source1.Play();
        }
        
        public void ChangeMusicState(int state) => ChangeMusicStateAsync((MusicState)state);
        
        public async void ChangeMusicStateAsync(MusicState state)
        {
            StopAllCoroutines();
            var sourceFromCross = _source1.clip ? _source1 : _source2;
            var sourceToCross = _source1.clip ? _source2 : _source1;
            try
            {
                var rand = Random.Range(0, _clips[state].Count);
                if (sourceFromCross.clip == _clips[state][rand])
                {
                    if (rand == 0) rand += 1;
                    else rand -= 1;
                }
                var clip = _clips[state][rand];
            
                _currentState = state;
            
                sourceToCross.clip = clip;
                sourceToCross.volume = 0;
                sourceToCross.Play();
                await foreach (var (a, b) in CrossfadeEffect.CrossfadeTwins(_crossFadeTime))
                {
                    sourceFromCross.volume = a;
                    sourceToCross.volume = b;
                }
                sourceFromCross.Stop();
                sourceFromCross.clip = null;
                StartCoroutine(LoopNext(sourceToCross));
            }
            catch (Exception e)
            {
                sourceFromCross.volume = 1;
                sourceToCross.volume = 1;
            }
        }

        private IEnumerator LoopNext(AudioSource source)
        {
            yield return new WaitForSeconds(source.clip.length - source.time - _crossFadeTime);
            ChangeMusicStateAsync(_currentState);
        }
        
    }

    [Serializable]
    public enum MusicState
    {
        Start, Normal
    }
}