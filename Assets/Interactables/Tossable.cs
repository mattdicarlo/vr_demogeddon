using UnityEngine;

public class Tossable : MonoBehaviour, IGrabbable
{
    private Rigidbody _rigidbody;
    private Throw _controller;

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
        get { return true; }
    }

    public Throw ConnectedHand
    {
        set { _controller = value; }
    }

    #endregion

    public void OnCollisionEnter(Collision collision)
    {
        if (_controller != null)
        {
            _controller.ForceFeedback(collision.relativeVelocity.magnitude);
        }
    }
}
