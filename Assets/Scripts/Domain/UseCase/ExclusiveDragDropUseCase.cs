using System;
using System.Collections.Generic;
using System.Linq;
using CAFU.DragDrop.Domain.Model;
using ExtraCollection;
using UniRx;
using UnityModule.ExtraBounds;

namespace CAFU.DragDrop.Domain.UseCase
{
    // Dropが排他的. 例: Drop先が3つあり、Dragしてくるものがどれか一つに入る
    public class ExclusiveDragDropUseCase : IDragDropUseCase
    {
        // map
        private IDictionary<int, IDictionary<int, float>> DragId2DropId2AreaMap
            = new Dictionary<int, IDictionary<int, float>>();

        private IDictionary<int, IDropModel> DropId2DropMap
            = new Dictionary<int, IDropModel>();

        // subject
        private ISubject<DragDropModel> MostOverlapDroppingSubject
            = new Subject<DragDropModel>();

        // subject map
        private IDictionary<int, ISubject<IDragModel>> DropId2DragSubjectMap
            = new Dictionary<int, ISubject<IDragModel>>();

        private IDictionary<int, ISubject<IDropModel>> DragId2DropSubjectMap
            = new Dictionary<int, ISubject<IDropModel>>();

        private ReactiveProperty<List<DragDropModel>> FixedDragDropListProperty =
            new ReactiveProperty<List<DragDropModel>>(new List<DragDropModel>());

        // Observables
        public IObservable<DragDropModel> MostOverlapDroppingAsObservable
            => this.MostOverlapDroppingSubject;

        public IObservable<IList<DragDropModel>> FixedDragDropAsObservable
            => this.FixedDragDropListProperty;

        public IObservable<IDragModel> GetDraggingAsObservable(IDropModel drop)
        {
            return this.DropId2DragSubjectMap.GetOrSet(drop.GetInstanceID(), () => new Subject<IDragModel>());
        }

        public IObservable<IDropModel> GetDroppedAsObservable(IDragModel drag)
        {
            return this.DragId2DropSubjectMap.GetOrSet(drag.GetInstanceID(), () => new Subject<IDropModel>());
        }

        public void DragOut(IDropModel drop, IDragModel drag)
        {
            var dropId = drop.GetInstanceID();
            var dragId = drag.GetInstanceID();

            this.DropId2DropMap.Remove(dropId);
            this.DragId2DropId2AreaMap.GetOrSet(dragId, () => new Dictionary<int, float>()).Remove(dropId);

            if (this.DragId2DropId2AreaMap[dragId].Count <= 0)
            {
                this.NotifyDragEmpty(drop, drag);
            }
            else
            {
                this.NotifyDrag(drop, drag);
            }
        }

        public void Drag(IDropModel drop, IDragModel drag)
        {
            var dropId = drop.GetInstanceID();
            var dragId = drag.GetInstanceID();
            var overlapArea = drop.Collider2D.CalcOverlapArea(drag.Collider2D);

            this.DropId2DropMap[dropId] = drop;
            this.DragId2DropId2AreaMap.GetOrSet(dragId, () => new Dictionary<int, float>())[dropId] = overlapArea;
            this.NotifyDrag(drop, drag);
        }

        public IDropModel Drop(IDragModel drag)
        {
            var dragId = drag.GetInstanceID();
            var drop = default(IDropModel);

            // dragに対するdropが存在する場合
            if (this.DragId2DropId2AreaMap.ContainsKey(dragId))
            {
                // dragに該当する最大のdropを探す
                var dropId = this.DragId2DropId2AreaMap[dragId].FindMaxOrDefault(it => it.Value).Key;
                drop = this.DropId2DropMap.GetOrDefault(dropId);

                // 他に専有していたDragがあれば、専有しなくなったことを通知する
                var found = this.FixedDragDropListProperty.Value.Find(it => it.Drop.GetInstanceID() == dropId);
                if (found != default(DragDropModel))
                {
                    this.DragId2DropSubjectMap
                        .GetOrSet(found.Drag.GetInstanceID(), () => new Subject<IDropModel>())
                        .OnNext(null);
                }

                // Dropを通知する
                this.DragId2DropSubjectMap.GetOrSet(dragId, () => new Subject<IDropModel>()).OnNext(drop);
                this.DragId2DropId2AreaMap[dragId].Clear();

                // 確定したDragDropを更新する
                this.FixedDragDropListProperty.Value = this.FixedDragDropListProperty.Value
                    .Where(it => it.Drag.GetInstanceID() != dragId)
                    .Where(it => it.Drop.GetInstanceID() != dropId)
                    .ToList();

                if (drop != default(IDropModel))
                {
                    this.FixedDragDropListProperty.Value.Add(new DragDropModel {Drag = drag, Drop = drop});
                }
            }

            // 通知する
            this.FixedDragDropListProperty.SetValueAndForceNotify(this.FixedDragDropListProperty.Value);
            return drop;
        }

        private void NotifyDragEmpty(IDropModel drop, IDragModel drag)
        {
            var dropId = drop.GetInstanceID();

            this.MostOverlapDroppingSubject.OnNext(new DragDropModel {Drop = null, Drag = drag});
            this.DropId2DragSubjectMap.GetOrSet(dropId, () => new Subject<IDragModel>()).OnNext(null);
        }

        private void NotifyDrag(IDropModel drop, IDragModel drag)
        {
            var dropId = drop.GetInstanceID();
            var dragId = drag.GetInstanceID();

            // NOTE: drop数がおおいと遅いかも
            var mostOverlapDropColliderId = this.DragId2DropId2AreaMap[dragId].FindMaxOrDefault(it => it.Value).Key;
            var mostOverlapDropCollider = this.DropId2DropMap[mostOverlapDropColliderId];
            this.MostOverlapDroppingSubject.OnNext(new DragDropModel {Drop = mostOverlapDropCollider, Drag = drag});
            this.DropId2DragSubjectMap.GetOrSet(dropId, () => new Subject<IDragModel>()).OnNext(drag);
        }
    }
}