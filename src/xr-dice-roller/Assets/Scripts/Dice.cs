using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public int CurrentValue;
    public float DotValue;
    public int HighestValue;
    public float HighestHeight;
    public Transform[] faces;
    private void Update()
    {
        CurrentValue = GetCurrentValue()+1;
    }

    private int GetCurrentValue()
    {
        int highestIndex = -1;
        float highestHeight = float.MinValue;
        int value = 0;
        float closest = -1f;
        for(int i = 0; i < faces.Length; i++)
        {
            Vector3 direction = (faces[i].position-this.transform.position).normalized;
            float dot = Vector3.Dot(Vector3.up, direction);
            if(dot > closest)
            {
                value = i;
                closest = dot;
            }

            // by height
            if(faces[i].position.y > highestHeight)
            {
                highestHeight = faces[i].position.y;
                highestIndex = i;
            }
        }

        HighestValue = highestIndex;
        HighestHeight = highestHeight;

        DotValue = closest;
        return value;
    }

    void OnDrawGizmosSelected()
    {
        for(int i = 0; i < faces.Length; i++)
        {
            // Display the explosion radius when selected
            Gizmos.color = new Color(1, 1, 0, 0.75F);
            Vector3 worldPos = faces[i].position;
            Gizmos.DrawSphere(worldPos, 0.1f);
            drawString($"{i+1}",worldPos, Color.red);
        }
    }

    static public void drawString(string text, Vector3 worldPos, Color? colour = null)
    {
        UnityEditor.Handles.BeginGUI();

        var restoreColor = GUI.color;

        if (colour.HasValue) GUI.color = colour.Value;
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

        if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
        {
            GUI.color = restoreColor;
            UnityEditor.Handles.EndGUI();
            return;
        }

        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
        GUI.color = restoreColor;
        UnityEditor.Handles.EndGUI();
    }
}
