using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Audio Manager", menuName = "Systems/Audio Manager")]
public class AudioManager : ScriptableObject {

    [System.Serializable]
    private class Group {
        public AudioMixerGroup mixer;
        public string volumeName;
    }

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Group master, music, ambience, sound, voice;
    [SerializeField] private float maxVolume;

    private void OnValidate() {
        Update();
    }

    public void StartInit() {
        Update();
    }

    public void Update() {

        void SetVolume(Group group, float volume) => mixer.SetFloat(group.volumeName, volume == 0 ? -80f : Mathf.Log10(volume) * maxVolume);

        Debug.Log("update audio manager");

        SetVolume(master,   Settings.masterVolume);
        SetVolume(music,    Settings.musicVolume);
        SetVolume(ambience, Settings.ambienceVolume);
        SetVolume(sound,    Settings.soundVolume);
        SetVolume(voice,    Settings.voiceVolume);
    }
}
