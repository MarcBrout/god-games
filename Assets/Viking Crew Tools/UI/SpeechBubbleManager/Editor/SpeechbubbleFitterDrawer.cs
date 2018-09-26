
using UnityEditor;

namespace VikingCrewTools.UI {
    [CustomEditor(typeof(RatioLayoutFitter))]
    public class SpeechbubbleFitterDrawer : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();
            base.DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();
        }
    }
}