using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;

public class RayTracing : MonoBehaviour
{
    public float min_num_rays;
    public int numFreq;
    public int numberHitsMax;

    //Random numbers
    float n1;
    float n2;

    //Ray variables
    float sqrt_num_rays;
    int num_rays;

    float[] freq = new float[7];

    public WaterLayerGenerator waterLayerGenerator;

    public ReadFile readFile;

    public Dropdown bottomLayer;

    public GameObject listener;

    public bool bake = false;
    public bool normalMode = false;
    public int xAxis;
    public float initialAngle;
    public float finalAngle;
    public float dephtAtPosition;

    //public GameObject[] listenersArray;
    public List<GameObject> listenersArray = new List<GameObject>();

    [Header("Ray Tracing Properties Input Fields")]
    [SerializeField] InputField numberRaysInputField;
    [SerializeField] InputField numberHitsInputFiedl;

    [Header("Message Box Object")]
    [SerializeField] GameObject messageBoxPrefab;
    [SerializeField] GameObject canvas;

    [Header("Listeners Manager Object")]
    [SerializeField] ListenersManager listenersManager;



    void Start()
    {

        n1 = UnityEngine.Random.Range(0.00f, 0.99f);
        n2 = UnityEngine.Random.Range(0.00f, 0.99f);

    }

    public void Bake()
    {
        n1 = UnityEngine.Random.Range(0.00f, 0.99f);
        n2 = UnityEngine.Random.Range(0.00f, 0.99f);

        listenersArray = listenersManager.listeners;

        foreach (GameObject o in listenersArray)
        {
            o.GetComponent<Listener>().ResetLevelAndPressure();
            o.GetComponent<Listener>().ResetRaysNumber();
            o.GetComponent<Listener>().RecalculateEnergyFactor();
        }

        min_num_rays = float.Parse(numberRaysInputField.text.Replace('.', ','));
        numberHitsMax = int.Parse(numberHitsInputFiedl.text);

        sqrt_num_rays = Mathf.Ceil(Mathf.Sqrt(min_num_rays));
        num_rays = (int)(sqrt_num_rays * sqrt_num_rays);

        for (int ray_index = 0; ray_index < num_rays; ++ray_index)
        {
            Vector3 direction = StratifiedSampleSphere(n1, n2, sqrt_num_rays, ray_index);
            //Debug.DrawRay(transform.position, direction * 10, Color.green);

            //Vector3 direction = transform.up;
            float[] sourcePressures = new float[numFreq];
            GetComponent<Source>().pressures.CopyTo(sourcePressures, 0);
            Ray ray = new Ray(transform.position, direction, numFreq, sourcePressures, 0);
            //Ray ray2 = new Ray(transform.position, transform.right, numFreq, sourcePressures, 0);

            //var task = new Task(() => TraceRay(ray));
            //task.Start();

            TraceRay(ray);
            //TraceRay(ray2);
        }

        Instantiate(messageBoxPrefab, canvas.transform);
        messageBoxPrefab.GetComponent<MessageBox>().SetMessage("Ray Tracing is done.");

        string result = "";
        for (int l = 0; l < listenersArray.Count; l++)
        {
            result += (l + 1).ToString() + " | " + listenersArray[l].GetComponent<Listener>().PrintAnswer() + "\n";
        }
        Debug.Log(result);

        StreamWriter sr = File.CreateText("Results.txt");
        sr.WriteLine(result);
        sr.Close();

    }

