using System;
using Language;
using UnityEditor;
using UnityEngine;

namespace Editor {
    /**
     * <summary>Editor Window for language dictionary (for traducing)</summary>
     */
    public class LanguageDictionaryWindow : EditorWindow {
        private Array _languagesList;
        private LanguageDictionary _languageDictionary;

        private readonly float[] _columnsWidth = {
            0.3f, 0.1f
        };

        private Vector2 _scrollPosition;

        private readonly GUIStyle _arrayStyle = new GUIStyle();
        private readonly GUIStyle _headerStyle = new GUIStyle();
        private readonly GUIStyle _linesStyle = new GUIStyle();
        private readonly GUIStyle _singleLStyle = new GUIStyle();

        [MenuItem("Window/Languages Editor")]
        public static void ShowWindow() {
            LanguageDictionaryWindow window = GetWindow<LanguageDictionaryWindow>("Languages Editor");
            window.GetScriptableObject();
            window.Show();
        }
        private void OnEnable() {
            GetScriptableObject();
            _languagesList = Enum.GetValues(typeof(Language.Language));
            
            _arrayStyle.margin = new RectOffset(5, 5, 0, 0);
            
            Texture2D arrayBg = new Texture2D(1, 1);
            arrayBg.SetPixel(0, 0, Color.gray);
            arrayBg.Apply();
            _headerStyle.normal.background = arrayBg;
            _headerStyle.margin = new RectOffset(0, 0, 0, 2);

            _linesStyle.normal.background = arrayBg;
            _linesStyle.wordWrap = false;
            
            Texture2D lineBg = new Texture2D(1, 1);
            //lineBg.SetPixel(0, 0, Color.white);
            lineBg.Apply();
            _singleLStyle.normal.background = lineBg;
            _singleLStyle.margin = new RectOffset(2, 2, 2, 2);
        }

        void GetScriptableObject() {
            _languageDictionary = Resources.Load<LanguageDictionary>("Data/LanguagesDictionary");
        }

        void OnGUI() {
            GUILayout.Space(10);
            
            // Languages list
            GUILayout.BeginHorizontal();
            GUILayout.Label("Languages", EditorStyles.boldLabel, GUILayout.Width(position.width*_columnsWidth[0]));

            var guiContent = "";
            var languagesCount = _languagesList.Length;
            for (int i = 0; i < languagesCount; i++) {
                guiContent += _languagesList.GetValue(i);
                if (i < languagesCount - 1)
                    guiContent += ", ";
            }
            GUILayout.Label(guiContent);
            GUILayout.EndHorizontal();
            
            GUILayout.Space(10);
            GUILayout.Label("Text entries", EditorStyles.boldLabel);

            // Dictionary content
            GUILayout.BeginVertical(_arrayStyle);
            // header
            GUILayout.BeginHorizontal(_headerStyle);
                GUILayout.Label("Key", GUILayout.Width(position.width*_columnsWidth[0]));
                GUILayout.Label("Value");
            GUILayout.EndHorizontal();
            // array
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            GUILayout.BeginVertical(_linesStyle);
            int index = 0;
            foreach (var entry in _languageDictionary.dictionary) {
                // one line (one dictionary entry)
                GUILayout.BeginVertical(_singleLStyle);
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(GUILayout.Width(position.width*_columnsWidth[0]));
                GUILayout.Label(""+index);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                var entryLen = entry.values.Count;
                for (var i = 0; i < entryLen; i++) {
                    // one language
                    var lang = entry.values[i];
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.key.ToString(),
                        GUILayout.MaxWidth(position.width*_columnsWidth[1]), GUILayout.MinWidth(60f));
                    lang.value = GUILayout.TextArea(lang.value);
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                Color defaultBg = GUI.backgroundColor;
                GUI.backgroundColor = Color.red;
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("-", GUILayout.Width(50f))) {
                    _languageDictionary.DeleteEntry(index);
                    break;
                }
                GUILayout.EndHorizontal();
                GUI.backgroundColor = defaultBg;
                GUILayout.EndVertical();    // line end
                index++;
            }
            if (GUILayout.Button("Add")) {
                _languageDictionary.AddEntry();
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            if (GUI.changed) {
                Undo.RecordObject(_languageDictionary, "Dictionary edited");
                EditorUtility.SetDirty(_languageDictionary);
            }
        }
    }
}
