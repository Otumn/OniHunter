using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(ColorSwap))]
[ExecuteInEditMode]
public class ColorSwapEditor : Editor
{
    ColorSwap t;
    SerializedObject GetTarget;
    SerializedProperty ThisList;
    int ListSize;

    void OnEnable()
    {
        t = (ColorSwap)target;
        GetTarget = new SerializedObject(t);
        ThisList = GetTarget.FindProperty("ColorSwapList"); // Référence à la liste du script
    }

    public override void OnInspectorGUI()
    {
        GetTarget.Update();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Number of colors to swap :");
        ListSize = ThisList.arraySize;
        ListSize = EditorGUILayout.IntField("Color List size", ListSize);

        if (ListSize != ThisList.arraySize)
        {
            while (ListSize > ThisList.arraySize)
            {
                ThisList.InsertArrayElementAtIndex(ThisList.arraySize);
            }
            while (ListSize < ThisList.arraySize)
            {
                ThisList.DeleteArrayElementAtIndex(ThisList.arraySize - 1);
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Or");
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Acquire the renderer Material :");
        if (GUILayout.Button("Acquire Material"))
        {
            if(t.mat != null)
            {
                Debug.Log("Material already assigned!");
            }

            else
            {
                Debug.Log("Material acquired!");
                t.AcquireMaterial();
            }
        }


        EditorGUILayout.Space(); 
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        for (int i = 0; i < ThisList.arraySize; i++)
        {
            SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
            SerializedProperty MyTargetColor = MyListRef.FindPropertyRelative("targetColor");
            SerializedProperty MyOutputColor = MyListRef.FindPropertyRelative("outputColor");
            SerializedProperty MyTolerance = MyListRef.FindPropertyRelative("tolerance");

            EditorGUILayout.PropertyField(MyTargetColor);
            EditorGUILayout.PropertyField(MyOutputColor);
            EditorGUILayout.PropertyField(MyTolerance);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Remove this Color Swap :");
            if (GUILayout.Button("Remove Color Swap (Index " + i.ToString() + ")"))
            {
                t.ColorSwapList[i].OutputColor = Color.white;
                t.ColorSwapList[i].TargetColor = Color.white;
                t.ColorSwapList[i].Tolerance = 0.001f;
                t.ColorSwapList.RemoveAt(i);
                ThisList.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();            
        }

        EditorGUILayout.LabelField("Add a new color swap to the list :");
        if (GUILayout.Button("Add New"))
        {
            t.ColorSwapList.Add(new ColorSwap.NewColorSwap());
        }

        GetTarget.ApplyModifiedProperties();
    }
}
