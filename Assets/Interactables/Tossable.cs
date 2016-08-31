using UnityEngine;

public class Tossable : MonoBehaviour, IGrabbable
{
    private Rigidbody _rigidbody;
    private Throw _controller;
    public bool moveToGrabberWhenGrabbed = true;
    public float breakForce = Mathf.Infinity;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    #region IGrabbable

    public Rigidbody Rigidbody
    {
        get { return _rigidbody; }
    }

    public Joint CreateGrabJoint()
    {
        FixedJoint grabJoint = gameObject.AddComponent<FixedJoint>();
        return grabJoint;
    }

    public Transform Transform
    {
        get { return GetComponent<Transform>(); }
    }

    public bool MoveToGrabberWhenGrabbed
    {
        get { return moveToGrabberWhenGrabbed; }
    }

    public float BreakForce
    {
        get { return breakForce; }
    }

    public Throw ConnectedHand
    {
        get { return _controller; }
        set { _controller = value; }
    }

    #endregion

    private float feedbackMultiplier = 100.0f;
    public void OnCollisionEnter(Collision collision)
    {
        if (_controller != null)
        {
            _controller.ForceFeedback(feedbackMultiplier * collision.relativeVelocity.magnitude);
        }
    }

    public virtual void OnJointBreak(float breakForce)
    {
        if (_controller)
        {
            _controller.DropObject(this, breakForce);
        }
    }
}
