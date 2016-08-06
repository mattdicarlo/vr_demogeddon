using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class Lever : MonoBehaviour, IGrabbable
{
    public HingeJoint leverHinge;
    private Rigidbody _rigidbody;

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
        leverHinge = GetComponent<HingeJoint>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    #region IGrabbable

    public Rigidbody Rigidbody
    {
        get { return _rigidbody; }
    }

    public Joint CreateGrabJoint()
    {
        SpringJoint grabJoint = gameObject.AddComponent<SpringJoint>();
        grabJoint.spring = 5000f;
        grabJoint.damper = 50f;

        return grabJoint;
    }

    #endregion
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
