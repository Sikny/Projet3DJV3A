using Game;
using UnityEditor;
using UnityEngine;

namespace Utility {
    public class PlayerTools : MonoBehaviour{
        [MenuItem("Tools/ClearPlayerData")]
        public static void ClearPlayerData() {
            PlayerPrefs.DeleteAll();
            Inventory inventory = Resources.FindObjectsOfTypeAll<Inventory>()[0];
            inventory.Clear();
        }
    }
}
