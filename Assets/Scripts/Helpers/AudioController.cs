using UnityEngine;
using UnityEngine.Events;

namespace KendaGame.Helpers
{
    public class AudioController : MonoBehaviour
    {
        public CommonSfx commonSfx;
        private bool sfxState = true;
        private bool musicState = true;

        private static AudioController _instance;
        public static AudioController Instance
        {
            get
            {
                if (!_instance)
                    _instance = GameObject.FindObjectOfType<AudioController>();

                return _instance;
            }
        }
        public void SetSfxState(bool state)
        {
            sfxState = state;
            SfxStateChanged?.Invoke(sfxState);
        }
        public void SetMusicState(bool state)
        {
            musicState = state;
            MusicStateChanged?.Invoke(sfxState);
        }
        public bool GetSfxState()
        {
            return sfxState;
        }
        public bool GetMusicState()
        {
            return musicState;
        }

        public void PlayAudioAtPoint(AudioClip audioClip) 
        {
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
        }

        public void PlayAudioAtPoint(AudioClip audioClip, float volume = 1f)
        {
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, volume);
        }

        public static event UnityAction<bool> SfxStateChanged;
        public static event UnityAction<bool> MusicStateChanged;

        [System.Serializable]
        public class CommonSfx 
        {
            public AudioClip[] draggableSfx;
            public AudioClip[] droppableSfx;
            public AudioClip[] clickableSfx;
            public AudioClip[] CorrectSfx;
            public AudioClip[] IncorrectSfx;
            public AudioClip GetDraggableSfx() 
            {
                return draggableSfx[Random.Range(0, draggableSfx.Length)];
            }
            public AudioClip GetDroppableSfx()
            {
                return droppableSfx[Random.Range(0, droppableSfx.Length)];
            }
            public AudioClip GetClickableSfx()
            {
                return clickableSfx[Random.Range(0, clickableSfx.Length)];
            }
            public AudioClip GetCorrectSfx()
            {
                return CorrectSfx[Random.Range(0, CorrectSfx.Length)];
            }

            public AudioClip GetInCorrectSfx()
            {
                return IncorrectSfx[Random.Range(0, IncorrectSfx.Length)];
            }
        }
    }
}