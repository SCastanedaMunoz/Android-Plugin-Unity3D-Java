using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCube : MonoBehaviour
{
    public float Speed = 30f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Speed * Time.deltaTime, 2 * Speed * Time.deltaTime, -Speed * Time.deltaTime);
    }
}
