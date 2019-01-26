using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pagann.OniHunter
{
    [CustomEditor(typeof(CableManager))]
    [CanEditMultipleObjects]
    public class CableManagerEditor : Editor
    {        
        private CableManager cableManager { get => target as CableManager; }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if(GUILayout.Button("Create cables"))
            {
                ResetCables();
            }
            if(GUILayout.Button("Destroy cables"))
            {
                DestroyCables();
            }
        }

        private void ResetCables()
        {
            DestroyCables();

            for (int i = 0; i < cableManager.Button.ElectricProps.Length; i++)
            {
                cableManager.Cables.Add(new Cable());
                cableManager.Cables[i].Name = "Cable 0" + i;
                GameObject newCable = PrefabUtility.InstantiatePrefab(cableManager.CablePrefab) as GameObject;
                newCable.transform.parent = cableManager.CableParent;
                cableManager.CableObjects.Add(newCable);
                cableManager.Cables[i].LineRenderer = newCable.GetComponent<LineRenderer>();
                cableManager.Cables[i].Points.Clear();
                for (int x = 0; x < 4; x++)
                {
                    cableManager.Cables[i].Points.Add(Vector3.zero);
                }
                cableManager.Cables[i].LineRenderer.positionCount = 4;
                cableManager.Cables[i].Target = cableManager.Button.ElectricProps[i];
                cableManager.Cables[i].Anim = cableManager.CableObjects[i].GetComponent<Animator>();
                cableManager.Cables[i].XOffset = cableManager.SaveOffsets[i];
            }
            cableManager.SaveOffsets.Clear();
        }

        private void DestroyCables()
        {
            for (int i = 0; i < cableManager.Cables.Count; i++)
            {
                cableManager.SaveOffsets.Add(cableManager.Cables[i].XOffset);

                if (cableManager.Cables[i].LineRenderer != null)
                {
                    DestroyImmediate(cableManager.Cables[i].LineRenderer.gameObject);
                }
            }

            cableManager.Cables.Clear();
            cableManager.CableObjects.Clear();
        }

    }
}
