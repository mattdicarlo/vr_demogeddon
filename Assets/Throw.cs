using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class Throw : MonoBehaviour
{
    [SerializeField]
    public IGrabbable selected;

    public Transform attachPoint;

    SteamVR_TrackedObject trackedObj;
    Joint joint;
    Rigidbody throwRigidbody;

    public Collider handCollider;
    public GameObject handModel;

    Animator handAnimator;

    private bool heldItemShouldUseGravity = true;

    // Editor Testing Flags
    public bool fake_trigger;
    private bool trigger_down;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        throwRigidbody = handCollider.gameObject.GetComponent<Rigidbody>();
        handAnimator = handModel.GetComponent<Animator>();

        fake_trigger = false;
        trigger_down = false;
    }

    void FixedUpdate()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);

        // Find and highlight a selected object
        selected = SelectNearbyObject();

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) || (!trigger_down && fake_trigger))
        {
            trigger_down = true;
            fake_trigger = false;

            handAnimator.SetBool("ShouldGrip", true);
            if (joint == null && selected != null)
            {
                //handCollider.enabled = false;
                if (selected.MoveToGrabberWhenGrabbed)
                {
                    selected.Transform.position = attachPoint.position;
                }
                joint = selected.CreateGrabJoint();
                joint.connectedBody = throwRigidbody;
                joint.enableCollision = false;

                heldItemShouldUseGravity = selected.Rigidbody.useGravity;
                selected.Rigidbody.useGravity = false;
                // joint.breakForce =
            }
        }
        else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) || (trigger_down && fake_trigger))
        {
            trigger_down = false;
            fake_trigger = false;

            handAnimator.SetBool("ShouldGrip", false);
            if (joint != null)
            {
                StartCoroutine(ThrowObject(device, heldItemShouldUseGravity));
            }
        }
    }

    private IEnumerator ThrowObject(SteamVR_Controller.Device device, bool shouldUseGravity)
    {
        var go = joint.gameObject;
        var rigidbody = go.GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        Object.Destroy(joint);
        joint = null;

        yield return null;

        // We should probably apply the offset between trackedObj.transform.position
        // and device.transform.pos to insert into the physics sim at the correct
        // location, however, we would then want to predict ahead the visual representation
        // by the same amount we are predicting our render poses.

        var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
        if (origin != null)
        {
            rigidbody.velocity = origin.TransformVector(device.velocity);
            rigidbody.angularVelocity = origin.TransformVector(device.angularVelocity);
        }
        else
        {
            rigidbody.velocity = device.velocity;
            rigidbody.angularVelocity = device.angularVelocity;
        }

        rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;

        yield return null;
        yield return null;
        yield return null;
        //handCollider.enabled = true;
    }

    private IGrabbable SelectNearbyObject()
    {
        Collider[] nearHandObjects = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (Collider col in nearHandObjects)
        {
            if (col.GetComponent<IGrabbable>() != null && col.gameObject.tag != "nonpickup")
            {
                return col.GetComponent<IGrabbable>();
            }
        }
        return null;
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Colliding with: " + collision.gameObject.name);
    }
}
