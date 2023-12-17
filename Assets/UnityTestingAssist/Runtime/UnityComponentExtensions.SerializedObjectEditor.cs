using System;
using UnityEditor;
using UnityEngine;

namespace UnityTestingAssist.Runtime
{
    public static partial class UnityComponentExtensions
    {
        /// <summary>
        /// Starts editing the serialized object of the provided component.
        /// </summary>
        /// <remarks>
        /// Do not forget to call <see cref="Apply"/> after you are done editing the serialized object.
        /// </remarks>
        /// <param name="component">The component whose serialized object should be edited.</param>
        /// <returns>An object that can be used to edit the serialized object.</returns>
        public static ISerializedObjectEditor EditSerializable(this Component component)
        {
            if (component is null)
                throw new ArgumentNullException(nameof(component));

            return new SerializableObjectEditor(component);
        }

        /// <summary>
        /// Sets the value of the <see cref="string"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name, string value) =>
            editor.Field(name, property => property.stringValue = value);

        /// <summary>
        /// Sets the value of the <see cref="int"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name, int value) =>
            editor.Field(name, property => property.intValue = value);

        /// <summary>
        /// Sets the value of the <see cref="float"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name, float value) =>
            editor.Field(name, property => property.floatValue = value);

        /// <summary>
        /// Sets the value of the <see cref="bool"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name, bool value) =>
            editor.Field(name, property => property.boolValue = value);

        /// <summary>
        /// Sets the value of the <see cref="Enum"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name, Enum value) =>
            editor.Field(name, property => property.enumValueIndex = Convert.ToInt32(value));

        /// <summary>
        /// Sets the value of the <see cref="Color"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name, Color value) =>
            editor.Field(name, property => property.colorValue = value);

        /// <summary>
        /// Sets the value of the <see cref="Vector2"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name, Vector2 value) =>
            editor.Field(name, property => property.vector2Value = value);

        /// <summary>
        /// Sets the value of the <see cref="Vector3"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name, Vector3 value) =>
            editor.Field(name, property => property.vector3Value = value);

        /// <summary>
        /// Sets the value of the <see cref="Vector4"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name, Vector4 value) =>
            editor.Field(name, property => property.vector4Value = value);

        /// <summary>
        /// Sets the value of the <see cref="Quaternion"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor
            Field(this ISerializedObjectEditor editor, string name, Quaternion value) =>
            editor.Field(name, property => property.quaternionValue = value);

        /// <summary>
        /// Sets the value of the <see cref="Rect"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name, Rect value) =>
            editor.Field(name, property => property.rectValue = value);

        /// <summary>
        /// Sets the value of the <see cref="Bounds"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name, Bounds value) =>
            editor.Field(name, property => property.boundsValue = value);

        /// <summary>
        /// Sets the value of the <see cref="AnimationCurve"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name,
            AnimationCurve value) =>
            editor.Field(name, property => property.animationCurveValue = value);

        /// <summary>
        /// Sets the value of the <see cref="Gradient"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name, Gradient value) =>
            editor.Field(name, property => property.gradientValue = value);

        /// <summary>
        /// Sets the value of the <see cref="UnityEngine.Object"/> field in the serialized object.
        /// </summary>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name,
            UnityEngine.Object value) =>
            editor.Field(name, property => property.objectReferenceValue = value);

        /// <summary>
        /// Edits the provided property of the serialized object.
        /// </summary>
        /// <remarks>If the property is not found, it will try to find the backing field of an auto property.</remarks>
        /// <param name="editor"></param>
        /// <param name="name">Name of the property in the serialized object.</param>
        /// <param name="action">Action that will be executed on the property.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"> thrown when the provided property name is not found in the serialized object.</exception>
        public static ISerializedObjectEditor Field(this ISerializedObjectEditor editor, string name,
            Action<SerializedProperty> action)
        {
            if (editor is not SerializableObjectEditor objectEditor)
                throw new ArgumentException("Invalid editor type", nameof(editor));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            if (!objectEditor.TryFindProperty(name, out var property))
                throw new ArgumentException($"Property {name} not found", nameof(name));

            action(property);
            return objectEditor;
        }

        /// <summary>
        /// Applies the changes to the serialized object.
        /// </summary>
        /// <param name="editor"></param>
        public static void Apply(this ISerializedObjectEditor editor)
        {
            if (editor is not SerializableObjectEditor objectEditor)
                throw new ArgumentException("Invalid editor type", nameof(editor));

            objectEditor.SerializedObject.ApplyModifiedProperties();
            objectEditor.SerializedObject.Dispose();
        }

        private static bool TryFindProperty(this SerializableObjectEditor editor, string name,
            out SerializedProperty property)
        {
            property = editor.SerializedObject.FindProperty(name);
            if (property is not null)
                return true;

            //Try to find backing field for auto property
            property = editor.SerializedObject.FindProperty($"<{name}>k__BackingField");
            return property is not null;
        }

        private sealed class SerializableObjectEditor : ISerializedObjectEditor
        {
            public SerializableObjectEditor(UnityEngine.Object unityObject) =>
                SerializedObject = new SerializedObject(unityObject);

            public SerializedObject SerializedObject { get; }
        }
    }
}