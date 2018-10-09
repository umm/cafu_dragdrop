using System;
using System.Collections.Generic;
using CAFU.Core.Presentation.Presenter;
using CAFU.DragDrop.Domain.Model;
using CAFU.DragDrop.Domain.UseCase;

namespace CAFU.DragDrop.Presentation.Presenter
{
    public interface IDragDropPresenter : IPresenter
    {
        IDragDropUseCase DragDropUseCase { get; }
    }

    public static class IDragDropPresenterExtension
    {
        public static IObservable<DragDropModel> MostOverlapDroppingAsObservable(
            this IDragDropPresenter presenter)
        {
            return presenter.DragDropUseCase.MostOverlapDroppingAsObservable;
        }

        public static IObservable<IList<DragDropModel>> GetFixedDragDropAsObservable(
            this IDragDropPresenter presenter)
        {
            return presenter.DragDropUseCase.FixedDragDropAsObservable;
        }

        public static IObservable<IDragModel> GetDraggingAsObservable(
            this IDragDropPresenter presenter, IDropModel drop)
        {
            return presenter.DragDropUseCase.GetDraggingAsObservable(drop);
        }

        public static IObservable<IDropModel> GetDroppedAsObservable(
            this IDragDropPresenter presenter, IDragModel drag)
        {
            return presenter.DragDropUseCase.GetDroppedAsObservable(drag);
        }

        public static void Drag(this IDragDropPresenter presenter, IDropModel drop, IDragModel drag)
        {
            presenter.DragDropUseCase.Drag(drop, drag);
        }

        public static void DragOut(this IDragDropPresenter presenter, IDropModel drop, IDragModel drag)
        {
            presenter.DragDropUseCase.DragOut(drop, drag);
        }

        public static void Drop(this IDragDropPresenter presenter, IDragModel drag)
        {
            presenter.DragDropUseCase.Drop(drag);
        }
    }
}