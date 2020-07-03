﻿using System;
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
        private void Awake()
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

        public void Play(string name)
        {
            //Debug.Log("PLAYING SOUND: " + name);
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.Log("sound " + name + "does not exist");
                return;
            }
            _settings = GameSingleton.Instance.gameSettings;
            if (s.loop) //is a music
                s.source.volume *= _settings.musicVolume;
            else
                s.source.volume *= _settings.soundVolume;
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

            s.source.Stop ();
        }

        public void StopPlayingAllMusics()
        {
            var s = Array.Find(sounds, item => item.loop);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            //s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
            //s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

            s.source.Stop();
        }
   
    }
}