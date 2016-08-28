using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Lever : MonoBehaviour, IGrabbable
{
    public HingeJoint leverHinge;
    private Rigidbody _rigidbody;
    private Throw _controller;

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

    private float baseForceFeedback = 10.0f;

    public void FixedUpdate()
    {
        if (Value > 0.8)
        {
            TriggerForceFeedback(Value * baseForceFeedback);
        }
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

    public Transform Transform
    {
        get { return GetComponent<Transform>(); }
    }

    public bool MoveToGrabberWhenGrabbed
    {
        get { return false; }
    }

    public float BreakForce
    {
        get { return Mathf.Infinity; }
    }

    public Throw ConnectedHand
    {
        get { return _controller; }
        set { _controller = value; }
    }

    #endregion

    public void OnCollisionEnter(Collision collision)
    {
        TriggerForceFeedback(collision.relativeVelocity.magnitude);
    }

    private void TriggerForceFeedback(float magnitude)
    {
        if (_controller != null)
        {
            _controller.ForceFeedback(magnitude);
        }
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
