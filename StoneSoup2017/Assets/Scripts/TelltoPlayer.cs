using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelltoPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Player.instance.blackMask.enabled = false;
        Player.instance.blackMask.transform.localScale = new Vector3(1.2f, 1.2f, 0);
    }

    private void OnDestroy() {
        Player.instance.blackMask.enabled = true;
        //Player.instance.blackMask.transform.localScale = new Vector3(1.2f, 1.2f, 0);
    }
}
