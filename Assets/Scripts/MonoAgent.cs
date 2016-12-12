using System;
using UnityEngine;

public class MonoAgent : MonoBehaviour //, IPointerDownHandler, IDragHandler
{
    [HideInInspector] public Agent Particle;
    [HideInInspector] public Vector3 Force = Vector3.zero;
    [HideInInspector] public Vector3 Acceleration = Vector3.zero;
    [HideInInspector] public Vector3 Velocity = Vector3.zero;
    public bool Anchor; //disables reational movement from other nodes
    public float Mass = 0.1f;
    public int Number;
    private const int Dims = 10;
    public Vector3 R;
    public Vector3 Offset; // offset from the origin of the click
    public Vector3 ScreenPoint;
    private Ray _ray;
    public RaycastHit Hit;

    private void OnMouseDown()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out Hit))
        {
            Hit.collider.GetComponent<MonoAgent>().Anchor = true;
        }
        ScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Offset = transform.position -
                 Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPoint.z));
    }

    private void OnMouseDrag()
    {

            var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPoint.z);
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + Offset;
            Particle.Position = curPosition;
            transform.position = curPosition;
        if (Particle.Position.y > 1.7f)
        {
            Particle.Position = new Vector3(Particle.Position.x, 1.7f, Particle.Position.z);
            transform.position = new Vector3(transform.position.x, 1.7f, transform.position.z);
        }
        //bot boundaries
        if (Particle.Position.y < -10.8f)
        {
            Particle.Position = new Vector3(Particle.Position.x, -10.8f, Particle.Position.z);
            transform.position = new Vector3(transform.position.x, -10.8f, transform.position.z);
        }
        //back wall boundaries
        if (Particle.Position.z > 5.0f)
        {
            Particle.Position = new Vector3(Particle.Position.x, Particle.Position.y, 5.0f);
            transform.position = new Vector3(transform.position.x, transform.position.y, 5.0f);
        }
        //your facewall boundaries
        if (Particle.Position.z < -4.0f)
        {
            Particle.Position = new Vector3(Particle.Position.x, Particle.Position.y, -4.0f);
            transform.position = new Vector3(transform.position.x, transform.position.y, -4.0f);
        }
        //left boundaries
        if (Particle.Position.x < -3.0f)
        {
            Particle.Position = new Vector3(-3.0f, Particle.Position.y, Particle.Position.z);
            transform.position = new Vector3(-3.0f, transform.position.y, transform.position.z);
        }
        //right boundaries
        if (Particle.Position.x > 13.0f)
        {
            Particle.Position = new Vector3(13.0f, Particle.Position.y, Particle.Position.z);
            transform.position = new Vector3(13.0f, transform.position.y, transform.position.z);
        }
            


    }


    // Use this for initialization
    private void Start()
    {
        //Particle = new Agent(GetComponent<MonoAgent>());
        //sets the first two anchors
        Number = Particle.Number;
        if (Number != 0 && Number != Dims - 1) return;
        Anchor = true;
        Particle.Anchor = true;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        //unsets an achor
        if (Input.GetMouseButtonDown(1))
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out Hit))
            {
                Hit.collider.GetComponent<MonoAgent>().Anchor = false;
                Hit.collider.GetComponent<MonoAgent>().Particle.Anchor = false;
            }
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