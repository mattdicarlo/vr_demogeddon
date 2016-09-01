using UnityEngine;

public class SlideProjector : MonoBehaviour
{
    private Slide currentSlide;
    private Slide _selected;

    [SerializeField]
    private Lever _nextSlideLever;

    [SerializeField]
    private Transform _slideSlot;

    [SerializeField]
    private GameObject _slideGhost;

    [SerializeField]
    private GameObject _spawnGhost;

    private Rigidbody _rigidbody;

    [SerializeField]
    private Material screenMaterial;

    private Texture _origScreenTexture;

    public void Awake()
    {
        _origScreenTexture = screenMaterial.GetTexture("_ShadowTex");
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        _selected = SelectNearbySlide();

        _slideGhost.SetActive(_selected != null && currentSlide == null);

        if (currentSlide == null && _selected != null)
        {
            if (_selected.ConnectedHand)
            {
                return;
            }
            currentSlide = _selected;
            currentSlide.Transform.position = _slideGhost.transform.position;
            currentSlide.Transform.rotation = _slideGhost.transform.rotation;
            var joint = currentSlide.CreateGrabJoint();
            joint.connectedBody = _rigidbody;
            joint.enableCollision = false;
            joint.breakForce = 1000.0f;
            //currentSlide.Rigidbody.useGravity = false;
            currentSlide.slideProjector = this;
        }

        if (_nextSlideLever.Value > _nextSlideLever.actuationPoint)
        {
            NextSlide();
        }
    }

    public void NextSlide()
    {
        if (currentSlide != null)
        {
            screenMaterial.SetTexture("_ShadowTex", currentSlide.SlideTexture);
        }
        else
        {
            screenMaterial.SetTexture("_ShadowTex", _origScreenTexture);
        }
    }

    public void OnDisable()
    {
        screenMaterial.SetTexture("_ShadowTex", _origScreenTexture);
    }

    private Slide SelectNearbySlide()
    {
        Collider[] nearProjectorObjects = Physics.OverlapSphere(_slideSlot.position, 0.15f);
        foreach (Collider col in nearProjectorObjects)
        {
            if (col.GetComponent<Slide>() != null)
            {
                return col.GetComponent<Slide>();
            }
        }
        return null;
    }

    public void NotifyJointBroken()
    {
        currentSlide.Rigidbody.useGravity = true;
        currentSlide.slideProjector = null;
        currentSlide = null;
    }

    public void SpawnSlide(Slide newSlide)
    {
        newSlide.transform.position = _spawnGhost.transform.position;
        newSlide.transform.rotation = _spawnGhost.transform.rotation;
        newSlide.AnimateMe();
    }
}
