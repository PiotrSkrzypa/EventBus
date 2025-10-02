using System;
using UnityEngine;

namespace PSkrzypa.EventBus
{
    public class UpdateCaller : MonoBehaviour
    {
        private static UpdateCaller _instance;
        private Action _updateCallback;

        public static void AddUpdateCallback(Action updateMethod)
        {
            if (_instance == null)
            {
                _instance = new GameObject("[Event Bus Update Caller]").AddComponent<UpdateCaller>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            _instance._updateCallback += updateMethod;
        }
        public static void RemoveUpdateCallback(Action updateMethod)
        {
            if (_instance == null)
            {
                return;
            }
            _instance._updateCallback -= updateMethod;
            if (_instance._updateCallback == null)
            {
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }


        private void Update()
        {
            _updateCallback();
        }
    }
}