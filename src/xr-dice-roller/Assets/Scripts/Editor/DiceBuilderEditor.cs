using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DiceBuilder))]
public class DiceBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button($"Transfer Values"))
        {
            DiceBuilder diceBuilder = (DiceBuilder)target;
            diceBuilder.TransferValues();
        }
    }
}
