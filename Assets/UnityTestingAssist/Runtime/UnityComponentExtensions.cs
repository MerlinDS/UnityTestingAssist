using System;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

namespace UnityTestingAssist.Runtime
{
    /// <summary>
    /// This class contains extension methods for Unity's Component class.
    /// These methods allow for the manual execution of Unity's lifecycle events.
    /// </summary>
    public static partial class UnityComponentExtensions
    {
        /// <exception cref="ArgumentException"> thrown when the provided component does not contain a method with the provided name.</exception>
        private static void ExecuteUnityEvent(this Component component, string methodName)
        {
            var type = component.GetType();
            var methodInfo = type.GetMethodInfo(methodName);
            if (methodInfo is null)
                throw new ArgumentException($"Method {methodName} not found in {type.Name}");

            methodInfo.Invoke(component, null);
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