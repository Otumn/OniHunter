using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    [ExecuteInEditMode]
    public class CustomSpriteAnimation : MonoBehaviour
    {
        [SerializeField] private Material mat;
        [SerializeField] private int frameNumber = 10;
        [SerializeField] private int lineNumber = 1;
        [SerializeField] private int currentFrame = 0;
        [SerializeField] private SpriteRenderer sprite;

        private void OnEnable()
        {
            sprite.sharedMaterial = new Material(mat);
        }

        private void Update()
        {
            sprite.sharedMaterial.SetVector("_Tiling", new Vector2 (1/(float)(frameNumber), lineNumber) );
            sprite.sharedMaterial.SetVector("_Offset", new Vector2 (Interpolator.Interpolate(0, 1, 0 , (frameNumber), currentFrame ), 0));
            
            if(currentFrame < 0)
            {
                currentFrame = 0;
            }

            if(currentFrame > frameNumber - 1)
            {
                currentFrame = frameNumber - 1;
            }
            
        }
    }
}
