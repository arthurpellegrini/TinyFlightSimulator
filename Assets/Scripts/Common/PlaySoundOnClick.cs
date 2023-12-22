using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class PlaySoundOnClick : MonoBehaviour
    {
        [SerializeField] private string m_SoundName;

        private void Start()
        {
            var button = GetComponent<Button>();
            if (button) button.onClick.AddListener(PlaySound);
        }

        private void PlaySound()
        {
            if (SfxManager.Instance) SfxManager.Instance.PlaySfx2D(m_SoundName);
        }
    }
}