using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public class AudioUnit : IPoolType, IPoolAble
    {
        private int m_ID = -1;
        private AudioSource m_AudioSource;
        private AudioClip m_AudioClip;
        //private Action m_OnFinishListener;
        private bool m_Loop;

        public int id
        {
            get { return m_ID; }
        }

        public void SetAudio(GameObject gameObject, AudioClip clip, bool loop, bool enable)
        {
            if (clip == null)
            {
                return;
            }
            m_Loop = loop;
            if (m_AudioSource == null)
                m_AudioSource = gameObject.AddComponent<AudioSource>();

            m_AudioClip = clip;
            m_AudioSource.clip = m_AudioClip;
            m_AudioSource.loop = loop;
            m_AudioSource.Play();

            CustomExtension.CallWithDelay(AudioMgr.S, OnAudioFinish, m_AudioClip.length);
            //m_AudioSource.
        }

        private void OnAudioFinish()
        {
            if (!m_Loop)
            {
                Release();
            }
        }

        private void Release()
        {
            m_AudioClip = null;
            m_ID = -1;
            Recycle2Cache();
        }

        public void SetVolume(float volume)
        {
            if (m_AudioSource == null) return;
            m_AudioSource.volume = volume;
        }

        public void Pause()
        {
            if (m_AudioSource == null) return;
            m_AudioSource.Pause();
        }

        public void Resume()
        {
            if (m_AudioSource == null) return;
            m_AudioSource.UnPause();
        }

        public void Stop()
        {
            if (m_AudioSource == null) return;
            m_AudioSource.Stop();
        }


        public static AudioUnit Allocate()
        {
            return ObjectPool<AudioUnit>.S.Allocate();
        }

        public void Recycle2Cache()
        {
            if (!ObjectPool<AudioUnit>.S.Recycle(this))
            {
                if (m_AudioSource != null)
                {
                    //Log.e(m_AudioSource);
                    GameObject.Destroy(m_AudioSource);
                    m_AudioSource = null;
                }
            }
        }

        public void OnCacheReset()
        {

        }
    }
}
