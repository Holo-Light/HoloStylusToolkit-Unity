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
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using HoloLight.Utilities;
using System;

namespace HoloLight.HoloStylus.Examples.SceneCollection
{
    /// <summary>
    /// Helper for scene collection names and button naming
    /// </summary>
    [Serializable]
    internal class SceneNameAndDescriptions
    {
        /// <summary>
        /// Name of the scene
        /// </summary>
        [Tooltip("Name of the scene")]
        public string Name = "";
        /// <summary>
        /// Button description
        /// </summary>
        [Tooltip("Button description"), Multiline]
        public string Description = "";

        /// <summary>
        /// Button won't be spawned, if inactive
        /// </summary>
        [Tooltip("Button won't be spawned, if inactive")]
        public bool IsActive = true;
    }

    /// <summary>
    /// Handles all scenes for the Holo-Stylus® sample scenes.
    /// </summary>
    public class SceneCollectionManager : MonoBehaviour
    {
        /// <summary>
        /// The build index of the collection scene.
        /// </summary>
        public const int COLLECTION_SCENE_BUILDINDEX = 2;

        /// <summary>
        /// Prefab of the scene button.
        /// </summary>
        [SerializeField, Tooltip("Prefab of the Button")]
        private GameObject _buttonPrefab;

        ///// <summary>
        ///// All listed sample scene.
        ///// </summary>
        //[SerializeField, Tooltip("Scenes of the collection.")]
        //private List<string> _sceneNames = new List<string>();

        /// <summary>
        /// All listed sample scenes.
        /// </summary>
        [SerializeField, Tooltip("Scenes of the collection.")]
        private List<SceneNameAndDescriptions> _sceneNames = new List<SceneNameAndDescriptions>();

        /// <summary>
        /// The instantiated scene buttons.
        /// </summary>
        private List<GameObject> _instancedButtons = new List<GameObject>();

        /// <summary>
        /// Current loaded scene.
        /// </summary>
        private string _currentLoadedScene;

        /// <summary>
        /// Collection scene name.
        /// </summary>
        private SceneNameAndDescriptions _collectionScene = new SceneNameAndDescriptions() { Name = "CollectionScene", Description = "Back to menu" };

        /// <summary>
        /// Is true, if loading routine still not done.
        /// </summary>
        private bool _currentlyLoading = false;

        [SerializeField, Tooltip("Position of the back button")]
        private Transform _backButtonPosition;

        [SerializeField, Tooltip("The headline of the scene collection"), Multiline]
        private string _headline = "";
        [SerializeField, Tooltip("The welcome text of the scene collection"), Multiline]
        private string _description = "";

        // Set headline and description of the scene collection
        private void SetHeadline()
        {
            DescriptionTextHandler.ChangeText(_headline, _description);
        }

        /// <summary>
        /// Resets and loads all scene buttons.
        /// </summary>
        private void ShowButtons()
        {
            ClearButtons();
            foreach (var sceneName in _sceneNames)
            {
                if(sceneName.IsActive)
                {
                    CreateSceneButton(sceneName);
                }
            }
        }

        /// <summary>
        /// Resets and shows the back button.
        /// </summary>
        private void ShowBackButton()
        {
            ClearButtons();
            CreateSceneButton(_collectionScene);
        }

        /// <summary>
        /// Creates back and scene buttons for the scene collection.
        /// </summary>
        /// <param name="scene">The scene which should be loaded.</param>
        private void CreateSceneButton(SceneNameAndDescriptions scene)
        {
            var buttonObject = Instantiate<GameObject>(_buttonPrefab);
            _instancedButtons.Add(buttonObject);

            var button = buttonObject.GetComponentInChildren<Button>();
            var text = buttonObject.GetComponentInChildren<Text>();

            if (scene.Name == _collectionScene.Name)
            {
                buttonObject.transform.SetParent(_backButtonPosition, false);
                button.onClick.AddListener(LoadSceneCollection);
                button.onClick.AddListener(SetHeadline);
                text.text = _collectionScene.Description;
            }
            else
            {
                buttonObject.transform.SetParent(transform, false);
                button.onClick.AddListener(delegate { LoadScene(scene.Name); });
                text.text = scene.Description;
            }
        }

        /// <summary>
        /// Clears all buttons from the scene collection list.
        /// </summary>
        private void ClearButtons()
        {
            foreach (var button in _instancedButtons)
            {
                DestroyImmediate(button);
            }
        }

        /// <summary>
        /// Initialize the buttons.
        /// </summary>
        private void Start()
        {
            SetHeadline();
            ShowButtons();
            _currentLoadedScene = _collectionScene.Name;

            if (_backButtonPosition == null)
            {
                var backButtonPostionGameObject = GameObject.Find("BackButtonPosition");
                if (backButtonPostionGameObject != null)
                {
                    _backButtonPosition = backButtonPostionGameObject.transform;
                }
            }
        }

        /// <summary>
        /// Load scene by name.
        /// </summary>
        /// <param name="sceneName">Scene name. Has to be set in build settings.</param>
        public void LoadScene(string sceneName)
        {
            if (_currentlyLoading)
                return;
            StartCoroutine(LoadSceneRoutine(sceneName));
        }

        /// <summary>
        /// Routine for loading scene asynchroniously.
        /// </summary>
        /// <param name="sceneName">Scene name to load.</param>
        /// <returns></returns>
        IEnumerator LoadSceneRoutine(string sceneName)
        {
            _currentlyLoading = true;
            var result = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            _currentLoadedScene = sceneName;
            while (!result.isDone)
            {
                yield return null;
            }
            var scene = SceneManager.GetSceneByName(_currentLoadedScene);
            SceneManager.SetActiveScene(scene);
            ShowBackButton();
            _currentlyLoading = false;
        }

        /// <summary>
        /// Reactivate the scene collection.
        /// </summary>
        public void LoadSceneCollection()
        {
            if (_currentLoadedScene != _collectionScene.Name)
            {
                StartCoroutine(UnloadCurrentSceneRoutine(_currentLoadedScene));
            }
            SetHeadline();
            ShowButtons();
        }

        /// <summary>
        /// Unloads the current loaded scene.
        /// </summary>
        /// <param name="scene">Scene name as to be a valid active scene.</param>
        /// <returns></returns>
        IEnumerator UnloadCurrentSceneRoutine(string scene)
        {
            var result = SceneManager.UnloadSceneAsync(scene);

            while (!result.isDone)
            {
                yield return null;
            }

            var sceneName = SceneManager.GetSceneByBuildIndex(COLLECTION_SCENE_BUILDINDEX);
            SceneManager.SetActiveScene(sceneName);
        }
    }
}
