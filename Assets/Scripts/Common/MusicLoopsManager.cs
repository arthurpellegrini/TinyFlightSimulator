using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
	/// <summary>
	///     Music loops manager.
	///     Gestion de boucles musicales avec Fade-In/Fade-Out entre 2 boucles
	/// </summary>
	public class MusicLoopsManager : Singleton<MusicLoopsManager>
    {
        [Header("MusicLoopsManager")] [SerializeField]
        private List<AudioClip> m_Clips = new();

        [SerializeField] private float m_FadeDuration;

        [SerializeField] private bool m_ShowGui;
        private AudioSource[] m_AudioSources;

        private int m_CurrClipIndex;

        private int m_IndexFadeIn;
        private readonly float[] m_MaxVolumes = new float[2];


        protected override void Awake()
        {
            base.Awake();

            m_IndexFadeIn = 0;

            m_AudioSources = GetComponents<AudioSource>();
            if (m_AudioSources.Length != 2)
                Debug.LogError("MusicLoopsManager needs 2 AudioSource to work properly!");

            for (var i = 0; i < m_AudioSources.Length; i++)
            {
                m_MaxVolumes[i] = m_AudioSources[i].volume;
                m_AudioSources[i].clip = m_Clips[i];
            }
        }

        private void OnGUI()
        {
            if (!m_ShowGui) return;

            GUILayout.BeginArea(new Rect(Screen.width / 2 - 210, 10, 200, Screen.height));

            GUILayout.Label("MUSIC LOOPS MANAGER");
            GUILayout.Space(20);
            for (var i = 0; i < m_Clips.Count; i++)
                if (GUILayout.Button("PLAY " + m_Clips[i].name))
                    PlayMusic(i);
            GUILayout.Space(20);
            if (GUILayout.Button("PLAY CURRENT MUSIC"))
                PlayCurrentMusic();

            if (GUILayout.Button("PLAY NEXT MUSIC"))
                PlayNextMusic();

            if (GUILayout.Button("STOP ALL - FADEOUT"))
                StopAll(0);

            if (GUILayout.Button("STOP ALL - NO FADEOUT"))
                StopAllRightAway();

            GUILayout.EndArea();
        }

        private IEnumerator FadeOutAndStopAll(float delay)
        {
            yield return
                new WaitForSeconds(delay +
                                   .1f); // Unity bug possiblement si la durée d'attente est nulle ... on ajoute 0,1 pour que cette durée ne soit jamais véritablement nulle
            float elapsedTime = 0;

            while (elapsedTime < m_FadeDuration)
            {
                var k = elapsedTime / m_FadeDuration;
                m_AudioSources[m_IndexFadeIn].volume =
                    Mathf.Lerp(0, m_MaxVolumes[m_IndexFadeIn], 1 - k); //Fade out 1st audiosource
                m_AudioSources[1 - m_IndexFadeIn].volume =
                    Mathf.Lerp(0, m_MaxVolumes[1 - m_IndexFadeIn], 1 - k); //Fade out 2nd audiosource
                elapsedTime += Time.timeScale != 0 ? Time.deltaTime : 1 / 60f;
                yield return null;
            }

            m_AudioSources[m_IndexFadeIn].volume = 0;
            m_AudioSources[m_IndexFadeIn].Stop();
            m_AudioSources[1 - m_IndexFadeIn].volume = 0;
            m_AudioSources[1 - m_IndexFadeIn].Stop();
        }


        private IEnumerator FadeCoroutine()
        {
            float elapsedTime = 0;
            while (elapsedTime < m_FadeDuration)
            {
                var k = elapsedTime / m_FadeDuration;
                m_AudioSources[m_IndexFadeIn].volume =
                    Mathf.Lerp(0, m_MaxVolumes[m_IndexFadeIn], k); //Fade in 1st audiosource
                m_AudioSources[1 - m_IndexFadeIn].volume =
                    Mathf.Lerp(0, m_MaxVolumes[1 - m_IndexFadeIn], 1 - k); //Fade out 2nd audiosource
                elapsedTime += Time.timeScale != 0 ? Time.deltaTime : 1 / 60f;
                yield return null;
            }

            m_AudioSources[m_IndexFadeIn].volume = Mathf.Lerp(0, m_MaxVolumes[m_IndexFadeIn], 1);
            m_AudioSources[1 - m_IndexFadeIn].volume = Mathf.Lerp(0, m_MaxVolumes[1 - m_IndexFadeIn], 0);
            m_AudioSources[1 - m_IndexFadeIn].Stop();
        }

        public void PlayMusic(int index, bool fade = true)
        {
            m_CurrClipIndex = index % m_Clips.Count;
            if (fade)
            {
                m_AudioSources[1 - m_IndexFadeIn].clip = m_Clips[m_CurrClipIndex];
                m_IndexFadeIn = 1 - m_IndexFadeIn;
                StartCoroutine(FadeCoroutine());

                var currentTimeScale = Time.timeScale;
                Time.timeScale = 1;
                m_AudioSources[m_IndexFadeIn].Play();
                Time.timeScale = currentTimeScale;
            }
        }

        public void PlayCurrentMusic()
        {
            if (!FlagsManager.Instance || FlagsManager.Instance.GetFlag("SETTINGS_MUSIC", true))
                PlayMusic(m_CurrClipIndex);
        }

        public void PlayNextMusic()
        {
            if (!FlagsManager.Instance || FlagsManager.Instance.GetFlag("SETTINGS_MUSIC", true))
                PlayMusic(m_CurrClipIndex + 1);
        }

        public void StopAll(float delay)
        {
            Debug.Log("InGameMusicManager StopAll(" + delay + ")");
            StartCoroutine(FadeOutAndStopAll(delay));
        }

        public void StopAllRightAway()
        {
            StopAllCoroutines();
            m_AudioSources[m_IndexFadeIn].volume = 0;
            m_AudioSources[1 - m_IndexFadeIn].volume = 0;
            m_AudioSources[1 - m_IndexFadeIn].Stop();
            m_AudioSources[m_IndexFadeIn].Stop();
        }
    }
}