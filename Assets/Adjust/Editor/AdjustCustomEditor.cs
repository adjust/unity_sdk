using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace com.adjust.sdk
{
    [CustomEditor(typeof(Adjust))]
    public class AdjustCustomEditor : Editor
    {
        private Editor settingsEditor;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (settingsEditor == null)
            {
                settingsEditor = CreateEditor(AdjustSettings.Instance);
            }
            settingsEditor.OnInspectorGUI();
        }
    }
}
