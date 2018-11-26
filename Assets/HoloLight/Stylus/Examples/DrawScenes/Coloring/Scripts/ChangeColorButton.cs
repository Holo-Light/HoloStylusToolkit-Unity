#region copyright
/*******************************************************
 * Copyright (C) 2017-2018 Holo-Light GmbH -> <info@holo-light.com>
 * 
 * This file is part of the Stylus SDK.
 * 
 * Stylus SDK can not be copied and/or distributed without the express
 * permission of the Holo-Light GmbH
 * 
 * Author of this file is Peter Roth
 *******************************************************/
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HoloLight.HoloStylus.Examples.DrawScenes
{
    /// <summary>
    /// Simple color selection for the painting.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ChangeColorButton : MonoBehaviour
    {
        // Attached button
        private Button _button;
        // Button normal color
        [SerializeField]
        private Color _color;

        /// <summary>
        /// Initialize
        /// </summary>
        private void Start()
        {
            _button = GetComponent<Button>();
            _color = _button.colors.normalColor;
            _button.onClick.AddListener(SetColor);
        }

        /// <summary>
        /// Set the color in the coloring manager
        /// </summary>
        private void SetColor()
        {
            var manager = FindObjectOfType<ColoringManager>();
            manager.SetColor(_color);
        }
    }
}
