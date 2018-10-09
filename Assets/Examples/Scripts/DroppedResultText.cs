using System.Collections.Generic;
using System.Linq;
using CAFU.Core.Presentation.View;
using CAFU.DragDrop.Domain.Model;
using CAFU.DragDrop.Presentation.Presenter;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace Example.CAFU.DragDrop.Presentation.View
{
    [RequireComponent(typeof(Text))]
    public class DroppedResultText : MonoBehaviour, IView
    {
        private Text Text => this.GetComponent<Text>();

        void Start()
        {
            this.GetPresenter<IDragDropPresenter>()
                .GetFixedDragDropAsObservable()
                .Subscribe(this.Render)
                .AddTo(this);
        }

        void Render(IList<DragDropModel> list)
        {
            var result = string.Join(",", list.Select(it => "{" + $"Drag:{it.Drag.Id}, Drop:{it.Drop.Id}" + "}"));
            this.Text.text = $"Dropped Result: [{result}]";
        }
    }
}