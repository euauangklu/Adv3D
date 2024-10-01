using UnityEngine;
using UnityEngine.Events;

namespace KongC
{
    public static class UtilityPreview
    {
        public static float AspectRatio2DToResolution(Vector2 aspect, Vector2 resolution, bool fixedWidth)
        {
            if (fixedWidth)
            {
                return (resolution.x / aspect.x) * aspect.y;
            }
            else
            {
                return (resolution.y / aspect.y) * aspect.x;
            }
        }
        
        public static float AspectRatioToResolution(float aspect, Vector2 resolution, bool fixedWidth)
        {
            if (fixedWidth)
            {
                return resolution.x / aspect;
            }
            else
            {
                return resolution.y * aspect;
            }
        }

        public static Vector2 FitInParent(Vector2 parent, Vector2 target)
        {
            float aspectRatioTarget = target.x / target.y;
            float aspectRatioParent = parent.x / parent.y;
            
            if (aspectRatioParent > aspectRatioTarget)
            {
                return new Vector2(AspectRatioToResolution(aspectRatioTarget, parent, false), parent.y);
            }
            else
            {
                return new Vector2(parent.x, AspectRatioToResolution(aspectRatioTarget, parent, true));
            }
        }

        public static Vector2 FixCenter(Vector2 parent, Vector2 position, Vector2 target)
        {
            return new Vector2((parent.x - target.x) / 2 + position.x, (parent.y - target.y) / 2 + position.y);
        }

        public static Vector2 AutoSetPositionOverWindow(Vector2 scaleParent, Vector2 positionTarget, Vector2 scaleTarget)
        {
            Vector2 result = new Vector2();
            Vector2 halfScaleParent = scaleParent / 2;
            Vector2 halfScaleTarget = scaleTarget / 2;
            float overPosX = halfScaleParent.x - positionTarget.x - halfScaleTarget.x;
            float overPosY = halfScaleParent.y - positionTarget.y - halfScaleTarget.y;
            if (overPosX < 0)
            {
                result.x = 0;
            }
            else
            {
                result.x = FixCenter(scaleParent, positionTarget, scaleTarget).x;
            }

            if (overPosY < 0)
            {
                result.y = 0;
            }
            else
            {
                result.y = FixCenter(scaleParent, positionTarget, scaleTarget).y;
            }

            return result;
        }

        public static Texture2D ConvertTextureToTexture2D(Texture texture)
        {
            Texture2D result = Texture2D.CreateExternalTexture(
                texture.width,
                texture.height,
                TextureFormat.RGBA32,
                false,
                false,
                texture.GetNativeTexturePtr()
            );

            return result;
        }

        public static Vector2 AutoScrollCenter(Vector2 scaleWindow, Vector2 scaleContent, Vector2 scrollPosition)
        {
            //Debug.Log($"Current Scroll = {scrollPosition} || Screen = {drawTRect.size - position.size + new Vector2((drawSize.y < position.height) ? 0 : 13,(drawSize.x < position.width) ? 0 : 13) /* <-- Scroll Bar Size = 13px */ }");
            Vector2 result = scrollPosition - scaleContent - scaleWindow + new Vector2((scaleContent.y < scaleWindow.y) ? 0 : 13, (scaleContent.x < scaleWindow.x) ? 0 : 13) / 2;
            Debug.Log($"Result 1 = {result / 2} || Result 2 {scrollPosition}");
            return result / 2;
        }

        public static bool MouseEvent(EventType type, int button, Event @event)
        {
            return (@event.type == type && @event.button == button) || @event.isScrollWheel;
        }
        
        public static bool MouseEvent(EventType type, int button, Event @event, UnityAction<Event> unityEvent)
        {
            bool isPress = MouseEvent(type, button, @event);
            if(isPress && unityEvent is not null)
                unityEvent?.Invoke(@event);
            
            return isPress;
        }
    }
}