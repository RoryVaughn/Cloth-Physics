using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour
{
    public Vector3 Force = Vector3.zero;
    public Vector3 Acceleration = Vector3.zero;
    public Vector3 Velocity = Vector3.zero;
    public bool Anchor;
    public float mass = 0.1f;
    public int number;
    int dims = 10;
    public Vector3 r;
    public Vector3 offset;
    public Vector3 screenPoint;
    private Ray ray;
    public RaycastHit hit;
    void OnMouseDown()
    {
        Anchor = true;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }


    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;

    }
    // Use this for initialization
  
    void Start()
    {
        if (number == 0 || number == 9)
        {
            Anchor = true;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (Input.GetMouseButtonDown(1))
        {
            Anchor = false;
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }

        if (Input.GetKeyDown("a"))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                hit.collider.GetComponent<Agent>().Anchor = true;
            }
        }
        if (Anchor != true)
        {
            if (transform.position.y < -10)
            {
                transform.position = new Vector3(transform.position.x,-10,transform.position.z);
                Force += new Vector3(0, - 0.7f*(Acceleration.y * mass), 0);
            }
            Acceleration = 1 / mass * Force;
            Velocity += Acceleration * Time.deltaTime;

            transform.position += Velocity * Time.deltaTime;
            transform.position = transform.position;






        }
    }
}
