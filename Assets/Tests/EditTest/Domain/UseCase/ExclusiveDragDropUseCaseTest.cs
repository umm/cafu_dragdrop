using CAFU.DragDrop.Domain.Model;
using CAFU.DragDrop.Domain.UseCase;
using ExtraUniRx;
using NUnit.Framework;
using UnityEngine;

namespace EditTest.CAFU.DragDrop.Domain.UseCase
{
    public class ExclusiveDragDropUseCaseTest
    {
        [Test]
        public void GetDroppedAsObservableTest()
        {
            var usecase = new ExclusiveDragDropUseCase();
            var collider1 = CreateCollider2D();
            var collider3 = CreateCollider2D();
            var collider4 = CreateCollider2D();
            var dropModel1 = new DropModel {Id = 1, Collider2D = collider1};
            var dragModel1 = new DragModel {Id = 1, Collider2D = collider3};
            var dragModel2 = new DragModel {Id = 2, Collider2D = collider4};
            var observer = new TestObserver<IDropModel>();
            usecase.GetDroppedAsObservable(dragModel1).Subscribe(observer);

            // drag1 into void
            Assert.IsNull(usecase.Drop(dragModel1));

            // drag1 into drop1
            usecase.Drag(dropModel1, dragModel1);
            Assert.IsNotNull(usecase.Drop(dragModel1));

            Assert.AreEqual(1, observer.OnNextCount);
            Assert.AreEqual(dropModel1.GetInstanceID(), observer.OnNextLastValue.GetInstanceID());
            Assert.AreEqual(dropModel1.Id, observer.OnNextLastValue.Id);

            // drag2 into drop1
            usecase.Drag(dropModel1, dragModel2);
            Assert.IsNotNull(usecase.Drop(dragModel2));

            Assert.AreEqual(2, observer.OnNextCount);
            Assert.IsNull(observer.OnNextLastValue);
        }

        private BoxCollider2D CreateCollider2D()
        {
            var go = new GameObject();
            return go.AddComponent<BoxCollider2D>();
        }

        class DropModel : IDropModel
        {
            public int Id { get; set; }
            public Collider2D Collider2D { get; set; }
        }

        class DragModel : IDragModel
        {
            public int Id { get; set; }
            public Collider2D Collider2D { get; set; }
        }
    }
}