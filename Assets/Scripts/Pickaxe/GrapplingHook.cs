using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private float grappleLength = 5f;  // Dist�ncia m�xima de 5 tiles
    [SerializeField] private LayerMask grappleLayer;     // Layer para os blocos que podem ser agarrados
    [SerializeField] private LineRenderer rope;            // Linha do grappling hook
    [SerializeField] private float ropeSpeed = 20f;       // Velocidade da anima��o da corda
    [SerializeField] private float pullForce = 10f;       // For�a a ser aplicada ao jogador
    [SerializeField] private float cooldown = 0f;       // Cooldown para o grapple
    [SerializeField] private GameObject pickaxe;       // Pickaxe principal
    [SerializeField] private GameObject pickaxeGrapple;       // Pickaxe da ponta do Grapple

    private Vector3 grapplePoint;                          // Ponto onde o grappling hook acertou
    private bool isGrappling = false;                     // Indica se o grappling hook est� ativo
    private Vector3 ropeTargetPosition;                    // Posi��o atual do alvo da corda
    private bool grappleHit = false;                       // Armazena se atingiu um ponto v�lido
    private DistanceJoint2D joint;                        // Refer�ncia ao DistanceJoint2D
    private Rigidbody2D playerRb;                         // Refer�ncia ao Rigidbody2D do jogador

    void Start()
    {
        cooldown = 0f;
        rope.enabled = false; // Desabilita a linha inicialmente
        joint = GetComponent<DistanceJoint2D>();           // Obt�m o DistanceJoint2D do jogador
        joint.enabled = false;                              // Garante que o joint esteja desativado inicialmente
        playerRb = GetComponent<Rigidbody2D>();            // Obt�m o Rigidbody2D do jogador
        pickaxeGrapple.SetActive(false);
    }

    void Update()
    {
        // Se o grappling n�o estiver ativo, verifica a entrada do mouse para lan�ar a corda
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q)) && !isGrappling && cooldown <= 0)
        {
            StartGrappling();
        }

        // Atualiza a posi��o do ponto inicial da corda se estiver grappling
        if (isGrappling)
        {
            rope.SetPosition(0, transform.position);  // Mant�m o ponto inicial da corda na posi��o do jogador
            
            pickaxeGrapple.transform.position = rope.GetPosition(1); // Mant�m a picareta na posi�ao certa

            // Inicia a retra��o ap�s alcan�ar o ponto ou atingir o comprimento m�ximo
            if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.Q))
                StartCoroutine(RetractRope());

            if (Input.GetKey(KeyCode.W) && joint.distance > 1)
            {
                joint.distance -= 5f * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S) && joint.distance < grappleLength)
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

        cooldown -= Time.deltaTime;
    }

    private void StartGrappling()
    {
        cooldown = 100f;
        isGrappling = true; // Ativa o grappling

        pickaxeGrapple.SetActive(true);
        pickaxe.SetActive(false);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Define z como 0 para 2D

        // Calcula a dire��o do hook em dire��o ao cursor
        Vector3 direction = (mousePosition - transform.position).normalized;

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
        // Move o ponto final da corda em dire��o ao ponto de destino (grapplePoint)
        while (Vector3.Distance(ropeTargetPosition, grapplePoint) > 0.1f && isGrappling)
        {
            ropeTargetPosition = Vector3.MoveTowards(ropeTargetPosition, grapplePoint, ropeSpeed * Time.deltaTime);
            rope.SetPosition(1, ropeTargetPosition); // Atualiza a posi��o final da corda
            pickaxeGrapple.transform.position = ropeTargetPosition; // Por o pickaxe na ponta do grapple
            yield return null; // Espera at� o pr�ximo frame
        }

        // Se a corda atingiu um ponto v�lido
        if (grappleHit)
        {
            joint.connectedAnchor = grapplePoint; // Set the anchor to the grapple point
            joint.enabled = true;  // Enable the joint to pull player naturally towards the grapple point

            // Aplica a for�a ao jogador na dire��o do grapplePoint imediatamente
            Vector2 pullDirection = (grapplePoint - transform.position).normalized; // Dire��o correta
            playerRb.AddForce(pullDirection * pullForce, ForceMode2D.Impulse); // Aplica a for�a

            // Espera um pequeno tempo para que a anima��o de puxar ocorra
            yield return new WaitForSeconds(0.1f); // Reduzido para 0.1f para dar a sensa��o de puxar sem grande atraso

            // Ativa o joint apenas agora, para evitar puxar o jogador antes do tempo
            joint.connectedAnchor = grapplePoint; // Define o ponto de conex�o do joint
            joint.enabled = true; // Ativa o joint
        }
        else
        {
            StartCoroutine(RetractRope());
        }
    }

    // Coroutine para fazer a corda retornar ao jogador
    private IEnumerator RetractRope()
    {
        cooldown = 1f;
        // Mant�m o ponto inicial da corda na posi��o do jogador
        Vector3 initialRopePosition = transform.position; // Mant�m a posi��o inicial na posi��o do jogador
        isGrappling = false; // Define que n�o est� mais grappling

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

        rope.enabled = false; // Desabilita a linha ap�s a retra��o
        pickaxeGrapple.SetActive(false);
        pickaxe.SetActive(true);

    }
}