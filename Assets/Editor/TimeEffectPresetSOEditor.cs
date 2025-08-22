//#if UNITY_EDITOR
//using UnityEditor;
//using UnityEditorInternal;
//using UnityEngine;

//[CustomEditor(typeof(TimeEffectPresetSO))]
//public class TimeEffectPresetSOEditor : Editor
//{
//    private ReorderableList list;

//    private void OnEnable()
//    {
//        var so = serializedObject;
//        var prop = so.FindProperty("effectPresets");

//        list = new ReorderableList(so, prop, true, true, false, false);
//    }

//    public override void OnInspectorGUI()
//    {
//        base.DrawDefaultInspector();
//        //serializedObject.Update();
//        list.DoLayoutList();
//        //serializedObject.ApplyModifiedProperties();
//    }
//}
//#endif
