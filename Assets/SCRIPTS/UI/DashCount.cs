using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pagann.OniHunter
{
    public class DashCount : Entity
    {
    	private int marksLeft = 0;
    	[SerializeField] private Text marksUI;

        public override void LevelStart()
        {
            marksLeft = GameManager.gameState.Parameters.MaximumDashs;
            marksUI.text = marksLeft.ToString();
        }

        public override void TargetPlaced()
    	{
    		marksLeft --;
    		marksUI.text = marksLeft.ToString();
        }

    }
}
