using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRay : MonoBehaviour
{
    public float min_num_rays;

    //Random numbers
    float n1;
    float n2;

    //Ray variables
    float sqrt_num_rays;
    float num_rays;
    void Start()
    {
        sqrt_num_rays = Mathf.Ceil(Mathf.Sqrt(min_num_rays));
        num_rays = sqrt_num_rays * sqrt_num_rays;

        n1 = Random.Range(0.00f, 0.99f);
        n2 = Random.Range(0.00f, 0.99f);

    }

    private void Update()
    {
        for (int ray_index = 0; ray_index < num_rays; ++ray_index)
        {
            Vector3 direction = StratifiedSampleSphere(n1, n2, sqrt_num_rays, ray_index);

            Debug.DrawRay(transform.position, direction * 10, Color.green);
        }
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

}
