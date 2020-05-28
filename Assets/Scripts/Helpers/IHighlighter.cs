using UnityEngine;

namespace Assets.Scripts
{
    public interface IHighlighter
    {
        void HighlightGameObject(GameObject gameObject);
        void RemoveHighlightFromObject(GameObject gameObject);
    }
}
