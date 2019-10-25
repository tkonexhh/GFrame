using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;

namespace GFrame
{


    [RequireComponent(typeof(Graphic))]
    [ExecuteInEditMode]
    public class MirrorImage : BaseMeshEffect
    {
        public enum MirrorType
        {
            /// <summary>
            /// 水平
            /// </summary>
            Horizontal,

            /// <summary>
            /// 垂直
            /// </summary>
            Vertical,

            /// <summary>
            /// 四分之一
            /// 相当于水平，然后再垂直
            /// </summary>
            Quarter,
        }


        [SerializeField] MirrorType m_MirrorType;
        public MirrorType mirrorType
        {
            get { return m_MirrorType; }
            set
            {
                if (m_MirrorType != value)
                {
                    m_MirrorType = value;
                    if (graphic != null)
                    {
                        graphic.SetVerticesDirty();
                    }
                }
            }
        }

        private RectTransform m_RectTransform;

        public RectTransform rectTransform
        {

            get
            {
                if (m_RectTransform == null)
                {
                    m_RectTransform = GetComponent<RectTransform>();
                }
                return m_RectTransform;//?? (m_RectTransform = GetComponent<RectTransform>());
            }
        }


        /// <summary>
        /// 设置原始尺寸
        /// </summary>
        public void SetNativeSize()
        {
            if (graphic != null && graphic is Image)
            {
                Sprite overrideSprite = (graphic as Image).overrideSprite;

                if (overrideSprite != null)
                {
                    float w = overrideSprite.rect.width / (graphic as Image).pixelsPerUnit;
                    float h = overrideSprite.rect.height / (graphic as Image).pixelsPerUnit;
                    rectTransform.anchorMax = rectTransform.anchorMin;

                    switch (m_MirrorType)
                    {
                        case MirrorType.Horizontal:
                            rectTransform.sizeDelta = new Vector2(w * 2, h);
                            break;
                        case MirrorType.Vertical:
                            rectTransform.sizeDelta = new Vector2(w, h * 2);
                            break;
                        case MirrorType.Quarter:
                            rectTransform.sizeDelta = new Vector2(w * 2, h * 2);
                            break;
                    }

                    graphic.SetVerticesDirty();
                }
            }
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }

            var output = ObjectPool<MyUIVertex>.S.Allocate();
            vh.GetUIVertexStream(output.uiVertex);

            int count = output.uiVertex.Count;

            if (graphic is Image)
            {
                Image.Type type = (graphic as Image).type;

                switch (type)
                {
                    case Image.Type.Simple:
                        DrawSimple(output.uiVertex, count);
                        break;
                    case Image.Type.Sliced:
                        DrawSliced(output.uiVertex, count);
                        break;
                    case Image.Type.Tiled:
                        DrawTiled(output.uiVertex, count);
                        break;
                    case Image.Type.Filled:

                        break;
                }
            }
            else
            {
                DrawSimple(output.uiVertex, count);
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(output.uiVertex);

            ObjectPool<MyUIVertex>.S.Recycle(output);
        }

        /// <summary>
        /// 绘制简单版
        /// </summary>
        /// <param name="output"></param>
        /// <param name="count"></param>
        protected void DrawSimple(List<UIVertex> output, int count)
        {
            Rect rect = graphic.GetPixelAdjustedRect();

            SimpleScale(rect, output, count);

            switch (m_MirrorType)
            {
                case MirrorType.Horizontal:
                    ExtendCapacity(output, count);
                    MirrorVerts(rect, output, count, true);
                    break;
                case MirrorType.Vertical:
                    ExtendCapacity(output, count);
                    MirrorVerts(rect, output, count, false);
                    break;
                case MirrorType.Quarter:
                    ExtendCapacity(output, count * 3);
                    MirrorVerts(rect, output, count, true);
                    MirrorVerts(rect, output, count * 2, false);
                    break;
            }
        }

