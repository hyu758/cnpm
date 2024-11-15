using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectController : MonoBehaviour
{
    // Start is called before the first frame update
        #region Datamembers

        #region Editor Settings

        [Tooltip("Material to switch to during the flash.")]
        [SerializeField] private Material flashMaterial;

        [Tooltip("Duration of the flash.")]
        [SerializeField] private float duration;

        #endregion
        #region Private Fields

        // The SpriteRenderer that should flash.
        private SpriteRenderer spriteRenderer;

        // The material that was in use, when the script started.
        private Material originalMaterial;

        // The currently running coroutine.
        private Coroutine flashRoutine;

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalMaterial = spriteRenderer.material;
            flashMaterial = new Material(flashMaterial);
        }

        #endregion

        public void Flash(Color color)
        {
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }
            
            flashRoutine = StartCoroutine(FlashRoutine(color));
        }

        private IEnumerator FlashRoutine(Color color)
        {

            spriteRenderer.material = flashMaterial;
            
            flashMaterial.color = color;
            
            yield return new WaitForSeconds(duration);
            
            spriteRenderer.material = originalMaterial;
            
            flashRoutine = null;
        }

        #endregion
}
