using System;
using System.Collections.Generic;
using CAFU.DragDrop.Domain.Model;

namespace CAFU.DragDrop.Domain.UseCase
{
    public interface ISelectiveDragDropUseCase
    {
        IDictionary<int, int> DragId2DropIdMap { get; }

        void SetUp(IList<int> dragIds);
        void Drag(IDropModel drop, IDragModel drag);
        void DragOut(IDropModel drop, IDragModel drag);

        IDropModel Drop(IDragModel drag);
        IObservable<IDragModel> GetDraggingAsObservable(IDropModel drop);
        IObservable<DragDropModel> MostOverlapDroppingAsObservable { get; }
        IObservable<IList<DragDropModel>> FixedDragDropAsObservable { get; }
        IObservable<IDropModel> GetDroppedAsObservable(IDragModel drag);
    }
}