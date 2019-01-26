using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pagann.OniHunter
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Rope))]
    public class RopeEditor : Editor
    {
        private Rope rope { get { return target as Rope; } }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if(GUILayout.Button("Link to Prop"))
            {
                if(rope.AttachedProp == null)
                {
                    Debug.LogError("No Prop attached");
                    return;
                }
                if(rope.transform.position.x != rope.AttachedProp.transform.position.x)
                {
                    Debug.LogError("Hinge and prop have to be on the same column");
                    return;
                }
                float yHinge = rope.transform.position.y;
                float yEntity = rope.AttachedProp.transform.position.y;
                float yDist = yHinge - yEntity;
                if((yDist * 2) % 1 > 0)
                {
                    Debug.LogError("The hinge and the prop are not snapped to a grid");
                }
                if (rope.Parts.Count > 0)
                {
                    for (int i = 0; i < rope.Parts.Count; i++)
                    {
                        if(rope.Parts[i] != null) DestroyImmediate(rope.Parts[i].gameObject);
                    }
                }
                rope.Parts.Clear();
                int neededParts = (int) yDist * 2;
                for (int i = 0; i < neededParts; i++)
                {
                    GameObject part = PrefabUtility.InstantiatePrefab(Resources.Load("RopePart")) as GameObject;
                    RopePart ropePart = part.GetComponent<RopePart>();

                    //transform setting
                    ropePart.transform.position = new Vector3(rope.transform.position.x, (yHinge - 0.25f - (0.5f * i)), 0);
                    ropePart.transform.parent = rope.transform;

                    // indexing
                    rope.Parts.Add(ropePart);
                    ropePart.AttachedRope = rope;
                }

                //linking
                rope.Joint.connectedBody = rope.Parts[0].Body;
                for (int i = 0; i < rope.Parts.Count - 1; i++)
                {
                    rope.Parts[i].Joint.connectedBody = rope.Parts[i + 1].Body;
                }
                rope.Parts[rope.Parts.Count - 1].Joint.connectedBody = rope.AttachedProp.Body;
                Debug.Log("Linked " + rope.AttachedProp.name + " to a rope, using " + rope.Parts.Count + " rope parts");
            }

            if(GUILayout.Button("Cut"))
            {
                if(rope.Parts.Count > 0)
                {
                    for (int i = 0; i < rope.Parts.Count; i++)
                    {
                        DestroyImmediate(rope.Parts[i].gameObject);
                    }
                    rope.Parts.Clear();
                }

                Debug.Log("not implemented yet");
            }
        }
    }
}
