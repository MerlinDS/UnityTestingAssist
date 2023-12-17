using System;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

namespace UnityTestingAssist.Runtime
{
    public static partial class UnityComponentExtensions
    {
        /// <summary>
        /// Executes the Awake event of the provided component.
        /// </summary>
        /// <param name="component">The component whose Awake event should be executed.</param>
        /// <returns>The component after the Awake event has been executed.</returns>
        /// <inheritdoc cref="ExecuteUnityEvent"/>
        public static Component ExecuteAwake(this Component component)
        {
            component.ExecuteUnityEvent(UnityEventFunctionNames.Awake);
            return component;
        }

        /// <summary>
        /// Executes the OnEnable event of the provided component.
        /// </summary>
        /// <param name="component">The component whose OnEnable event should be executed.</param>
        /// <returns>The component after the OnEnable event has been executed.</returns>
        /// <inheritdoc cref="ExecuteUnityEvent"/> 
        public static Component ExecuteOnEnable(this Component component)
        {
            component.ExecuteUnityEvent(UnityEventFunctionNames.OnEnable);
            return component;
        }

        /// <summary>
        /// Executes the Start event of the provided component.
        /// </summary>
        /// <param name="component">The component whose Start event should be executed.</param>
        /// <returns>The component after the Start event has been executed.</returns>
        /// <inheritdoc cref="ExecuteUnityEvent"/>
        public static Component ExecuteStart(this Component component)
        {
            component.ExecuteUnityEvent(UnityEventFunctionNames.Start);
            return component;
        }

        /// <summary>
        /// Executes the FixedUpdate event of the provided component.
        /// </summary>
        /// <param name="component">The component whose FixedUpdate event should be executed.</param>
        /// <returns>The component after the FixedUpdate event has been executed.</returns>
        /// <inheritdoc cref="ExecuteUnityEvent"/>
        public static Component ExecuteFixedUpdate(this Component component)
        {
            component.ExecuteUnityEvent(UnityEventFunctionNames.FixedUpdate);
            return component;
        }

        /// <summary>
        /// Executes the Update event of the provided component.
        /// </summary>
        /// <param name="component">The component whose Update event should be executed.</param>
        /// <returns>The component after the Update event has been executed.</returns>
        /// <inheritdoc cref="ExecuteUnityEvent"/>
        public static Component ExecuteUpdate(this Component component)
        {
            component.ExecuteUnityEvent(UnityEventFunctionNames.Update);
            return component;
        }

        /// <summary>
        /// Executes the LateUpdate event of the provided component.
        /// </summary>
        /// <param name="component">The component whose LateUpdate event should be executed.</param>
        /// <returns>The component after the LateUpdate event has been executed.</returns>
        /// <inheritdoc cref="ExecuteUnityEvent"/>
        public static Component ExecuteLateUpdate(this Component component)
        {
            component.ExecuteUnityEvent(UnityEventFunctionNames.LateUpdate);
            return component;
        }

        /// <summary>
        /// Executes the OnDisable event of the provided component.
        /// </summary>
        /// <param name="component">The component whose OnDisable event should be executed.</param>
        /// <returns>The component after the OnDisable event has been executed.</returns>
        /// <inheritdoc cref="ExecuteUnityEvent"/>
        public static Component ExecuteOnDisable(this Component component)
        {
            component.ExecuteUnityEvent(UnityEventFunctionNames.OnDisable);
            return component;
        }

        /// <summary>
        /// Executes the OnDestroy event of the provided component.
        /// </summary>
        /// <param name="component">The component whose OnDestroy event should be executed.</param>
        /// <returns>The component after the OnDestroy event has been executed.</returns>
        /// <inheritdoc cref="ExecuteUnityEvent"/>
        public static Component ExecuteOnDestroy(this Component component)
        {
            component.ExecuteUnityEvent(UnityEventFunctionNames.OnDestroy);
            return component;
        }
        
        /// <exception cref="ArgumentException"> thrown when the provided component does not contain a method with the provided name.</exception>
        private static void ExecuteUnityEvent(this Component component, string methodName)
        {
            var type = component.GetType();
            var methodInfo = type.GetMethodInfo(methodName);
            if (methodInfo is null)
                throw new ArgumentException($"Method {methodName} not found in {type.Name}");

            try
            {
                methodInfo.Invoke(component, null);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? e;
            }
        }

        [CanBeNull]
        private static MethodInfo GetMethodInfo(this Type type, string methodName)
        {
            while (type is not null)
            {
                var methodInfo = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (methodInfo is not null)
                    return methodInfo;

                type = type.BaseType;
            }

            return null;
        }
    }
}