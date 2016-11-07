using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Networking;
public class ClothSystem : MonoBehaviour
{
    public List<Agent> Agents;
    public List<SpringDamper> SpringDampers;
    public List<Triangle> Triangles;
    public GameObject Joint;
    public float mass = 0.1f;
    public int dims = 10;
    private int amount = 0;
    [Range(0, 2)]
    public float Grav = 1;
    [Range(0, 2)]
    public float Spr = 1;
    [Range(0, 2)]
    public float Damp = 1;
    [Range(0, 2)]
    public float Dense = 1;
    [Range(0, 2)]
    public float Drag = 1;
    [Range(-2.5f, 2.5f)]
    public float Air1 = 1;
    public bool Break = false;

    // Use this for initialization
    void Awake()
    {
        Agents = new List<Agent>();
        SpringDampers = new List<SpringDamper>();
        Triangles = new List<Triangle>();
        if (Joint == null)
            return;
        for (int x = 0; x < dims; x++)
        {
            for (int y = 0; y < dims; y++)
            {
                Vector3 spawn = new Vector3(y, -x, 0);
                GameObject newGameObject = Instantiate(Joint, spawn, Quaternion.identity) as GameObject;

                Agent newagent;
                if (newGameObject.GetComponent<Agent>() != null)
                {
                    newagent = newGameObject.GetComponent<Agent>();
                }
                else
                {
                    newagent = newGameObject.AddComponent<Agent>();
                }
                Agents.Add(newagent);
                newGameObject.GetComponent<Agent>().number = amount;
                newGameObject.GetComponent<Agent>().Position = spawn;
                amount++;
                newGameObject.name = "Joint index" + (Agents.Count - 1).ToString();
            }
        }
    }
    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            if (Agents[i].number % dims != 0 && Agents[i].number != 0)
            {
                SpringDamper newSpringDamperx = new SpringDamper(Agents[i], Agents[i - 1]);
                SpringDampers.Add(newSpringDamperx);
            }
            if (Agents[i].number >= dims)
            {
                SpringDamper newSpringDampery = new SpringDamper(Agents[i], Agents[i - dims]);
                SpringDampers.Add(newSpringDampery);
            }
            if (Agents[i].number % dims != dims - 1 && Agents[i].number < dims * dims - 1 - dims)
            {
                SpringDamper newSpringDamperz = new SpringDamper(Agents[i], Agents[i + dims + 1]);
                SpringDampers.Add(newSpringDamperz);
            }
            if (Agents[i].number % dims != dims - 1 && Agents[i].number >= dims)
            {
                SpringDamper newSpringDamperw = new SpringDamper(Agents[i], Agents[i - dims + 1]);
                SpringDampers.Add(newSpringDamperw);
            }
            if (Agents[i].number % dims != dims - 1 && Agents[i].number < dims * dims - 1 - dims)
            {
                Triangle newTrianglex = new Triangle(Agents[i], Agents[i + dims + 1], Agents[i + 1]);
                Triangles.Add(newTrianglex);
                Triangle newTriangley = new Triangle(Agents[i], Agents[i + dims + 1], Agents[i + dims]);
                Triangles.Add(newTriangley);
            }
        }
    }
    public Vector3 Gravity(Agent x)
    {
        Vector3 gravity = new Vector3(0, -10, 0) * x.mass;
        return gravity;
    }
    public void Limit(Agent x)
    {
        if (Mathf.Abs(x.Velocity.magnitude) > 1)
        {
            x.Velocity = (x.Velocity / Mathf.Abs(x.Velocity.magnitude) * 1).normalized;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpringDamper instance;
        Triangle Tinstance;
        for (int j = 0; j < amount; j++)
        {
            Agents[j].Force = Grav * Gravity(Agents[j]);
        }
        for (int i = 0; i < SpringDampers.Count; i++)
        {
            instance = SpringDampers[i];
            if (instance.a.Velocity.magnitude >= 10 || instance.b.Velocity.magnitude >= 17)
            {
                Break = true;
                SpringDampers.Remove(instance);
                if (Air1 != 0 && Break)
                {
                    for (int k = 0; k < Triangles.Count; k++)
                    {
                        Tinstance = Triangles[k];
                        if (Tinstance.P1.Velocity.magnitude >= 17 || Tinstance.P2.Velocity.magnitude >= 17 ||
                            Tinstance.P3.Velocity.magnitude >= 17)
                        {
                            Triangles.Remove(Tinstance);
                        }
                    }
                }
            }
            instance.ComputeForce(Spr, Damp);
        }
        if (Air1 != 0)
        {
            for (int k = 0; k < Triangles.Count; k++)
            {
                Tinstance = Triangles[k];

                Tinstance.Air(Dense, Drag, Air1 * Vector3.forward);
            }
        }
        Break = false;
    }
    public class SpringDamper
    {
        public Agent a;
        public Agent b;
        private float Spring;
        private float Damper;
        Vector3 e1;
        float l;
        Vector3 e;
        float dir1;
        float dir2;
        Vector3 Force;
        float Spr1;
        float Damp1;
        public void ComputeForce(float Spr, float Damp)
        {
            Spr1 = Spr;
            Damp1 = Damp;
            e1 = b.Position - a.Position;
            l = e1.magnitude;
            e = e1 / l;
            dir1 = Vector3.Dot(e, a.Velocity);
            dir2 = Vector3.Dot(e, b.Velocity);
            Spring = -Spr1 * (1.0f - l);
            Damper = -Damp1 * (dir1 - dir2);
            Force = (Spring + Damper) * e;
            a.Force += Force;
            b.Force += -Force;
            Debug.DrawLine(a.Position, b.Position, Color.red);

        }
        public SpringDamper(Agent a, Agent b)
        {
            this.a = a;
            this.b = b;
        }
    }
    public class Triangle
    {
        public Agent P1;
        public Agent P2;
        public Agent P3;
        public float p;
        public float c;
        public float a;
        public float Aa;
        public Vector3 n;
        public Vector3 Vsurface;
        public Vector3 Vair1;
        public Vector3 v;
        public void Air(float Dense, float Drag, Vector3 Vair)
        {
            p = Dense;
            c = Drag;
            Vair1 = Vair;
            Vsurface = (P1.Velocity + P2.Velocity + P3.Velocity) / 3;
            v = Vsurface - Vair;
            n = Vector3.Cross(P2.Position - P1.Position, P3.Position - P1.Position)
              / Vector3.Cross(P2.Position - P1.Position, P3.Position - P1.Position).magnitude;
            a = 0.5f * Vector3.Cross(P2.Position - P1.Position, P3.Position - P1.Position).magnitude;
            Aa = a * (Vector3.Dot(v, n) / v.magnitude);
            Vector3 Faero = -0.5f * p * (v.magnitude * v.magnitude) * c * Aa * n;
            P1.Force += Faero / 3;
            P2.Force += Faero / 3;
            P3.Force += Faero / 3;
        }
        public Triangle(Agent P1, Agent P2, Agent P3)
        {
            this.P1 = P1;
            this.P2 = P2;
            this.P3 = P3;
        }
    }
}