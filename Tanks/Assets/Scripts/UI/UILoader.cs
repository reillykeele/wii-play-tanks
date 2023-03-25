using System.Collections.Generic;
using Data.Enum;
using UnityEngine;
using Util.Enums;

namespace UI
{
    public class UILoader : MonoBehaviour
    {
        public List<UIPageType> UIToLoad;
        public List<SceneType> ScenesToLoad;

        void Start()
        {
            // foreach (var ui in UIToLoad)
            // {
            //     if (SceneManager.GetSceneByName(ui.ToString()).isLoaded == false)
            //         SceneManager.LoadSceneAsync(ui.ToString(), LoadSceneMode.Additive);
            // }
            //
            // foreach (var scene in ScenesToLoad)
            // {
            //     if (SceneManager.GetSceneByName(scene.ToString()).isLoaded == false)
            //         SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive);
            // }
        }
    }
}
