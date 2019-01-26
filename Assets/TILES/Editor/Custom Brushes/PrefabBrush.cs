using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pagann.OniHunter
{
    [CreateAssetMenu(fileName = "New Prefab Brush", menuName = "TileMap/PrefabBrush")]
    [CustomGridBrush(false, true, false, "Prefab Brush")]
    public class PrefabBrush : GridBrushBase
    {
        public int chosenGameObject = 0;
        public int zPos = 0;
        public GameObject[] prefabs;

        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;
            if (prefabs.Length == 0)
                return;
            GameObject prefab = prefabs[chosenGameObject];
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            PrefabUtility.RevertObjectOverride(instance, InteractionMode.AutomatedAction);
            Undo.RegisterCreatedObjectUndo((Object)instance, "Paint prefab");
            if (instance != null)
            {
                instance.transform.SetParent(brushTarget.transform);
                instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, zPos) + new Vector3(0.5f, 0.5f, 0.5f)));
            }
        }

        public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            Transform erased = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, zPos));
            if (erased != null)
                Undo.DestroyObjectImmediate(erased.gameObject);
        }

        private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
        {
            int childCount = parent.childCount;
            Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
            Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
            Bounds bounds = new Bounds((max + min) * .5f, max - min);

            for (int i = 0; i < childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (bounds.Contains(child.position))
                    return child;
            }
            return null;
        }
    }
}
