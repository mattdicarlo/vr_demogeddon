using UnityEngine;

public class Tossable : MonoBehaviour, IGrabbable
{
    private Rigidbody _rigidbody;

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

    #endregion
}
