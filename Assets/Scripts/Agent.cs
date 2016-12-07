using UnityEngine;
using System.Collections;

public class Agent
{
//    public MonoAgent a;
    public Vector3 Force = Vector3.zero;
    public Vector3 Acceleration = Vector3.zero;
    public Vector3 Velocity = Vector3.zero;
    public Vector3 Position;
    public int number;
    public bool Anchor; //disables reational movement from other nodes
    public float mass = 0.1f;
    int dims = 10;

    public Vector3 CalcuateForce()
    {
        Acceleration = 1/mass*Force;
        Velocity += Acceleration*Time.deltaTime;
        Velocity = Vector3.ClampMagnitude(Velocity, 5.0f);
        Position += Velocity*Time.deltaTime;
        return Position;
    }

    public Agent()
    {
        this.Position = Vector3.zero;
    }

    public Agent(Vector3 position)
    {
        this.Position = position;
    }

    public Agent(Vector3 position, int id)
    {
        this.Position = position;
        this.number = id;
    }

    // Use this for initialization
}