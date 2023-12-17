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
    public class LifecycleEventsExecutionTests
    {
        [Test]
        public void ExecuteAwake_When_component_has_method_Should_invoke_method() =>
            Execute_When_component_has_method(c => c.ExecuteAwake()).Received().Awake();

        [Test]
        public void ExecuteAwake_When_base_component_has_method_Should_invoke_method() =>
            Execute_When_base_component_has_method(c => c.ExecuteAwake()).Received().Awake();

        [Test]
        public void ExecuteAwake_When_component_has_no_method_Should_throw_exception() =>
            Execute_When_component_has_no_method(c => c.ExecuteAwake()).Should().Throw<ArgumentException>();

        [Test]
        public void ExecuteOnEnable_When_component_has_method_Should_invoke_method() =>
            Execute_When_component_has_method(c => c.ExecuteOnEnable()).Received().OnEnable();

        [Test]
        public void ExecuteOnEnable_When_base_component_has_method_Should_invoke_method() =>
            Execute_When_base_component_has_method(c => c.ExecuteOnEnable()).Received().OnEnable();

        [Test]
        public void ExecuteOnEnable_When_component_has_no_method_Should_throw_exception() =>
            Execute_When_component_has_no_method(c => c.ExecuteOnEnable()).Should().Throw<ArgumentException>();

        [Test]
        public void ExecuteStart_When_component_has_method_Should_invoke_method() =>
            Execute_When_component_has_method(c => c.ExecuteStart()).Received().Start();

        [Test]
        public void ExecuteStart_When_base_component_has_method_Should_invoke_method() =>
            Execute_When_base_component_has_method(c => c.ExecuteStart()).Received().Start();

        [Test]
        public void ExecuteStart_When_component_has_no_method_Should_throw_exception() =>
            Execute_When_component_has_no_method(c => c.ExecuteStart()).Should().Throw<ArgumentException>();

        [Test]
        public void ExecuteFixedUpdate_When_component_has_method_Should_invoke_method() =>
            Execute_When_component_has_method(c => c.ExecuteFixedUpdate()).Received().FixedUpdate();

        [Test]
        public void ExecuteFixedUpdate_When_base_component_has_method_Should_invoke_method() =>
            Execute_When_base_component_has_method(c => c.ExecuteFixedUpdate()).Received().FixedUpdate();

        [Test]
        public void ExecuteFixedUpdate_When_component_has_no_method_Should_throw_exception() =>
            Execute_When_component_has_no_method(c => c.ExecuteFixedUpdate()).Should().Throw<ArgumentException>();

        [Test]
        public void ExecuteUpdate_When_component_has_method_Should_invoke_method() =>
            Execute_When_component_has_method(c => c.ExecuteUpdate()).Received().Update();

        [Test]
        public void ExecuteUpdate_When_base_component_has_method_Should_invoke_method() =>
            Execute_When_base_component_has_method(c => c.ExecuteUpdate()).Received().Update();

        [Test]
        public void ExecuteUpdate_When_component_has_no_method_Should_throw_exception() =>
            Execute_When_component_has_no_method(c => c.ExecuteUpdate()).Should().Throw<ArgumentException>();

        [Test]
        public void ExecuteLateUpdate_When_component_has_method_Should_invoke_method() =>
            Execute_When_component_has_method(c => c.ExecuteLateUpdate()).Received().LateUpdate();

        [Test]
        public void ExecuteLateUpdate_When_base_component_has_method_Should_invoke_method() =>
            Execute_When_base_component_has_method(c => c.ExecuteLateUpdate()).Received().LateUpdate();

        [Test]
        public void ExecuteLateUpdate_When_component_has_no_method_Should_throw_exception() =>
            Execute_When_component_has_no_method(c => c.ExecuteLateUpdate()).Should().Throw<ArgumentException>();

        [Test]
        public void ExecuteOnDisable_When_component_has_method_Should_invoke_method() =>
            Execute_When_component_has_method(c => c.ExecuteOnDisable()).Received().OnDisable();

        [Test]
        public void ExecuteOnDisable_When_base_component_has_method_Should_invoke_method() =>
            Execute_When_base_component_has_method(c => c.ExecuteOnDisable()).Received().OnDisable();

        [Test]
        public void ExecuteOnDisable_When_component_has_no_method_Should_throw_exception() =>
            Execute_When_component_has_no_method(c => c.ExecuteOnDisable()).Should().Throw<ArgumentException>();

        [Test]
        public void ExecuteOnDestroy_When_component_has_method_Should_invoke_method() =>
            Execute_When_component_has_method(c => c.ExecuteOnDestroy()).Received().OnDestroy();

        [Test]
        public void ExecuteOnDestroy_When_base_component_has_method_Should_invoke_method() =>
            Execute_When_base_component_has_method(c => c.ExecuteOnDestroy()).Received().OnDestroy();

        [Test]
        public void ExecuteOnDestroy_When_component_has_no_method_Should_throw_exception() =>
            Execute_When_component_has_no_method(c => c.ExecuteOnDestroy()).Should().Throw<ArgumentException>();


        [Test]
        public void ExecuteChainOfEvents_When_chain_is_not_empty_Should_invoke_methods()
        {
            //Arrange & Act
            var mock = Execute_When_component_has_method(c =>
                c.ExecuteAwake().ExecuteOnEnable().ExecuteStart());
            //Assert
            mock.Received(1).Awake();
            mock.Received(1).OnEnable();
            mock.Received(1).Start();
        }

        [Test]
        public void ExecuteChainOfEvents_When_component_has_no_methods_Should_throw_exception() =>
            Execute_When_component_has_no_method(c => c.ExecuteAwake().ExecuteOnEnable().ExecuteStart())
                .Should().Throw<ArgumentException>();
        
        [Test]
        public void ExecuteUnityEvent_When_event_method_has_exception_Should_throw_exception()
        {
            //Arrange
            var mock = Substitute.For<IMonoBehaviourMethods>();
            mock.When(m => m.Awake()).Do(_ => throw new Exception("Test"));
            var test = TestComponent.Create(mock);
            Action act = () => test.component.ExecuteAwake();
            //Act & Assert
            act.Should().Throw<Exception>().WithMessage("Test");
        }

        private static IMonoBehaviourMethods Execute_When_component_has_method(Action<Component> action)
        {
            //Arrange
            var mock = Substitute.For<IMonoBehaviourMethods>();
            var test = TestComponent.Create(mock);
            //Act & Return
            action(test.component);
            return mock;
        }

        private static IMonoBehaviourMethods Execute_When_base_component_has_method(Action<Component> action)
        {
            //Arrange
            var mock = Substitute.For<IMonoBehaviourMethods>();
            var test = InheritTestComponent.Create(mock);
            //Act & Return
            action(test.component);
            return mock;
        }

        private static Action Execute_When_component_has_no_method(Action<Component> action) =>
            () => action(EmptyTestComponent.Create().component);

        private class EmptyTestComponent : MonoBehaviour, IMonoBehaviourTest
        {
            public static MonoBehaviourTest<EmptyTestComponent> Create() =>
                new(false);

            [ExcludeFromCodeCoverage]
            public bool IsTestFinished => true;
        }

        private class InheritTestComponent : TestComponent
        {
            public new static MonoBehaviourTest<InheritTestComponent> Create(IMonoBehaviourMethods mock)
            {
                var test = new MonoBehaviourTest<InheritTestComponent>(false);
                test.component.Set(mock);
                return test;
            }
        }

        private class TestComponent : MonoBehaviour, IMonoBehaviourTest
        {
            private IMonoBehaviourMethods _mock;

            public static MonoBehaviourTest<TestComponent> Create(IMonoBehaviourMethods mock)
            {
                var test = new MonoBehaviourTest<TestComponent>(false);
                test.component.Set(mock);
                return test;
            }

            protected void Set(IMonoBehaviourMethods mock) =>
                _mock = mock;

            [ExcludeFromCodeCoverage]
            public bool IsTestFinished => true;

            private void Awake() =>
                _mock.Awake();

            private void OnEnable() =>
                _mock.OnEnable();

            private void Start() =>
                _mock.Start();

            private void FixedUpdate() =>
                _mock.FixedUpdate();

            private void Update() =>
                _mock.Update();

            private void LateUpdate() =>
                _mock.LateUpdate();

            private void OnDisable() =>
                _mock.OnDisable();

            private void OnDestroy() =>
                _mock.OnDestroy();
        }

        public interface IMonoBehaviourMethods
        {
            void Awake();
            void OnEnable();
            void Start();
            void FixedUpdate();
            void Update();
            void LateUpdate();
            void OnDisable();
            void OnDestroy();
        }
    }
}