using System.Collections;
using UnityEngine;

[System.Serializable]
public class SoundEffect {

    public AudioClip[] clip;
    [Header("Parameters")]
    public float volume = 1;
    public float minPitch = 1, maxPitch = 1;
    public bool
        sequential = false,
        overlap = true,
        loop = false;
        //finishPlayingBeforeDestroy = true;

    internal AudioSource source;

    private int clipIndex;
    private float soundTimeRemaining;

    /// <summary> Ensures host has an AudioSource (Must be called in Start) </summary>
    public void Init(GameObject host) {

        if (clip.Length == 0) {
            Debug.LogError($"Sound Effect on \"{host.name}\" doesn't haven't any audio clips!");
            return;
        }

        // generate audio source
        //if (finishPlayingBeforeDestroy) {

        //    string name = $"SoundEffect host for {host.name}";

        //    // source host gameObject
        //    var sourceHostGo = GameObject.Find(name);
        //    if (sourceHostGo == null) sourceHostGo = new GameObject(name);

        //    // source host component
        //    if (!sourceHostGo.TryGetComponent(out SoundEffectSourceHost sourceHost)) sourceHost = sourceHostGo.AddComponent<SoundEffectSourceHost>();

        //    // audio source
        //    source = sourceHost.gameObject.AddComponent<AudioSource>();

        //    // destruction notifier
        //    if (!host.TryGetComponent(out SoundEffectDestructionNotifier destruction)) destruction = host.AddComponent<SoundEffectDestructionNotifier>();
        //    destruction.onDestroy += () => sourceHost.Destroy(soundTimeRemaining);
        //}

        source = host.AddComponent<AudioSource>();

        //source.outputAudioMixerGroup = GameManager.AudioManager.SoundGroup;
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
        } else currentClip = clip[Random.Range(0, clip.Length - 1)];

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

//public class SoundEffectDestructionNotifier : MonoBehaviour {

//    public event System.Action onDestroy;

//    private void OnDestroy() {
//        onDestroy?.Invoke();
//    }
//}

//public class SoundEffectSourceHost : MonoBehaviour {

//    private float timeToDestroy;

//    public void Destroy(float time) {

//        timeToDestroy = Mathf.Max(timeToDestroy, time);
//        name += " - Dying";

//        if (this != null) StartCoroutine(WaitAFrame());
//        IEnumerator WaitAFrame() {
//            yield return null;
//            GameObject.Destroy(gameObject, timeToDestroy - Time.time);
//        }
//    }
//}