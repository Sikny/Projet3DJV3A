using System;
using Language;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor {
    public class LanguageDictionary : EditorWindow {
        private MultiColumnHeader _columnHeader;
        private MultiColumnHeaderState.Column[] _columns;
        
        [MenuItem("Window/Languages Editor")]
        public static void ShowWindow() {
            GetWindow<LanguageDictionary>("Languages Editor")
                .Show();
        }

        private void OnEnable() {
            _columns = new MultiColumnHeaderState.Column[] {
                new MultiColumnHeaderState.Column() {
                    headerContent = new GUIContent("Entry"),
                    width = 200,
                    minWidth = 100,
                    maxWidth = 300,
                    autoResize = true,
                    sortingArrowAlignment = TextAlignment.Right
                },
                new MultiColumnHeaderState.Column() {
                    headerContent = new GUIContent("Values"),
                    width = 200,
                    minWidth = 100,
                    maxWidth = 300,
                    autoResize = true,
                    sortingArrowAlignment = TextAlignment.Right
                },
            };
            _columnHeader = new MultiColumnHeader(new MultiColumnHeaderState(_columns));
            _columnHeader.height = 20;
            _columnHeader.ResizeToFit();
        }

        void OnGUI() {
            GUILayout.Label("Text entries", EditorStyles.boldLabel);
            
            GUILayout.FlexibleSpace();
            var windowVisibleRect = GUILayoutUtility.GetLastRect();
            windowVisibleRect.width = position.width;
            windowVisibleRect.height = position.height;
            
            var headerRect = windowVisibleRect;
            headerRect.height = _columnHeader.height;
            float xScroll = 0;
            _columnHeader.OnGUI(headerRect, xScroll);

            for (int i = 0; i < _columns.Length; i++) {
                var contentRect = _columnHeader.GetColumnRect(i);
                contentRect.x -= xScroll;
                contentRect.y = contentRect.yMax;
                contentRect.yMax = windowVisibleRect.yMax;
            }
            
            foreach (var entry in StringResources.Instance.Dictionary) {
                
                EditorGUILayout.TextField(entry.Key);
            }
        }
    }
}
