using UnityEngine;

namespace Views.Binders
{
    public class AudioBinder : MonoBehaviour
    {
        [SerializeField] AudioSource ambientSource;
        [SerializeField] AudioSource sfxSource;

        public AudioSource AmbientSource => ambientSource;
        public AudioSource SfxSource => sfxSource;
    }
}