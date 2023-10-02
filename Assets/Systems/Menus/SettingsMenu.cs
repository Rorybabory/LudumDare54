using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SettingsMenu : MonoBehaviour {

    #if UNITY_EDITOR
    [CustomEditor(typeof(SettingsMenu))]
    private class SettingsMenuEditor : Editor {

        public override void OnInspectorGUI() {

            base.OnInspectorGUI();

            var settings = target as SettingsMenu;

            if (GUILayout.Button("Generate UI"))
                settings.GenerateUI();
        }
    }
    #endif

    [SerializeField] private RectTransform scrollViewContent;
    [SerializeField] private GameObject sliderPrefab, togglePrefab, dropdownPrefab, sectionPrefab, spacerPrefab;

    private static readonly IEntry[] entries = new IEntry[] {

        /* type/title                                       setting                         min     max     stepSize    scale   is a percent */

        new Title("Controls"),
        new Number("Horizontal Mouse Sensitivity",          Settings.cameraSensitivityX,    1,      20,     1,          1,      false),
        new Number("Vertical Mouse Sensitivity",            Settings.cameraSensitivityY,    1,      20,     1,          1,      false),
        new Bool("Invert Horizontal Camera Control",         Settings.invertCameraX),
        new Bool("Invert Vertical Camera Control",           Settings.invertCameraY),
        new Bool("Invert Horizontal Movement Control",      Settings.invertMoveX),
        new Bool("Invert Vertical Movement Control",        Settings.invertMoveY),
        new Number("Gear Shift Sensitivity",                Settings.shiftSensitivity,  1,      20,     1,          1,      false),

        new Title("Gameplay"),
        new Number("Field of View",                         Settings.fieldOfView,           30,     150,    5,          1,      false),
        new Number("Camera Tilt Percent",                   Settings.cameraTiltPercent,     0,      2,      0.1f,       100,    true),
        new Number("Camera Shake Percent",                  Settings.cameraShakePercent,    0,      2,      0.1f,       100,    true),
        new Number("Camera Bob Percent",                    Settings.cameraBobPercent,      0,      2,      0.1f,       100,    true),
        new Bool("Show Input Display",                      Settings.showInputDisplay),

        new Title("Audio"),
        new Number("Master Volume",                         Settings.masterVolume,          0,      1,      0.05f,      100,    true),
        new Number("Music Volume",                          Settings.musicVolume,           0,      1,      0.05f,      100,    true),
        new Number("Sound Volume",                          Settings.soundVolume,           0,      1,      0.05f,      100,    true),

        new Title("Graphics"),
        new Number("Target Framerate",                      Settings.frameRate,             10,     200,    10,         1,      false),
        new Number("V-Sync",                                Settings.vSync,                 0,      4,      1,          1,      false),
        new Bool("Show FPS",                                Settings.showFPS),
        new Bool("Fullscreen",                              Settings.fullscreen),
        new Enum("Resolution",                              Settings.resolution, () => new(System.Array.ConvertAll(Screen.resolutions, r => new TMP_Dropdown.OptionData(r.ToString())))),

        new Spacer(),
    };

    private void Awake() {
        GenerateUI();
    }

    private void GenerateUI() {

        while (scrollViewContent.childCount > 0)
            #if UNITY_EDITOR
            DestroyImmediate(scrollViewContent.GetChild(0).gameObject);
            #else
            Destroy(scrollViewContent.GetChild(0).gameObject);
            #endif

        foreach (var entry in entries) {

            var prefab = entry switch {
                Number      => sliderPrefab,
                Bool        => togglePrefab,
                Enum        => dropdownPrefab,
                Title       => sectionPrefab,
                Custom c    => c.getGameObject.Invoke(this),
                Spacer      => spacerPrefab,
                _           => null
            };

            var instantiated =

            #if UNITY_EDITOR
                PrefabUtility.InstantiatePrefab(prefab, scrollViewContent) as GameObject;
            #else
                Instantiate(prefab, scrollViewContent);
            #endif

            entry.Setup(instantiated);
        }
    }

    private interface IEntry {
        public void Setup(GameObject gameObject);
    }

    private class Spacer : IEntry {
        public void Setup(GameObject gameObject) { }
    }

    private class Enum : Value {
        private readonly System.Func<List<TMP_Dropdown.OptionData>> getOptions;
        public Enum(string name, Settings.Entry settingsEntry, System.Func<List<TMP_Dropdown.OptionData>> getOptions) : base(name, settingsEntry)
            => this.getOptions = getOptions;
        public override void Setup(GameObject gameObject) {
            base.Setup(gameObject);
            var dropdown = gameObject.GetComponentInChildren<TMP_Dropdown>();
            dropdown.options = getOptions.Invoke();
            dropdown.value = settingsEntry;
            dropdown.onValueChanged.AddListener(i => settingsEntry.value = i);
        }
    }

    private class Custom : IEntry {
        public readonly System.Func<SettingsMenu, GameObject> getGameObject;
        public Custom(System.Func<SettingsMenu, GameObject> getGameObject) => this.getGameObject = getGameObject;
        public void Setup(GameObject gameObject) { }
    }

    private class Title : IEntry {
        protected readonly string name;
        public Title(string name) => this.name = name;
        public virtual void Setup(GameObject gameObject) => gameObject.GetComponentInChildren<TextMeshProUGUI>().text = name;
    }

    private abstract class Value : Title {
        protected readonly Settings.Entry settingsEntry;
        public Value(string name, Settings.Entry settingsEntry) : base(name) => this.settingsEntry = settingsEntry;
    }

    private class Number : Value {

        private readonly float min, max, displayScale, stepSize;
        private readonly bool percent;

        public Number(string name, Settings.Entry settingsEntry, float min, float max, float stepSize, float displayScale, bool percent) : base(name, settingsEntry)
            => (this.min,   this.max,   this.stepSize,  this.displayScale,  this.percent)
             = (min,        max,        stepSize,       displayScale,       percent);

        public override void Setup(GameObject gameObject) {
            base.Setup(gameObject);

            var number = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];
            var slider = gameObject.GetComponentInChildren<Slider>();

            string percentPostfix = percent ? "%" : "";
            void SetNumber(float value) => number.text = Mathf.RoundToInt(value * stepSize * displayScale).ToString() + percentPostfix;

            float value = settingsEntry.value / stepSize;

            slider.minValue = min / stepSize;
            slider.maxValue = max / stepSize;
            slider.value = value;
            SetNumber(value);

            slider.onValueChanged.AddListener(value => {
                settingsEntry.value = value * stepSize;
                SetNumber(value);
            });
        }
    }

    private class Bool : Value {

        public Bool(string name, Settings.Entry settingsEntry) : base(name, settingsEntry) { }

        public override void Setup(GameObject gameObject) {
            base.Setup(gameObject);

            var toggle = gameObject.GetComponentInChildren<Toggle>();

            toggle.isOn = settingsEntry;
            toggle.onValueChanged.AddListener(value => settingsEntry.value = value ? 1 : 0);
        }
    }
}
