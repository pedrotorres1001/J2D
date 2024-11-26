using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private Vector3 grapplePoint;                       
    public bool isGrappling = false;                     
    private bool isGrappleMoving = false;
    private bool stopGrappling = false;
    private Vector3 ropeTargetPosition;              
    private bool grappleHit = false;                    
    private DistanceJoint2D joint;               
    private Rigidbody2D playerRb;  
    private PlayerMovement movement;
    private AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        launchCooldown = 0f;
        retractCooldown = 0f;
        attachedCooldown = 0f;
        rope.enabled = false; // Desabilita a linha inicialmente
        joint = GetComponent<DistanceJoint2D>();           // Obt�m o DistanceJoint2D do jogador
        joint.enabled = false;                              // Garante que o joint esteja desativado inicialmente
        playerRb = GetComponent<Rigidbody2D>();            // Obt�m o Rigidbody2D do jogador
        pickaxeGrapple.SetActive(false);
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Se o grappling n�o estiver ativo, verifica a entrada do mouse para lan�ar a corda
        if ((Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.E)) && !isGrappling && launchCooldown == 0)
        {
            stopGrappling = false;
            StartGrappling();
        }

        if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.E))
        {
            stopGrappling = true;
        }

        // Atualiza a posi��o do ponto inicial da corda se estiver grappling
        if (isGrappling)
        {
            float angle = Mathf.Tan((transform.position.x - grapplePoint.x) / (transform.position.y - grapplePoint.y));
            Debug.Log(angle);

            rope.SetPosition(0, transform.position);  // Mant�m o ponto inicial da corda na posi��o do jogador
            
            pickaxeGrapple.transform.position = rope.GetPosition(1); // Mant�m a picareta na posi�ao certa

            // Inicia a retra��o ap�s alcan�ar o ponto ou atingir o comprimento m�ximo
            if (!isGrappleMoving && (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.E)))
            {
                stopGrappling = true;
                StartCoroutine(RetractRope());
            }

            if (Input.GetKey(KeyCode.W) && joint.distance > 1)
            {
                joint.distance -= 5f * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S) && joint.distance < grappleLength && grapplePoint.y > transform.position.y && Mathf.Abs(grapplePoint.x - transform.position.x) < 3)
            {
                joint.distance += 5f * Time.deltaTime;
            }
        }
        else
        {
            joint.enabled = false;
            rope.enabled = false;
            pickaxeGrapple.SetActive(false);
            pickaxe.SetActive(true);
        }

        launchCooldown = Mathf.Max(0, launchCooldown - Time.deltaTime);;
    }

    private void StartGrappling()
    {
        launchCooldown = attachedCooldown;

        audioManager.PlaySFX(audioManager.swing);
        
        isGrappling = true; // Ativa o grappling
        isGrappleMoving = true;


        pickaxeGrapple.SetActive(true);
        pickaxe.SetActive(false);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Define z como 0 para 2D

        // Calcula a dire��o do hook em dire��o ao cursor
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Ajusta a direção do player com base na direção do clique
        if (mousePosition.x > transform.position.x)
        {
            movement.lastDirection = 1;
        }
        else
        {
            movement.lastDirection = -1;
        }

        // Roda a picareta para a dire�ao que foi lan�ada
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        pickaxeGrapple.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        pickaxeGrapple.transform.Rotate(0, 0, -45);

        // Realiza o Raycast na dire��o calculada e na dist�ncia de grappleLength
        RaycastHit2D hit = Physics2D.Raycast(
            origin: transform.position,
            direction: direction,
            distance: grappleLength,
            layerMask: grappleLayer
        );

        // Se encontrou um ponto v�lido, define o grapplePoint para onde o hook ir�
        if (hit.collider != null)
        {
            grapplePoint = hit.point; // Atualiza o ponto de grappling
            grappleHit = true; // Marcamos que acertou um ponto v�lido
        }
        else
        {
            grapplePoint = transform.position + direction * grappleLength; // Se n�o, vai at� o comprimento m�ximo
            grappleHit = false; // N�o acertou
        }

        ropeTargetPosition = transform.position; // Posi��o inicial da corda
        rope.SetPosition(0, transform.position);  // Define a posi��o inicial da corda
        rope.SetPosition(1, transform.position);  // Define a posi��o final da corda
        rope.enabled = true;                      // Habilita a linha

        // Inicia a coroutine para movimentar a corda
        StartCoroutine(MoveRope());
        
    }

    private IEnumerator MoveRope()
    {
        // Move o ponto final da corda em direção ao ponto de destino (grapplePoint)
        while (Vector3.Distance(ropeTargetPosition, grapplePoint) > 0.1f && isGrappling)
        {
            ropeTargetPosition = Vector3.MoveTowards(ropeTargetPosition, grapplePoint, ropeSpeed * Time.deltaTime);
            rope.SetPosition(1, ropeTargetPosition); // Atualiza a posição final da corda
            pickaxeGrapple.transform.position = ropeTargetPosition; // Posiciona a picareta no ponto do grapple
            yield return null; // Espera até o próximo frame
        }

        isGrappleMoving = false;

        if (grappleHit)
            audioManager.PlaySFX(audioManager.hitRock);


        // Se a corda atingiu um ponto válido
        if (grappleHit && !stopGrappling)
        {
            joint.connectedAnchor = grapplePoint; // Define o ponto de ancoragem
            joint.enabled = true;  // Ativa o joint

            // Define a distância para o joint, garantindo que fique próximo do jogador
            joint.distance = 0.1f; // Define a distância mínima

            // Aplica uma força imediata para puxar o jogador em direção ao ponto de grappling
            Vector2 pullDirection = (grapplePoint - transform.position).normalized; // Direção correta
            playerRb.AddForce(pullDirection * pullForce, ForceMode2D.Impulse); // Aplica a força de impulso

            // Espera um pequeno tempo para que a animação de puxar ocorra
            yield return new WaitForSeconds(0.1f); // Pequena espera para estabilizar

            // Aplica uma força contínua enquanto o jogador está grappling
            while (isGrappling)
            {
                playerRb.AddForce(pullDirection * pullForce * Time.deltaTime, ForceMode2D.Force); // Força contínua
                yield return null; // Espera até o próximo frame
            }
        }
        else if (grappleHit)
        {
            joint.connectedAnchor = grapplePoint; // Define o ponto de ancoragem
            joint.enabled = true;  // Ativa o joint

            // Define a distância para o joint, garantindo que fique próximo do jogador
            joint.distance = 0.1f; // Define a distância mínima

            // Aplica uma força imediata para puxar o jogador em direção ao ponto de grappling
            Vector2 pullDirection = (grapplePoint - transform.position).normalized; // Direção correta
            playerRb.AddForce(pullDirection * pullForce, ForceMode2D.Impulse); // Aplica a força de impulso

            // Espera um pequeno tempo para que a animação de puxar ocorra
            yield return new WaitForSeconds(0.1f); // Pequena espera para estabilizar

            StartCoroutine(RetractRope()); // Retraí a corda se não houver um ponto válido

        }
        else
        {
            StartCoroutine(RetractRope()); // Retraí a corda se não houver um ponto válido
        }
    }

    // Coroutine para fazer a corda retornar ao jogador
    private IEnumerator RetractRope()
    {
        isGrappleMoving = true;
        Debug.Log("Retracting rope");


        retractCooldown = 1f;
        // Mant�m o ponto inicial da corda na posi��o do jogador
        Vector3 initialRopePosition = transform.position; // Mant�m a posi��o inicial na posi��o do jogador

        // Desabilita o joint ap�s a retra��o da corda
        joint.enabled = false;

        // Volta a corda para a posi��o do jogador
        while (Vector3.Distance(ropeTargetPosition, initialRopePosition) > 0.1f)
        {
            ropeTargetPosition = Vector3.MoveTowards(ropeTargetPosition, initialRopePosition, ropeSpeed * Time.deltaTime);
            rope.SetPosition(1, ropeTargetPosition); // Atualiza a posi��o final da corda
            pickaxeGrapple.transform.position = ropeTargetPosition; // Por o pickaxe na ponta do grapple

            // Mant�m o ponto inicial da corda na posi��o do jogador
            rope.SetPosition(0, transform.position); // Atualiza a posi��o inicial da corda

            yield return null; // Espera at� o pr�ximo frame
        }

        isGrappleMoving = false;
        stopGrappling = false;
        launchCooldown = .001f;
        isGrappling = false; // Define que n�o est� mais grappling
        rope.enabled = false; // Desabilita a linha ap�s a retra��o
        pickaxeGrapple.SetActive(false);
        pickaxe.SetActive(true);

    }

    public void RemoveGrapple()
    {
        StartCoroutine(RetractRope());
    }
}