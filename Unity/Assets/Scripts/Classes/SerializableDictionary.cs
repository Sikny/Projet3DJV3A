using System;
using System.Collections.Generic;

namespace Classes {
    [Serializable]
    public class SerializableLangStrPair {
        public Language.Language key;
        public string value;
    }

    [Serializable]
    public class SerializableLanguagePair {
        public string key;
        public List<SerializableLangStrPair> value;
    }
}
