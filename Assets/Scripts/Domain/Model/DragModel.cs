using UnityEngine;

namespace CAFU.DragDrop.Domain.Model
{
    public class DragModel : IDragModel
    {
        public int Id { get; set; }
        public Collider2D Collider2D { get; set; }
        public Sprite Sprite { get; set; }
    }
}