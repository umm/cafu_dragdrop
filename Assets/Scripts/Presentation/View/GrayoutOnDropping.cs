using System.Linq;
using CAFU.Core.Presentation.View;
using CAFU.DragDrop.Domain.Model;
using CAFU.DragDrop.Presentation.Presenter;
using CAFU.DragDrop.Presentation.View;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype.PretendLand.Mufg.Presentation.View
{
    // Dropして配置済みの場合は、無効にする
    [RequireComponent(typeof(Image))]
    public class GrayoutOnDropping : MonoBehaviour, IView
    {
        public Draggable Draggable;
        public Color GrayoutColor = Color.gray;

        void Start()
        {
            var image = this.GetComponent<Image>();
            var originalColor = image.color;

            this.GetPresenter<IDragDropPresenter>()
                .GetFixedDragDropAsObservable()
                .Select(list => list.Any(it => it.Drag.GetInstanceID() == this.Draggable.DragModel.GetInstanceID()))
                .Subscribe(isDropping => image.color = isDropping ? this.GrayoutColor : originalColor)
                .AddTo(this);
        }
    }
}