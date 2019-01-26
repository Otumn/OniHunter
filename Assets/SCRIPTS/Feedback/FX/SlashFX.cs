using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class SlashFX : MonoBehaviour
    {
        [SerializeField] private GameObject[] slashObjects;
        [SerializeField] private Player player;
        private void Update()
        {
            for (int i = 0; i < slashObjects.Length; i++)
            {
                slashObjects[i].SetActive(player.IsDashing);
            }
        }
    }
}
