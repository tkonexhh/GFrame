using UnityEngine;
using UnityEngine.UI;

namespace GFrame
{
    public abstract class IUListItemView : MonoBehaviour
    {
        RectTransform rectTransform;

        public virtual Vector2 GetItemSize(int index)
        {
            if (null == rectTransform)
            {
                rectTransform = transform as RectTransform;
            }
            return rectTransform.rect.size;
        }
    }
}
