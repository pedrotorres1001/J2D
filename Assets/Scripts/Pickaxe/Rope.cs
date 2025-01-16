using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField, Range(2, 50)] int segmentsCount = 2;

    public Vector2 pointA;
    public Vector2 pointB;

    public HingeJoint2D hingePrefab;

    [HideInInspector] public Transform[] segments;

    LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();

        line.enabled = true;
        line.positionCount = segmentsCount;
    }

    void Update()
    {
        GenerateRope();

        for (int i = 0; i < segments.Length; i++) 
        { 
            line.SetPosition(i, segments[i].position);
        }
    }

    Vector2 GetSegmentPosition(int segmentIndex)
    {
        Vector2 posA = pointA;
        Vector2 posB = pointB;

        float fraction = 1f / segmentsCount;
        return Vector2.Lerp(posA, posB, fraction * segmentIndex);
    }


    void GenerateRope()
    {
        segments = new Transform[segmentsCount];

        for (int i = 0; i < segmentsCount; i++)
        {
            var currJoint = Instantiate(hingePrefab, GetSegmentPosition(i), Quaternion.identity, this.transform); 
            segments[i] = currJoint.transform;

            if (i > 0) // not first hinge
            {
                int prevIndex = i - 1;
                currJoint.connectedBody = segments[prevIndex].GetComponent<Rigidbody2D>();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (pointA == null || pointB == null) return;
        Gizmos.color = Color.green;
        for (int i = 0; i < segmentsCount; i++)
        {
            Vector2 posAtIndex = GetSegmentPosition(i);
            Gizmos.DrawSphere(posAtIndex, 0.1f);
        }
    }
}
