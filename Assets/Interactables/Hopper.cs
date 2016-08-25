using UnityEngine;
using System.Collections;

public class Hopper : MonoBehaviour {

    public Object _itemToDispense;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator dispense() {
        for (int ii = 0; ii < 12; ii++)
        {
            Instantiate(_itemToDispense, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
