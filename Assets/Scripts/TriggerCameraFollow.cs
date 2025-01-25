using System.Collections;
using Cinemachine;
using UnityEngine;

public class TriggerCameraFollow : MonoBehaviour
{
    [SerializeField] GameObject virtualCamera;
    [SerializeField] float transitionDuration = 1f; // Duration for damping transition

    private Coroutine dampingCoroutine; // Reference to the current damping coroutine

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartDampingTransition(0, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartDampingTransition(0.9f, 0.9f);
        }
    }

    private void StartDampingTransition(float targetX, float targetY)
    {
        // Stop the current coroutine if one is already running
        if (dampingCoroutine != null)
        {
            StopCoroutine(dampingCoroutine);
        }

        // Start a new coroutine for the damping transition
        dampingCoroutine = StartCoroutine(ChangeDampingGradually(targetX, targetY));
    }

    private IEnumerator ChangeDampingGradually(float targetX, float targetY)
    {
        var cinemachineComponent = virtualCamera?.GetComponent<CinemachineVirtualCamera>();
        if (cinemachineComponent != null)
        {
            var transposer = cinemachineComponent.GetCinemachineComponent<CinemachineTransposer>();
            if (transposer != null)
            {
                float initialX = transposer.m_XDamping;
                float initialY = transposer.m_YDamping;
                float elapsedTime = 0f;

                while (elapsedTime < transitionDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / transitionDuration;

                    // Interpolate the damping values
                    transposer.m_XDamping = Mathf.Lerp(initialX, targetX, t);
                    transposer.m_YDamping = Mathf.Lerp(initialY, targetY, t);

                    yield return null; // Wait for the next frame
                }

                // Ensure the final values are set
                transposer.m_XDamping = targetX;
                transposer.m_YDamping = targetY;
            }
        }
    }
}