using Game.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Systems
{
    public partial class UpdateDragWispPositionSystem : SystemBase
    {
        private Camera _camera;

        protected override void OnUpdate()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
                if (_camera == null)
                {
                    Debug.LogError("Unable to find main camera for updating wisp position.");
                    return;
                }
            }
            var worldSpacePosition = Input.mousePosition;
            worldSpacePosition.z = 1;
            Entities.ForEach((DragSource dragSource) =>
            {
                dragSource.thumbnail.transform.position = worldSpacePosition;
            }).WithoutBurst().Run();
        }
    }
}