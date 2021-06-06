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

        EditorGUILayout.PropertyField(_serializedConfig.FindProperty("WinThreshold"));
        EditorGUILayout.PropertyField(_serializedConfig.FindProperty("SecondsToGameEnd"));
        EditorGUILayout.Separator();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if(GUILayout.Button("Import Data"))
        {
            ImportData();
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        for (int i = 0; i < _serializedScores.arraySize; i++)
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

    private void ImportData()
    {
        _configurationAsset.BuildingData = new List<BuildingNetwork>();
        string sConvertedString = EditorGUIUtility.systemCopyBuffer.Replace("\t", ",");
        string[] sLinesArray = sConvertedString.Split('\n');
        for (int i = 0; i < sLinesArray.Length; i++)
        {
            string[] sStringItem = sLinesArray[i].Split(',');
            if (sStringItem[0].Equals("Building ID")) continue;
            BuildingNetwork pNewItem = new BuildingNetwork();
            pNewItem.id = int.Parse(sStringItem[0].Replace("Building_", ""));
            pNewItem.color = sStringItem[1];
            pNewItem.height = int.Parse(sStringItem[2]);
            pNewItem.maxDamage = int.Parse(sStringItem[3]);
            pNewItem.maxPlant = int.Parse(sStringItem[4]);
            pNewItem.people = int.Parse(sStringItem[5]);
            _configurationAsset.BuildingData.Add(pNewItem);


        }
    }

}


