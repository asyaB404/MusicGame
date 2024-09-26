using System;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    [SerializeField] private bool isOpen;
    [SerializeField] private float xSpeed = 0;
    [SerializeField] private float ySpeed = 0;
    [SerializeField] private float zSpeed = 1;


    private void FixedUpdate()
    {
        if (!isOpen) return;
        transform.Rotate(xSpeed, ySpeed, zSpeed);
    }
}