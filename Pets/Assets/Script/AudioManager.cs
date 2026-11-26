using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Dino.Utility.Audio;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using AudioType = Dino.Utility.Audio.AudioType;

namespace Dino.Utility.Audio
{

    public class AudioManager : MonoBehaviour
    {
        #region Singleton
        public static AudioManager Instance { get; private set; }
        #endregion

        #region serialized fields



        [Header("Audio Manager Data")]
        [SerializeField]
        private AudioManagerData _audioManagerData;


        [Header("Audio Test")]
        public string _soundNameTest;

        [Header("Musica inicial")]
        [SerializeField] private string _musicOnStart = "BackgroundMusic";

        [SerializeField] private string _soundOnClick = "";

        #endregion

        #region private fields

        private GameObject _soundsContainer;
        private List<AudioSource> _audioSources = new List<AudioSource>();

        private List<AudioData> _sfxAudioData = new List<AudioData>();
        private List<AudioData> _musicAudioData = new List<AudioData>();

        #endregion

        public AudioManagerData AudioManagerData
        {
            get => _audioManagerData;
            set => _audioManagerData = value;
        }


        #region unity methods

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            Initialize();
        }

        private void Start()
        {
            LoadSavedVolumes();

            if (!string.IsNullOrEmpty(_musicOnStart))
                PlaySound(_musicOnStart);
        }

        private void Update()
        {


        }

        public void LoadSavedVolumes()
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

            Debug.Log("MUSIC GUARDADO: " + musicVolume);
            Debug.Log("SFX GUARDADO: " + sfxVolume);

            UpdateAudioMixerGroupVolume(musicVolume, AudioType.Music);
            UpdateAudioMixerGroupVolume(sfxVolume, AudioType.SFX);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LoadSavedVolumes();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        #endregion

        #region private methods



        private AudioSource FindAudioSource(AudioClip clip)
        {
            return _audioSources.Find(x => x.clip == clip);
        }

        private void Initialize()
        {
            _soundsContainer = new GameObject("SoundsContainer");
            _audioSources = new List<AudioSource>();
            _soundsContainer.transform.SetParent(transform);
            PrepareLists();
        }



        private void PrepareLists()
        {
            _sfxAudioData = _audioManagerData.audioData.FindAll(x => x.audioType == AudioType.SFX);
            _musicAudioData = _audioManagerData.audioData.FindAll(x => x.audioType == AudioType.Music);
        }

        private void SetAudioInMixerGroup(AudioSource audioSource, AudioType audioType)
        {
            switch (audioType)
            {
                case AudioType.Music:
                    audioSource.outputAudioMixerGroup =
                        _audioManagerData.MusicAudioMixerGroup;
                    break;

                case AudioType.SFX:
                    audioSource.outputAudioMixerGroup =
                        _audioManagerData.SfxAudioMixerGroup;
                    break;

                case AudioType.Ambience:
                    audioSource.outputAudioMixerGroup =
                        _audioManagerData.AmbienceAudioMixerGroup;
                    break;
            }
        }

        #endregion


        #region public methdos

        public void PlaySound(string soundName)
        {
            AudioData audioData = _audioManagerData.audioData.Find(x => x.name == soundName);
            if (audioData == null)
            {
                Debug.LogWarning("Sound: " + soundName + " not found!");
                return;
            }

            AudioSource audioSource = FindAudioSource(audioData.clip);

            if (audioSource == null)
            {
                GameObject go = new GameObject("AS : " + audioData.name);
                audioSource = go.AddComponent<AudioSource>();
                audioSource.transform.SetParent(_soundsContainer.transform);
                audioSource.clip = audioData.clip;
                audioSource.loop = audioData.loop;
                audioSource.volume = audioData.volume;
                audioSource.pitch = audioData.pitch;
                audioSource.Play();
                SetAudioInMixerGroup(audioSource, audioData.audioType);
                _audioSources.Add(audioSource);
            }
            else
            {
                audioSource.Play();
            }
        }

        public void StopSound(string soundName)
        {
            AudioData audioData = _audioManagerData.audioData.Find(x => x.name == soundName);
            if (audioData == null)
            {
                Debug.LogWarning("Sound: " + soundName + " not found!");
                return;
            }

            AudioSource audioSource = FindAudioSource(audioData.clip);
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }

        public void UpdateAudioMixerGroupVolume(float value, AudioType audioType)
        {
            AudioMixerGroup group = null;
            string parameter = "";

            switch (audioType)
            {
                case AudioType.Music:
                    group = _audioManagerData.MusicAudioMixerGroup;
                    parameter = _audioManagerData.MusicVolumeParameterName;
                    break;

                case AudioType.SFX:
                    group = _audioManagerData.SfxAudioMixerGroup;
                    parameter = _audioManagerData.SfxVolumeParameterName;
                    break;

                case AudioType.Ambience:
                    group = _audioManagerData.AmbienceAudioMixerGroup;
                    parameter = _audioManagerData.AmbienceVolumeParameterName;
                    break;
            }

            if (group == null)
            {
                Debug.LogError("No se encontró el AudioMixerGroup de " + audioType);
                return;
            }

            float volumeDB = value <= 0.0001f
                ? -80f
                : Mathf.Log10(value) * 20f;

            bool success = group.audioMixer.SetFloat(parameter, volumeDB);

            Debug.Log(
                "AUDIO: " + audioType +
                " | Valor: " + value +
                " | Parámetro: " + parameter +
                " | Resultado: " + success
            );
        }

        #endregion

        [Button]
        void TestSound()
        {
            PlaySound(_soundNameTest);
            Debug.Log("Playing sound: " + _soundNameTest);

        }

        public void ButtonSound()
        {
            PlaySound(_soundOnClick);
        }
    }
}

namespace Dino.Utility.Audio
{
    public enum AudioType
    {
        Music,
        SFX,
        Ambience
    }
}
    

