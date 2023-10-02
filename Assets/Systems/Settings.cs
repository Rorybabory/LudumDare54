using UnityEngine;
using System.Collections.Generic;

public static class Settings {

    private const string KeyPrefix = "Settings_";
    private static string Key(string key) => string.Concat(KeyPrefix, key);

    private static AudioManager _audioManager;
    private static AudioManager AudioManager => _audioManager == null ? _audioManager = Resources.Load<AudioManager>("Audio Manager") : _audioManager;

    private static void SetResolution(this Resolution resolution) => Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

    public static readonly Entry

    /*  name                                                                default value   */

    // controls
        cameraSensitivityX  = new Entry(Key(nameof(cameraSensitivityX)),    6),     // number
        cameraSensitivityY  = new Entry(Key(nameof(cameraSensitivityY)),    6),     // number

    // gameplay
        fieldOfView         = new Entry(Key(nameof(fieldOfView)),           90),    // number

    // graphics
        frameRate           = new Entry(Key(nameof(frameRate)),             60)     // number
            .CustomSet(value => Application.targetFrameRate = value),
        vSync               = new Entry(Key(nameof(vSync)),                 0)      // number
            .CustomSet(value => QualitySettings.vSyncCount = value),
        fullscreen          = new Entry(Key(nameof(fullscreen)),            1)      // bool
            .CustomSet(value => Screen.fullScreen = value),
        resolution          = new Entry(Key(nameof(resolution)),            0)      // enum
            .DefaultValueGetter(() => Mathf.Max(0, new List<Resolution>(Screen.resolutions).FindIndex(r => r.width == 1920)))
            .CustomSet(value => Screen.resolutions[value].SetResolution()),

    // audio
        musicVolume         = new Entry(Key(nameof(musicVolume)),           0.5f)   // number
            .CustomSet(updateAudio),
        ambienceVolume      = new Entry(Key(nameof(ambienceVolume)),        0.5f)   // number
            .CustomSet(updateAudio),
        soundVolume         = new Entry(Key(nameof(soundVolume)),           0.5f)   // number
            .CustomSet(updateAudio),
        voiceVolume         = new Entry(Key(nameof(voiceVolume)),           0.5f)   // number
            .CustomSet(updateAudio),
        masterVolume        = new Entry(Key(nameof(masterVolume)),          0.5f)   // number
            .CustomSet(updateAudio);

    private static System.Action<Entry> updateAudio => value => AudioManager.Update();

    [RuntimeInitializeOnLoadMethod]
    private static void ApplyCustomSetSettings() {
        foreach (var setting in new[] {
            frameRate,
            vSync,
            resolution,
            fullscreen,
            musicVolume,
            soundVolume,
            masterVolume,
        })
            setting.ApplyCustomSet();
    }

    public class Entry {

        private readonly string key;
        private readonly float defaultValue;
        private System.Action<Entry> customSet = null;
        private System.Func<float> defaultValueGetter = null;

        public Entry(string name, float defaultValue)
            => (this.key, this.defaultValue, _value, this.customSet) = (name, defaultValue, null, null);

        public Entry CustomSet(System.Action<Entry> customSet) {
            this.customSet = customSet;
            return this;
        }

        public Entry DefaultValueGetter(System.Func<float> defaultValueGetter) {
            this.defaultValueGetter = defaultValueGetter;
            return this;
        }

        private float? _value;
        public float value {
            get => _value ??= PlayerPrefs.GetFloat(key, defaultValueGetter != null ? defaultValueGetter.Invoke() : defaultValue);
            set {
                PlayerPrefs.SetFloat(key, (float)(_value = value));
                customSet?.Invoke(this);
            }
        }

        public void ApplyCustomSet() {
            _ = value;
            customSet?.Invoke(this);
        }

        public static implicit operator float   (Entry entry) => entry.value;
        public static implicit operator int     (Entry entry) => (int)entry.value;
        public static implicit operator bool    (Entry entry) => entry.value == 1;
    }
}
