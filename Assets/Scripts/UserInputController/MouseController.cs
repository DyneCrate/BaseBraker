using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : Singleton<MouseController>
{
    public Action<RaycastHit> OnLeftMouseCklick;
    public Action<RaycastHit> OnRightMouseCklick;
    public Action<RaycastHit> OnMiddleMouseCklick;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckMouseClick(0);
        }
        if (Input.GetMouseButtonDown(1))
        {
            CheckMouseClick(1);
        }
        if (Input.GetMouseButtonDown(2))
        {
            CheckMouseClick(2);
        }
    }

    void CheckMouseClick(int mouseButton)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (mouseButton == 0)
            {
                OnLeftMouseCklick?.Invoke(hit);
            }
            else if (mouseButton == 1)
            {
                OnRightMouseCklick?.Invoke(hit);
            }
            else if (mouseButton == 2)
            {
                OnMiddleMouseCklick?.Invoke(hit);
            }
        }
    }
}
