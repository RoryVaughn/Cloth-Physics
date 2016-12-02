using UnityEngine;
using System.Collections;

public class LineConnect : MonoBehaviour {

    public GameObject P1;
    public GameObject P2;
    private LineRenderer Line;

    // Use this for initialization
    void Awake () {
        Line = gameObject.GetComponent<LineRenderer>();
        Line.SetWidth(0.1f,0.1f);
    }
	
	// Update is called once per frame
	void LateUpdate () {
        Line.SetPosition(0, P1.transform.position);
        Line.SetPosition(1, P2.transform.position);
    }
}