    /*
    private void Update()
    {
        freq = new float[7] { 31.5f, 63.0f, 125.0f, 250.0f, 500.0f, 1000.0f, 2000.0f};


        //Debug.Log((transform.position - listener.transform.position).magnitude * readFile.cellsize);
        if (bake)
        {
            double t = Time.realtimeSinceStartup;
            n1 = UnityEngine.Random.Range(0.00f, 0.99f);
            n2 = UnityEngine.Random.Range(0.00f, 0.99f);

            sqrt_num_rays = Mathf.Ceil(Mathf.Sqrt(min_num_rays));
            num_rays = (int)(sqrt_num_rays * sqrt_num_rays);

            if (normalMode)
            {
                //float offset = 360.0f / sqrt_num_rays;
                float offsetYAxis = (initialAngle - finalAngle) / min_num_rays;
                float offsetXAxis = 360.0f / xAxis;

                for (int i = 0; i < xAxis; i++)
                {
                    //Vector3 direction = Quaternion.Euler(-initialAngle + (offsetYAxis * j), offsetXAxis * i, 0) * Vector3.forward;
                    //direction = Quaternion.Euler(0, , 0) * direction;
                    
                    for (int j = 0; j <= min_num_rays; j++)
                    {
                        Vector3 direction = Quaternion.Euler(-initialAngle + (offsetYAxis * j), offsetXAxis * i, 0) * Vector3.forward;
                        //Debug.DrawRay(transform.position, direction * 10, Color.green);

                        //Vector3 direction = transform.up;
                        float[] sourcePressures = new float[numFreq];
                        GetComponent<Source>().pressures.CopyTo(sourcePressures, 0);
                        Ray ray = new Ray(transform.position, direction, numFreq, sourcePressures, 0);
                        //Ray ray2 = new Ray(transform.position, transform.right, numFreq, sourcePressures, 0);

                        //var task = new Task(() => TraceRay(ray));
                        //task.Start();

                        TraceRay(ray);
                        //TraceRay(ray2);
                    }
                }
                */
                /*for (int i = 0; i < sqrt_num_rays; i++)
                {
                    for (int j = 0; j < sqrt_num_rays; j++)
                    {
                        Vector3 direction = Quaternion.Euler(offset * j, offset * i, 0) * transform.forward;
                        Debug.DrawRay(transform.position, direction * 10, Color.green);

                        //Vector3 direction = transform.up;
                        float[] sourcePressures = new float[numFreq];
                        GetComponent<Source>().pressures.CopyTo(sourcePressures, 0);
                        Ray ray = new Ray(transform.position, direction, numFreq, sourcePressures, 0);
                        //Ray ray2 = new Ray(transform.position, transform.right, numFreq, sourcePressures, 0);

                        //var task = new Task(() => TraceRay(ray));
                        //task.Start();

                        //TraceRay(ray);
                        //TraceRay(ray2);
                    }
                }*//*
            }
            else
            {
                for (int ray_index = 0; ray_index < num_rays; ++ray_index)
                {
                    Vector3 direction = StratifiedSampleSphere(n1, n2, sqrt_num_rays, ray_index);
                    //Debug.DrawRay(transform.position, direction * 10, Color.green);

                    //Vector3 direction = transform.up;
                    float[] sourcePressures = new float[numFreq];
                    GetComponent<Source>().pressures.CopyTo(sourcePressures, 0);
                    Ray ray = new Ray(transform.position, direction, numFreq, sourcePressures, 0);
                    //Ray ray2 = new Ray(transform.position, transform.right, numFreq, sourcePressures, 0);

                    //var task = new Task(() => TraceRay(ray));
                    //task.Start();

                    TraceRay(ray);
                    //TraceRay(ray2);

                }
            }
            
            bake = false;

            string result = "";
            for (int l = 0; l < listenersArray.Count; l++)
            {
                result += (l+1).ToString() + " " + listenersArray[l].GetComponent<Listener>().PrintAnswer() + "\n";

            }
            Debug.Log(result);
            double t2 = Time.realtimeSinceStartup;
            Debug.Log(t2 - t);
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))
            {
                dephtAtPosition = (hit.distance) / (readFile.depthFactor);
            }
        }

    }
    */

