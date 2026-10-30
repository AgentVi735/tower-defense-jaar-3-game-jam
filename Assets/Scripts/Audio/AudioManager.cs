using System.Collections.Generic;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace FckAI.Audio
{
    public partial class AudioManager : MonoBehaviour
    {
        [AutoStaticsCleanup]
        private static AudioManager s_Instance;

        [SerializeField] private AudioSource m_musicSource;
        [SerializeField] private AudioSource m_sfxSource;

        [SerializeField] private Dictionary<AudioType, AudioClip> m_audioClips;

        private void Awake()
        {
            Initialise();
        }

        private void OnDestroy()
        {
            if (s_Instance == this)
                s_Instance = null;
        }

        private void Initialise()
        {
            if (s_Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayMusic();
        }

        private AudioClip GetClip(AudioType audioType)
        {
            if (!m_audioClips.ContainsKey(audioType))
            {
                Debug.LogError($"Audio {audioType} does not exist in dictionary");
                return null;
            }

            bool couldGetClip = m_audioClips.TryGetValue(audioType, out AudioClip clip);
            if (couldGetClip) return clip;
            Debug.LogError($"Unable to get audio {audioType}");
            return null;

        }

        public static void PlaySFX(AudioType audioToPlay) => s_Instance.PlaySFXInternal(audioToPlay);

        private void PlaySFXInternal(AudioType audioToPlay)
        {
            AudioClip clip = GetClip(audioToPlay);

            if (clip == null)
                return;

            m_sfxSource.PlayOneShot(clip);
        }

        private void PlayMusic()
        {
            AudioClip clip = GetClip(AudioType.Music);

            if (clip == null)
                return;

            m_musicSource.clip = clip;
            m_musicSource.loop = true;
            m_musicSource.Play();
        }
    }
}