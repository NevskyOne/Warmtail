using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;

namespace Entities.Sound
{
    public class ObjectSfx : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private SerializedDictionary<string, AudioClip> _clips = new();

        public void PlaySfx(string sfxName)
        {
            _source.PlayOneShot(_clips[sfxName]);
        }
        
        public void PlaySfx(AudioClip sfx)
        {
            _source.PlayOneShot(sfx);
        }
        
        public async void PlayLoopSfx(AudioClip sfx, int delay = 0)
        {
            await UniTask.Delay(delay);
            _source.clip = sfx;
            _source.Play();
        }
        
        public void StopLoopSfx()
        {
            _source.Stop();
            _source.clip = null;
        }
    }
}