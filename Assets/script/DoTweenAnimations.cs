using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoTweenAnimations : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start the animation
        StartGrowShrinkAnimation();
    }

    void StartGrowShrinkAnimation()
    {
        // Define the initial scale
        Vector3 initialScale = transform.localScale;

        // Set up the grow animation
        transform.localScale = initialScale; // Ensure the initial scale is set
        transform.DOScale(targetScale, 1.0f) // 1.0f is the duration of the grow animation
            .SetEase(Ease.OutQuad) // Adjust the ease type as needed
            .OnComplete(() =>
            {
                // Set up the shrink animation after the grow animation is complete
                transform.DOScale(initialScale, 1.0f) // 1.0f is the duration of the shrink animation
                    .SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        // Repeat the animation by calling StartGrowShrinkAnimation recursively
                        StartGrowShrinkAnimation();
                    });
            });
    }
}
