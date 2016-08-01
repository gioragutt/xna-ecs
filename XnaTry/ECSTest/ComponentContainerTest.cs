using ECS;
using NUnit.Framework;

namespace ECSTest
{
    [TestFixture]
    public class EmptyContainerTest
    {
        protected IComponentContainer container;

        [SetUp]
        public void Init()
        {
            container = new ComponentContainer();
        }

        [Test]
        public void CallingExistsOnAnyTypeOfComponentReturnsFalse()
        {
            Assert.IsFalse(container.HasComponent<DummyComponent>());
        }

        [Test]
        public void CallingGetComponentOnAnyTypeOfComponentReturnsNull()
        {
            Assert.IsNull(container.GetComponent<DummyComponent>());
        }
    }

    [TestFixture]
    public class AddingToContainerTest : EmptyContainerTest
    {
        [Test]
        public void AfterAddingAComponentHasComponentReturnsTrueForThatType()
        {
            container.AddComponent(new DummyComponent());
            Assert.That(container.HasComponent<DummyComponent>());
        }

        [Test]
        public void AfterAddingAComponentGetComponentReturnsSameComponent()
        {
            var component = new DummyComponent();
            container.AddComponent(component);
            Assert.AreSame(component, container.GetComponent<DummyComponent>());
        }

        [Test]
        public void AfterAddingAComponentCheckingIfADifferentTypeExistsWillReturnFalse()
        {
            container.AddComponent(new DummyComponent());
            Assert.IsFalse(container.HasComponent<CounterComponent>());
        }

        [Test]
        public void AfterAddingAComponentGetComponentReturnsNullForDifferentType()
        {
            container.AddComponent(new DummyComponent());
            Assert.IsNull(container.GetComponent<CounterComponent>());
        }

        [Test]
        public void AddingAComponentOfSameTypeTwiceDoesntAddItMoreThanOnce()
        {
            container.AddComponent<DummyComponent>(new DummyComponent());
            container.AddComponent<DummyComponent>(new DummyComponent());
            Assert.AreEqual(container.Count, 1);
        }
    }
}