        /// <summary>
        /// 绘制Sliced版
        /// </summary>
        /// <param name="output"></param>
        /// <param name="count"></param>
        protected void DrawSliced(List<UIVertex> output, int count)
        {
            //sprite的border为零的情况下，遵从Image的设定，按照Simple模式绘制。
            if (!(graphic as Image).hasBorder)
            {
                DrawSimple(output, count);

                return;
            }

            Rect rect = graphic.GetPixelAdjustedRect();

            SlicedScale(rect, output, count);

            SliceExcludeVerts(output, count);

            switch (m_MirrorType)
            {
                case MirrorType.Horizontal:
                    ExtendCapacity(output, count);
                    MirrorVerts(rect, output, count, true);
                    break;
                case MirrorType.Vertical:
                    ExtendCapacity(output, count);
                    MirrorVerts(rect, output, count, false);
                    break;
                case MirrorType.Quarter:
                    ExtendCapacity(output, count * 3);
                    MirrorVerts(rect, output, count, true);
                    MirrorVerts(rect, output, count * 2, false);
                    break;
            }
        }

        /// <summary>
        /// 绘制Tiled版
        /// </summary>
        /// <param name="output"></param>
        /// <param name="count"></param>
        protected void DrawTiled(List<UIVertex> verts, int count)
        {
            Sprite overrideSprite = (graphic as Image).overrideSprite;

            if (overrideSprite == null)
            {
                return;
            }

            Rect rect = graphic.GetPixelAdjustedRect();

            //此处使用inner是因为Image绘制Tiled时，会把透明区域也绘制了。

            Vector4 inner = DataUtility.GetInnerUV(overrideSprite);

            float w = overrideSprite.rect.width / (graphic as Image).pixelsPerUnit;
            float h = overrideSprite.rect.height / (graphic as Image).pixelsPerUnit;

            int len = count / 3;

            for (int i = 0; i < len; i++)
            {
                UIVertex v1 = verts[i * 3];
                UIVertex v2 = verts[i * 3 + 1];
                UIVertex v3 = verts[i * 3 + 2];

                float centerX = GetCenter(v1.position.x, v2.position.x, v3.position.x);

                float centerY = GetCenter(v1.position.y, v2.position.y, v3.position.y);

                if (m_MirrorType == MirrorType.Horizontal || m_MirrorType == MirrorType.Quarter)
                {
                    //判断三个点的水平位置是否在偶数矩形内，如果是，则把UV坐标水平翻转
                    if (Mathf.FloorToInt((centerX - rect.xMin) / w) % 2 == 1)
                    {
                        v1.uv0 = GetOverturnUV(v1.uv0, inner.x, inner.z, true);
                        v2.uv0 = GetOverturnUV(v2.uv0, inner.x, inner.z, true);
                        v3.uv0 = GetOverturnUV(v3.uv0, inner.x, inner.z, true);
                    }
                }

                if (m_MirrorType == MirrorType.Vertical || m_MirrorType == MirrorType.Quarter)
                {
                    //判断三个点的垂直位置是否在偶数矩形内，如果是，则把UV坐标垂直翻转
                    if (Mathf.FloorToInt((centerY - rect.yMin) / h) % 2 == 1)
                    {
                        v1.uv0 = GetOverturnUV(v1.uv0, inner.y, inner.w, false);
                        v2.uv0 = GetOverturnUV(v2.uv0, inner.y, inner.w, false);
                        v3.uv0 = GetOverturnUV(v3.uv0, inner.y, inner.w, false);
                    }
                }

                verts[i * 3] = v1;
                verts[i * 3 + 1] = v2;
                verts[i * 3 + 2] = v3;
            }
        }

        /// <summary>
        /// 返回三个点的中心点
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        protected float GetCenter(float p1, float p2, float p3)
        {
            float max = Mathf.Max(Mathf.Max(p1, p2), p3);

            float min = Mathf.Min(Mathf.Min(p1, p2), p3);

            return (max + min) / 2;
        }

        /// <summary>
        /// 返回翻转UV坐标
        /// </summary>
        /// <param name="uv"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="isHorizontal"></param>
        /// <returns></returns>
        protected Vector2 GetOverturnUV(Vector2 uv, float start, float end, bool isHorizontal = true)
        {
            if (isHorizontal)
            {
                uv.x = end - uv.x + start;
            }
            else
            {
                uv.y = end - uv.y + start;
            }

            return uv;
        }


