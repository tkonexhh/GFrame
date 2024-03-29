﻿//  Desc:        Framework For Game Develop with Unity3d
//  Copyright:   Copyright (C) 2017 SnowCold. All rights reserved.
//  WebSite:     https://github.com/SnowCold/Qarth
//  Blog:        http://blog.csdn.net/snowcoldgame
//  Author:      SnowCold
//  E-mail:      snowcold.ouyang@gmail.com
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GFrame
{
    public class USimpleDiffSizeListView : IUListView
    {
        public Alignment alignment;

        public override void Init()
        {
            base.Init();
        }

        public override int GetMaxShowItemNum()
        {
            int max = 0;
            int index = GetStartIndex();
            float sum = 0;
            switch (layout)
            {
                case Layout.Horizontal:
                    while (index < lstCount && sum < scrollRectSize.x)
                    {
                        sum += (itemView.GetItemSize(index).x + spacing.x);
                        index++;
                        max++;
                    }
                    break;
                case Layout.Vertical:
                    while (index < lstCount && sum < scrollRectSize.y)
                    {
                        sum += (itemView.GetItemSize(index).y + spacing.y);
                        index++;
                        max++;
                    }
                    break;
            }
            return max + 1;
        }

        public override int GetStartIndex()
        {
            Vector2 anchorPosition = content.anchoredPosition;
            anchorPosition.x *= -1;
            int index = -1;
            float sum = 0;

            switch (layout)
            {
                case Layout.Horizontal:
                    sum = -spacing.x;
                    for (int i = 0; i < lstCount; ++i)
                    {
                        Vector2 itemSize = itemView.GetItemSize(i);
                        sum += (itemSize.x + spacing.x);
                        if (sum <= anchorPosition.x)
                        {
                            index = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                case Layout.Vertical:
                    sum = spacing.y;
                    for (int i = 0; i < lstCount; ++i)
                    {
                        Vector2 itemSize = itemView.GetItemSize(i);
                        sum += (itemSize.y + spacing.y);
                        if (sum <= anchorPosition.y)
                        {
                            index = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
            }
            ++index;
            if (index < 0) index = 0;
            if (index >= lstCount) index = 0;

            return index;
        }

        public override Vector2 GetItemAnchorPos(int index)
        {
            Vector2 basePos = Vector2.zero;
            Vector2 offset = Vector2.zero;
            RectTransform contentRectTransform = content.transform as RectTransform;
            Vector2 contentRectSize = contentRectTransform.rect.size;

            if (layout == Layout.Horizontal)
            {
                basePos.x = -contentRectSize.x / 2 - itemView.GetItemSize(index).x / 2;
                for (int i = 0; i <= index; ++i)
                {
                    offset.x += (itemView.GetItemSize(i).x + spacing.x);
                }

                switch (alignment)
                {
                    case Alignment.Top:
                        offset.y = -(contentRectSize.y - itemView.GetItemSize(index).y) / 2;
                        break;
                    case Alignment.Bottom:
                        offset.y = (contentRectSize.y - itemView.GetItemSize(index).y) / 2;
                        break;
                }
            }
            else
            {
                basePos.y = contentRectSize.y / 2 + itemView.GetItemSize(index).y / 2;
                for (int i = 0; i <= index; ++i)
                {
                    offset.y -= (itemView.GetItemSize(i).y + spacing.y);
                }
                switch (alignment)
                {
                    case Alignment.Left:
                        offset.x = -(contentRectSize.x - itemView.GetItemSize(index).x) / 2;
                        break;
                    case Alignment.Right:
                        offset.x = (contentRectSize.x - itemView.GetItemSize(index).x) / 2;
                        break;
                }
            }
            return basePos + offset;
        }

        public override Vector2 GetContentSize()
        {
            Vector2 size = scrollRectSize;
            switch (layout)
            {
                case Layout.Horizontal:
                    size.x = 0;
                    break;
                case Layout.Vertical:
                    size.y = 0;
                    break;
            }

            for (int i = 0; i < lstCount; ++i)
            {
                if (layout == Layout.Horizontal)
                {
                    size.x += itemView.GetItemSize(i).x + spacing.x;
                }
                else
                {
                    size.y += itemView.GetItemSize(i).y + spacing.y;
                }
            }
            return size;
        }

        public override GameObject GetItemGameObject(Transform content, int index)
        {
            if (index < lstItems.Count)
            {
                GameObject go = lstItems[index];
                if (false == go.activeSelf)
                {
                    go.SetActive(true);
                }
                return lstItems[index];
            }
            else
            {
                GameObject go = GameObject.Instantiate(itemPrefab, content, false) as GameObject;
                lstItems.Add(go);
                return go;
            }
        }

        public override void HideNonuseableItems()
        {
            for (int i = lstCount; lstItems != null && i < lstItems.Count; ++i)
            {
                if (lstItems[i].activeSelf)
                {
                    lstItems[i].SetActive(false);
                }
            }
        }

        public override Vector2 GetItemSize(int index)
        {
            return itemView.GetItemSize(index);
        }
    }
}
