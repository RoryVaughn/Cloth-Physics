using UnityEngine;
using System.Collections;

public class LineConnect : MonoBehaviour {

    public GameObject P1;
    public GameObject P2;
    private LineRenderer Line;
    public GameObject Spawn;
    // Use this for initialization
    void Start () {
        Line = gameObject.GetComponent<LineRenderer>();
        foreach (GameObject i in GetComponentsInChildren<GameObject>());
        {
            int i = 0;
                P1 = GetComponentInChildren<Transform>().gameObject;
            if (i > 0)
            {
                P2 = GetComponentInChildren<Transform>().gameObject;
            }
        }
     
        Line.SetPosition(0, P1.transform.position);
        Line.SetPosition(1, P2.transform.position);
    }
	
	// Update is called once per frame
	void Update () {
        Line.SetPosition(0, P1.transform.position);
        Line.SetPosition(1, P2.transform.position);
    }
}
