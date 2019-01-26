using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    [System.Serializable]
    
    public class Cable
    {
        #region Fields

        [Header("Components")]
        [HideInInspector] [SerializeField] private string name;
        [HideInInspector] [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private ElectricProp target;
        [HideInInspector] [SerializeField] private Animator anim;

        [Header("Parameters")]
        [SerializeField] private float xOffset = 0.8f;
        [HideInInspector] [SerializeField] private List<Vector3> points = new List<Vector3>();

        public LineRenderer LineRenderer { get => lineRenderer; set => lineRenderer = value; }
        public ElectricProp Target { get => target; set => target = value; }
        public float XOffset { get => xOffset; set => xOffset = value; }
        public List<Vector3> Points { get => points; set => points = value; }
        public string Name { get => name; set => name = value; }
        public Animator Anim { get => anim; set => anim = value; }

        #endregion
    }

    [ExecuteInEditMode]
    public class CableManager : MonoBehaviour
    {
        #region Fields

        [Header("Components")]
        [SerializeField] private GameObject cablePrefab;
        [SerializeField] private Transform cableParent;
        [SerializeField] private ElectricButton button;
        [SerializeField] private List<Cable> cables = new List<Cable>();
        [SerializeField] private List<GameObject> cableObjects = new List<GameObject>();
        [SerializeField] private List<float> saveOffsets = new List<float>();

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            SwitchCables();
        }

        private void Update()
        {
            CableUpdate();
        }

        #endregion

        #region Public Methods

        [ContextMenu("Melt Cables")]
        public void MeltCables()
        {
            for (int i = 0; i < cables.Count; i++)
            {
                cables[i].Anim.SetTrigger("Melt");
            }
        }

        [ContextMenu("Switch Cables")]
        public void SwitchCables()
        {
            for (int i = 0; i < cables.Count; i++)
            {
                if(!cables[i].Target.IsOn)
                {
                    cables[i].Anim.SetTrigger("Deactivated");
                }

                if (cables[i].Target.IsOn)
                {
                    cables[i].Anim.SetTrigger("Activated");
                }
            }
        }

        #endregion

        #region Private Methods

        private void CableUpdate()
        {
            for (int i = 0; i < cables.Count; i++)
            {
                if(cables[i].Target != null)
                {
                    Vector3 xDir = new Vector3(cables[i].Target.transform.position.x, transform.position.y, 0);
                    cables[i].Points[0] = transform.position;
                    cables[i].Points[1] = new Vector3(transform.position.x + cables[i].XOffset, transform.position.y, 0);
                    cables[i].Points[2] = new Vector3(cables[i].Points[1].x, cables[i].Target.transform.position.y, 0);
                    cables[i].Points[3] = cables[i].Target.transform.position;                
                }

                else
                {
                    for (int x = 0; x < cables[i].Points.Count; x++)
                    {
                        cables[i].Points[x] = transform.position;
                    }
                }

                if (cables[i].LineRenderer != null)
                cables[i].LineRenderer.SetPositions(cables[i].Points.ToArray());
            }            
        }

        #endregion

        public GameObject CablePrefab { get => cablePrefab; }
        public Transform CableParent { get => cableParent; }
        public List<Cable> Cables { get => cables; }
        public ElectricButton Button { get => button; }
        public List<GameObject> CableObjects { get => cableObjects; set => cableObjects = value; }
        public List<float> SaveOffsets { get => saveOffsets; set => saveOffsets = value; }
    }
}
