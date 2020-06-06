using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace WebClient {
    public class ConnectModule {
        private static ConnectModule _instance;  
        public static ConnectModule Instance {
            get {
                if(_instance == null) _instance = new ConnectModule();
                return _instance;
            }
        }

        /**
         * Connects user with given login & password
         */
        public IEnumerator ConnectUser(string login, string password, Action<UnityWebRequest> callback) {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection> {
                new MultipartFormDataSection("mail", login),
                new MultipartFormDataSection("psw", password)
            };
            UnityWebRequest www = UnityWebRequest.Post("http://piwelengine.eu/sauron/WS/LOGIN.php", formData);
            yield return www.SendWebRequest();
            callback(www);
        }

        /**
         * Validates given token
         */
        public IEnumerator ValidToken(string token, Action<UnityWebRequest> callback) {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection> {
                new MultipartFormDataSection("token", token),
            };
            UnityWebRequest www = UnityWebRequest.Post("http://piwelengine.eu/sauron/WS/VALID_TOKEN.php", formData);
            yield return www.SendWebRequest();
            callback(www);
        }
        
        public IEnumerator BestScores( Action<UnityWebRequest> callback) {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection> {
            };
            UnityWebRequest www = UnityWebRequest.Post("http://piwelengine.eu/sauron/WS/BEST_SCORES.php", formData);
            yield return www.SendWebRequest();
            callback(www);
        }

        /**
         * Registers new user with given data
         */
        public IEnumerator RegisterUser(string firstname, string lastname, string mail, string password, Action<UnityWebRequest> callback) {
            // TODO VALUES CHECK
            List<IMultipartFormSection> formData = new List<IMultipartFormSection> {
                new MultipartFormDataSection("firstname", firstname),
                new MultipartFormDataSection("lastname", lastname),
                new MultipartFormDataSection("mail", mail),
                new MultipartFormDataSection("psw", password)
            };
            UnityWebRequest www = UnityWebRequest.Post("http://piwelengine.eu/sauron/WS/INSCRIPTION.php", formData);
            yield return www.SendWebRequest();
            callback(www);
            //{firstname:"xxx",lastname:"xxx",mail:"aaa@bbb.fr","psw":1234,psw2:"1234"
        }
    }
}
