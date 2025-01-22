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
    public bool hit = false;
    public bool reachedPosition = false;
    private bool isGrappleMoving = false;
    public DistanceJoint2D grappleJoint;

    private float lastLaunch;

    [SerializeField] private float ropeSpeed = 20f;    
    [SerializeField] private float grappleLength = 20f;
    [SerializeField] private float grappleDistance;
    [SerializeField] private GameObject pickaxe;
    [SerializeField] private GameObject pickaxeGrapple;
    [SerializeField] private GameObject pickaxeGrapplePrefab;
    [SerializeField] private GameObject rope;          
    [SerializeField] private Rope currentRope;

    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
    }

    void Update()
    {
        PlayerMovement playerMovement = gameObject.GetComponent<PlayerMovement>();

        if (Input.GetKeyDown(KeyManager.KM.grapplinghook) && !isGrappling)
        {
            stopGrappling = false;
            StartGrappling();
        }

        switch (isGrappling)
        {
            case true:

                float movementDirection = Input.GetAxisRaw("Horizontal");
                if (movementDirection > 0)
                {
                    Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
                    rb.velocity = new Vector2(rb.velocity.x + playerMovement.speed * Time.deltaTime, rb.velocity.y);
                }
                else if (movementDirection < 0)
                {
                    Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
                    rb.velocity = new Vector2(rb.velocity.x + -playerMovement.speed * Time.deltaTime, rb.velocity.y);
                }

                if (!isGrappleMoving && !reachedPosition && Vector3.Distance(transform.position, pickaxeGrapple.transform.position) <= 0.5)
                {
                    reachedPosition = true;
                }

                if (Input.GetKeyDown(KeyManager.KM.grapplinghook) && reachedPosition)
                {
                    stopGrappling = true;
                    direction = (transform.position - pickaxeGrapple.transform.position).normalized;
                    StartCoroutine(RetractRope());
                }
                else if (isGrappling && Input.GetKeyUp(KeyManager.KM.grapplinghook))
                {
                    stopGrappling = true;
                }

                if (stopGrappling && reachedPosition && !isGrappleMoving)
                {
                    direction = (transform.position - pickaxeGrapple.transform.position).normalized;
                    StartCoroutine(RetractRope());
                }

                if (reachedPosition)
                {
                    currentRope.EndPoint.position = transform.position;
                    currentRope.ropeSegLen = Vector3.Distance(currentRope.StartPoint.position, currentRope.EndPoint.position) / currentRope.segmentLength;

                    if (Input.GetKey(KeyManager.KM.moveup) && joint.distance > 1)
                    {
                        joint.distance -= 5f * Time.deltaTime;
                    }
                    else if (Input.GetKey(KeyManager.KM.movedown) && joint.distance < grappleLength && grapplePoint.y > transform.position.y && Mathf.Abs(pickaxeGrapple.transform.position.x - transform.position.x) < 3)
                    {
                        joint.distance += 5f * Time.deltaTime;
                    }
                }
                if (!isGrappleMoving && isGrappling && !reachedPosition && Vector3.Distance(transform.position, pickaxeGrapple.transform.position) <= 0.5)
                {
                    reachedPosition = true;
                }

                if (Input.GetKeyDown(KeyManager.KM.grapplinghook) && reachedPosition)
                {
                    stopGrappling = true;
                    direction = (transform.position - pickaxeGrapple.transform.position).normalized;
                    StartCoroutine(RetractRope());
                }
                else if (isGrappling && Input.GetKeyUp(KeyManager.KM.grapplinghook))
                {
                    stopGrappling = true;
                }

                if (stopGrappling && reachedPosition && !isGrappleMoving)
                {
                    direction = (transform.position - pickaxeGrapple.transform.position).normalized;
                    StartCoroutine(RetractRope());
                }

                if (reachedPosition)
                {
                    currentRope.EndPoint.position = transform.position;
                    currentRope.ropeSegLen = Vector3.Distance(currentRope.StartPoint.position, currentRope.EndPoint.position) / currentRope.segmentLength;

                    if (Input.GetKey(KeyManager.KM.moveup) && joint.distance > 1)
                    {
                        joint.distance -= 5f * Time.deltaTime;
                    }
                    else if (Input.GetKey(KeyManager.KM.movedown) && joint.distance < grappleLength && grapplePoint.y > transform.position.y && Mathf.Abs(pickaxeGrapple.transform.position.x - transform.position.x) < 3)
                    {
                        joint.distance += 5f * Time.deltaTime;
                    }
                }

                if (hit)
                {
                    joint.connectedAnchor = pickaxeGrapple.transform.position;
                }

                break;
            default:

                if (playerMovement.CheckGround())
                {
                    playerMovement.enabled = true;
                }

                break;
        }
    }

    private void StartGrappling()
    {
        isGrappling = true;
        isGrappleMoving = true;
        reachedPosition = false;
        reachedPosition = false;
        pickaxeGrapple = Instantiate(pickaxeGrapplePrefab);
        grappleJoint = pickaxeGrapple.GetComponent<DistanceJoint2D>();
        grappleJoint.enabled = false;
        pickaxe.SetActive(false);
        pickaxeGrapple.SetActive(true);

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

        lastLaunch = Time.time;

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

        if (hit)
        {
            joint.connectedAnchor = pickaxeGrapple.transform.position;
            joint.enabled = true;

            PlayerMovement playerMovement = gameObject.GetComponent<PlayerMovement>();
            playerMovement.enabled = false;


            joint.distance = 0.1f;

            while (isGrappling && Vector3.Distance(currentRope.EndPoint.position, currentRope.StartPoint.position) > 0.1 && direction == (transform.position - pickaxeGrapple.transform.position).normalized)
            {
                if (Time.time - lastLaunch >= 2)
                {
                    isGrappling = false;
                    StartCoroutine(RetractRope());
                }

                currentRope.EndPoint.position += direction * (ropeSpeed / 2) * Time.deltaTime;
                currentRope.ropeSegLen = Vector3.Distance(startPoint, pickaxeGrapple.transform.position) / currentRope.segmentLength;
                yield return null; 
            }
        }
        else
        {
            direction = (transform.position - pickaxeGrapple.transform.position).normalized;
            StartCoroutine(RetractRope()); 
        }
        yield break;
    }

    private IEnumerator RetractRope()
    {
        while (pickaxeGrapple != null
            && Vector3.Distance(transform.position, pickaxeGrapple.transform.position) > 0.1f
            && direction == (transform.position - pickaxeGrapple.transform.position).normalized)
        {
            if (pickaxeGrapple == null)
                yield break; // Exit the coroutine if the object is destroyed

            pickaxeGrapple.transform.position += direction * ropeSpeed * Time.deltaTime;
            currentRope.ropeSegLen = Vector3.Distance(currentRope.StartPoint.position, currentRope.EndPoint.position) / currentRope.segmentLength;
            yield return null;
        }

        // Safeguard in case the object is destroyed before this point
        if (pickaxeGrapple == null)
            yield break;

        joint.enabled = false;

        isGrappling = false;
        stopGrappling = false;
        hit = false;

        Destroy(pickaxeGrapple);
        pickaxeGrapple = null;

        pickaxe.SetActive(true);
        Destroy(currentRope.gameObject);
    }
}