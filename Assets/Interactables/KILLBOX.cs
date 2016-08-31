using UnityEngine;
using System.Collections;

public class KILLBOX : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider victim)
    {
        Destroy(victim.gameObject, 1);
    }
}
