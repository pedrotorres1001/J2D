using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private float grappleLength = 5f;  // Distância máxima de 5 tiles
    [SerializeField] private LayerMask grappleLayer;     // Layer para os blocos que podem ser agarrados
    [SerializeField] private LineRenderer rope;            // Linha do grappling hook
    [SerializeField] private float ropeSpeed = 20f;       // Velocidade da animação da corda
    [SerializeField] private float pullForce = 10f;       // Força a ser aplicada ao jogador

    private Vector3 grapplePoint;                          // Ponto onde o grappling hook acertou
    private bool isGrappling = false;                     // Indica se o grappling hook está ativo
    private Vector3 ropeTargetPosition;                    // Posição atual do alvo da corda
    private bool grappleHit = false;                       // Armazena se atingiu um ponto válido
    private DistanceJoint2D joint;                        // Referência ao DistanceJoint2D
    private Rigidbody2D playerRb;                         // Referência ao Rigidbody2D do jogador

    void Start()
    {
        rope.enabled = false; // Desabilita a linha inicialmente
        joint = GetComponent<DistanceJoint2D>();           // Obtém o DistanceJoint2D do jogador
        joint.enabled = false;                              // Garante que o joint esteja desativado inicialmente
        playerRb = GetComponent<Rigidbody2D>();            // Obtém o Rigidbody2D do jogador
    }

    void Update()
    {
        // Se o grappling não estiver ativo, verifica a entrada do mouse para lançar a corda
        if (Input.GetMouseButtonDown(1) && !isGrappling)
        {
            StartGrappling();
        }

        // Atualiza a posição do ponto inicial da corda se estiver grappling
        if (isGrappling)
        {
            rope.SetPosition(0, transform.position);  // Mantém o ponto inicial da corda na posição do jogador

            // Inicia a retração após alcançar o ponto ou atingir o comprimento máximo
            if (Input.GetMouseButtonUp(1))
                StartCoroutine(RetractRope());

            if (Input.GetKey(KeyCode.W) && joint.distance > 1)
            {
                joint.distance -= 0.05f;
            }
            else if (Input.GetKey(KeyCode.S) && joint.distance < 20)
            {
                joint.distance += 0.05f;
            }
        }

        
    }

    private void StartGrappling()
    {
        isGrappling = true; // Ativa o grappling

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Define z como 0 para 2D

        // Calcula a direção do hook em direção ao cursor
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Realiza o Raycast na direção calculada e na distância de grappleLength
        RaycastHit2D hit = Physics2D.Raycast(
            origin: transform.position,
            direction: direction,
            distance: grappleLength,
            layerMask: grappleLayer
        );

        // Se encontrou um ponto válido, define o grapplePoint para onde o hook irá
        if (hit.collider != null)
        {
            grapplePoint = hit.point; // Atualiza o ponto de grappling
            grappleHit = true; // Marcamos que acertou um ponto válido
        }
        else
        {
            grapplePoint = transform.position + direction * grappleLength; // Se não, vai até o comprimento máximo
            grappleHit = false; // Não acertou
        }

        ropeTargetPosition = transform.position; // Posição inicial da corda
        rope.SetPosition(0, transform.position);  // Define a posição inicial da corda
        rope.SetPosition(1, transform.position);  // Define a posição final da corda
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
            yield return null; // Espera até o próximo frame
        }

        // Se a corda atingiu um ponto válido
        if (grappleHit)
        {
            joint.connectedAnchor = grapplePoint; // Set the anchor to the grapple point
            joint.enabled = true;  // Enable the joint to pull player naturally towards the grapple point

            // Aplica a força ao jogador na direção do grapplePoint imediatamente
            Vector2 pullDirection = (grapplePoint - transform.position).normalized; // Direção correta
            playerRb.AddForce(pullDirection * pullForce, ForceMode2D.Impulse); // Aplica a força

            // Espera um pequeno tempo para que a animação de puxar ocorra
            yield return new WaitForSeconds(0.1f); // Reduzido para 0.1f para dar a sensação de puxar sem grande atraso

            // Ativa o joint apenas agora, para evitar puxar o jogador antes do tempo
            joint.connectedAnchor = grapplePoint; // Define o ponto de conexão do joint
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
        // Mantém o ponto inicial da corda na posição do jogador
        Vector3 initialRopePosition = transform.position; // Mantém a posição inicial na posição do jogador
        isGrappling = false; // Define que não está mais grappling

        // Desabilita o joint após a retração da corda
        joint.enabled = false;

        // Volta a corda para a posição do jogador
        while (Vector3.Distance(ropeTargetPosition, initialRopePosition) > 0.1f)
        {
            ropeTargetPosition = Vector3.MoveTowards(ropeTargetPosition, initialRopePosition, ropeSpeed * Time.deltaTime);
            rope.SetPosition(1, ropeTargetPosition); // Atualiza a posição final da corda

            // Mantém o ponto inicial da corda na posição do jogador
            rope.SetPosition(0, transform.position); // Atualiza a posição inicial da corda

            yield return null; // Espera até o próximo frame
        }

        rope.enabled = false; // Desabilita a linha após a retração
    }
}