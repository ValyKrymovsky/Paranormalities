using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using MyCode.Data.Settings;
using MyCode.Managers;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public class Manager<T> : MonoBehaviour where T : class
{
    internal static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = FindObjectOfType(typeof(T)) as T;

            if (_instance != null) return _instance;

            Addressables.InstantiateAsync(typeof(T).Name).Completed += handle =>
            {
                if (!handle.Status.Equals(AsyncOperationStatus.Succeeded)) return;

                _instance = ((GameObject)handle.Result).GetComponent<T>();
                DontDestroyOnLoad((GameObject)handle.Result);
            };
            

            return _instance;
        }
    }

    public virtual async UniTask SetUpManager(DifficultyProperties _properties) { await UniTask.WaitForSeconds(1); }

    public static async UniTask<T> LoadManager(T _instance)
    {
        if (_instance != null) return _instance;

        _instance = GameObject.FindObjectOfType(typeof(T)) as T;

        if (_instance != null) return _instance;

        AsyncOperationHandle instantiationHandler = Addressables.InstantiateAsync(typeof(T).Name);
        await instantiationHandler.Task;

        if (!instantiationHandler.Status.Equals(AsyncOperationStatus.Succeeded)) return null;

        _instance = ((GameObject)instantiationHandler.Result).GetComponent<T>();
        DontDestroyOnLoad((GameObject)instantiationHandler.Result);

        return _instance;
    }
}