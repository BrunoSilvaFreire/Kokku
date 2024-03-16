using UnityEngine;

namespace Game.Behaviours
{
    public class GlobalStage : Singleton<GlobalStage>
    {
        [SerializeField]
        private Canvas _canvas;

        public Canvas Canvas => _canvas;
    }
}