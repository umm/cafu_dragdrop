using CAFU.Core.Presentation.View;
using CAFU.DragDrop.Domain.Model;
using CAFU.DragDrop.Presentation.Presenter;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace CAFU.DragDrop.Presentation.View
{
    // Dropされたイベントを送信する
    public class Droppable : MonoBehaviour, IView
    {
        public Collider2D Collider;
        public int Id;

        void Start()
        {
            var dropModel = this.CreateDropModel();

            this.Collider.OnTriggerEnter2DAsObservable()
                .Select(collider => collider.GetComponent<Draggable>())
                .Where(draggable => draggable != default(Draggable))
                .Subscribe(draggable => this.GetPresenter<IDragDropPresenter>().Drag(dropModel, draggable.DragModel))
                .AddTo(this);

            this.Collider.OnTriggerStay2DAsObservable()
                .Select(collider => collider.GetComponent<Draggable>())
                .Where(draggable => draggable != default(Draggable))
                .Subscribe(draggable => this.GetPresenter<IDragDropPresenter>().Drag(dropModel, draggable.DragModel))
                .AddTo(this);

            this.Collider.OnTriggerExit2DAsObservable()
                .Select(collider => collider.GetComponent<Draggable>())
                .Where(draggable => draggable != default(Draggable))
                .Subscribe(draggable => this.GetPresenter<IDragDropPresenter>().DragOut(dropModel, draggable.DragModel))
                .AddTo(this);
        }

        protected virtual IDropModel CreateDropModel()
        {
            return new DropModel
            {
                Id = this.Id,
                Collider2D = this.Collider,
            };
        }
    }
}