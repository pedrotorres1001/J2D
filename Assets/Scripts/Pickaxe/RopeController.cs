using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    public GameObject anchor, endPoint;
    public GameObject[] segments;
    public LineRenderer lineRenderer;

    [SerializeField, Range(0.01f, 1.5f)] public float size = 1.0f;
    [SerializeField] bool isGrapple;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments.Length;
        if (isGrapple)
            lineRenderer.positionCount = segments.Length + 1;
    }

    private void Update()
    {
        Resize();

        int cont = 0;
        foreach (var segment in segments)
        {
            lineRenderer.SetPosition(cont, segment.transform.position);
            cont++;
        }

        if (isGrapple)
            lineRenderer.SetPosition(cont, GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    public void Resize()
    {
        foreach (var segment in segments)
        {
            segment.transform.localScale = new Vector3(1, size, 1);
        }
    }

}
