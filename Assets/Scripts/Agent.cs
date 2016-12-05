using UnityEngine;

public class Agent : MonoBehaviour//, IPointerDownHandler, IDragHandler
{
    [HideInInspector]
    public Vector3 Force = Vector3.zero;
    [HideInInspector]
    public Vector3 Acceleration = Vector3.zero;
    [HideInInspector]
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
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            hit.collider.GetComponent<Agent>().Anchor = true;
        }
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
        if (number == 0 || number == dims - 1)
        {
            Anchor = true;
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {

        if (Input.GetMouseButtonDown(1))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                hit.collider.GetComponent<Agent>().Anchor = false;
            }
        }
        //top
        if (transform.position.y > 1.7f)
        {
            transform.position = new Vector3(transform.position.x, 1.7f, transform.position.z);
            Force += new Vector3(0, -0.7f * (Acceleration.y * mass), 0);
        }
        //bot
        if (transform.position.y < -10.8f)
        {
            transform.position = new Vector3(transform.position.x, -10.8f, transform.position.z);
            Force += new Vector3(0, -0.7f * (Acceleration.y * mass), 0);
        }
        //back wall
        if (transform.position.z > 5.0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 5.0f);
            Force += new Vector3(0, 0, -0.7f * (Acceleration.z * mass));
        }
        //your facewall
        if (transform.position.z < -4.0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -4.0f);
            Force += new Vector3(0, 0, 0.7f * (Acceleration.z * mass));
        }
        //left
        if (transform.position.x < -3.0f)
        {
            transform.position = new Vector3(-3.0f, transform.position.y, transform.position.z);
            Force += new Vector3(-0.7f * (Acceleration.x * mass), 0, 0);
        }
        //right
        if (transform.position.x > 13.0f)
        {
            transform.position = new Vector3(13.0f, transform.position.y, transform.position.z);
            Force += new Vector3(-0.7f * (Acceleration.x * mass), 0, 0);
        }
        if (Anchor != true)
        {
            Acceleration = 1 / mass * Force;
            Velocity += Acceleration * Time.deltaTime;
            Velocity = Vector3.ClampMagnitude(Velocity, 5.0f);
            transform.position += Velocity * Time.deltaTime;
            transform.position = transform.position;
        }
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    //offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    //    GetComponent<Agent>().Anchor = true;
    //    Debug.Log("pointer down");
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    offset = new Vector3(eventData.delta.x, eventData.delta.y , 0.0f) * Time.deltaTime;
                
    //    transform.position = (transform.position + offset) ;

        
    
        
    //}
}
