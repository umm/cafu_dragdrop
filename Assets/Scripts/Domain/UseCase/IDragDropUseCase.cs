using System;
using System.Collections.Generic;
using CAFU.Core.Domain.UseCase;
using CAFU.DragDrop.Domain.Model;

namespace CAFU.DragDrop.Domain.UseCase
{
    public interface IDragDropUseCase : IUseCase
    {
        void Drag(IDropModel drop, IDragModel drag);
        void DragOut(IDropModel drop, IDragModel drag);
        IDropModel Drop(IDragModel drag);

        // Dragging中のModelが流れる
        IObservable<IDragModel> GetDraggingAsObservable(IDropModel drop);

        // Drop時にDropModelが流れる
        IObservable<IDropModel> GetDroppedAsObservable(IDragModel drag);

        // 重複領域が一番大きい組み合わせが流れる
        IObservable<DragDropModel> MostOverlapDroppingAsObservable { get; }

        // Dropしたときの決定値が流れる
        IObservable<IList<DragDropModel>> FixedDragDropAsObservable { get; }
    }
}