using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour
{
    public Vector3 Force = Vector3.zero;
    public Vector3 Acceleration = Vector3.zero;
    public Vector3 Position;
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
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
        Position = transform.position;
    }
    // Use this for initialization
    void Awake()
    {
        Position = transform.position;
        
    }
    void OnTriggerEnter(Collider enemy) 
    {
        if (enemy.gameObject.name == "Wall")  
        {
            float mag = Velocity.magnitude;
            float dot = Vector3.Dot(Velocity.normalized, transform.forward);
            Velocity = transform.forward - 2 * dot * transform.forward;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (number == 0 || number == 9)
        {
            Anchor = true;
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

            Position = transform.position;
            Acceleration = 1 / mass * Force;
            Velocity += Acceleration * Time.deltaTime;
            Position += Velocity * Time.deltaTime;
            transform.position = Position;


        }
    }
}
