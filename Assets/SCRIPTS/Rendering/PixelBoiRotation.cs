using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    [ExecuteInEditMode]
    public class PixelBoiRotation : MonoBehaviour
    {
        public SpriteRenderer sprite;

        private void Start()
        {
            AssignValues();
        }

        private void AssignValues()
        {
            if (sprite == null)
            {
                sprite = GetComponent<SpriteRenderer>();
            }
        }

        private void Update()
        {
            RotationManager();
        }

        private void RotationManager()
        {
            sprite.sharedMaterial.SetFloat("_Rotation", transform.eulerAngles.z);
        }
    }
}
