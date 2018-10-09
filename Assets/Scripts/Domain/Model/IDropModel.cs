using CAFU.Core.Domain.Model;
using UnityEngine;

namespace CAFU.DragDrop.Domain.Model
{
    public interface IDropModel : IModel
    {
        int Id { get; }
        Collider2D Collider2D { get; }
    }

    public static class IDropModelExtension
    {
        public static int GetInstanceID(this IDropModel model)
        {
            return model.Collider2D.GetInstanceID();
        }
    }
}