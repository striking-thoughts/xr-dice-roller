using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DiceBuilder : MonoBehaviour
{
    public Color facePointColor;
    public Dice target;
    [SerializeField] private Transform[] facePoints;

    public void TransferValues()
    {
        Vector3[] arr = new Vector3[facePoints.Length];
        Vector3 temp;
        for (int i = 0; i < facePoints.Length; i++)
        {
            temp = facePoints[i].position;
            temp.x = MathF.Round(temp.x * 100f)/100f;
            temp.y = MathF.Round(temp.y * 100f) / 100f;
            temp.z = MathF.Round(temp.z * 100f) / 100f;
            arr[i] = temp - target.transform.position;
        }

        SetPrivateFieldValue(target, "faceDirections", arr);
    }

    private void SyncChildWithFacePoints()
    {
        int childCount = this.transform.childCount;
        if (facePoints.Length != childCount)
        {
            facePoints = new Transform[childCount];
        }

        for (int i = 0; i < childCount; i++)
        {
            facePoints[i] = this.transform.GetChild(i);
        }
    }

    void OnDrawGizmos()
    {
        SyncChildWithFacePoints();

        if (this.target == null || facePoints == null || facePoints.Length == 0) return;

        //Matrix4x4 m = Matrix4x4.TRS(this.target.transform.position, this.target.transform.rotation, this.target.transform.localScale);
        Gizmos.color = facePointColor;
        for (int i = 0; i < facePoints.Length; i++)
        {
            if (facePoints[i] == null) continue;

            Vector3 worldPos = facePoints[i].position; //this.target.transform.position + facePoints[i].localPosition; // + m.MultiplyPoint3x4(facePoints[i].localPosition).normalized;

            Gizmos.DrawSphere(worldPos, 0.1f);
            drawString($"{i + 1}", worldPos, facePointColor);
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

    public static Vector3[] GetPrivatePropertyValue(Dice obj, string propName)
    {
        if (obj == null) throw new ArgumentNullException("obj");
        PropertyInfo pi = obj.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (pi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
        return (Vector3[])pi.GetValue(obj, null);
    }

    public static Vector3[] GetPrivateFieldValue(Dice obj, string propName)
    {
        if (obj == null) throw new ArgumentNullException("obj");
        Type t = obj.GetType();
        FieldInfo fi = null;
        while (fi == null && t != null)
        {
            fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            t = t.BaseType;
        }
        if (fi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
        return (Vector3[])fi.GetValue(obj);
    }

    public static void SetPrivatePropertyValue(Dice obj, string propName, Vector3[] val)
    {
        Type t = obj.GetType();
        if (t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
            throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
        t.InvokeMember(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, obj, new object[] { val });
    }

    public static void SetPrivateFieldValue(Dice obj, string propName, Vector3[] val)
    {
        if (obj == null) throw new ArgumentNullException("obj");
        Type t = obj.GetType();
        FieldInfo fi = null;
        while (fi == null && t != null)
        {
            fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            t = t.BaseType;
        }
        if (fi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
        fi.SetValue(obj, val);
    }
}
