using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public float segmentStep;
    Vector3 currentPos;
    public Vector3 line;
    private LineRenderer lr;
    public float certainty;
    public GameObject pSegment;
    public float magnitudeForColor;

    private void Start() {
        currentPos = transform.position;
        //line = UpdateLineSegment();
        //DrawSegment();
        
        StartCoroutine(UpdateLineFlowing());
    }

    void ClearSegments() {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }
    }

    void DrawSegment() {
        float cPercent = Mathf.Min(magnitudeForColor, 30) / 30;
        Color color = Color.Lerp(Color.blue, Color.red, cPercent);

        lr = Instantiate(pSegment, currentPos, new Quaternion(), this.transform).GetComponent<LineRenderer>();

        lr.startColor = color; lr.endColor = color;
        lr.SetPosition(0, currentPos);
        lr.SetPosition(1, currentPos + (line.normalized * segmentStep));
    }

    Vector3 UpdateLineSegment() {
        GameObject sensors = GameObject.Find("SensorContainer");
        Vector3 sum = Vector3.zero;
        float certaintyScore = 0;
        magnitudeForColor = 0;
        foreach (Transform g in sensors.transform) {
            if(!Physics.Linecast(currentPos, g.position)) {
                float dist = Vector3.Distance(currentPos, g.position);
                Vector3 contr = g.gameObject.GetComponent<Sensor>().sensorDirection.normalized * g.gameObject.GetComponent<Sensor>().sensorSpeed * (Mathf.Min(5, dist) / dist);
                sum += contr;
                magnitudeForColor += contr.magnitude;
                certaintyScore += g.gameObject.GetComponent<Sensor>().sensorSpeed * (Mathf.Min(5, dist) / dist);
                Debug.Log("Consider " + g.name + ": " + contr);
            }
        }
        if (certaintyScore > 0) {
            certainty = certaintyScore;
            sum /= certaintyScore;
            return sum;
        } else {
            return Vector3.zero;
        }
    }

    public IEnumerator UpdateLineFlowing() {
        currentPos = this.transform.position;
        ClearSegments();

        NodeSpawner ns = GameObject.Find("NodeContainer").GetComponent<NodeSpawner>();
        //Debug.Log(VectorWithin(currentPos, ns.minBounds, ns.maxBounds));
        while (VectorWithin(currentPos, ns.minBounds, ns.maxBounds) && Physics.OverlapSphere(currentPos, .1f).Length == 0) {
            Debug.Log(currentPos);
            line = UpdateLineSegment();
            DrawSegment();

            currentPos += line.normalized * segmentStep;
            yield return null;
        }
        yield break;
    }

    bool VectorWithin(Vector3 point, Vector3 low, Vector3 high) {
        
        return point.x >= low.x &&
            point.y >= low.y &&
            point.z >= low.z &&
            point.x <= high.x &&
            point.y <= high.y &&
            point.z <= high.z;
    }
}
