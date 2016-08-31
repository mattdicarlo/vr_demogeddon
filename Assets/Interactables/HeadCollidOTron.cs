using UnityEngine;
using System.Collections;

public class HeadCollidOTron : MonoBehaviour {

    [SerializeField]
    private GameObject _hmdModel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (_hmdModel)
        {
            transform.position = _hmdModel.transform.position;
            transform.rotation = _hmdModel.transform.rotation;
        }
	}
}
