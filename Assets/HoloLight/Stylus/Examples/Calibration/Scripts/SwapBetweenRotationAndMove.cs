using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloLight.HoloStylus.Examples.Calibration
{
    public class SwapBetweenRotationAndMove : MonoBehaviour
    {
        public GameObject RotationCanvas;
        public GameObject MoveCanvas;
        private Transform _camTransform;

        public void ToggleMode()
        {
            RotationCanvas.SetActive(!RotationCanvas.activeSelf);
            MoveCanvas.SetActive(!RotationCanvas.activeSelf);
        }

        private void Start()
        {
            ToggleMode();
            _camTransform = Camera.main.transform;
        }

        private void Update()
        {
            if (_camTransform != null)
            {
                transform.forward = transform.position - _camTransform.transform.position;
            }
        }
    }
}
