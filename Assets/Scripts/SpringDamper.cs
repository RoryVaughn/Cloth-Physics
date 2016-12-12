using UnityEngine;

public class SpringDamper
{
    //reference to the two particles in the spring damper
    public Agent A;
    public Agent B;
    private float _spring;
    private float _damper;
    private Vector3 _e1; //displacement between the particles
    public float L; //magnitude of the displacement
    private Vector3 _e; //normalized displacement vecotr
    private float _dir1; //particle A's Velocity
    private float _dir2; //particle B's Velocity
    private Vector3 _force;
    private float _spr1; //Spring coefficient
    private float _damp1; //damping coefficient
    private float _rest1; //rest length coefficient
    public GameObject Line;

    public void ComputeForce(float spr, float damp, float rest)
    {
        _spr1 = spr;
        _damp1 = damp;
        _rest1 = rest;
        _e1 = B.Position - A.Position;
        L = _e1.magnitude;
        _e = _e1/L;
        _dir1 = Vector3.Dot(_e, A.Velocity);
        _dir2 = Vector3.Dot(_e, B.Velocity);
        _spring = -_spr1*(_rest1 - L);
        _damper = -_damp1*(_dir1 - _dir2);
        _force = (_spring + _damper)*_e;
        //adds the force to each of the partles in the spring damper
        A.Force += _force;
        B.Force += -_force;
    }

    public SpringDamper()
    {
        this.A = null;
        this.B = null;
    }

    public SpringDamper(Agent a, Agent b)
    {
        this.A = a;
        this.B = b;
    }
}