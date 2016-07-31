using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class Lever : MonoBehaviour
{
    private HingeJoint leverHinge;
    [SerializeField]
    public float Value
    {
        get
        {
            float _val = ((leverHinge.angle + 90) % 360) / 180;
            return Mathf.Clamp01(_val);
        }
    }

    void Awake()
    {
        leverHinge = transform.Find("Handle").GetComponent<HingeJoint>();
    }
}

[CustomEditor(typeof(Lever))]
public class LeverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Lever myTarget = (Lever)target;
        EditorGUILayout.Slider("Value", myTarget.Value, 0, 1);
    }
}
