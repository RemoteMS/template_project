using System;
using UnityEngine;

namespace Services.Global
{
    public interface IInputManager
    {
        public string Name { get; }
    }

    public class InputManager : IInputManager, IDisposable
    {
        public string Name { get; } = "TestIInputManager";

        public InputManager()
        {
        }

        public void Dispose()
        {
            Debug.Log("InputManager Dispose");
        }
    }
}