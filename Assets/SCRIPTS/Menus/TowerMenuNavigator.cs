using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class TowerMenuNavigator : MonoBehaviour
    {
        #region Private Variables

        [SerializeField] private Transform cam;
        [SerializeField] private float speed = 5f;
        [Header("Mouse navigation")]
        [SerializeField] private bool usingMouse = false;
        [SerializeField] private float margins = 10;

        private Vector3 mouseStart;
        private Vector3 mouseMoveDir;
        private bool sliding = false;
        private bool slip = false;
        private float decrementer;
        #endregion

        #region Monobehaviour callbacks

        private void Start()
        {
            if(SelectedLevelHolder.lastMainMenuPos != Vector3.zero)
            {
                cam.position = SelectedLevelHolder.lastMainMenuPos;
            }
        }

        private void Update()
        {
            if(!usingMouse)
            {
                TouchNavigation();
            }
            else
            {
                MouseNvigation();
            }

            if (cam.position.y > 70.2f)
            {
                cam.position = new Vector3(0, 70.2f, -10f);
            }
            if (cam.position.y < 0)
            {
                cam.position = new Vector3(0, 0, -10);
            }
        }

        #endregion

        #region Private Methods

        private void MouseNvigation()
        {
            if (sliding)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos = new Vector3(0, mousePos.y, -10);
                mouseMoveDir = (mousePos - mouseStart);
                cam.Translate(-mouseMoveDir * speed * Time.deltaTime);
            }

            if (slip)
            {
                cam.Translate(-mouseMoveDir * speed * decrementer * Time.deltaTime);
                decrementer -= Time.deltaTime;
                decrementer = Mathf.Clamp(decrementer, 0f, 1f);
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                sliding = true;
                slip = false;
                decrementer = 0f;
                mouseStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseStart = new Vector3(0, mouseStart.y, -10);
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
                touchPos = new Vector3(touchPos.x, 0, -10);
                mouseMoveDir = (touchPos - mouseStart);
                cam.Translate(-mouseMoveDir * speed * Time.deltaTime);
            }

            if (slip)
            {
                cam.Translate(-mouseMoveDir * speed * decrementer * Time.deltaTime);
                decrementer -= Time.deltaTime;
                decrementer = Mathf.Clamp(decrementer, 0f, 1f);
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                sliding = true;
                slip = false;
                decrementer = 0f;
                mouseStart = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                mouseStart = new Vector3(mouseStart.x, 0, -10);
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                sliding = false;
                slip = true;
                decrementer = 1f;
            }
        }

        #endregion

    }
}

