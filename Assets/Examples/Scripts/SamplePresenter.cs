using CAFU.Core.Presentation.Presenter;
using CAFU.DragDrop.Domain.UseCase;
using CAFU.DragDrop.Presentation.Presenter;

namespace Example.CAFU.DragDrop.Presentation.Presenter
{
    public class SamplePresenter : IDragDropPresenter
    {
        public class Factory : DefaultPresenterFactory<SamplePresenter>
        {
            protected override void Initialize(SamplePresenter instance)
            {
                base.Initialize(instance);
                instance.DragDropUseCase = new ExclusiveDragDropUseCase();
            }
        }

        public IDragDropUseCase DragDropUseCase { get; private set; }
    }
}