using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeFrame : MonoBehaviour {

    [Header("Parameters")]
    [SerializeField] private float freezeFrameSpeed = 1f;
    [SerializeField] private AnimationCurve freezeFrameCurve;
    private float freezeFrameCompletion = 0f;
    private bool isFreezeFraming = false;

    private void Update()
    {
        FreezeFrameManager();
        CheatCodes();
    }

    /// <summary>
    /// Slows down time for a short period.
    /// </summary>
    public void TriggerFreezeFrame()
    {
        if(!isFreezeFraming)
        isFreezeFraming = true;
    }

    private void FreezeFrameManager()
    {
        if(isFreezeFraming)
        {
            //Debug.Log("FreezeFrame!");
            freezeFrameCompletion += freezeFrameSpeed * Time.unscaledDeltaTime;
            Time.timeScale = freezeFrameCurve.Evaluate(freezeFrameCompletion);

            if(freezeFrameCompletion >= 1)
            {
                isFreezeFraming = false;
                freezeFrameCompletion = 0f;
                Time.timeScale = 1f;
            }
        }
    }

    private void CheatCodes()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            TriggerFreezeFrame();
        }
    }

}
