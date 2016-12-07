using System;
using UnityEngine;

public class MonoAgent : MonoBehaviour //, IPointerDownHandler, IDragHandler
{
    [HideInInspector] public Agent Particle;
    [HideInInspector] public Vector3 Force = Vector3.zero;
    [HideInInspector] public Vector3 Acceleration = Vector3.zero;
    [HideInInspector] public Vector3 Velocity = Vector3.zero;
    public bool Anchor; //disables reational movement from other nodes
    public float mass = 0.1f;
    public int number;
    int dims = 10;
    public Vector3 r;
    public Vector3 offset; // offset from the origin of the click
    public Vector3 screenPoint;
    private Ray ray;
    public RaycastHit hit;

    void OnMouseDown()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            hit.collider.GetComponent<MonoAgent>().Anchor = true;
        }
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position -
                 Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        Particle.Position = curPosition;
        transform.position = curPosition;
    }


    // Use this for initialization
    void Start()
    {
        //Particle = new Agent(GetComponent<MonoAgent>());
        //sets the first two anchors
        number = Particle.number;
        if (number == 0 || number == dims - 1)
        {
            Anchor = true;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //unsets an achor
        if (Input.GetMouseButtonDown(1))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                hit.collider.GetComponent<MonoAgent>().Anchor = false;
            }
        }
        //top boundaries
        if (transform.position.y > 1.7f)
        {
            transform.position = new Vector3(transform.position.x, 1.7f, transform.position.z);
            Force += new Vector3(0, -0.7f*(Acceleration.y*mass), 0);
        }
        //bot boundaries
        if (transform.position.y < -10.8f)
        {
            transform.position = new Vector3(transform.position.x, -10.8f, transform.position.z);
            Force += new Vector3(0, -0.7f*(Acceleration.y*mass), 0);
        }
        //back wall boundaries
        if (transform.position.z > 5.0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 5.0f);
            Force += new Vector3(0, 0, -0.7f*(Acceleration.z*mass));
        }
        //your facewall boundaries
        if (transform.position.z < -4.0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -4.0f);
            Force += new Vector3(0, 0, -0.7f*(Acceleration.z*mass));
        }
        //left boundaries
        if (transform.position.x < -3.0f)
        {
            transform.position = new Vector3(-3.0f, transform.position.y, transform.position.z);
            Force += new Vector3(-0.7f*(Acceleration.x*mass), 0, 0);
        }
        //right boundaries
        if (transform.position.x > 13.0f)
        {
            transform.position = new Vector3(13.0f, transform.position.y, transform.position.z);
            Force += new Vector3(-0.7f*(Acceleration.x*mass), 0, 0);
        }
        if (Anchor != true)
        {
            //addition of the forces to change the position of the particles.

            transform.position = Particle.CalcuateForce(); //changing the position over delta time delta time.
        }
    }

    //TEACHER CODE---KEEP INCASE
    //
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    //offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    //    GetComponent<MonoAgent>().Anchor = true;
    //    Debug.Log("pointer down");
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    offset = new Vector3(eventData.delta.x, eventData.delta.y , 0.0f) * Time.deltaTime;

    //    transform.position = (transform.position + offset) ;


    //}
}