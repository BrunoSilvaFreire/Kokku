using UnityEngine;

namespace Game.Behaviours
{
    /// <summary>
    /// Global Stage marks the canvas where components can be placed freely. This is currently used for the drag and drop
    /// indication "Wisp".
    /// </summary>
    public class GlobalStage : Singleton<GlobalStage>
    {
        [SerializeField]
        private Canvas _canvas;

        public Canvas Canvas => _canvas;
    }
}