using System.Collections;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private float grappleLength = 5f;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LineRenderer rope;
    [SerializeField] private float ropeSpeed = 20f;
    [SerializeField] private float pullForce = 10f;
    [SerializeField] private GameObject pickaxe;
    [SerializeField] private GameObject pickaxeGrapple;
    [SerializeField] private float verticalMoveSpeed = 2f; // Adjust this to control the up/down speed

    private Vector3 grapplePoint;
    public bool isGrappling = false;
    private bool isAiming = false;
    private Vector3 aimDirection;
    private DistanceJoint2D joint;
    private Rigidbody2D playerRb;

    void Start()
    {
        rope.enabled = false; // Disable the rope initially
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        playerRb = GetComponent<Rigidbody2D>();
        pickaxeGrapple.SetActive(false);
    }

    void Update()
    {
        // Handle right mouse button press and release (for aiming and launching)
        if (Input.GetMouseButtonDown(1))
        {
            if (isGrappling)
            {
                // Retract the grappling hook if it's already attached
                StartCoroutine(RetractRope());
            }
            else
            {
                // Start aiming when the button is pressed
                isAiming = true;
            }
        }

        if (Input.GetMouseButtonUp(1) && isAiming)
        {
            // Launch the grappling hook when the right mouse button is released
            isAiming = false;
            LaunchGrapplingHook();
        }

        // Handle space key (only to release the grappling hook)
        if (Input.GetKeyDown(KeyCode.Space) && isGrappling)
        {
            // Only retract the grappling hook if we're currently grappling
            StartCoroutine(RetractRope());
        }

        // Update aiming visuals
        if (isAiming)
        {
            UpdateAimDirection();
        }

        // Keep rope attached while grappling
        if (isGrappling)
        {
            rope.SetPosition(0, transform.position);
            pickaxeGrapple.transform.position = rope.GetPosition(1);

            RotatePickaxeGrapple(); // Rotate the pickaxe towards the grapple point
            HandleVerticalMovement(); // Handle movement up and down the rope
        }
    }



    private void HandleVerticalMovement()
    {
        float verticalInput = Input.GetAxis("Vertical"); // 'W' and 'S' or arrow keys by default

        if (Mathf.Abs(verticalInput) > 0.1f) // Ensure there's enough input
        {
            // Adjust the joint's distance based on input and vertical movement speed
            joint.distance = Mathf.Clamp(joint.distance - verticalInput * verticalMoveSpeed * Time.deltaTime, 0.1f, grappleLength);
        }
    }

    private void RotatePickaxeGrapple()
    {
        // Calculate the direction from pickaxeGrapple to the grapple point
        Vector3 direction = grapplePoint - pickaxeGrapple.transform.position;

        // Ensure direction is valid
        if (aimDirection.sqrMagnitude > 0.01f)
        {
            // Calculate the angle in degrees
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            // Apply the rotation to the pickaxeGrapple
            pickaxeGrapple.transform.rotation = Quaternion.Euler(0, 0, angle - 45);
        }
    }

    private void UpdateAimDirection()
    {
        // Get the mouse position in world space and calculate direction
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure it's in 2D
        aimDirection = (mousePosition - transform.position).normalized;
    }

    private void LaunchGrapplingHook()
    {
        // Always launch the grappling hook towards the aim direction, regardless of a hit
        RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, grappleLength, grappleLayer);

        // Update the grapple point even if the ray doesn't hit anything
        grapplePoint = transform.position + aimDirection * grappleLength;

        // If it hits a valid target, attach the grappling hook and start pulling
        if (hit.collider != null)
        {
            grapplePoint = hit.point; // Update the grapple point to the hit location
            isGrappling = true;

            rope.SetPosition(1, grapplePoint);
            rope.enabled = true;

            joint.connectedAnchor = grapplePoint;
            joint.enabled = true;
            joint.distance = 0.1f;

            Vector2 pullDirection = (grapplePoint - transform.position).normalized;
            playerRb.AddForce(pullDirection * pullForce, ForceMode2D.Impulse);

            pickaxeGrapple.SetActive(true);
            pickaxe.SetActive(false);
        }
        else
        {
            // Even if it doesn't hit anything, allow the rope to appear and extend
            isGrappling = false;

            rope.SetPosition(1, grapplePoint);
            rope.enabled = true;
        }

        // Ensure the rope always extends until the maximum grapple length
        if (!isGrappling)
        {
            // Move the rope towards the grapple point (even if no collision occurred)
            rope.SetPosition(1, grapplePoint);
        }
    }


    private IEnumerator RetractRope()
    {
        isGrappling = false;
        joint.enabled = false;

        Vector3 retractPosition = transform.position;
        while (Vector3.Distance(rope.GetPosition(1), retractPosition) > 0.1f)
        {
            Vector3 currentPosition = rope.GetPosition(1);
            Vector3 nextPosition = Vector3.MoveTowards(currentPosition, retractPosition, ropeSpeed * Time.deltaTime);
            rope.SetPosition(1, nextPosition);

            pickaxeGrapple.transform.position = nextPosition;
            rope.SetPosition(0, transform.position);

            yield return null;
        }

        rope.enabled = false;
        pickaxeGrapple.SetActive(false);
        pickaxe.SetActive(true);
    }
}
