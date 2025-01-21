using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.ReloadAttribute;

public class GrapplingHook : MonoBehaviour
{
    private Vector3 startPoint;                       
    private Vector3 grapplePoint;
    public bool isGrappling = false;             
    private bool stopGrappling = false;
    private Vector3 direction;
    private DistanceJoint2D joint;
    private bool isGrappleMoving = false;
    public bool hit = false;

    [SerializeField] private float ropeSpeed = 20f;    
    [SerializeField] private float grappleLength = 20f;
    [SerializeField] private float grappleDistance;
    [SerializeField] private GameObject pickaxe;
    [SerializeField] private GameObject pickaxeGrapple;
    [SerializeField] private GameObject rope;          
    [SerializeField] private Rope currentRope;

    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        pickaxeGrapple.SetActive(false);
    }

    void Update()
    {
        // Se o grappling n�o estiver ativo, verifica a entrada do mouse para lan�ar a corda
        if ((Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.E)) && !isGrappling)
        {
            stopGrappling = false;
            StartGrappling();
        }

        if (!isGrappleMoving && (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.E)))
        {
            stopGrappling = true;
            StartCoroutine(RetractRope());
        }
        else if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.E))
        {
            stopGrappling = true;
        }

        if (isGrappling && !isGrappleMoving)
        {
            currentRope.EndPoint.position = transform.position;
            currentRope.ropeSegLen = Vector3.Distance(currentRope.StartPoint.position, currentRope.EndPoint.position) / currentRope.segmentLength;

            if (Input.GetKey(KeyCode.W) && joint.distance > 1)
            {
                joint.distance -= 5f * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S) && joint.distance < grappleLength && grapplePoint.y > transform.position.y && Mathf.Abs(grapplePoint.x - transform.position.x) < 3)
            {
                joint.distance += 5f * Time.deltaTime;
            }
        }
    }

    private void StartGrappling()
    {
        isGrappling = true;
        isGrappleMoving = true;
        pickaxeGrapple.SetActive(true);
        pickaxe.SetActive(false);
        
        currentRope = Instantiate(rope).GetComponent<Rope>();
        currentRope.StartPoint = pickaxeGrapple.transform;
        currentRope.EndPoint = transform;

        pickaxeGrapple.transform.position = transform.position;
        startPoint = transform.position;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 

        direction = (mousePosition - transform.position).normalized;

        currentRope.ropeSegLen = Vector3.Distance(currentRope.StartPoint.position, currentRope.EndPoint.position) / currentRope.segmentLength;

        // Roda a picareta para a dire�ao que foi lan�ada
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        pickaxeGrapple.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        pickaxeGrapple.transform.Rotate(0, 0, -45);

        StartCoroutine(MoveRope());
    }

    private IEnumerator MoveRope()
    {        
        while (Vector3.Distance(startPoint, pickaxeGrapple.transform.position) <= grappleDistance && isGrappling && !hit)
        {
            pickaxeGrapple.transform.position += direction * ropeSpeed * Time.deltaTime;
            currentRope.ropeSegLen = Vector3.Distance(currentRope.StartPoint.position, currentRope.EndPoint.position) / currentRope.segmentLength;
            yield return null; 
        }

        isGrappleMoving = false;

        if (hit && !stopGrappling)
        {
            joint.connectedAnchor = pickaxeGrapple.transform.position;
            joint.enabled = true; 


            joint.distance = 0.1f; 

            while (isGrappling && Vector3.Distance(currentRope.EndPoint.position, currentRope.StartPoint.position) > 0.1 && direction == (transform.position - pickaxeGrapple.transform.position).normalized)
            {
                currentRope.EndPoint.position += direction * ropeSpeed * Time.deltaTime;
                currentRope.ropeSegLen = Vector3.Distance(startPoint, pickaxeGrapple.transform.position) / currentRope.segmentLength;
                yield return null; 
            }

            //joint.connectedAnchor = grapplePoint;
        }
        else if (hit)
        {
            joint.connectedAnchor = pickaxeGrapple.transform.position;
            joint.enabled = true;


            joint.distance = 0.1f;

            while (isGrappling && Vector3.Distance(currentRope.EndPoint.position, currentRope.StartPoint.position) > 0.1 && direction == (transform.position - pickaxeGrapple.transform.position).normalized)
            {
                currentRope.EndPoint.position += direction * ropeSpeed * Time.deltaTime;
                currentRope.ropeSegLen = Vector3.Distance(currentRope.StartPoint.position, currentRope.EndPoint.position) / currentRope.segmentLength;
                yield return null;
            }

            StartCoroutine(RetractRope()); // Retraí a corda se não houver um ponto válido
        }
        else
        {
            StartCoroutine(RetractRope()); // Retraí a corda se não houver um ponto válido
        }

    }

    // Coroutine para fazer a corda voltar ao jogador
    private IEnumerator RetractRope()
    {
        direction = (transform.position - pickaxeGrapple.transform.position).normalized;

        while (Vector3.Distance(transform.position, pickaxeGrapple.transform.position) > 0.1 && direction == (transform.position - pickaxeGrapple.transform.position).normalized)
        {
            pickaxeGrapple.transform.position += direction * ropeSpeed * Time.deltaTime;
            currentRope.ropeSegLen = Vector3.Distance(currentRope.StartPoint.position, currentRope.EndPoint.position) / currentRope.segmentLength;
            yield return null; // Espera até o próximo frame
        }

        joint.enabled = false;

        isGrappling = false;
        hit = false;
        pickaxeGrapple.SetActive(false);
        pickaxe.SetActive(true);
        Destroy(currentRope.gameObject);
        yield return null;
    }

    public void RemoveGrapple()
    {
        StartCoroutine(RetractRope());
    }
}