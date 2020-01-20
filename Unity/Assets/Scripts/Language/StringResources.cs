using System;
using System.Collections.Generic;
using Settings;

namespace Language {
    public enum Language {
        English,
        Francais
    }
    
    public class StringResources {
        private static StringResources _instance;
        public static StringResources Instance {
            get {
                if(_instance == null) _instance = new StringResources();
                return _instance;
            }
        }
        
        public Dictionary<string, List<KeyValuePair<Language, string>>> Dictionary
            = new Dictionary<string, List<KeyValuePair<Language, string>>> {
                    {"ok", new List<KeyValuePair<Language, string>>()}
                };

        public string Translate(string value) {
            foreach (var entry in Dictionary) {
                foreach (var lang in entry.Value) {
                    if (lang.Value == value) {
                        return GetString(value, GameSettings.Instance.language);
                    }
                }
            }
            return "Undefined";
        }

        public string GetString(string value, Language lang) {
            var entry = Dictionary[value];
            foreach (var subEntry in entry) {
                if (subEntry.Key == lang)
                    return subEntry.Value;
            }
            return "Undefined";
        }

        public void AddEntry() {
            var entry = new List<KeyValuePair<Language, string>>();
            entry.Add(new KeyValuePair<Language, string>(Language.English, ""));
            entry.Add(new KeyValuePair<Language, string>(Language.Francais, ""));
            Dictionary.Add("", entry);
        }
    }
}
