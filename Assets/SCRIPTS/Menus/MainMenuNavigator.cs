using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class MainMenuNavigator : MonoBehaviour
    {
        [SerializeField] private Transform towerMenuButtonParent;
        [SerializeField] private int numberOfTowers = 1;
        [SerializeField] private float distBetweenButtons = 6.8f;
        [SerializeField] private float speed = 5f;
        [SerializeField] private bool usingMouse = false;

        private Vector3 mouseStart;
        private Vector3 mouseMoveDir;
        private bool sliding = false;
        private bool slip = false;
        private float decrementer;
        private float startX = 0f;


        private void Start()
        {
            if (numberOfTowers % 2 == 0) //even
            {
                startX = (numberOfTowers / 2) * (distBetweenButtons * 0.5f);
            }
            else // odd
            {
                startX = ((numberOfTowers - 1) / 2) * distBetweenButtons;
            }
        }

        private void Update()
        {
            if(usingMouse)
            {
                MouseNvigation();
            }
            else
            {
                TouchNavigation();
            }

            
            if(towerMenuButtonParent.position.x < -startX)
            {
                towerMenuButtonParent.position = new Vector3(-startX, towerMenuButtonParent.position.y, 0);
            }
            if(towerMenuButtonParent.position.x > startX)
            {
                towerMenuButtonParent.position = new Vector3(startX, towerMenuButtonParent.position.y, 0);
            }

        }

        private void MouseNvigation()
        {
            if (sliding)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos = new Vector3(mousePos.x, 0, 0);
                mouseMoveDir = (mousePos - mouseStart);
                towerMenuButtonParent.Translate(mouseMoveDir * speed * Time.deltaTime);
            }

            if (slip)
            {
                towerMenuButtonParent.Translate(mouseMoveDir * speed * decrementer * Time.deltaTime);
                decrementer -= Time.deltaTime;
                decrementer = Mathf.Clamp(decrementer, 0f, 1f);
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                sliding = true;
                slip = false;
                decrementer = 0f;
                mouseStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseStart = new Vector3(mouseStart.x, 0, 0);
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                sliding = false;
                slip = true;
                decrementer = 1f;
            }
        }

        private void TouchNavigation()
        {
            if (sliding)
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                touchPos = new Vector3(touchPos.x, 0, 0);
                mouseMoveDir = (touchPos - mouseStart);
                towerMenuButtonParent.Translate(mouseMoveDir * speed * Time.deltaTime);
            }

            if (slip)
            {
                towerMenuButtonParent.Translate(mouseMoveDir * speed * decrementer * Time.deltaTime);
                decrementer -= Time.deltaTime;
                decrementer = Mathf.Clamp(decrementer, 0f, 1f);
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                sliding = true;
                slip = false;
                decrementer = 0f;
                mouseStart = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                mouseStart = new Vector3(mouseStart.x, 0, 0);
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                sliding = false;
                slip = true;
                decrementer = 1f;
            }
        }
    }
}
