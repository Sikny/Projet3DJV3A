using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using Utility;


namespace Sounds
{
    public class SoundManager : MonoBehaviour
    {
        //take game setting volume with gamesingleton.instance.gamesettings.volume 
        public Sound[] sounds;
        private GameSettings _settings;
        public void Init()
        {
            
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }

        public void Play(string soundName)
        {
            #if UNITY_EDITOR
            Debug.Log("PLAYING SOUND: " + soundName);
            #endif
            Sound s = Array.Find(sounds, sound => sound.name == soundName);
            if (s == null)
            {
                #if UNITY_EDITOR
                Debug.LogError("sound " + soundName + "does not exist");
                #endif
                return;
            }
            _settings = GameSingleton.Instance.gameSettings;
            if (s.loop) //is a music
                s.source.volume *= _settings.musicVolume;
            else
                s.source.volume *= _settings.soundVolume;
            
            #if UNITY_EDITOR
            Debug.Log("volume is " + s.source.volume);
            #endif
            s.source.Play();
        }
        
        public void StopPlaying (string sound)
        {
            Sound s = Array.Find(sounds, item => item.name == sound);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            //s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
            //s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
            //s.source.volume = 0;
            //Debug.Log("stopping sound" + s.name);
            s.source.Stop();
        }

        public void StopPlayingAllMusics()
        {
            foreach(var s in sounds)
            {
                if (s.loop)
                {
                    s.source.Stop();
                }
            }
            //s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
            //s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        }

        public void StopPlayingAllSounds()
        {
            foreach (var s in sounds)
            {
                s.source.Stop();
            }
        }
   
    }
}