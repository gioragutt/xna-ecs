using ECS;
using ECS.BaseTypes;
using ECS.Interfaces;
using NUnit.Framework;
using System;
using System.Linq;

namespace ECSTest
{
    [TestFixture]
    public class EntityPoolTests
    {
        protected IEntityPool pool;
        protected IEntity entity;

        [SetUp]
        public void Init()
        {
            pool = new EntityPool(); 
            entity = new Entity(Guid.Empty);
        }
    }

    [TestFixture]
    public class BasicOperationsOnPool : EntityPoolTests
    {
        [Test]
        public void CountReturnsZeroWhenPoolIsEmpty()
        {
            Assert.AreEqual(pool.Count, 0);
        }

        [Test]
        public void ExistsWithNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => pool.Exists(null));
        }

        [Test]
        public void AddWithNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => pool.Add(null));
        }

        [Test]
        public void GetComponentsWithNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => pool.GetComponents(null));
        }

        [Test]
        public void ExistsWithEntityReturnsFalseBeforeInsertion()
        {
            Assert.IsFalse(pool.Exists(entity));
        }
    }

    [TestFixture]
    public class AddTests : EntityPoolTests
    {
        [Test]
        public void AfterAddingAnEntityExistsReturnsTrue()
        {
            Assert.IsFalse(pool.Exists(entity));
            pool.Add(entity);
            Assert.IsTrue(pool.Exists(entity));
        }

        [Test]
        public void AddingSameEntityMoreThanOnceOnlyAddsItOnce()
        {
            for (var i = 0; i < 3; ++i)
            {
                pool.Add(entity);
                Assert.AreEqual(pool.Count, 1);
            }
        }
    }

    [TestFixture]
    public class GetComponentTests : EntityPoolTests
    {
        [Test]
        public void GettingAComponentOfAnEntityThatsNotInThePoolReturnsNull()
        {
            Assert.IsNull(pool.GetComponents(entity));
        }

        [Test]
        public void GettingAComponentOfAnEntityThatsInThePoolReturnsAValidContainer()
        {
            pool.Add(entity);
            Assert.IsNotNull(pool.GetComponents(entity));
        }
    }

    [TestFixture]
    public class GetAllOfTests : EntityPoolTests
    {
        [Test]
        public void GettingAllOfATypeReturnsAnEmptyListWhenPoolIsEmpty()
        {
            Assert.AreEqual(pool.GetAllOf<DummyComponent>().Count, 0);
        }

        [Test]
        public void GettingAllOfATypeReturnsAnEmptyListWhenPoolIsNotEmpty()
        {
            pool.Add(entity);
            Assert.AreEqual(pool.GetAllOf<DummyComponent>().Count, 0);
        }

        [Test]
        public void GettingAllOfDummyComponentReturnsTheEntityWhenItHasIt()
        {
            pool.Add(entity);
            var container = pool.GetComponents(entity);
            var component = new DummyComponent();
            container.Add(component);
            var allDummies = pool.GetAllOf<DummyComponent>();
            Assert.AreEqual(allDummies.Count, 1);
            Assert.AreSame(component, allDummies.First());
        }
    }

    [TestFixture]
    public class AllThatTest : EntityPoolTests
    {
        [Test]
        public void GetAllThatReturnsEmptyWhenPoolIsEmpty()
        {
            Assert.AreEqual(pool.AllThat(c => true).Count(), 0);
        }

        [Test]
        public void AddingAComponentToAContainerReturnsIt()
        {
            pool.Add(entity);
            var container = pool.GetComponents(entity);
            container.Add(new DummyComponent());
            Assert.AreEqual(pool.AllThat(c => c.Has<DummyComponent>()).Count(), 1);
        }

        [Test]
        public void TwoEntitiesInPoolOnlyOneHasDummy()
        {
            pool.AddToPool(entity).Add(new DummyComponent());
            pool.AddToPool(new Entity(Guid.NewGuid())).Add(new DummyComponent());
            Assert.AreEqual(pool.AllThat(c => c.Has<DummyComponent>()).Count(), 2);
        }
        [Test]
        public void TwoEntitiesInPoolBothHaveDummies()
        {
            pool.Add(entity);
            var container = pool.GetComponents(entity);
            container.Add(new DummyComponent());
            var anotherEntity = new Entity(Guid.NewGuid());
            pool.Add(anotherEntity);

            Assert.AreEqual(pool.AllThat(c => c.Has<DummyComponent>()).Count(), 1);
        }
    }
}