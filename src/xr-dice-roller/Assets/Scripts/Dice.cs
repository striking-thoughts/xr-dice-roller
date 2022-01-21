using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public int CurrentValue;
    public float DotValue;

    public Vector3[] faceDirections;

    private Matrix4x4 trsMatrix;

    private void Update()
    {
        CurrentValue = GetCurrentValue()+1;
    }

    private int GetCurrentValue()
    {
        int value = 0;
        float closest = -1f;
        float dot;
        Vector3 worldPosition;
        Vector3 direction;

        trsMatrix  = Matrix4x4.TRS(this.transform.position, this.transform.rotation, this.transform.localScale);
        for (int i = 0; i < faceDirections.Length; i++)
        {
            worldPosition = trsMatrix.MultiplyPoint3x4(faceDirections[i]);
            direction = worldPosition.normalized;
            dot = Vector3.Dot(Vector3.up, direction);

            if (dot > closest)
            {
                value = i;
                closest = dot;
            }
        }

        DotValue = closest;

        return value;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < faceDirections.Length; i++)
        {
            Matrix4x4 m = Matrix4x4.TRS(this.transform.position, this.transform.rotation, this.transform.localScale);
            Vector3 worldPos = m.MultiplyPoint3x4(faceDirections[i]);
            
            Gizmos.DrawSphere(this.transform.position + worldPos.normalized, 0.1f);
            drawString($"{i + 1}", this.transform.position + worldPos.normalized, Color.red);
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
