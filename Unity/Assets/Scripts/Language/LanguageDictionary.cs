using System;
using System.Collections.Generic;
using UnityEngine;

namespace Language {
    public enum Language {
        English,
        Francais
    }

    [Serializable]
    public class LanguageStringPair {
        public Language key;
        public string value;

        public LanguageStringPair(Language lang, string val) {
            key = lang;
            value = val;
        }
    }

    [Serializable]
    public class StringLanguagesPair {
        public string key;
        public List<LanguageStringPair> values;

        public string GetTranslation(Language lang) {
            int listLen = values.Count;
            for (int i = 0; i < listLen; i++)
                if (values[i].key == lang)
                    return values[i].value;
            return key;
        }
    }
    
    public class LanguageDictionary : ScriptableObject {
        public List<StringLanguagesPair> dictionary = new List<StringLanguagesPair>();

        public string GetString(string key, Language lang) {
            int listLen = dictionary.Count;
            for (int i = 0; i < listLen; i++) {
                if (dictionary[i].key == key)
                    return dictionary[i].GetTranslation(lang);
            }
            return key;
        }

        private bool Contains(string key) {
            int listLen = dictionary.Count;
            for (int i = 0; i < listLen; i++)
                if (dictionary[i].key == key)
                    return true;
            return false;
        }

        public void AddEntry(string key = "") {
            if (Contains(key)) return;
            StringLanguagesPair entry = new StringLanguagesPair {
                key = key, values = new List<LanguageStringPair>()
            };
            Array langArray = Enum.GetValues(typeof(Language));
            int langLen = langArray.Length;
            for(int i = 0; i < langLen; i++)
                entry.values.Add(new LanguageStringPair((Language) langArray.GetValue(i), ""));
            dictionary.Add(entry);
        }

        public void DeleteEntry(string key) {
            int listLen = dictionary.Count;
            for (int i = 0; i < listLen; i++)
                if (dictionary[i].key == key) {
                    dictionary.RemoveAt(i);
                    return;
                }
        }

        public void ModifyKey(string key, string newKey) {
            if (Contains(newKey)) return;
            int listLen = dictionary.Count;
            for (int i = 0; i < listLen; i++)
                if (dictionary[i].key == key)
                    dictionary[i].key = newKey;
        }
    }
}