        protected void ExtendCapacity(List<UIVertex> verts, int addCount)
        {
            var neededCapacity = verts.Count + addCount;
            if (verts.Capacity < neededCapacity)
            {
                verts.Capacity = neededCapacity;
            }
        }

        protected void SimpleScale(Rect rect, List<UIVertex> verts, int count)
        {
            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = verts[i];

                Vector3 position = vertex.position;

                if (m_MirrorType == MirrorType.Horizontal || m_MirrorType == MirrorType.Quarter)
                {
                    position.x = (position.x + rect.x) * 0.5f;
                }

                if (m_MirrorType == MirrorType.Vertical || m_MirrorType == MirrorType.Quarter)
                {
                    position.y = (position.y + rect.y) * 0.5f;
                }

                vertex.position = position;

                verts[i] = vertex;
            }
        }

        protected void MirrorVerts(Rect rect, List<UIVertex> verts, int count, bool isHorizontal = true)
        {
            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = verts[i];

                Vector3 position = vertex.position;

                if (isHorizontal)
                {
                    position.x = rect.center.x * 2 - position.x;
                }
                else
                {
                    position.y = rect.center.y * 2 - position.y;
                }

                vertex.position = position;

                verts.Add(vertex);
            }
        }

        /// <summary>
        /// 返回矫正过的范围
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        protected Vector4 GetAdjustedBorders(Rect rect)
        {
            Sprite overrideSprite = (graphic as Image).overrideSprite;

            Vector4 border = overrideSprite.border;

            border = border / (graphic as Image).pixelsPerUnit;

            for (int axis = 0; axis <= 1; axis++)
            {
                float combinedBorders = border[axis] + border[axis + 2];
                if (rect.size[axis] < combinedBorders && combinedBorders != 0)
                {
                    float borderScaleRatio = rect.size[axis] / combinedBorders;
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }
            }

            return border;
        }

        /// <summary>
        /// Sliced缩放位移顶点（减半）
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="verts"></param>
        /// <param name="count"></param>
        protected void SlicedScale(Rect rect, List<UIVertex> verts, int count)
        {

            Vector4 border = GetAdjustedBorders(rect);

            float halfWidth = rect.width * 0.5f;

            float halfHeight = rect.height * 0.5f;

            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = verts[i];

                Vector3 position = vertex.position;

                if (m_MirrorType == MirrorType.Horizontal || m_MirrorType == MirrorType.Quarter)
                {
                    if (halfWidth < border.x && position.x >= rect.center.x)
                    {
                        position.x = rect.center.x;
                    }
                    else if (position.x >= border.x)
                    {
                        position.x = (position.x + rect.x) * 0.5f;
                    }
                }

                if (m_MirrorType == MirrorType.Vertical || m_MirrorType == MirrorType.Quarter)
                {
                    if (halfHeight < border.y && position.y >= rect.center.y)
                    {
                        position.y = rect.center.y;
                    }
                    else if (position.y >= border.y)
                    {
                        position.y = (position.y + rect.y) * 0.5f;
                    }
                }

                vertex.position = position;

                verts[i] = vertex;
            }
        }

        /// <summary>
        /// 清理掉不能成三角面的顶点
        /// </summary>
        /// <param name="verts"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected int SliceExcludeVerts(List<UIVertex> verts, int count)
        {
            int realCount = count;

            int i = 0;

            while (i < realCount)
            {
                UIVertex v1 = verts[i];
                UIVertex v2 = verts[i + 1];
                UIVertex v3 = verts[i + 2];

                if (v1.position == v2.position || v2.position == v3.position || v3.position == v1.position)
                {
                    verts[i] = verts[realCount - 3];
                    verts[i + 1] = verts[realCount - 2];
                    verts[i + 2] = verts[realCount - 1];

                    realCount -= 3;
                    continue;
                }

                i += 3;
            }

            if (realCount < count)
            {
                verts.RemoveRange(realCount, count - realCount);
            }

            return realCount;
        }
    }

    public class MyUIVertex : IPoolAble, IPoolType
    {
        List<UIVertex> m_UIVertex = new List<UIVertex>();
        public List<UIVertex> uiVertex
        {
            get { return m_UIVertex; }
        }

        public void OnCacheReset()
        {

        }

        public void Recycle2Cache()
        {

        }

    }


}




