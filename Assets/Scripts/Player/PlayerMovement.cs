using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float stairsForce;
    [SerializeField] bool isGrounded;

    [SerializeField] float groundCheckDistance;
    [SerializeField] float wallCheckDistance;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] float gravity;
    [SerializeField] float wallSlideSpeed;
    [SerializeField] float wallSlideDuration = 2f; // Limit slide time

    [SerializeField] GameObject player;

    [SerializeField] ParticleSystem dust;
    public float interval = 1f; // Intervalo em segundos
    private float timer = 0f; // Temporizador interno

    [SerializeField] AudioManager audioManager;

    public float knockbackForce;
    public float knockbackCounter;
    public float knockbackTotalTime;
    public bool knockbackFromRight;

    private Animator animator;
    private Rigidbody2D rb;
    public float lastDirection = 1;

    private bool upPressed;

    public float direction;
    public float altitude;

    private bool isGrappling = false;
    private bool isSliding = false;
    private float slideTimer;
    private bool isInvulnerable = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastDirection = -1;
        animator.SetFloat("LastDirection", lastDirection);
    }

    void Update()
    {

        if(player.GetComponent<GrapplingHook>() != null)
        {
            isGrappling = player.GetComponent<GrapplingHook>().isGrappling;
        }
        
        direction = Input.GetAxisRaw("Horizontal");
        altitude = Input.GetAxisRaw("Vertical");

        CheckGround();

        if (direction != 0)
        {
            animator.SetFloat("Direction", direction);
            animator.SetFloat("LastDirection", lastDirection);
            lastDirection = direction;
            
            if(isGrounded)
                animator.SetBool("IsWalking", true);
                //audioManager.Play("walk");
            
        }

        timer += Time.deltaTime;

        if (timer >= interval && direction != 0 && isGrounded)
        {
            dust.Play(); // Reproduz as partículas
            timer = 0f; // Reseta o temporizador
        }

        if (Input.GetKeyDown(KeyManager.KM.jump) && isGrounded)
        {
            lastDirection = 2;
        }
        else if (Input.GetKeyDown(KeyManager.KM.moveleft)) 
        {
            lastDirection = 3;
        }

        if (direction == 0)
        {
            animator.SetFloat("Direction", direction);
            animator.SetFloat("LastDirection", lastDirection);
            animator.SetBool("IsWalking", false);
        }

        if (Input.GetKeyDown(KeyManager.KM.jump) && isGrounded)
        {
            dust.Play(); 
            Jump();
        }

        if(isGrappling) 
        {
            animator.SetBool("IsGrappling", true);
        }
        else if(!isGrappling)
        {
            animator.SetBool("IsGrappling", false);
        }

        HandleWallSlide();
        
    }

    private void FixedUpdate()
    {
        if (!isSliding && knockbackCounter <= 0)
        {
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        }
        

        if(knockbackCounter > 0)
        {
            if(knockbackFromRight) 
            {
                rb.velocity = new Vector2(-knockbackForce, knockbackForce);
            }
            if(!knockbackFromRight)
            {
                rb.velocity = new Vector2(knockbackForce, knockbackForce);
            }

            knockbackCounter -= Time.deltaTime;
            knockbackCounter = Mathf.Max(knockbackCounter, 0); // Clamp to zero
        }

        if (upPressed)
        {
            rb.velocity = new Vector3(rb.velocity.x, altitude * speed);
            rb.gravityScale = 0; 
            upPressed = false;
        }
        else
        {
            rb.gravityScale = gravity;
        }
    }

    public bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;
        return isGrounded;
    }

    private float CheckGroundDistance()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, groundLayer);
        if (hit.collider != null)
        {
            return hit.distance;
        }
        return Mathf.Infinity;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false; // Ensures that jump only happens once per ground contact
    }

    private void HandleWallSlide()
    {
        // Raycasts para verificar se há uma parede à esquerda e à direita
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, groundLayer);

        // Verifica a distância do jogador em relação ao chão
        float groundDistance = CheckGroundDistance();

        // Verifica se o jogador está a mais de 2 unidades acima do chão e se ele não está tocando o chão
        if (!isGrounded && (hitRight.collider != null || hitLeft.collider != null) && rb.velocity.y < 0 && groundDistance > 2f)
        {
            if (!isSliding)
            {
                isSliding = true;
                slideTimer = wallSlideDuration;
            }

            slideTimer -= Time.deltaTime;

            // Se pressionar para a direita e estiver tocando a parede direita, libera a parede
            if (direction > 0)
            {
                // Libera a parede e começa a andar para a direita
                isSliding = false;
                rb.velocity = new Vector2(speed, rb.velocity.y); // Começa a se mover para a direita
            }
            // Se pressionar para a esquerda e estiver tocando a parede esquerda, libera a parede
            else if (direction < 0)
            {
                // Libera a parede e começa a andar para a esquerda
                isSliding = false;
                rb.velocity = new Vector2(-speed, rb.velocity.y); // Começa a se mover para a esquerda
            }

            // Se ainda estiver tocando a parede, mas não pressionando nada, mantém a velocidade de deslize
            if (hitRight.collider != null)
            {
                animator.SetFloat("LastDirection", 1); // Definir animação para a direita
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed)); // Deslizar para a direita
            }
            else if (hitLeft.collider != null)
            {
                animator.SetFloat("LastDirection", -1); // Definir animação para a esquerda
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed)); // Deslizar para a esquerda
            }

            animator.SetBool("IsSliding", true); // Ativar animação de deslizamento
        }
        else
        {
            // Se não há paredes ou o jogador está no chão, parar o deslize
            isSliding = false;
            animator.SetBool("IsSliding", false);
        }
    }


    private bool IsTouchingWall()
    {
        Vector2 directionToCheck = Vector2.right * Mathf.Sign(direction);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToCheck, wallCheckDistance, groundLayer);
        return hit.collider != null;
    }
}