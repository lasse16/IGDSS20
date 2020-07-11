using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName ="data",menuName ="ScriptableObjects/MoverSettings")]
    internal class MoverSettings : ScriptableObject
    {
        public float WeightDurationRatio;
    }
}