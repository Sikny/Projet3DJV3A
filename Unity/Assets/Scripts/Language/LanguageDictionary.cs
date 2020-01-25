using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace Language {
    public enum Language {
        English,
        Francais
    }

    public class LanguageStringPair {
        public Language Key;
        public string Value;

        public LanguageStringPair(Language lang, string val) {
            Key = lang;
            Value = val;
        }
    }
    
    public class LanguageDictionary : ScriptableObject {
        private static LanguageDictionary _instance;
        public static LanguageDictionary Instance {
            get {
                if(_instance == null) _instance = CreateInstance<LanguageDictionary>();
                return _instance;
            }
        }
        
        public Dictionary<string, List<LanguageStringPair>> dictionary
            = new Dictionary<string, List<LanguageStringPair>>();

        public string Translate(string value) {
            foreach (var entry in dictionary) {
                foreach (var lang in entry.Value) {
                    if (lang.Value == value) {
                        return GetString(value, GameSettings.Instance.language);
                    }
                }
            }
            return "Undefined";
        }

        public string GetString(string value, Language lang) {
            var entry = dictionary[value];
            foreach (var subEntry in entry) {
                if (subEntry.Key == lang)
                    return subEntry.Value;
            }
            return "Undefined";
        }

        public string GetString(string value) {
            return GetString(value, GameSettings.Instance.language);
        }

        public void AddEntry(string key = "") {
            var entry = new List<LanguageStringPair>();
            entry.Add(new LanguageStringPair(Language.English, ""));
            entry.Add(new LanguageStringPair(Language.Francais, ""));
            dictionary.Add(key, entry);
        }

        public void DeleteEntry(string value) {
            dictionary.Remove(value);
        }

        public void ModifyKey(string key, string newKey) {
            if (key == newKey) return;
            AddEntry(newKey);
            dictionary[newKey] = dictionary[key];
            dictionary.Remove(key);
        }
    }
}
