using CAFU.Core.Presentation.View;
using CAFU.DragDrop.Presentation.Presenter;
using UniRx;
using UnityEngine;

namespace CAFU.DragDrop.Presentation.View
{
    [RequireComponent(typeof(Draggable))]
    public class ResetPositionOnDropped : MonoBehaviour, IView
    {
        private Vector3 originalPosition;

        void Start()
        {
            this.originalPosition = this.transform.localPosition;

            this.GetPresenter<IDragDropPresenter>()
                .GetFixedDragDropAsObservable()
                .Subscribe(list => this.transform.localPosition = this.originalPosition)
                .AddTo(this);
        }
    }
}