﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


namespace Sounds
{
    public class SoundManager : MonoBehaviour
    {
        //take game setting volume with gamesingleton.instance.gamesettings.volume 
        public Sound[] sounds;

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
            Debug.Log("PLAYING SOUND: " + name);
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                
                Debug.Log("sound " + name + "does not exist");
                return;

            }
            s.source.Play();
        }
    }
}