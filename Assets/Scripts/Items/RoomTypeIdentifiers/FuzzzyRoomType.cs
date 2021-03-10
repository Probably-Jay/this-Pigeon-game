using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Created Jay 07/02
/// 

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
[CustomEditor(typeof(FuzzzyRoomType),true)]
public class FuzzzyRoomTypeUI : Editor
{

    SerializedProperty vec;
    SerializedProperty values;
    //SerializedProperty normVec;
    //SerializedProperty normVals;

    private void OnEnable()
    {
        vec = serializedObject.FindProperty(nameof(FuzzzyRoomType.rawValues));
        values = vec.FindPropertyRelative(nameof(VectorN.values));
        //normVec = serializedObject.FindProperty(nameof(FuzzzyRoomType.normalisedValues));
        //normVals = normVec.FindPropertyRelative(nameof(VectorN.values));

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var enumVals = System.Enum.GetValues(typeof(RoomData.RoomType));

        float[] newVals = new float[enumVals.Length];
        for (int i = 1; i < values.arraySize; i++ )
        {
            EditorGUILayout.BeginHorizontal();
            var elementProperty = values.GetArrayElementAtIndex(i);
            newVals[i] = EditorGUILayout.Slider( new GUIContent(((RoomData.RoomType)i).ToString()),elementProperty.floatValue,0,1);
            EditorGUILayout.EndHorizontal();
        }

        for (int i = 0; i < enumVals.Length; i++ )
        {
            values.GetArrayElementAtIndex(i).floatValue = newVals[i];
        }
       

        serializedObject.ApplyModifiedProperties();


        //if (GUILayout.Button("Refresh"))
        //{
        //    normVals.GetArrayElementAtIndex(0).floatValue = 0;
        //    for (int i = 1; i < values.arraySize; i++)
        //    {
        //        normVals.GetArrayElementAtIndex(i).floatValue = values.GetArrayElementAtIndex(i).floatValue;
        //    }
        //    bool b = serializedObject.hasModifiedProperties;
        //    serializedObject.ApplyModifiedProperties();
        //}
        //GUI.enabled = false;
        //EditorGUILayout.LabelField("Normalised Values:");
        //EditorGUILayout.BeginHorizontal();
        //for (int i = 1; i < enumVals.Length; i++)
        //{
        //   EditorGUILayout.DelayedFloatField(normVals.GetArrayElementAtIndex(i).floatValue);
        //}
        //EditorGUILayout.EndHorizontal();
        //GUI.enabled = true;

    }

}


#endif



[RequireComponent(typeof(PlantItem))]
public class FuzzzyRoomType : MonoBehaviour
{
   [HideInInspector] public VectorN rawValues = new VectorN(System.Enum.GetNames(typeof(RoomData.RoomType)).Length);


}

