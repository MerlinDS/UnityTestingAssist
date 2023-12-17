using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityTestingAssist.Runtime;

namespace UnityTestingAssist.Tests
{
    [TestFixture]
    [TestOf(typeof(UnityComponentExtensions))]
    public class SerializeObjectEditorTests
    {
        private const string TestFieldName = "_testField";
        private const string TestPropertyName = nameof(FieldTestComponent<int>.TestProperty);

        [Test]
        public void EditSerializable_When_called_Should_return_editor()
        {
            //Arrange
            var test = CreateTest<TestComponent>();
            //Act
            var editor = test.component.EditSerializable();
            //Assert
            editor.Should().NotBeNull();
        }

        [Test]
        public void EditSerializable_When_called_with_null_component_Should_throw_exception()
        {
            //Arrange
            TestComponent test = null;
            //Act
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => test!.EditSerializable();
            //Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Field_When_called_with_custom_type_Should_set_field_value()
        {
            //Arrange
            const int expectedFieldValue = 1;
            const int expectedPropertyValue = 2;
            var test = CreateTest<IntFieldTestComponent>();
            //Act
            test.component.EditSerializable()
                .Field(TestFieldName, p => p.intValue = expectedFieldValue)
                .Field(TestPropertyName, p => p.intValue = expectedPropertyValue)
                .Apply();
            //Assert
            test.component.TestField.Should().Be(expectedFieldValue, "because the field value should be set");
            test.component.TestProperty.Should().Be(expectedPropertyValue, "because the property value should be set");
        }

        [Test]
        public void Field_When_editor_is_null_Should_throw_exception()
        {
            //Arrange
            ISerializedObjectEditor editor = null;
            //Act
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => editor!.Field(TestFieldName, p => p.intValue = 1);
            //Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Field_When_editor_is_invalid_Should_throw_exception()
        {
            //Arrange
            var editor = Substitute.For<ISerializedObjectEditor>();
            //Act
            Action act = () => editor.Field(TestFieldName, p => p.intValue = 1);
            //Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Field_When_called_with_null_field_name_Should_throw_exception()
        {
            //Arrange
            var test = CreateTest<TestComponent>();
            var editor = test.component.EditSerializable();
            //Act
            Action act = () => editor.Field(null, p => p.intValue = 1);
            //Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Field_When_called_with_empty_field_name_Should_throw_exception()
        {
            //Arrange
            var test = CreateTest<TestComponent>();
            var editor = test.component.EditSerializable();
            //Act
            Action act = () => editor.Field(string.Empty, p => p.intValue = 1);
            //Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Field_When_called_with_unknown_field_name_Should_throw_exception()
        {
            //Arrange
            var test = CreateTest<TestComponent>();
            var editor = test.component.EditSerializable();
            //Act
            Action act = () => editor.Field(TestFieldName, p => p.intValue = 1);
            //Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Finalize_When_called_with_null_editor_Should_throw_exception()
        {
            //Arrange
            ISerializedObjectEditor editor = null;
            //Act
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => editor!.Apply();
            //Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Finalize_When_called_with_invalid_editor_Should_throw_exception()
        {
            //Arrange
            var editor = Substitute.For<ISerializedObjectEditor>();
            //Act
            Action act = () => editor.Apply();
            //Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Field_When_called_with_string_Should_set_value()
        {
            const string expected = "test";
            Field_When_called_for_field_Should_set_field_value<string, StringFieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<string, StringFieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        [Test]
        public void Field_When_called_with_int_Should_set_value()
        {
            const int expected = 1;
            Field_When_called_for_field_Should_set_field_value<int, IntFieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<int, IntFieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        [Test]
        public void Field_When_called_with_float_Should_set_value()
        {
            const float expected = 1.1f;
            Field_When_called_for_field_Should_set_field_value<float, FloatFieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<float, FloatFieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        [Test]
        public void Field_When_called_with_bool_Should_set_value()
        {
            const bool expected = true;
            Field_When_called_for_field_Should_set_field_value<bool, BoolFieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<bool, BoolFieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        [Test]
        public void Field_When_called_with_enum_Should_set_value()
        {
            const EnumFieldTestComponent.TestEnum expected = EnumFieldTestComponent.TestEnum.Test;
            Field_When_called_for_field_Should_set_field_value<EnumFieldTestComponent.TestEnum, EnumFieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<EnumFieldTestComponent.TestEnum,
                EnumFieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        [Test]
        public void Field_When_called_with_color_Should_set_value()
        {
            var expected = Color.red;
            Field_When_called_for_field_Should_set_field_value<Color, ColorFieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<Color, ColorFieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        [Test]
        public void Field_When_called_with_vector2_Should_set_value()
        {
            var expected = Vector2.one;
            Field_When_called_for_field_Should_set_field_value<Vector2, Vector2FieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<Vector2, Vector2FieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        [Test]
        public void Field_When_called_with_vector3_Should_set_value()
        {
            var expected = Vector3.one;
            Field_When_called_for_field_Should_set_field_value<Vector3, Vector3FieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<Vector3, Vector3FieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        [Test]
        public void Field_When_called_with_vector4_Should_set_value()
        {
            var expected = Vector4.one;
            Field_When_called_for_field_Should_set_field_value<Vector4, Vector4FieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<Vector4, Vector4FieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        [Test]
        public void Field_When_called_with_quaternion_Should_set_value()
        {
            var expected = Quaternion.identity;
            Field_When_called_for_field_Should_set_field_value<Quaternion, QuaternionFieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<Quaternion,
                QuaternionFieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        [Test]
        public void Field_When_called_with_rect_Should_set_value()
        {
            var expected = new Rect(1, 1, 1, 1);
            Field_When_called_for_field_Should_set_field_value<Rect, RectFieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<Rect, RectFieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        [Test]
        public void Field_When_called_with_bounds_Should_set_value()
        {
            var expected = new Bounds(Vector3.one, Vector3.one);
            Field_When_called_for_field_Should_set_field_value<Bounds, BoundsFieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<Bounds, BoundsFieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        [Test]
        public void Field_When_called_with_animation_curve_Should_set_value()
        {
            var expected = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
            Field_When_called_for_field_Should_set_field_value<AnimationCurve, AnimationCurveFieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<AnimationCurve,
                AnimationCurveFieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        [Test]
        public void Field_When_called_with_gradient_Should_set_value()
        {
            var expected = new Gradient();
            Field_When_called_for_field_Should_set_field_value<Gradient, GradientFieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<Gradient, GradientFieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        [Test]
        public void Field_When_called_with_object_reference_Should_set_value()
        {
            var expected = new GameObject();
            Field_When_called_for_field_Should_set_field_value<UnityEngine.Object, ObjectReferenceFieldTestComponent>(
                e => e.Field(TestFieldName, expected), expected);
            Field_When_called_for_auto_property_Should_set_auto_property_value<UnityEngine.Object,
                ObjectReferenceFieldTestComponent>(
                e => e.Field(TestPropertyName, expected), expected);
        }

        private static void Field_When_called_for_field_Should_set_field_value<T, TComponent>
            (Action<ISerializedObjectEditor> action, T expectedValue)
            where TComponent : FieldTestComponent<T>
        {
            CreateTest<T, TComponent>(action).TestField.Should()
                .Be(expectedValue, "because the field value should be set");
        }

        private static void Field_When_called_for_auto_property_Should_set_auto_property_value<T, TComponent>
            (Action<ISerializedObjectEditor> action, T expectedValue)
            where TComponent : FieldTestComponent<T>
        {
            CreateTest<T, TComponent>(action).TestProperty.Should()
                .Be(expectedValue, "because the property value should be set");
        }

        private static TComponent CreateTest<T, TComponent>(Action<ISerializedObjectEditor> action)
            where TComponent : FieldTestComponent<T>
        {
            //Arrange
            var test = CreateTest<TComponent>();
            var editor = test.component.EditSerializable();
            //Act
            action.Invoke(editor);
            editor.Apply();
            return test.component;
        }

        public abstract class FieldTestComponent<T> : TestComponent
        {
            [SerializeField] private T _testField;
            public T TestField => _testField;
            [field: SerializeField] public T TestProperty { get; [ExcludeFromCodeCoverage] private set; }
        }

        public class TestComponent : MonoBehaviour, IMonoBehaviourTest
        {
            [ExcludeFromCodeCoverage] public bool IsTestFinished => true;
        }

        private static MonoBehaviourTest<T> CreateTest<T>() where T : TestComponent =>
            new(false);


        public sealed class IntFieldTestComponent : FieldTestComponent<int>
        {
        }

        public sealed class StringFieldTestComponent : FieldTestComponent<string>
        {
        }

        public sealed class FloatFieldTestComponent : FieldTestComponent<float>
        {
        }

        public sealed class BoolFieldTestComponent : FieldTestComponent<bool>
        {
        }

        public sealed class EnumFieldTestComponent : FieldTestComponent<EnumFieldTestComponent.TestEnum>
        {
            public enum TestEnum
            {
                Test
            }
        }

        public sealed class ColorFieldTestComponent : FieldTestComponent<Color>
        {
        }

        public sealed class Vector2FieldTestComponent : FieldTestComponent<Vector2>
        {
        }

        public sealed class Vector3FieldTestComponent : FieldTestComponent<Vector3>
        {
        }

        public sealed class Vector4FieldTestComponent : FieldTestComponent<Vector4>
        {
        }

        public sealed class QuaternionFieldTestComponent : FieldTestComponent<Quaternion>
        {
        }

        public sealed class RectFieldTestComponent : FieldTestComponent<Rect>
        {
        }

        public sealed class BoundsFieldTestComponent : FieldTestComponent<Bounds>
        {
        }

        public sealed class AnimationCurveFieldTestComponent : FieldTestComponent<AnimationCurve>
        {
        }

        public sealed class GradientFieldTestComponent : FieldTestComponent<Gradient>
        {
        }

        public sealed class ObjectReferenceFieldTestComponent : FieldTestComponent<UnityEngine.Object>
        {
        }
    }
}