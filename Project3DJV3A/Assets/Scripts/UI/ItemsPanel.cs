using Units;
using UnityEngine;

namespace UI {
    public class ItemsPanel : MonoBehaviour {
        public SystemUnit systemUnit;

        public GameObject unitPanel;
        public GameObject itemsPanel;
        public GameObject equipmentsPanel;

        public Transform itemsParent;
        public Transform equipmentsParent;
        public Transform unitsParent;

        public void ShowUnitsPanel() {
            itemsPanel.SetActive(false);
            equipmentsPanel.SetActive(false);
            unitPanel.SetActive(true);
        }

        public void ShowItemsPanel() {
            equipmentsPanel.SetActive(false);
            unitPanel.SetActive(false);
            itemsPanel.SetActive(true);
        }

        public void ShowEquipmentsPanel() {
            itemsPanel.SetActive(false);
            unitPanel.SetActive(false);
            equipmentsPanel.SetActive(true);
        }
    }
}