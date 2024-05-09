using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpawner : MonoBehaviour
{
    public Vector3 minBounds, maxBounds;

    public float step = 0.25f;

    List<Vector3> places= new List<Vector3>();

    [SerializeField]
    GameObject pNode;

    private void OnEnable() {
        minBounds = new Vector3(-10, 0, -10);
        maxBounds = new Vector3(10, 13, 10);
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn() {

        for (float xp = minBounds.x; xp < maxBounds.x; xp += step) { 
            for (float yp = minBounds.y; yp < maxBounds.y; yp += step) { 
                for (float zp = minBounds.z; zp < maxBounds.z; zp += step) { 
                    Vector3 pos = new Vector3(xp, yp, zp);
                    Collider[] cols = Physics.OverlapSphere(pos, .1f);
                    //Debug.Log(cols.Length);
                    bool flag = false;
                    foreach (Collider col in cols) {
                        if (col.gameObject.CompareTag("Solid")) {
                            flag = true;
                        }
                    }

                    if (!flag) {
                        //Debug.Log('y');
                        Instantiate(pNode, pos, new Quaternion(), this.transform);
                    }
                    places.Add(pos);

                    Debug.Log(pos);
                }
            }
        }

        yield break;
    }

    private void OnDrawGizmos() {
        foreach (Vector3 pos in places) {
            Gizmos.DrawWireSphere(pos, .1f);
        }
    }
}
