using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChallengeEditor : EditorWindow
{
    [SerializeField] private ChallengeConfiguration _configurationAsset;

    SerializedObject _serializedConfig;
    SerializedProperty _serializedChallenges;
    private List<bool> itemFoldoutRef;

    [MenuItem("Challenges/Challenge Editor")]
    static void Configuration()
    {
        ChallengeEditor window = (ChallengeEditor)GetWindow(typeof(ChallengeEditor), true, "ChallengeEditor Editor");
        window.Show();

    }

    private void OnEnable()
    {
        _serializedConfig = new SerializedObject(_configurationAsset);
        _serializedChallenges = _serializedConfig.FindProperty("Challenges");
        itemFoldoutRef = new List<bool>(_serializedChallenges.arraySize);
    }


    private void OnGUI()
    {
        _serializedConfig.Update();

        for(int i = 0; i < _serializedChallenges.arraySize; i++)
        {
            SerializedProperty itemRef = _serializedChallenges.GetArrayElementAtIndex(i);
            DrawItem(i, itemRef);
            EditorGUILayout.Separator();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Add New"))
        {
            _configurationAsset.Challenges.Add(new Challenge());
            itemFoldoutRef.Add(false);
        }

        _serializedConfig.ApplyModifiedProperties();

    }


    private void DrawItem(int index, SerializedProperty itemRef)
    {
        SerializedProperty itemName = itemRef.FindPropertyRelative("Name");
        SerializedProperty itemValue = itemRef.FindPropertyRelative("Value");
        SerializedProperty itemChallengeType = itemRef.FindPropertyRelative("ChallengeType");
        SerializedProperty itemComparisonType = itemRef.FindPropertyRelative("ComparisonType");
        SerializedProperty itemRewardType = itemRef.FindPropertyRelative("RewardType");
        SerializedProperty itemRewardAmount = itemRef.FindPropertyRelative("RewardAmount");

        itemName.stringValue = EditorGUILayout.TextField("Challenge Name: ", itemName.stringValue);
        EditorGUI.indentLevel++;
        itemChallengeType.intValue = (int)(ChallengeType)EditorGUILayout.EnumPopup("Challenge Type", (ChallengeType)itemChallengeType.intValue);
        itemComparisonType.intValue = (int)(ComparisonType)EditorGUILayout.EnumPopup("Comparison Type", (ComparisonType)itemComparisonType.intValue);
        EditorGUILayout.PropertyField(itemValue);
        itemRewardType.intValue = (int)(RewardType)EditorGUILayout.EnumPopup("Reward Type", (RewardType)itemComparisonType.intValue);
        EditorGUILayout.PropertyField(itemRewardAmount);

        if (GUILayout.Button("Remove " + itemName.stringValue))
        {
            RemoveItem(index);
        }
        EditorGUI.indentLevel--;
    }

    void RemoveItem(int index)
    {
        _serializedChallenges.DeleteArrayElementAtIndex(index);
        itemFoldoutRef.RemoveAt(index);
    }


}


