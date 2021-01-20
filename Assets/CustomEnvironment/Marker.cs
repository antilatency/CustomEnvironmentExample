using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Marker : MonoBehaviour
{
    #if UNITY_EDITOR
    public void OnDrawGizmos(){
        float size = 0.05f;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        Handles.color = Color.white;
        Handles.CircleCap(0, transform.position, Quaternion.LookRotation(Vector3.up, Vector3.right), size);
    }
    #endif
}
