using UnityEngine;

namespace CAFU.DragDrop.Domain.Model
{
    public class DropModel : IDropModel
    {
        public int Id { get; set; }
        public Collider2D Collider2D { get; set; }
   }
}