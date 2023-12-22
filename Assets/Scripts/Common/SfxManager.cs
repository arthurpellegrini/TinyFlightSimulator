using System;
using System.Collections.Generic;
using System.Xml;
using Common;
using UnityEngine;

[Serializable]
public class MyAudioClip
{
    public AudioClip clip;
    public float volume;

    public MyAudioClip(AudioClip clip, float volume)
    {
        this.clip = clip;
        this.volume = volume;
    }
}

/// <summary>
///     Sfx manager.
/// </summary>
public class SfxManager : Singleton<SfxManager>
{
    [Header("SfxManager")] [SerializeField]
    private TextAsset m_SfxXmlSetup;

    [SerializeField] private string m_ResourcesFolderName;

    [SerializeField] private int m_NAudioSources = 2;
    [SerializeField] private GameObject m_AudioSourceModel;

    [SerializeField] private bool m_ShowGui;

    private readonly List<AudioSource> m_AudioSources = new();
    private readonly Dictionary<string, MyAudioClip> m_DicoAudioClips = new();

    private void Start()
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(m_SfxXmlSetup.text);

        foreach (XmlNode node in xmlDoc.GetElementsByTagName("SFX"))
            if (node.NodeType != XmlNodeType.Comment)

                if (node.Attributes != null)
                    m_DicoAudioClips.Add(
                        node.Attributes["name"].Value,
                        new MyAudioClip(
                            (AudioClip)Resources.Load(m_ResourcesFolderName + "/" + node.Attributes["name"].Value,
                                typeof(AudioClip)),
                            float.Parse(node.Attributes["volume"].Value)));

        m_AudioSources.Add(m_AudioSourceModel.GetComponent<AudioSource>());
        for (var i = 0; i < m_NAudioSources - 1; i++)
            AddAudioSource();
    }

    private void OnGUI()
    {
        if (!m_ShowGui) return;


        GUILayout.BeginArea(new Rect(Screen.width * .5f + 10, 10, 200, Screen.height));
        GUILayout.Label("SFX MANAGER");
        GUILayout.Space(20);
        foreach (var item in m_DicoAudioClips)
            if (GUILayout.Button("PLAY " + item.Key))
                PlaySfx2D(item.Key);
        GUILayout.EndArea();
    }

    private AudioSource AddAudioSource()
    {
        var newGO = Instantiate(m_AudioSourceModel, transform, true);
        newGO.name = "AudioSource";

        var audioSource = newGO.GetComponent<AudioSource>();
        m_AudioSources.Add(audioSource);

        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1;

        return audioSource;
    }

    public void PlaySfx3D(string sfxName, Vector3 pos)
    {
        PlaySfx(sfxName, pos);
    }

    public void PlaySfx2D(string sfxName)
    {
        PlaySfx3D(sfxName, Camera.main.transform.position);
    }

    private void PlaySfx(string sfxName, Vector3 pos)
    {
        if (FlagsManager.Instance && !FlagsManager.Instance.GetFlag("SETTINGS_SFX", true))
            return;

        MyAudioClip audioClip;
        if (!m_DicoAudioClips.TryGetValue(sfxName, out audioClip))
        {
            Debug.LogError("SFX, no audio clip with name: " + sfxName);
            return;
        }

        var audioSource = m_AudioSources.Find(item => !item.isPlaying);
        if (audioSource)
        {
            audioSource.transform.position = pos;
            audioSource.PlayOneShot(audioClip.clip, audioClip.volume);
        }
    }
}