using System;
using ECS.BaseTypes;
using ECS.Interfaces;
using NUnit.Framework;

namespace ECSTest
{

    [TestFixture]
    public class ComponentContainerTest
    {
        protected IComponentContainer container;

        [SetUp]
        public void Init() { container = new ComponentContainer(); }
    }

    [TestFixture]
    public class EmptyContainerTest : ComponentContainerTest
    {
        [Test]
        public void CallingExistsOnAnyTypeOfComponentReturnsFalse()
        {
            Assert.IsFalse(container.Has<DummyComponent>());
        }

        [Test]
        public void CallingGetComponentOnAnyTypeOfComponentReturnsNull()
        {
            Assert.IsNull(container.Get<DummyComponent>());
        }
    }

    [TestFixture]
    public class AddingToContainerTest : ComponentContainerTest
    {
        #region Helper Methods

        protected void AddDummy()
        {
            container.Add(new DummyComponent());
        }

        protected void AddDummies(int count)
        {
            for (var i = 0; i < count; ++i)
                container.Add(new DummyComponent());
        }

        protected void InsertBaseAndDerived(bool sameBaseType)
        {
            if (sameBaseType)
            {
                container.Add<DummyComponent>(new DummyComponent());
                container.Add<DummyComponent>(new AnotherDummyComponent());
            }
            else
            {
                container.Add<DummyComponent>(new DummyComponent());
                container.Add<AnotherDummyComponent>(new AnotherDummyComponent());
            }
        }

        #endregion

        [Test]
        public void AddingNullComponentThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => container.Add<IComponent>(null));
        }

        [Test]
        public void AfterAddingAComponentHasComponentReturnsTrueForThatType()
        {
            AddDummy();
            Assert.That(container.Has<DummyComponent>());
        }

        [Test]
        public void AfterAddingAComponentCountIsOne()
        {
            AddDummy();
            Assert.AreEqual(container.Count, 1);
        }

        [Test]
        public void AfterAddingAComponentGetComponentReturnsSameComponent()
        {
            var component = new DummyComponent();
            container.Add(component);
            Assert.AreSame(component, container.Get<DummyComponent>());
        }

        [Test]
        public void AfterAddingAComponentCheckingIfADifferentTypeExistsWillReturnFalse()
        {
            AddDummy();
            Assert.IsFalse(container.Has<CounterComponent>());
        }

        [Test]
        public void AfterAddingAComponentGetComponentReturnsNullForDifferentType()
        {
            AddDummy();
            Assert.IsNull(container.Get<CounterComponent>());
        }

        [Test]
        public void AddingAComponentOfSameTypeTwiceDoesntAddItMoreThanOnce()
        {
            AddDummies(7);
            Assert.AreEqual(container.Count, 1);
        }

        [Test]
        public void AddingAComponentOfTypeAndAnotherOfADerivedTypeInsertsBoth()
        {
            InsertBaseAndDerived(sameBaseType: false);
            Assert.AreEqual(container.Count, 2);
        }

        [Test]
        public void AddingAComponentOfTypeAndAnotherOfADerivedTypeWhenSpecifingBaseAddtsItOnce()
        {
            InsertBaseAndDerived(sameBaseType: true);
            Assert.AreEqual(container.Count, 1);
        }
    }

    [TestFixture]
    public class SameBaseTypeTests : AddingToContainerTest
    {
        [Test]
        public void AddingBaseAndDerivedWouldReturnBaseOnGet()
        {
            var justADummy = new DummyComponent();
            var anotherDummy = new AnotherDummyComponent();
            container.Add(justADummy);
            container.Add(anotherDummy);
            var getDummy = container.Get<DummyComponent>();
            Assert.AreSame(getDummy, justADummy);
            Assert.AreNotSame(getDummy, anotherDummy);
        }

        [Test]
        public void AddingBaseAndDerivedWouldReturnDerivedOnGetWithDerived()
        {
            var justADummy = new DummyComponent();
            var anotherDummy = new AnotherDummyComponent();
            container.Add(justADummy);
            container.Add(anotherDummy);
            var getDummy = container.Get<AnotherDummyComponent>();
            Assert.AreSame(getDummy, anotherDummy);
            Assert.AreNotSame(getDummy, justADummy);
        }

        [Test]
        public void AddingOnlyDerivedWouldReturnItOnGet()
        {
            var anotherDummy = new AnotherDummyComponent();
            container.Add(anotherDummy);
            var getDummy = container.Get<DummyComponent>();
            Assert.AreSame(getDummy, anotherDummy);
        }

        [Test]
        public void AddingBothBaseAndDerivedReturnsBothOnGetAllOfBase()
        {
            InsertBaseAndDerived(sameBaseType: false);
            Assert.AreEqual(container.GetAllOf<DummyComponent>().Count, 2);
        }
    }
}
