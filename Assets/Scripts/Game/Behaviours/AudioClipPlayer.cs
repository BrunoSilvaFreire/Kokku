using Game.Components;
using UnityEngine;

namespace Game.Behaviours
{
    public class AudioClipPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;

        public void Play(AudioClip clip)
        {
            _source.pitch = AudioUtility.GetRandomPitch(UIConfiguration.Instance);
            _source.PlayOneShot(clip);
        }
    }
}