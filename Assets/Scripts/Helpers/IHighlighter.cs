using UnityEngine;

namespace IGDSS20.Helpers
{
    public interface IHighlighter
    {
        void HighlightGameObject(GameObject gameObject);
        void RemoveHighlightFromObject(GameObject gameObject);
    }
}
