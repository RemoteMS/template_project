using System;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;

namespace Services.Global
{
    public interface IAudioService
    {
        void Bind(GameObject audioSource);

        void StopPlaying();
        void StartPlaying();
        void InjectAudioMixer(AudioMixer mixer);
    }

    public class AudioService : IAudioService, IDisposable
    {
        private readonly ReactiveProperty<AudioMixer> _mixer;
        private readonly CompositeDisposable _compositeDisposable = new();

        public AudioService()
        {
            _mixer = new ReactiveProperty<AudioMixer>();
            _mixer.Subscribe(x => { Debug.Log("new Value audio loaded"); }).AddTo(_compositeDisposable);
            _mixer.AddTo(_compositeDisposable);
        }


        public void InjectAudioMixer(AudioMixer mixer)
        {
            _mixer.Value = mixer;
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            Debug.Log("AudioService disposed");
        }

        public void Bind(GameObject audioSource)
        {
        }

        public void StopPlaying()
        {
            _mixer.Value.SetFloat("AmbientVolume", -80);
        }

        public void StartPlaying()
        {
            _mixer.Value.SetFloat("AmbientVolume", 0);
        }
    }
}