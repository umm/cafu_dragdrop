using System;
using System.Collections.Generic;
using CAFU.DragDrop.Domain.Model;

namespace CAFU.DragDrop.Domain.UseCase
{
    // Drag選択肢の中からDrop選択肢へ選ぶタイプのDrag&Drop. 例: 6キャラから、3キャラ選出
    public class SelectiveDragDropUseCase : ISelectiveDragDropUseCase
    {
        public IDictionary<int, int> DragId2DropIdMap { get; private set; }
        private ExclusiveDragDropUseCase ExDragDropUseCase { get; set; }

        public SelectiveDragDropUseCase()
        {
            this.DragId2DropIdMap = new Dictionary<int, int>();
            this.ExDragDropUseCase = new ExclusiveDragDropUseCase();
        }

        public void SetUp(IList<int> dragIds)
        {
            foreach (var dragId in dragIds)
            {
                this.DragId2DropIdMap[dragId] = -1;
            }
        }

        public void Drag(IDropModel drop, IDragModel drag)
        {
            this.ExDragDropUseCase.Drag(drop, drag);
        }

        public void DragOut(IDropModel drop, IDragModel drag)
        {
            this.ExDragDropUseCase.DragOut(drop, drag);
        }

        public IDropModel Drop(IDragModel drag)
        {
            var drop = this.ExDragDropUseCase.Drop(drag);

            if (drop != default(IDropModel))
            {
                this.DragId2DropIdMap[drag.GetInstanceID()] = drop.GetInstanceID();
            }
            else
            {
                this.DragId2DropIdMap[drag.GetInstanceID()] = -1;
            }

            return drop;
        }

        public IObservable<IDragModel> GetDraggingAsObservable(IDropModel drop)
        {
            return this.ExDragDropUseCase.GetDraggingAsObservable(drop);
        }

        public IObservable<DragDropModel> MostOverlapDroppingAsObservable
            => this.ExDragDropUseCase.MostOverlapDroppingAsObservable;

        public IObservable<IList<DragDropModel>> FixedDragDropAsObservable
            => this.FixedDragDropAsObservable;

        public IObservable<IDropModel> GetDroppedAsObservable(IDragModel drag)
        {
            return this.ExDragDropUseCase.GetDroppedAsObservable(drag);
        }
    }
}