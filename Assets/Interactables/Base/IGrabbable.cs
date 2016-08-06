using UnityEngine;

public interface IGrabbable
{
    Rigidbody Rigidbody
    {
        get;
    }
    Joint CreateGrabJoint();
}