    void TraceRay(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        {
            //Debug.Log("Hit distance: " + hit.distance);
            //Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.white);

            ray.numberHits += 1;
            float[] attenuationCoefficient = new float[numFreq];

            if (ray.numberHits > numberHitsMax)
            {
                if (hit.collider.gameObject.tag == "Listener")
                {
                    //Attenuation
                    CalculateAttenuationCoefficient(attenuationCoefficient, -AverageDepth(ray.origin.y, hit.point.y));
                    CalculateAttenuation(ray, hit, attenuationCoefficient);
                    //Debug.Log("Hit Listener.");
                    hit.collider.gameObject.GetComponent<Listener>().SumPressureListener(ray.pressure);
                    //hit.collider.gameObject.GetComponent<Listener>().SumEnergyListener(AllPressureToSoundLevel(ray.pressure));
                    //hit.collider.gameObject.GetComponent<Listener>().PrintAnswer();
                    //Debug.Log((ray.origin - hit.collider.gameObject.transform.position).magnitude);
                    //Debug.Log(PressureToSoundLevel(ray.pressure[0]));

                    /////soma pressão do resultado final/////

                    Ray rayContinue = new Ray(hit.point, ray.direction, numFreq, ray.pressure, ray.numberHits);
                    rayContinue.origin += rayContinue.direction.normalized / 10000;
                    TraceRay(rayContinue);

                }

            }
            else
            {
                if (hit.collider.gameObject.tag == "Listener")
                {
                    //Attenuation
                    CalculateAttenuationCoefficient(attenuationCoefficient, -AverageDepth(ray.origin.y, hit.point.y));
                    CalculateAttenuation(ray, hit, attenuationCoefficient);
                    //Debug.Log("Hit Listener.");
                    hit.collider.gameObject.GetComponent<Listener>().SumPressureListener(ray.pressure);
                    //hit.collider.gameObject.GetComponent<Listener>().SumEnergyListener(AllPressureToSoundLevel(ray.pressure));
                    //hit.collider.gameObject.GetComponent<Listener>().PrintAnswer();
                    //Debug.Log(PressureToSoundLevel(ray.pressure[0]));

                    Ray rayContinue = new Ray(hit.point, ray.direction, numFreq, ray.pressure, ray.numberHits);
                    rayContinue.origin += rayContinue.direction.normalized / 10000;
                    TraceRay(rayContinue);
                }
                else
                {
                    //Attenuation
                    CalculateAttenuationCoefficient(attenuationCoefficient, -AverageDepth(ray.origin.y, hit.point.y));
                    CalculateAttenuation(ray, hit, attenuationCoefficient);

                    if (hit.collider.tag == "Water Layer")
                    {
                        if (FindLayerByObject(hit.collider.gameObject) == 0 && hit.normal.y < 0.0f)
                        {
                            // Reflaction with air
                            float reflectionCoefficient = CalculateWaterReflectionWithAir(ray, hit);
                            float[] pressureReflection = new float[numFreq];
                            for (int i = 0; i < numFreq; i++)
                            {
                                pressureReflection[i] = ray.pressure[i] * Mathf.Abs(reflectionCoefficient);
                            }

                            if (!VerifySoundLevelThreshold(pressureReflection))
                            {
                                /////Dispara Ray reflection
                                Vector3 reflectionDirection = ray.direction - 2 * (Vector3.Dot(ray.direction, hit.normal)) * hit.normal;
                                Ray rayReflection = new Ray(hit.point, reflectionDirection, numFreq, pressureReflection, ray.numberHits);
                                rayReflection.origin += rayReflection.direction.normalized / 10000;
                                //Debug.DrawRay(rayReflection.origin, rayReflection.direction * 100);
                                
                                for (int l = 0; l < listenersArray.Count; l++)
                                {
                                    if (FindLayerByDepth(rayReflection.origin.y) == FindLayerByDepth(listenersArray[l].transform.position.y))
                                    {
                                        RaycastHit hit2;
                                        if (Physics.Raycast(rayReflection.origin, (listenersArray[l].transform.position - rayReflection.origin), out hit2, Mathf.Infinity))
                                        {

                                            if (hit2.collider.gameObject == listenersArray[l])
                                            {
                                                //Debug.DrawRay(rayReflection.origin, (listenersArray[l].transform.position - rayReflection.origin), Color.red);
                                                //Attenuation
                                                Ray rayToListener = rayReflection;

                                                CalculateAttenuationCoefficient(attenuationCoefficient, -AverageDepth(rayToListener.origin.y, listenersArray[l].transform.position.y));
                                                CalculateAttenuation(rayToListener, hit2, attenuationCoefficient);

                                                /*float y = 2 * (Mathf.Atan2(listenersArray[l].GetComponent<SphereCollider>().radius, hit2.distance));
                                                float o = Mathf.Deg2Rad * Mathf.Abs(Vector3.Angle(hit.normal, (listenersArray[l].transform.position - rayReflection.origin)));
                                                float d = (1.0f - Mathf.Cos(y / 2.0f)) * 2.0f * Mathf.Cos(o);
                                                //Debug.Log("y " + listenersArray[l].name + " y: " + y + " o: " + o + " d: " + d);

                                                for (int p = 0; p < rayToListener.pressure.Length; p++)
                                                {
                                                    rayToListener.pressure[p] = rayToListener.pressure[p] * d;
                                                }*/

                                                //Debug.Log("Hit Listener.");
                                                hit2.collider.gameObject.GetComponent<Listener>().SumPressureListener(rayToListener.pressure);
                                                //hit2.collider.gameObject.GetComponent<Listener>().SumEnergyListener(AllPressureToSoundLevel(rayReflection.pressure));
                                                //hit2.collider.gameObject.GetComponent<Listener>().PrintAnswer();
                                                //Debug.Log(PressureToSoundLevel(ray.pressure[0]));
                                            }
                                        }
                                    }
                                }
                                

                                

                                TraceRay(rayReflection);

                            }
                        }
                        else
                        {
                            /////Reflection and refraction/////
                            int medium1Index = FindLayerByObject(hit.collider.gameObject);
                            int medium2Index = FindMedium2(medium1Index, hit.normal);

                            float c1 = waterLayerGenerator.layers[medium1Index].GetComponent<WaterLayer>().speed;
                            float c2 = waterLayerGenerator.layers[medium2Index].GetComponent<WaterLayer>().speed;

                            float density1 = waterLayerGenerator.layers[medium1Index].GetComponent<WaterLayer>().density;
                            float density2 = waterLayerGenerator.layers[medium2Index].GetComponent<WaterLayer>().density;

                            float incidenceAngle = CalculateIncidenceAngle(ray.direction, hit.normal); //In degrees
                            float transmissionAngle = CalculateTransmissionAngle(incidenceAngle, c1, c2);//In degress

                            float impedance1 = WaterImpedance(density1, c1, incidenceAngle);
                            float impedance2 = WaterImpedance(density2, c2, transmissionAngle);

                            float reflectionCoefficient;
                            float transmissionCoefficient;
                            //Debug.Log("Cri: " + CriticalAngle(c1, c2));
                            //Debug.Log("IncAngle: " + incidenceAngle);
                            //Debug.Log("TraAngle: " + transmissionAngle);
                            if (c2 > c1 && incidenceAngle < CriticalAngle(c1, c2))
                            {
                                reflectionCoefficient = 1.0f;
                                transmissionCoefficient = 0.0f;
                            }
                            else
                            {
                                reflectionCoefficient = CalculateWaterReflection(impedance1, impedance2);
                                transmissionCoefficient = 1 + reflectionCoefficient;
                            }

                            float[] pressureReflection = new float[numFreq];
                            float[] pressureTransmission = new float[numFreq];

                            for (int i = 0; i < numFreq; i++)
                            {
                                pressureReflection[i] = ray.pressure[i] * Mathf.Abs(reflectionCoefficient);
                                pressureTransmission[i] = ray.pressure[i] * transmissionCoefficient;
                            }
                            //Debug.Log("RefCoef: " + reflectionCoefficient);
                            //Debug.Log("PressReflec: " + pressureReflection[0]);
                            //Debug.Log("LevelReflec: " + PressureToSoundLevel(pressureReflection[0]));

                            if (!VerifySoundLevelThreshold(pressureReflection))
                            {
                                /////Dispara Ray reflection
                                //Debug.Log("Reflection");
                                Vector3 reflectionDirection = ray.direction - 2 * (Vector3.Dot(ray.direction, hit.normal)) * hit.normal;
                                Ray rayReflection = new Ray(hit.point, reflectionDirection, numFreq, pressureReflection, ray.numberHits);
                                rayReflection.origin += rayReflection.direction.normalized / 10000;
                                //Debug.DrawRay(rayReflection.origin, rayReflection.direction * 100, Color.white);

                                for (int l = 0; l < listenersArray.Count; l++)
                                {
                                    if (FindLayerByDepth(rayReflection.origin.y) == FindLayerByDepth(listenersArray[l].transform.position.y))
                                    {
                                        RaycastHit hit2;
                                        if (Physics.Raycast(rayReflection.origin, (listenersArray[l].transform.position - rayReflection.origin), out hit2, Mathf.Infinity))
                                        {

                                            if (hit2.collider.gameObject == listenersArray[l])
                                            {
                                                //Debug.DrawRay(rayReflection.origin, (listenersArray[l].transform.position - rayReflection.origin), Color.red);
                                                //Attenuation
                                                Ray rayToListener = rayReflection;

                                                CalculateAttenuationCoefficient(attenuationCoefficient, -AverageDepth(rayToListener.origin.y, listenersArray[l].transform.position.y));
                                                CalculateAttenuation(rayToListener, hit2, attenuationCoefficient);

                                                /*float y = 2 * (Mathf.Atan2(listenersArray[l].GetComponent<SphereCollider>().radius, hit2.distance));
                                                float o = Mathf.Deg2Rad * Mathf.Abs(Vector3.Angle(hit.normal, (listenersArray[l].transform.position - rayReflection.origin)));
                                                float d = (1.0f - Mathf.Cos(y / 2.0f)) * 2.0f * Mathf.Cos(o);
                                                //Debug.Log("y " + listenersArray[l].name + " y: " + y + " o: " + o + " d: " + d);

                                                for (int p = 0; p < rayToListener.pressure.Length; p++)
                                                {
                                                    rayToListener.pressure[p] = rayToListener.pressure[p] * d;
                                                }*/

                                                //Debug.Log("Hit Listener.");
                                                hit2.collider.gameObject.GetComponent<Listener>().SumPressureListener(rayToListener.pressure);
                                                //hit2.collider.gameObject.GetComponent<Listener>().SumEnergyListener(AllPressureToSoundLevel(rayReflection.pressure));
                                                //hit2.collider.gameObject.GetComponent<Listener>().PrintAnswer();
                                                //Debug.Log(PressureToSoundLevel(ray.pressure[0]));
                                            }
                                        }
                                    }
                                }

                                
                                

                                TraceRay(rayReflection);

                            }

                            if (!VerifySoundLevelThreshold(pressureTransmission))
                            {
                                /////Dispara Ray refraction

                                Vector3 transmissionDirection = new Vector3(ray.direction.x, -1 * hit.normal.y * Mathf.Tan(transmissionAngle * Mathf.Deg2Rad) * new Vector2(ray.direction.x, ray.direction.z).magnitude, ray.direction.z);
                                Ray rayTransmission = new Ray(hit.point, transmissionDirection, numFreq, pressureTransmission, ray.numberHits);
                                rayTransmission.origin += rayTransmission.direction.normalized / 10000;
                                TraceRay(rayTransmission);

                            }

                        }
                    }
                    else
                    {
                        /////Reflection/////
                        ///

                        float solidReflectionCoefficient = CalculateSolidReflection(ray, hit, bottomLayer.captionText.text);

                        float[] pressureReflection = new float[numFreq];

                        for (int i = 0; i < numFreq; i++)
                        {
                            pressureReflection[i] = ray.pressure[i] * Mathf.Abs(solidReflectionCoefficient);
                        }

                        //Debug.Log(PressureToSoundLevel(pressureReflection[0]));
                        //Debug.Log(solidReflectionCoefficient);

                        if (!VerifySoundLevelThreshold(pressureReflection))
                        {
                            /////Dispara Ray reflection
                            //Debug.Log("Reflection");
                            Vector3 reflectionDirection = ray.direction - 2 * (Vector3.Dot(ray.direction, hit.normal)) * hit.normal;
                            Ray rayReflection = new Ray(hit.point, reflectionDirection, numFreq, pressureReflection, ray.numberHits);
                            rayReflection.origin += rayReflection.direction.normalized / 10000;
                            //Debug.DrawRay(rayReflection.origin, rayReflection.direction * 100, Color.white);

                            for (int l = 0; l < listenersArray.Count; l++)
                            {
                                if (FindLayerByDepth(rayReflection.origin.y) == FindLayerByDepth(listenersArray[l].transform.position.y))
                                {
                                    RaycastHit hit2;
                                    if (Physics.Raycast(rayReflection.origin, (listenersArray[l].transform.position - rayReflection.origin), out hit2, Mathf.Infinity))
                                    {

                                        if (hit2.collider.gameObject == listenersArray[l])
                                        {
                                            //Debug.DrawRay(rayReflection.origin, (listenersArray[l].transform.position - rayReflection.origin), Color.red);
                                            //Attenuation
                                            Ray rayToListener = rayReflection;

                                            CalculateAttenuationCoefficient(attenuationCoefficient, -AverageDepth(rayToListener.origin.y, listenersArray[l].transform.position.y));
                                            CalculateAttenuation(rayToListener, hit2, attenuationCoefficient);

                                            /*float y = 2 * (Mathf.Atan2(listenersArray[l].GetComponent<SphereCollider>().radius, hit2.distance));
                                            float o = Mathf.Deg2Rad * Mathf.Abs(Vector3.Angle(hit.normal, (listenersArray[l].transform.position - rayReflection.origin)));
                                            float d = (1.0f - Mathf.Cos(y / 2.0f)) * 2.0f * Mathf.Cos(o);
                                            //Debug.Log("y " + listenersArray[l].name + " y: " + y + " o: " + o + " d: " + d);

                                            for (int p = 0; p < rayToListener.pressure.Length; p++)
                                            {
                                                rayToListener.pressure[p] = rayToListener.pressure[p] * d;
                                            }*/

                                            //Debug.Log("Hit Listener.");
                                            hit2.collider.gameObject.GetComponent<Listener>().SumPressureListener(rayToListener.pressure);
                                            //hit2.collider.gameObject.GetComponent<Listener>().SumEnergyListener(AllPressureToSoundLevel(rayReflection.pressure));
                                            //hit2.collider.gameObject.GetComponent<Listener>().PrintAnswer();
                                            //Debug.Log(PressureToSoundLevel(ray.pressure[0]));
                                        }
                                    }
                                }
                            }

                            

                            TraceRay(rayReflection);

                        }

                    }
                }




            }

        }
        else
        {
            //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        }
    }

    float[] AllPressureToSoundLevel(float[] pressures)
    {
        float[] levels = new float[pressures.Length];
        for (int i = 0; i < pressures.Length; i++)
        {
            levels[i] = PressureToSoundLevel(pressures[i]);
        }

        return levels;
    }

    float CriticalAngle(float c1, float c2)
    {
        return Mathf.Rad2Deg * Mathf.Acos(c1 / c2);
    }

    bool VerifySoundLevelThreshold(float[] level)
    {
        for (int i = 0; i < level.Length; i++)
        {
            if (PressureToSoundLevel(level[i]) > Mathf.Epsilon)
            {
                return false;
            }
        }

        return true;
    }

    float AverageDepth(float origin, float hit)
    {
        if (origin < hit)
        {
            return ((hit - origin) / 2) + origin;
        }
        else
        {
            return ((origin - hit) / 2) + hit;
        }
    }

    float CalculateWaterReflection(float impedance1, float impedance2)
    {
        return (impedance2 - impedance1) / (impedance2 + impedance1);

    }

    float CalculateSolidReflection(Ray ray, RaycastHit hit, string bottomLayer)
    {
        int medium1Index = FindLayerByDepth(-AverageDepth(ray.origin.y, hit.point.y));

        float c1 = waterLayerGenerator.layers[medium1Index].GetComponent<WaterLayer>().speed;
        float ct = GetBottomLayerTransverseSpeed(bottomLayer, -hit.point.y * readFile.cellsize);
        float cl = GetBottomLayerLongitudinalSpeed(bottomLayer);

        float density1 = waterLayerGenerator.layers[medium1Index].GetComponent<WaterLayer>().density;
        float density2 = GetBottomLayerDensity(bottomLayer);

        if (ct == -1 || cl == -1 || density2 == -1)
        {
            Debug.LogError("Transverse or Longitudinal Speed are -1");
        }

        float incidenceAngle = CalculateIncidenceAngle(ray.direction, hit.normal); //In degrees
        float transverseAngle = CalculateTransmissionAngle(incidenceAngle, c1, ct);//In degress
        //Debug.Log("Inc: " + incidenceAngle + " c1: " + c1 + " ct: " + ct);

        //Debug.Log(((ct/c1) * Mathf.Cos(Mathf.Deg2Rad * incidenceAngle)));

        //Debug.Log();
        float longitudinalAngle = CalculateTransmissionAngle(incidenceAngle, c1, cl);//In degress

        if (float.IsNaN(transverseAngle))
        {
            transverseAngle = 0.0f;
        }
        if (float.IsNaN(longitudinalAngle))
        {
            longitudinalAngle = 0.0f;
        }

        float impedance1 = WaterImpedance(density1, c1, incidenceAngle);

        float impedanceTransverse = WaterImpedance(density2, ct, transverseAngle);
        float impedanceLongitudinal = WaterImpedance(density2, cl, longitudinalAngle);

        float impedanceTot = SolidImpedance(impedanceTransverse, impedanceLongitudinal, transverseAngle);


        return (impedanceTot - impedance1) / (impedanceTot + impedance1);

    }

    float SolidImpedance(float transverseImpedance, float longitudinalImpedance, float transverseAngle)
    {
        float a = longitudinalImpedance * Mathf.Pow(Mathf.Cos(2.0f * (transverseAngle * Mathf.Deg2Rad)), 2);
        float b = transverseAngle * Mathf.Pow(Mathf.Sin(2.0f * (transverseAngle * Mathf.Deg2Rad)), 2);

        return a + b;
    }

    float GetBottomLayerTransverseSpeed(string bottomLayer, float depth)
    {
        if (bottomLayer == "Sand")
        {
            return Sand.GetTransverseSpeed(depth);
        }

        if (bottomLayer == "Silt")
        {
            return Silt.GetTransverseSpeed(depth);
        }

        if (bottomLayer == "Clay")
        {
            return Clay.transverseSpeed;
        }

        if (bottomLayer == "Gravel")
        {
            return Gravel.GetTransverseSpeed(depth);
        }

        if (bottomLayer == "Moraine")
        {
            return Moraine.transverseSpeed;
        }

        if (bottomLayer == "Chalk")
        {
            return Chalk.transverseSpeed;
        }

        if (bottomLayer == "Limestone")
        {
            return Limestone.transverseSpeed;
        }

        if (bottomLayer == "Basalt")
        {
            return Basalt.transverseSpeed;
        }

        return -1;
    }

    float GetBottomLayerLongitudinalSpeed(string bottomLayer)
    {
        if (bottomLayer == "Sand")
        {
            return Sand.longitudinalSpeed;
        }

        if (bottomLayer == "Silt")
        {
            return Silt.longitudinalSpeed;
        }

        if (bottomLayer == "Clay")
        {
            return Clay.longitudinalSpeed;
        }

        if (bottomLayer == "Gravel")
        {
            return Gravel.longitudinalSpeed;
        }

        if (bottomLayer == "Moraine")
        {
            return Moraine.longitudinalSpeed;
        }

        if (bottomLayer == "Chalk")
        {
            return Chalk.longitudinalSpeed;
        }

        if (bottomLayer == "Limestone")
        {
            return Limestone.longitudinalSpeed;
        }

        if (bottomLayer == "Basalt")
        {
            return Basalt.longitudinalSpeed;
        }

        return -1;
    }

    float GetBottomLayerDensity(string bottomLayer)
    {
        if (bottomLayer == "Sand")
        {
            return Sand.density;
        }

        if (bottomLayer == "Silt")
        {
            return Silt.density;
        }

        if (bottomLayer == "Clay")
        {
            return Clay.density;
        }

        if (bottomLayer == "Gravel")
        {
            return Gravel.density;
        }

        if (bottomLayer == "Moraine")
        {
            return Moraine.density;
        }

        if (bottomLayer == "Chalk")
        {
            return Chalk.density;
        }

        if (bottomLayer == "Limestone")
        {
            return Limestone.density;
        }

        if (bottomLayer == "Basalt")
        {
            return Basalt.density;
        }

        return -1;
    }

    float CalculateWaterReflectionWithAir(Ray ray, RaycastHit hit)
    {
        int medium1Index = 0;

        float c1 = waterLayerGenerator.layers[medium1Index].GetComponent<WaterLayer>().speed;
        float c2 = 343.0f;

        float density1 = waterLayerGenerator.layers[medium1Index].GetComponent<WaterLayer>().density;
        float density2 = 1.275f;

        float incidenceAngle = CalculateIncidenceAngle(ray.direction, hit.normal); //In degrees
        float transmissionAngle = CalculateTransmissionAngle(incidenceAngle, c1, c2);//In degress

        float impedance1 = WaterImpedance(density1, c1, incidenceAngle);
        float impedance2 = WaterImpedance(density2, c2, transmissionAngle);

        return (impedance2 - impedance1) / (impedance2 + impedance1);

    }

    float CalculateIncidenceAngle(Vector3 direction, Vector3 normal)
    {
        return 90 - Vector3.Angle(-direction, normal);
    }

    float CalculateTransmissionAngle(float incidenceAngle, float c1, float c2)
    {
        return Mathf.Rad2Deg * (Mathf.Acos((c2 / c1) * Mathf.Cos(Mathf.Deg2Rad * incidenceAngle)));
    }

    float WaterImpedance(float density, float speed, float theta)
    {
        return (density * speed) / Mathf.Sin(theta * Mathf.Deg2Rad);
    }

    float TotalLevelSound(float[] levels)
    {
        float sum = 0.0f;

        for (int i = 0; i < levels.Length; i++)
        {
            sum += Mathf.Pow(10, levels[i] / 10);
        }
        //Debug.Log("Sum: " + sum);
        //Debug.Log("LOGSum: " + Mathf.Log10(sum));
        return 10 * Mathf.Log10(sum);
    }

    void CalculateAttenuation(Ray ray, RaycastHit hit, float[] attenuationCoefficient)
    {
        //float[] lev = new float[numFreq];
        for (int pressureIndex = 0; pressureIndex < attenuationCoefficient.Length; pressureIndex++)
        {
            //Debug.Log("Pressure " + pressureIndex + ": " + ray.pressure[pressureIndex]);
            //Debug.Log("Attenuation " + pressureIndex + ": " + attenuationCoefficient[pressureIndex]);
            //ray.pressure[pressureIndex] = ray.pressure[pressureIndex] * Mathf.Exp(-attenuationCoefficient[pressureIndex] * (hit.distance * readFile.cellsize));
            ray.pressure[pressureIndex] = ray.pressure[pressureIndex] * Mathf.Exp(-attenuationCoefficient[pressureIndex] * CalculateDistance(ray.origin, hit.point));
            //ray.pressure[pressureIndex] /= hit.distance;
            //Debug.Log("Pressure " + pressureIndex + ": " + ray.pressure[pressureIndex]);
            //Debug.Log("LS " + pressureIndex + ": " + PressureToSoundLevel(ray.pressure[pressureIndex]));
            //lev[pressureIndex] = PressureToSoundLevel(ray.pressure[pressureIndex]);
        }

        //Debug.Log("LS: " + TotalLevelSound(lev));
    }

    float CalculateDistance(Vector3 origin, Vector3 point)
    {
        float x = (Mathf.Abs(origin.x - point.x) * readFile.cellsize) / readFile.sizeFactor;
        float y = Mathf.Abs(origin.y - point.y) / (readFile.depthFactor);
        float z = (Mathf.Abs(origin.z - point.z) * readFile.cellsize) / readFile.sizeFactor;

        Vector3 newV = new Vector3(x, y, z);

        return newV.magnitude;
    }

    void CalculateAttenuationCoefficient(float[] attenuationCoefficient, float depth)
    {
        //Debug.Log(depth);
        int layerIndex = FindLayerByDepth(depth);

        if (layerIndex == -1)
        {
            Debug.LogError("Invalid depth.");
        }

        depth = depth / (readFile.depthFactor);
        depth = depth / 1000.0f;

        WaterLayer waterLayer = waterLayerGenerator.layers[layerIndex].GetComponent<WaterLayer>();

        float f1 = 0.78f * Mathf.Sqrt((waterLayer.salinity / 35.0f)) * Mathf.Exp(waterLayer.temperature / 26.0f);
        float f2 = 42.0f * Mathf.Exp(waterLayer.temperature / 17.0f);

        //Debug.Log(depth);
        for (int frequencyIndex = 0; frequencyIndex < attenuationCoefficient.Length; frequencyIndex++)
        {
            float f = freq[frequencyIndex];
            f = f / 1000.0f;

            float a1 = (f1 * Mathf.Pow(f, 2.0f)) / (Mathf.Pow(f, 2.0f) + Mathf.Pow(f1, 2.0f));
            float a2 = Mathf.Exp((waterLayer.ph - 8.0f) / 0.56f);
            float a3 = 0.52f * (1.0f + (waterLayer.temperature / 43.0f)) * (waterLayer.salinity / 35.0f);
            float a4 = (f2 * Mathf.Pow(f, 2.0f)) / (Mathf.Pow(f, 2.0f) + Mathf.Pow(f2, 2.0f));
            float a5 = Mathf.Exp(-depth / 6.0f);
            float a6 = 0.00049f * Mathf.Pow(f, 2.0f) * Mathf.Exp(-((waterLayer.temperature / 27.0f) + (depth / 17.0f)));

            float alphaLine = (0.106f * a1 * a2) + (a3 * a4 * a5) + a6;

            attenuationCoefficient[frequencyIndex] = alphaLine / 8686.0f;
            //attenuationCoefficient[frequencyIndex] = alphaLine;

            //Debug.Log(attenuationCoefficient[frequencyIndex]);
        }
        //Debug.Log("End");
    }

    int FindMedium2(int medium1Index, Vector3 hitNormal)
    {
        if (medium1Index == 0)
        {
            return 1;
        }
        else
        {
            if (hitNormal.y < 0.0f)
            {
                return medium1Index - 1;
            }
            else
            {
                return medium1Index + 1;
            }
        }
        
    }

    int FindLayerByDepth(float depth)
    {
        for (int i = 0; i < waterLayerGenerator.layers.Count; i++)
        {
            WaterLayer waterLayer = waterLayerGenerator.layers[i].GetComponent<WaterLayer>();
            if (depth > waterLayer.initialDepth)
            {
                if (waterLayer.finalDepth != -1.0f)
                {
                    if (depth < waterLayer.finalDepth)
                    {
                        return i;
                    }
                }
                else
                {
                    return i;
                }
            }
        }

        return -1;
    }

    int FindLayerByObject(GameObject obj)
    {
        for (int i = 0; i < waterLayerGenerator.layers.Count; i++)
        {
            if (waterLayerGenerator.layers[i].gameObject == obj)
            {
                return i;
            }
        }

        return -1;
    }

    float PressureToSoundLevel(float pressure)
    {
        return 20 * Mathf.Log10(pressure / Mathf.Pow(10, -6));
    }

    float LevelSoundToPressure(float dB)
    {
        return Mathf.Pow(10, -6) * (Mathf.Pow(10, dB / 20));
    }


    Vector3 StratifiedSampleSphere(float u, float v, float sqrt_num_samples, float sample_index)
    {

        float cell_x = (sample_index / sqrt_num_samples);
        float cell_y = (sample_index % sqrt_num_samples);

        float cell_width = 1.0f / (sqrt_num_samples);
        return UniformSampleSphere((cell_x + u) * cell_width, (cell_y + v) * cell_width);
    }

    Vector3 UniformSampleSphere(float u, float v)
    {

        float cos_theta = 1.0f - 2.0f * v;
        float sin_theta = 2.0f * Mathf.Sqrt(v * (1.0f - v));
        float phi = 2 * Mathf.PI * u;
        return new Vector3(Mathf.Cos(phi) * sin_theta, Mathf.Sin(phi) * sin_theta, cos_theta);
    }




    ///////////// RAY STRUCT /////////////
    struct Ray
    {
        public float[] pressure;

        public int numberHits;

        public Vector3 origin;
        public Vector3 direction;

        public float incidenceAngle;

        public Vector3 reflectionDirection;
        public float reflectionAngle;

        public Vector3 transmissionDirection;
        public float transmissionAngle;

        public Ray(Vector3 origin, Vector3 direction, int numFreq, float[] pressures, int numberHits)
        {
            this.numberHits = numberHits;

            this.origin = origin;
            this.direction = direction;

            this.incidenceAngle = 0.0f;

            this.reflectionDirection = Vector3.zero;
            this.reflectionAngle = 0.0f;

            this.transmissionDirection = Vector3.zero;
            this.transmissionAngle = 0.0f;

            this.pressure = new float[numFreq];
            pressure = pressures;
        }

        /*void SetIncidenceAngle(Vector3 normal)
        {
            this.incidenceAngle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(this.direction, normal)) - 90;
        }

        void SetReflectionAngleAndDirection(Vector3 normal)
        {
            this.reflectionDirection = this.direction - 2 * (Vector3.Dot(this.direction, normal)) * normal;
            this.reflectionAngle = this.incidenceAngle;
        }

        void SetTransmissionAngleAndDirection(float soundSpeed1, float soundSpeed2, Vector3 normal)
        {
            this.transmissionDirection = this.direction - (Vector3.Dot(this.direction, normal) * normal);
            this.transmissionAngle = Mathf.Rad2Deg * (Mathf.Acos((soundSpeed2 / soundSpeed1) * Mathf.Cos(Mathf.Deg2Rad * this.incidenceAngle)));
        }*/
    }

    static class Clay
    {
        public const float density = 1500.0f;
        public const float longitudinalSpeed = 1500.0f;
        public const float transverseSpeed = 100.0f;

    }

    static class Silt
    {
        public const float density = 1700.0f;
        public const float longitudinalSpeed = 1575.0f;

        static public float GetTransverseSpeed(float depth)
        {
            return 80.0f * Mathf.Pow(depth, 0.3f);
        }
    }

    static class Sand
    {
        public const float density = 1900.0f;
        public const float longitudinalSpeed = 1650.0f;

        static public float GetTransverseSpeed(float depth)
        {
            return 110.0f * Mathf.Pow(depth, 0.3f);
        }
    }

    static class Gravel
    {
        public const float density = 2000.0f;
        public const float longitudinalSpeed = 1800.0f;

        static public float GetTransverseSpeed(float depth)
        {
            return 180.0f * Mathf.Pow(depth, 0.3f);
        }
    }

    static class Moraine
    {
        public const float density = 2100.0f;
        public const float longitudinalSpeed = 1950.0f;
        public const float transverseSpeed = 600.0f;
    }

    static class Chalk
    {
        public const float density = 2200.0f;
        public const float longitudinalSpeed = 2400.0f;
        public const float transverseSpeed = 1000.0f;
    }

    static class Limestone
    {
        public const float density = 2400.0f;
        public const float longitudinalSpeed = 3000.0f;
        public const float transverseSpeed = 1500.0f;
    }

    static class Basalt
    {
        public const float density = 2700.0f;
        public const float longitudinalSpeed = 5250.0f;
        public const float transverseSpeed = 2500.0f;
    }

}
