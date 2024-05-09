using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public Vector3 sensorDirection;
    public float sensorSpeed;

    LineRenderer lr;

    private void Start() {
        sensorDirection = sensorDirection.normalized;
        lr = GetComponent<LineRenderer>();
    }

    private void Update() {
        sensorDirection = sensorDirection.normalized;

        Vector3 line = sensorDirection * sensorSpeed;

        lr.SetPosition(0, this.transform.position - (line/2));
        lr.SetPosition(1, this.transform.position + (line/2));
    }
}
