using Game.Behaviours;
using UnityEngine;

namespace Game.Components
{
    public static class AudioUtility
    {
        public static float GetRandomPitch(UIConfiguration uiConfiguration)
        {
            return Random.Range(
                uiConfiguration.ItemSfxPitchRangeMin,
                uiConfiguration.ItemSfxPitchRangeMax
            );
        }
        public static void PlayClipOnItemView(ItemView view, AudioClip clip)
        {
            PlayClipOnItemView(view, clip, UIConfiguration.Instance);
        }

        public static void PlayClipOnItemView(ItemView view, AudioClip clip, UIConfiguration uiConfiguration)
        {
            view.audioSource.pitch = GetRandomPitch(uiConfiguration);
            view.audioSource.PlayOneShot(clip);
        }
    }
}