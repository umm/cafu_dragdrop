using System.Linq;
using CAFU.Core.Presentation.View;
using CAFU.DragDrop.Domain.Model;
using CAFU.DragDrop.Presentation.Presenter;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CAFU.DragDrop.Presentation.View
{
    [RequireComponent(typeof(Image))]
    public class ImageOnDropping : MonoBehaviour, IView
    {
        public Droppable Droppable;
        public Sprite DefaultSprite;
        private Image Image;

        void Start()
        {
            var droppableCollider = this.Droppable.Collider;
            this.Image = this.GetComponent<Image>();

            this.GetPresenter<IDragDropPresenter>()
                .GetFixedDragDropAsObservable()
                .Select(list => list.ToList().Find(it => it.Drop.GetInstanceID() == droppableCollider.GetInstanceID()))
                .Select(matched => matched?.GetDrag<DragModel>())
                .Subscribe(this.SetImage)
                .AddTo(this);
        }

        public void SetImage(DragModel dragModel)
        {
            this.Image.sprite = dragModel != default(DragModel) ? dragModel.Sprite : this.DefaultSprite;
        }
    }
}