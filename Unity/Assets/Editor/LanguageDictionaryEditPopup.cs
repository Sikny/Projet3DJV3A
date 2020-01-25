using Language;
using UnityEditor;
using UnityEngine;

namespace Editor {
    public class LanguageDictionaryEditPopup : EditorWindow {
        private static string _value, _oldValue;
        public static void EditKey(string value) {
            LanguageDictionaryEditPopup window = CreateInstance<LanguageDictionaryEditPopup>();
            _oldValue = value;
            _value = value;
            Rect r = window.position;
            Vector2 position = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            r.x = position.x;
            r.y = position.y;
            r.width = 200f;
            r.height = 50f;
            window.position = r;
            window.ShowPopup();
        }
        
        void OnGUI()
        {
            _value = GUILayout.TextField(_value);
            if (GUILayout.Button("Ok")) {
                LanguageDictionary.Instance.ModifyKey(_oldValue, _value);
                Close();
            }
        }
    }
}
