using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GFrame
{
    [RequireComponent(typeof(Graphic))]
    public class TurnImage : BaseMeshEffect
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
                DrawSimple(output.uiVertex, count);
            }
            else
            {
                DrawSimple(output.uiVertex, count);
            }
            vh.Clear();
            vh.AddUIVertexTriangleStream(output.uiVertex);

            ObjectPool<MyUIVertex>.S.Recycle(output);
        }


        protected void DrawSimple(List<UIVertex> output, int count)
        {
            Rect rect = graphic.GetPixelAdjustedRect();
            switch (m_MirrorType)
            {
                case MirrorType.Horizontal:
                    MirrorVerts(rect, output, count, true);
                    break;
                case MirrorType.Vertical:
                    MirrorVerts(rect, output, count, false);
                    break;

            }
        }

        protected void MirrorVerts(Rect rect, List<UIVertex> verts, int count, bool isHorizontal = true)
        {

            List<UIVertex> nverts = new List<UIVertex>(verts);
            verts.Clear();
            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = nverts[i];

                Vector3 position = vertex.position;

                if (isHorizontal)
                {
                    position.x = rect.center.x - position.x;
                }
                else
                {
                    position.y = rect.center.y - position.y;
                }

                vertex.position = position;

                verts.Add(vertex);
            }
        }
    }


}





