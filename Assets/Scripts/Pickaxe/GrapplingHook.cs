using System.Collections;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private float grappleLength = 5f;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LineRenderer rope;
    [SerializeField] private float ropeSpeed = 20f;
    [SerializeField] private float pullForce = 10f;
    [SerializeField] private float launchCooldown;
    [SerializeField] private float retractCooldown;
    [SerializeField] private float attachedCooldown;
    [SerializeField] private GameObject pickaxe;
    [SerializeField] private GameObject pickaxeGrapple;
    [SerializeField] private float moveSpeed = 5f; // Speed for horizontal and vertical movement.
    

    private Vector3 grapplePoint;
    public bool isGrappling = false;
    private bool isGrappleMoving = false;
    private bool stopGrappling = false;
    private Vector3 ropeTargetPosition;
    private bool grappleHit = false;
    private DistanceJoint2D joint;
    private Rigidbody2D playerRb;
    private PlayerMovement movement;
    private float defaultGravityScale;


    void Start()
    {
        launchCooldown = 0f;
        retractCooldown = 0f;
        attachedCooldown = 0f;
        rope.enabled = false;
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        playerRb = GetComponent<Rigidbody2D>();
        pickaxeGrapple.SetActive(false);
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        defaultGravityScale = playerRb.gravityScale; // Store default gravity

    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.L)) && !isGrappling && launchCooldown == 0)
        {
            Vector3 direction = GetDirectionFromInput();
            if (direction != Vector3.zero)
            {
                stopGrappling = false;
                StartGrappling(direction);
            }
        }

        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.L))
        {
            stopGrappling = true;
        }

        if (isGrappling)
        {
            rope.SetPosition(0, transform.position);
            pickaxeGrapple.transform.position = rope.GetPosition(1);

            if (!isGrappleMoving && (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.L)))
            {
                stopGrappling = true;
                StartCoroutine(RetractRope());
            }

            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && joint.distance > 1 && grapplePoint.y > transform.position.y)
            {
                joint.distance -= 5f * Time.deltaTime;
            }
            // Permita aumentar a distância apenas se o ponto de grapple está acima do jogador
            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && 
                joint.distance < grappleLength && 
                grapplePoint.y > transform.position.y) // Só se o ponto estiver acima
            {
                joint.distance += 5f * Time.deltaTime;
            }

            // Horizontal movement while grappling
            float horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
            playerRb.velocity = new Vector2(horizontalInput * moveSpeed, playerRb.velocity.y);

            // Limit the distance horizontally from the grapple point
            Vector2 toGrapplePoint = grapplePoint - transform.position;
            if (toGrapplePoint.magnitude > joint.distance)
            {
                Vector2 directionBack = toGrapplePoint.normalized;
                playerRb.AddForce(directionBack * pullForce * Time.deltaTime, ForceMode2D.Force);
            }
        }
        else
        {
            joint.enabled = false;
            rope.enabled = false;
            pickaxeGrapple.SetActive(false);
            pickaxe.SetActive(true);
        }

        launchCooldown = Mathf.Max(0, launchCooldown - Time.deltaTime);
    }

    private Vector3 GetDirectionFromInput()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            direction += Vector3.up;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            direction += Vector3.down;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            direction += Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            direction += Vector3.right;

        return direction.normalized;
    }

    private void StartGrappling(Vector3 direction)
    {
        launchCooldown = attachedCooldown;

        isGrappling = true;
        isGrappleMoving = true;

        pickaxeGrapple.SetActive(true);
        pickaxe.SetActive(false);

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pickaxeGrapple.transform.rotation = Quaternion.Euler(0, 0, angle - 45);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, grappleLength, grappleLayer);

        if (hit.collider != null)
        {
            grapplePoint = hit.point;
            grappleHit = true;
        }
        else
        {
            grapplePoint = transform.position + direction * grappleLength;
            grappleHit = false;
        }

        ropeTargetPosition = transform.position;
        rope.SetPosition(0, transform.position);
        rope.SetPosition(1, transform.position);
        rope.enabled = true;

        StartCoroutine(MoveRope());
    }

    private IEnumerator MoveRope()
    {
        while (Vector3.Distance(ropeTargetPosition, grapplePoint) > 0.1f && isGrappling)
        {
            ropeTargetPosition = Vector3.MoveTowards(ropeTargetPosition, grapplePoint, ropeSpeed * Time.deltaTime);
            rope.SetPosition(1, ropeTargetPosition);
            pickaxeGrapple.transform.position = ropeTargetPosition;
            yield return null;
        }

        isGrappleMoving = false;

        if (grappleHit && !stopGrappling)
        {
            joint.connectedAnchor = grapplePoint;
            joint.enabled = true;

            joint.distance = 0.1f;

            Vector2 pullDirection = (grapplePoint - transform.position).normalized;
            playerRb.AddForce(pullDirection * pullForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.1f);

            while (isGrappling)
            {
                yield return null;
            }
        }
        else
        {
            StartCoroutine(RetractRope());
        }
    }

    private IEnumerator RetractRope()
    {
        isGrappleMoving = true;

        retractCooldown = 1f;
        Vector3 initialRopePosition = transform.position;

        joint.enabled = false;

        while (Vector3.Distance(ropeTargetPosition, initialRopePosition) > 0.1f)
        {
            ropeTargetPosition = Vector3.MoveTowards(ropeTargetPosition, initialRopePosition, ropeSpeed * Time.deltaTime);
            rope.SetPosition(1, ropeTargetPosition);
            pickaxeGrapple.transform.position = ropeTargetPosition;
            rope.SetPosition(0, transform.position);

            yield return null;
        }

        isGrappleMoving = false;
        stopGrappling = false;
        launchCooldown = .001f;
        isGrappling = false;
        rope.enabled = false;
        pickaxeGrapple.SetActive(false);
        pickaxe.SetActive(true);
    }

    public void RemoveGrapple()
    {
        StartCoroutine(RetractRope());
    }
}