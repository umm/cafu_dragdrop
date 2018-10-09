using CAFU.Core.Domain.Model;
using CAFU.DragDrop.Domain.Model;

namespace CAFU.DragDrop.Domain.Model
{
    public class DragDropModel : IModel
    {
        public IDragModel Drag { get; set; }
        public IDropModel Drop { get; set; }

        public TDragModel GetDrag<TDragModel>() where TDragModel : IDragModel
        {
            return (TDragModel) this.Drag;
        }

        public TDropModel GetDrop<TDropModel>() where TDropModel : IDropModel
        {
            return (TDropModel) this.Drop;
        }
    }
}