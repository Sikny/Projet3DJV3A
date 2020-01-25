using System;
using Language;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor {
    public class LanguageDictionaryWindow : EditorWindow {
        private Array _languagesList;
        private LanguageDictionary _languageDictionary;

        private readonly float[] _columnsWidth = {
            0.3f, 0.1f
        };

        private readonly GUIStyle _arrayStyle = new GUIStyle();
        private readonly GUIStyle _headerStyle = new GUIStyle();
        private GUIStyle _linesStyle = new GUIStyle();
        private GUIStyle _singleLStyle = new GUIStyle();

        [MenuItem("Window/Languages Editor")]
        public static void ShowWindow() {
            GetWindow<LanguageDictionaryWindow>("Languages Editor")
                .Show();
        }

        private void OnEnable() {
            _languagesList = Enum.GetValues(typeof(Language.Language));
            _languageDictionary = LanguageDictionary.Instance;
            
            _arrayStyle.margin = new RectOffset(5, 5, 0, 0);
            
            Texture2D arrayBg = new Texture2D(1, 1);
            arrayBg.SetPixel(0, 0, Color.gray);
            arrayBg.Apply();
            _headerStyle.normal.background = arrayBg;
            _headerStyle.margin = new RectOffset(0, 0, 0, 2);

            _linesStyle.normal.background = arrayBg;
            _linesStyle.wordWrap = false;
            
            Texture2D lineBg = new Texture2D(1, 1);
            lineBg.SetPixel(0, 0, Color.white);
            arrayBg.Apply();
            _singleLStyle.normal.background = lineBg;
            _singleLStyle.margin = new RectOffset(2, 2, 2, 2);
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
            GUILayout.BeginVertical(_linesStyle);
            foreach (var entry in _languageDictionary.dictionary) {
                // one line (one dictionary entry)
                GUILayout.BeginVertical(_singleLStyle);
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUILayout.Label(entry.Key, GUILayout.Width(position.width*_columnsWidth[0]));
                if (GUILayout.Button("Edit")) {
                    LanguageDictionaryEditPopup.EditKey(entry.Key);
                    break;
                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                var entryLen = entry.Value.Count;
                for (var i = 0; i < entryLen; i++) {
                    // one language
                    var lang = entry.Value[i];
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.Key.ToString(), GUILayout.MaxWidth(position.width*_columnsWidth[1]), 
                        GUILayout.MinWidth(60f));
                    lang.Value = GUILayout.TextArea(lang.Value);
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Remove")) {
                    _languageDictionary.DeleteEntry(entry.Key);
                    break;
                }
                GUILayout.EndVertical();
            }
            if (GUILayout.Button("Add")) {
                _languageDictionary.AddEntry();
            }
            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }
    }
}
