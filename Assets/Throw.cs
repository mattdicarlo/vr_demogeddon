using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class Throw : MonoBehaviour
{
    public GameObject selected;

    public Transform attachPoint;

    SteamVR_TrackedObject trackedObj;
    FixedJoint joint;
    Rigidbody throwRigidbody;

    public Collider handCollider;
    public GameObject handModel;

    Animator handAnimator;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        throwRigidbody = GetComponent<Rigidbody>();
        handAnimator = handModel.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);

        // Find and highlight a selected object
        selected = SelectNearbyObject();

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            handAnimator.SetBool("ShouldGrip", true);
            if (joint == null && selected != null)
            {
                //handCollider.enabled = false;
                selected.transform.position = attachPoint.position;
                joint = selected.AddComponent<FixedJoint>();
                joint.connectedBody = throwRigidbody;
            }
        }
        else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            handAnimator.SetBool("ShouldGrip", false);
            if (joint != null)
            {
                StartCoroutine(ThrowObject(device));
            }
        }
    }

    private IEnumerator ThrowObject(SteamVR_Controller.Device device)
    {
        var go = joint.gameObject;
        var rigidbody = go.GetComponent<Rigidbody>();
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

    private GameObject SelectNearbyObject()
    {
        Collider[] nearHandObjects = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (Collider col in nearHandObjects)
        {
            if (col.GetComponent<Rigidbody>() != null && col.gameObject.tag != "nonpickup")
            {
                return col.gameObject;
            }
        }
        return null;
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Colliding with: " + collision.gameObject.name);
    }
}
