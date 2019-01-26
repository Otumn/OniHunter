using UnityEngine;
using System;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ColorSwap : MonoBehaviour
{
    [System.Serializable]
    public class NewColorSwap
    {
        [SerializeField] [Tooltip("The color we want to replace.")] private Color targetColor = Color.white;
        [SerializeField] [Tooltip("Which color should our Target color be replaced by?")] private Color outputColor = Color.white;
        [SerializeField][Range(0,1)][Tooltip("Higher values will grab more color variations.")] private float tolerance = 0.001f;

        #region Properties
        public Color TargetColor
        {
            get
            {
                return targetColor;
            }

            set
            {
                targetColor = value;
            }
        }

        public Color OutputColor
        {
            get
            {
                return outputColor;
            }

            set
            {
                outputColor = value;
            }
        }

        public float Tolerance
        {
            get
            {
                return tolerance;
            }

            set
            {
                tolerance = value;
            }
        }
        #endregion
    }

    public Material mat;
    public List<NewColorSwap> ColorSwapList = new List<NewColorSwap>();
    [SerializeField] private Color[] targetColor;
    [SerializeField] private Color[] outputColor;
    [SerializeField] private float[] tolerance;

    public void AddNew()
    {
        ColorSwapList.Add(new NewColorSwap());
    }

    public void Remove(int index)
    {
        ColorSwapList.RemoveAt(index);  
    }

    private void Update()
    {
        UpdateArray();
    }

    public void AcquireMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();
        mat = renderer.sharedMaterial;
    }

    private void UpdateArray()
    {
        if(ColorSwapList.Count > 0)
        {
            targetColor = new Color[ColorSwapList.Count];
            outputColor = new Color[ColorSwapList.Count];
            tolerance = new float[ColorSwapList.Count];

            for (int i = 0; i < ColorSwapList.Count; i++)
            {
                targetColor[i] = ColorSwapList[i].TargetColor;
                outputColor[i] = ColorSwapList[i].OutputColor;
                tolerance[i] = ColorSwapList[i].Tolerance;
            }

            Debug.Log(targetColor.Length);

            mat.SetFloat("_ArrayLength", ColorSwapList.Count);
            mat.SetColorArray("_OutputColors", outputColor);
            mat.SetFloatArray("_Tolerance", tolerance);
            mat.SetColorArray("_TargetColors", targetColor);
        }        
    }
}

