using Language;
using UnityEditor;
using UnityEngine;

namespace Editor {
    public class LanguageDictionaryEditPopup : EditorWindow {
        private static string _value, _oldValue;
        private static int _type;
        private static LanguageDictionary _languageDictionary;
        public static void Init(string value, int type) {
            LanguageDictionaryEditPopup window = CreateInstance<LanguageDictionaryEditPopup>();
            
            _languageDictionary = Resources.Load<LanguageDictionary>("Data/LanguagesDictionary");
            
            _oldValue = value;
            _value = value;
            _type = type;
            Rect r = window.position;
            Vector2 position = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            r.x = position.x;
            r.y = position.y;
            r.width = 200f;
            r.height = 45f;
            window.position = r;
            window.ShowPopup();
        }
        
        void OnGUI()
        {
            _value = GUILayout.TextField(_value);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Ok")) {
                if (_type == 0) {
                    // adding new entry
                    _languageDictionary.AddEntry(_value);
                } else if (_type == 1) {
                    // modifying entry
                    _languageDictionary.ModifyKey(_oldValue, _value);
                }
                Close();
            }
            if (GUILayout.Button("Cancel")) {
                Close();
            }
            GUILayout.EndHorizontal();
        }
    }
}
