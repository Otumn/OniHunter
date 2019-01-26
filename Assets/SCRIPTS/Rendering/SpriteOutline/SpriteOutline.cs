using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    [ExecuteInEditMode]
    public class SpriteOutline : MonoBehaviour
    {
        #region Fields
        [Header("Components")]
        [SerializeField] private Material material;
        [HideInInspector] [SerializeField] private GameObject outlineObject;
        [HideInInspector] [SerializeField] private SpriteRenderer outlineSprite;
        [HideInInspector] [SerializeField] private SpriteRenderer objectSprite;

        [Header("Parameters")]
        [SerializeField] [Range(0, 0.05f)] private float outlineWidth = 0.025f;
        [SerializeField] [Range(0, 0.05f)] private float outlineHeight = 0.025f;
        [SerializeField] private Color outlineColor = Color.white;

        [HideInInspector]
        [SerializeField]
        private bool hasInitialized = false;

        [HideInInspector]
        [SerializeField]
        private bool hasRemoved = true;
        #endregion

        #region Properties
        public bool HasInitialized
        {
            get
            {
                return hasInitialized;
            }

            set
            {
                hasInitialized = value;
            }
        }

        public bool HasRemoved
        {
            get
            {
                return hasRemoved;
            }

            set
            {
                hasRemoved = value;
            }
        }
        #endregion

        #region Public Methods
        public void InstantiateOutline()
        {
            if (HasInitialized && !HasRemoved)
            {
                Debug.Log("Already added Outline!");
            }

            else if (material == null)
            {
                Debug.Log("Please create and assign a new Material.");
            }

            else
            {
                Debug.Log("Added Outline.");
                if (GetComponent<Animator>() != null)
                {
                    GetComponent<Animator>().updateMode = AnimatorUpdateMode.AnimatePhysics;
                }
                outlineObject = new GameObject();
                outlineObject.transform.parent = transform;
                outlineObject.name = name + "_Outline";
                outlineObject.transform.localPosition = Vector3.zero;

                outlineSprite = outlineObject.AddComponent<SpriteRenderer>();
                outlineSprite.sortingLayerName = "Outlines";
                outlineSprite.sharedMaterial = material;
                objectSprite = GetComponent<SpriteRenderer>();

                HasInitialized = true;
                HasRemoved = false;
            }
        }

        public void RemoveOutline()
        {
            if (!HasRemoved)
            {
                Debug.Log("Removed Outline.");
                DestroyImmediate(outlineObject);
                HasRemoved = true;
            }

            else
            {
                Debug.Log("Already Removed Outline!");
            }
        }
        #endregion

        #region Private Methods
        private void Update()
        {
            if (HasInitialized && !HasRemoved)
            {
                OutlineProperties();
            }
        }

        private void OutlineProperties()
        {
            outlineSprite.sharedMaterial.SetFloat("_Width", outlineWidth * 2);
            outlineSprite.sharedMaterial.SetFloat("_Height", outlineHeight * 2);

            outlineSprite.sharedMaterial.SetColor("_Color", outlineColor);
            outlineSprite.sprite = objectSprite.sprite;

            outlineSprite.flipX = objectSprite.flipX;
        }
        #endregion
    }
}
