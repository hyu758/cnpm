using System.Collections;
using UnityEngine;

public class StatusEffectController : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    [SerializeField] private int flashCount = 3;

    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    #region Unity Callbacks

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        flashMaterial = new Material(flashMaterial);
    }

    #endregion

    public void Flash(Color color, int count = 1, float duration = 0.1f)
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashCount = count;
        flashRoutine = StartCoroutine(FlashRoutine(color, duration));
    }

    private IEnumerator FlashRoutine(Color color, float duration)
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.material = flashMaterial;
            flashMaterial.color = color;
            
            yield return new WaitForSeconds(duration);
            
            spriteRenderer.material = originalMaterial;
            
            yield return new WaitForSeconds(duration);
        }
        
        flashRoutine = null;
    }
}