using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;

    private void Start()
    {
        AssignFields();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            TestBlink();
        }
    }

    [ContextMenu("Test blink")]
    public void TestBlink()
    {
        Blinks(3, .2f, .2f);
    }

    [ContextMenu("Assign fields")]
    public void AssignFields()
    {
        if(sprite == null)
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
        }
    }

    /// <summary>
    /// Makes the sprite blink.
    /// </summary>
    /// <param name="number">The number of blinks.</param>
    /// <param name="delay">The delay between each blink.</param>
    /// <param name="time">The time of each blink.</param>
    public void Blinks(int number, float delay, float time)
    {
        StartCoroutine(Blinking(number, delay, time));
    }

    private IEnumerator Blinking(int number, float delay, float time)
    {
        for (int i = 0; i < number; i++)
        {
            sprite.material.SetFloat("_AddedStrength", 1);
            yield return new WaitForSecondsRealtime(time);
            sprite.material.SetFloat("_AddedStrength", 0);
            yield return new WaitForSecondsRealtime(delay);
        }
    }
}
