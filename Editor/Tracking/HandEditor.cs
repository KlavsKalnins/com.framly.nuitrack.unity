using UnityEditor;
using UnityEngine;

namespace Framly.Nuitrack
{
    [CustomEditor(typeof(Hand))]
    public class HandEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!Application.isPlaying)
            {
                GUI.enabled = false;
                return;
            }
            if (GUILayout.Button($"Toggle Interactability: {Hand.isHandTracking}"))
            {
                Hand script = (Hand)target;
                script.ToggleInteractive(!Hand.isHandTracking);
            }
        }
    }
}