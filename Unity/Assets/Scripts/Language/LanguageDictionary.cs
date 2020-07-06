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
    public class LanguageDictionaryEntry {
        public List<LanguageStringPair> values;

        public string GetTranslation(Language lang) {
            int listLen = values.Count;
            for (int i = 0; i < listLen; i++)
                if (values[i].key == lang)
                    return values[i].value;
            return "";
        }

        public bool Contains(string value) {
            int listLen = values.Count;
            for(int i = 0; i < listLen; i++)
                if (values[i].value == value)
                    return true;
            return false;
        }
    }
    
    public class LanguageDictionary : ScriptableObject {
        public List<LanguageDictionaryEntry> dictionary = new List<LanguageDictionaryEntry>();

        public string SearchAndTraduce(string value, Language lang) {
            int listLen = dictionary.Count;
            for (int i = 0; i < listLen; i++) {
                if (!dictionary[i].Contains(value)) continue;
                return dictionary[i].GetTranslation(lang);
            }
            return value;
        }

        public void AddEntry() {
            LanguageDictionaryEntry entry = new LanguageDictionaryEntry() {
                values = new List<LanguageStringPair>()
            };
            Array langArray = Enum.GetValues(typeof(Language));
            int langLen = langArray.Length;
            for(int i = 0; i < langLen; i++)
                entry.values.Add(new LanguageStringPair((Language) langArray.GetValue(i), ""));
            dictionary.Add(entry);
        }

        public void DeleteEntry(int index) {
            if (index > dictionary.Count - 1)
                return;
            dictionary.RemoveAt(index);
        }
    }
}
