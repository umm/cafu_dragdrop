using CAFU.Core.Presentation.View;
using CAFU.DragDrop.Domain.Model;
using CAFU.DragDrop.Presentation.Presenter;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CAFU.DragDrop.Presentation.View
{
    public class Draggable : UIBehaviour, IView
    {
        public IDragModel DragModel => this.dragModel = this.dragModel ?? this.CreateDragModel();
        private IDragModel dragModel;

        public Collider2D Collider2D;
        public Sprite Sprite;
        public int Id;

        protected override void Start()
        {
            base.Start();

            this.OnEndDragAsObservable()
                .Subscribe(_ => this.GetPresenter<IDragDropPresenter>().Drop(this.DragModel));
        }

        protected virtual IDragModel CreateDragModel()
        {
            return new DragModel
            {
                Id = this.Id,
                Collider2D = this.Collider2D,
                Sprite = this.Sprite,
            };
        }
    }
}