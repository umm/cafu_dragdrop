using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CAFU.DragDrop.Presentation.View
{
    [RequireComponent(typeof(Image))]
    public class DragMove : UIBehaviour
    {
        public Camera Camera;

        public Image Image => this.GetComponent<Image>();

        protected override void Awake()
        {
            base.Awake();

            if (this.Camera == default(Camera))
            {
                this.Camera = Camera.main;
            }
        }

        protected override void Start()
        {
            base.Start();

            var thisRectTransform = this.GetComponent<RectTransform>();
            var parentRectTransform = this.transform.parent.GetComponent<RectTransform>();

            this.OnDragAsObservable()
                .Select(it => it.position)
                .Select(position =>
                {
                    Vector2 outPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        parentRectTransform,
                        position,
                        this.Camera,
                        out outPoint
                    );
                    return outPoint;
                })
                .Select(it => new Vector3(it.x, it.y, 0))
                .Subscribe(position => thisRectTransform.localPosition = position)
                .AddTo(this)
                ;
        }
    }
}