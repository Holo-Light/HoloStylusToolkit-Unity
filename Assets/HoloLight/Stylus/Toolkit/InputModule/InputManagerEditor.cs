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
#if UNITY_EDITOR
using HoloLight.HoloStylus.Configuration;
using UnityEditor;
using UnityEngine;

namespace HoloLight.HoloStylus.InputModule
{
    /// <summary>
    /// Helper class, for the editor
    /// </summary>
    [CustomEditor(typeof(InputManager))]
    public class InputManagerEditor : Editor
    {
        /// <summary>
        /// Called for redesign inspector fields
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var reference = (InputManager)target;

            if (reference.HMUTransform == null)
            {
                reference.AutoSetHMU = EditorGUILayout.Toggle("Auto set HMU", reference.AutoSetHMU);
                if (!reference.AutoSetHMU)
                {
                    EditorGUILayout.HelpBox(Globals.NO_HMU_SET_MSG, MessageType.Warning);
                }
            }

            if (reference.Configuration == null)
            {               
                EditorGUILayout.HelpBox(Globals.NO_CONFIG_ASSET_MSG, MessageType.Error);
                if (GUILayout.Button("Create configuration"))
                {
                    var config = new ConfigurationObject { };
                    AssetDatabase.CreateAsset(config, "Assets/Resources/StylusConfig2.asset");
                    reference.Configuration = config;
                }
            }
        }
    }
}
#endif