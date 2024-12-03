using UnityEngine;

public class SimpleSpriteAnimator : MonoBehaviour
{
    public Sprite[] sprites; // Drag and drop your 3 sprites here in the Inspector
    public float animationSpeed = 0.1f; // Time between frame changes

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[0];
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= animationSpeed)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % sprites.Length;
            spriteRenderer.sprite = sprites[currentFrame];
        }
    }
}