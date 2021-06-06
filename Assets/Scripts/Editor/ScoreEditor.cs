using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScoreEditor : EditorWindow
{
    [SerializeField] private GameConfiguration _configurationAsset;

    SerializedObject _serializedConfig;
    SerializedProperty _serializedScores;
    private List<bool> itemFoldoutRef;

    [MenuItem("Game Editor/Score Editor")]
    static void Configuration()
    {
        ScoreEditor window = (ScoreEditor)GetWindow(typeof(ScoreEditor), true, "Score Editor");
        window.Show();

    }

    private void OnEnable()
    {
        _serializedConfig = new SerializedObject(_configurationAsset);
        _serializedScores = _serializedConfig.FindProperty("Scores");
        itemFoldoutRef = new List<bool>(_serializedScores.arraySize);
    }


    private void OnGUI()
    {
        _serializedConfig.Update();

        for(int i = 0; i < _serializedScores.arraySize; i++)
        {
            VerifyFoldoutList(itemFoldoutRef, i);
            SerializedProperty itemRef = _serializedScores.GetArrayElementAtIndex(i);
            SerializedProperty itemName = itemRef.FindPropertyRelative("Name");
            itemFoldoutRef[i] = EditorGUILayout.Foldout(itemFoldoutRef[i], itemName.stringValue, true);

            if (itemFoldoutRef[i])
            {
                DrawItem(i, itemRef);
            }
            EditorGUILayout.Separator();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Add New"))
        {
            _configurationAsset.Scores.Add(new Score());
            itemFoldoutRef.Add(false);
        }

        _serializedConfig.ApplyModifiedProperties();

    }


    private void DrawItem(int index, SerializedProperty itemRef)
    {
        SerializedProperty itemName = itemRef.FindPropertyRelative("Name");
        SerializedProperty itemScoreType = itemRef.FindPropertyRelative("ScoreType");
        SerializedProperty itemAmount = itemRef.FindPropertyRelative("Amount");


        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(itemName);
        itemScoreType.intValue = (int)(ScoreType)EditorGUILayout.EnumPopup("Score Type", (ScoreType)itemScoreType.intValue);
        EditorGUILayout.PropertyField(itemAmount);


        if (GUILayout.Button("Remove " + itemName.stringValue))
        {
            RemoveItem(index);
        }
        EditorGUI.indentLevel--;
    }

    void RemoveItem(int index)
    {
        _serializedScores.DeleteArrayElementAtIndex(index);
        itemFoldoutRef.RemoveAt(index);
    }

    public static void VerifyFoldoutList(in List<bool> pList, int pCurrentIndex)
    {
        while (pList.Count <= pCurrentIndex)
        {
            pList.Add(false);
        }
    }

}


