using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.ReloadAttribute;

public class GrapplingHook : MonoBehaviour
{
    private Vector3 startPoint;                       
    private Vector3 grapplePoint;
    public bool isGrappling = false;             
    private bool stopGrappling = false;
    private Vector3 direction;

    [SerializeField] private float ropeSpeed = 20f;    
    [SerializeField] private float grappleDistance;
    [SerializeField] private GameObject pickaxe;
    [SerializeField] private GameObject pickaxeGrapple;

    void Start()
    {
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

        if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.E))
        {
            stopGrappling = true;
        }
    }

    private void StartGrappling()
    {
        isGrappling = true;
        pickaxeGrapple.SetActive(true);
        pickaxe.SetActive(false);

        startPoint = transform.position;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 

        direction = (mousePosition - transform.position).normalized;


        StartCoroutine(MoveRope());
    }

    private IEnumerator MoveRope()
    {
        bool hit = false;
        while (Vector3.Distance(startPoint, pickaxeGrapple.transform.position) <= grappleDistance && isGrappling)
        {
            pickaxeGrapple.transform.position += direction * ropeSpeed * Time.deltaTime;
            yield return null; // Espera até o próximo frame
        }


        StartCoroutine(RetractRope());
     
    }

    // Coroutine para fazer a corda retornar ao jogador
    private IEnumerator RetractRope()
    {
        direction = (transform.position - pickaxeGrapple.transform.position).normalized;

        while (Vector3.Distance(transform.position, pickaxeGrapple.transform.position) > 0.1 && direction == (transform.position - pickaxeGrapple.transform.position).normalized)
        {
            pickaxeGrapple.transform.position += direction * ropeSpeed * Time.deltaTime;
            yield return null; // Espera até o próximo frame
        }

        pickaxeGrapple.SetActive(false);
        pickaxe.SetActive(true);
        isGrappling = false;
        yield return null;
    }

    public void RemoveGrapple()
    {
        StartCoroutine(RetractRope());
    }
}