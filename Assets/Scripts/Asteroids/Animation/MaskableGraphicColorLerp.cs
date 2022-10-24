using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Animation
{
    public class MaskableGraphicColorLerp : IAnimated
    {
        [SerializeField] private MaskableGraphic _graphicObject;
        [SerializeField] private Color _colorLower;
        [SerializeField] private Color _colorUpper;

        public void Animate(float clippedValue)
        {
            _graphicObject.color = Color.Lerp(_colorLower, _colorUpper, clippedValue);
        }
    }
}