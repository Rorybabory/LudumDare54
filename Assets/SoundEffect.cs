using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New Sound Effect", menuName = "Sound Effect")]
public class SoundEffect : ScriptableObject{

    public AudioClip[] clip;
    public AudioMixerGroup mixerGroup;
    [Header("Parameters")]
    public float volume = 1;
    public float minPitch = 1, maxPitch = 1;
    public bool
        sequential = false,
        overlap = true,
        loop = false;

    internal AudioSource source;

    private int clipIndex;
    private float soundTimeRemaining;

    /// <summary> Ensures host has an AudioSource (Must be called in Start) </summary>
    public void Init(GameObject host) {

        if (clip.Length == 0) {
            Debug.LogError($"Sound Effect on \"{host.name}\" doesn't haven't any audio clips!");
            return;
        }

        source = host.AddComponent<AudioSource>();

        source.outputAudioMixerGroup = mixerGroup;
        source.loop = loop;
    }

    /// <summary> Play the sound effect. </summary>
    public void Play() {

        // return if overlapping matters
        if (!overlap && source.isPlaying) return;

        // get clip
        AudioClip currentClip;
        if (sequential) {
            currentClip = clip[clipIndex % clip.Length];
            clipIndex++;
        } else currentClip = clip[Random.Range(0, clip.Length)];

        // play sound
        source.pitch = Random.Range(minPitch, maxPitch);
        source.volume = volume;

        if (loop) {
            source.clip = currentClip;
            source.Play();
        }
        else {
            source.PlayOneShot(currentClip);
            soundTimeRemaining = Time.time + currentClip.length;
        }
    }

    /// <summary> Stop the sound effect. </summary>
    public void Stop() {
        source.Stop();
        soundTimeRemaining = 0;
    }
}
