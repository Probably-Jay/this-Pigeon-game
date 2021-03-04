using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Created Jay 05/02


// custom editor for the re-orderable list
#region Custom Editor 
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
[CustomEditor(typeof(InventoryList))]
public class InventoryListEditor : Editor
{
    SerializedProperty inventory;
    ReorderableList list;

    private void OnEnable()
    {
        inventory = serializedObject.FindProperty(nameof(InventoryList.list));

        list = new ReorderableList(
            serializedObject: serializedObject
            , elements: inventory
            , draggable: true
            , displayHeader: true
            , displayAddButton: true
            , displayRemoveButton: true
            );

        list.drawElementCallback = DrawListItems; 
        list.drawHeaderCallback = DrawHeader; 

    }

    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused) // draws each element
    {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index); // element in list

        element.objectReferenceValue = EditorGUI.ObjectField(
            new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight)
            , new GUIContent ($"Item {index}: {(element.objectReferenceValue != null ? ((InventoryItem)element.objectReferenceValue).ToString() : "(Empty)") }")
            , element.objectReferenceValue
            , typeof(InventoryItem)
            , false
        );

    }

    void DrawHeader(Rect rect) // the title of the list
    {
        EditorGUI.LabelField(rect, "List of inventory objects");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        list.DoLayoutList(); // draw the list

        serializedObject.ApplyModifiedProperties();
    }


}
#endif
#endregion


[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/InventoryList", order = 1)]
public class InventoryList : ScriptableObject
{
    public List<InventoryItem> list;
}
