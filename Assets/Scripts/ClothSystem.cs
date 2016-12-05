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
    [Range(0.5f, 2)]
    public float rest = 1;
    [Range(1, 10)]
    public float Spr = 1;
    [Range(0.1f, 1)]
    public float Damp = 1;
    [Range(0, 1)]
    public float Dense = 1;
    [Range(0, 2)]
    public float Drag = 1;
    [Range(-2.5f, 2.5f)]
    public float Air1 = 0;
    [Range(-2.5f, 2.5f)]
    public float Air2 = 0;
    public bool Break = false;
    public bool intruct = false;
    public GameObject text;
    public GameObject Line;


    public void UISetGravity(UnityEngine.UI.Slider slider)
    {
        Grav = slider.value;
    }
    public void UIintructions(UnityEngine.UI.Button button)
    {
        intruct = button;
        if (text.activeSelf)
        {
            text.SetActive(false);
        }
        else text.SetActive(true);
    }

    public void UIReset(UnityEngine.UI.Button button)
    {
        Application.LoadLevel(0);
    }
    public void UISetRest(UnityEngine.UI.Slider slider)
    {
        rest = slider.value;
    }
    public void UISetSpr(UnityEngine.UI.Slider slider)
    {
        Spr = slider.value;
    }
    public void UISetDamp(UnityEngine.UI.Slider slider)
    {
        Damp = slider.value;
    }
    public void UISetDense(UnityEngine.UI.Slider slider)
    {
        Dense = slider.value;
    }
    public void UISetAir1(UnityEngine.UI.Slider slider)
    {
        Air1 = slider.value;
    }
    public void UISetAir2(UnityEngine.UI.Slider slider)
    {
        Air2 = slider.value;
    }

    void CreateSpring(Agent a, Agent b, int index)
    {
        SpringDamper sd = new SpringDamper(a, b);
        SpringDampers.Add(sd);

        
        GameObject go = Instantiate(Line) as GameObject;
        go.name = "Line Index" + (index).ToString();
        sd.line = go;
        go.GetComponent<LineConnect>().P1 = a.gameObject;
        go.GetComponent<LineConnect>().P2 = b.gameObject;

        
    }
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
                newGameObject.GetComponent<Agent>().transform.position = spawn;
                amount++;
                newGameObject.name = "Joint index" + (Agents.Count - 1).ToString();
            }
        }
    }
    void Start()
    {
        text.SetActive(false);
        for (int i = 0; i < amount; i++)
        {
            if (Agents[i].number % dims != 0 && Agents[i].number != 0)
            {
                CreateSpring(Agents[i], Agents[i - 1], i); //connect left
            }
            if (Agents[i].number >= dims)
            {
                CreateSpring(Agents[i], Agents[i - dims], i); //Connect down
            }
            if (Agents[i].number % dims != dims - 1 && Agents[i].number < dims * dims - 1 - dims)
            {

                CreateSpring(Agents[i], Agents[i + dims + 1], i); //top right

            }
            if (Agents[i].number % dims != dims - 1 && Agents[i].number >= dims)
            {

                CreateSpring(Agents[i], Agents[i - dims + 1], i); //bot right

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
        x.Velocity = Vector3.ClampMagnitude(x.Velocity, 3f);
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



            if (instance.l >= 10)
            {
                Break = true;

                SpringDampers.Remove(instance);
                instance.line.active = false;
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
            instance.ComputeForce(Spr, Damp, rest);

        }

        if (Air1 != 0 || Air2 != 0)
        {
            for (int k = 0; k < Triangles.Count; k++)
            {
                Tinstance = Triangles[k];
                Tinstance.Air(Dense, Drag, Air1 * Vector3.forward + Air2 * Vector3.left);
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
        public float l;
        Vector3 e;
        float dir1;
        float dir2;
        Vector3 Force;
        float Spr1;
        float Damp1;
        float Rest1;
        public GameObject line;
        public void ComputeForce(float Spr, float Damp, float Rest)
        {
            Spr1 = Spr;
            Damp1 = Damp;
            Rest1 = Rest;
            e1 = b.transform.position - a.transform.position;
            l = e1.magnitude;
            e = e1 / l;
            dir1 = Vector3.Dot(e, a.Velocity);
            dir2 = Vector3.Dot(e, b.Velocity);
            Spring = -Spr1 * (Rest1 - l);
            Damper = -Damp1 * (dir1 - dir2);
            Force = (Spring + Damper) * e;
            a.Force += Force;
            b.Force += -Force;

            

        }
        public SpringDamper()
        {
            this.a = null;
            this.b = null;
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
            n = Vector3.Cross(P2.transform.position - P1.transform.position, P3.transform.position - P1.transform.position)
              / Vector3.Cross(P2.transform.position - P1.transform.position, P3.transform.position - P1.transform.position).magnitude;
            a = 0.5f * Vector3.Cross(P2.transform.position - P1.transform.position, P3.transform.position - P1.transform.position).magnitude;
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