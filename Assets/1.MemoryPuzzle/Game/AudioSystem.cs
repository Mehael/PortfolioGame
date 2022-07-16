using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPuzzle
{
    public class AudioSystem : MonoBehaviour {
        public static AudioSystem instance;
        public List<AudioSource> Sounds;

        private Dictionary<string, AudioSource> NameToClip = new Dictionary<string, AudioSource>();
        private void Awake()
        {
            instance = this;
            foreach (var sound in Sounds){
                NameToClip.Add(sound.clip.name, sound);
            }
        }

        public static bool Play(string sound, float delay = 0)
        {
            if (instance == null || !instance.NameToClip.ContainsKey(sound))
            {
                Debug.LogWarning(sound + " sound not found.");
                return false;
            }

            if (delay <= 0)
                instance.NameToClip[sound].Play();
            else 
                instance.NameToClip[sound].PlayDelayed(delay);

            return true;
        }
    }
}