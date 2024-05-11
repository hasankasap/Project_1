using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Game.GridSystem
{
    public class GridInput : MonoBehaviour
    {
        private Camera cam;
        private void Start()
        {
            cam = Camera.main;
        }
        private void Update()
        {
            MouseInput();
        }
        private void MouseInput()
        {
            if (Input.GetMouseButtonDown(0) && !MouseUtils.MouseIsOnUI())
            {
                CheckCell();
            }
        }
        private void CheckCell()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit))
            {
                if (rayHit.collider == null)
                    return;
                EventManager.TriggerEvent(GameEvents.PLACE_INTO_CELL, new object[] { rayHit.collider.gameObject, rayHit.point });
            }
        }
    }
}