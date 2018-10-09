using CAFU.Core.Domain.Model;
using UnityEngine;

namespace CAFU.DragDrop.Domain.Model
{
    public interface IDragModel : IModel
    {
        int Id { get; }
        Collider2D Collider2D { get; }
    }

    public static class IDragModelExtension
    {
        public static int GetInstanceID(this IDragModel model)
        {
            return model?.Collider2D?.GetInstanceID() ?? 0;
        }
    }
}