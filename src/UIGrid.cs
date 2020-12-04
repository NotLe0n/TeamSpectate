using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TeamSpectate.src
{
    public class UIGrid : UIElement
    {
        public delegate bool ElementSearchMethod(UIElement element);

        private class UIInnerList : UIElement
        {
            public override bool ContainsPoint(Vector2 point)
            {
                return true;
            }

            protected override void DrawChildren(SpriteBatch spriteBatch)
            {
                Vector2 position = Parent.GetDimensions().Position();
                Vector2 dimensions = new Vector2(Parent.GetDimensions().Width, Parent.GetDimensions().Height);
                foreach (UIElement current in Elements)
                {
                    Vector2 position2 = current.GetDimensions().Position();
                    Vector2 dimensions2 = new Vector2(current.GetDimensions().Width, current.GetDimensions().Height);
                    if (Collision.CheckAABBvAABBCollision(position, dimensions, position2, dimensions2))
                    {
                        current.Draw(spriteBatch);
                    }
                }
            }
        }

        public List<UIElement> _items = new List<UIElement>();
        protected UIScrollbar _scrollbar;
        internal UIElement _innerList = new UIInnerList();
        private float _innerListHeight;
        public float ListPadding = 5f;

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        private int cols = 1;

        public UIGrid(int columns = 1)
        {
            cols = columns;
            _innerList.OverflowHidden = false;
            _innerList.Width.Set(0f, 1f);
            _innerList.Height.Set(0f, 1f);
            OverflowHidden = true;
            Append(_innerList);
        }

        public float GetTotalHeight()
        {
            return _innerListHeight;
        }

        public void Goto(ElementSearchMethod searchMethod, bool center = false)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (searchMethod(_items[i]))
                {
                    _scrollbar.ViewPosition = _items[i].Top.Pixels;
                    if (center)
                    {
                        _scrollbar.ViewPosition = _items[i].Top.Pixels - GetInnerDimensions().Height / 2 + _items[i].GetOuterDimensions().Height / 2;
                    }
                    return;
                }
            }
        }

        public virtual void Add(UIElement item)
        {
            _items.Add(item);
            _innerList.Append(item);
            UpdateOrder();
            _innerList.Recalculate();
        }

        public virtual bool Remove(UIElement item)
        {
            _innerList.RemoveChild(item);
            UpdateOrder();
            return _items.Remove(item);
        }

        public virtual void Clear()
        {
            _innerList.RemoveAllChildren();
            _items.Clear();
        }

        public override void Recalculate()
        {
            base.Recalculate();
            UpdateScrollbar();
        }

        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            base.ScrollWheel(evt);
            if (_scrollbar != null)
            {
                _scrollbar.ViewPosition -= evt.ScrollWheelValue;
            }
        }

        public override void RecalculateChildren()
        {
            base.RecalculateChildren();
            float top = 0f;
            float left = 0f;
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].Top.Set(top, 0f);
                _items[i].Left.Set(left, 0f);
                _items[i].Recalculate();
                if (i % cols == cols - 1)
                {
                    top += _items[i].GetOuterDimensions().Height + ListPadding;
                    left = 0;
                }
                else
                {
                    left += _items[i].GetOuterDimensions().Width + ListPadding;
                }
                //num += this._items[i].GetOuterDimensions().Height + this.ListPadding;
            }
            if (_items.Count > 0)
            {
                top += ListPadding + _items[0].GetOuterDimensions().Height;
            }
            _innerListHeight = top;
        }

        private void UpdateScrollbar()
        {
            if (_scrollbar == null)
            {
                return;
            }
            _scrollbar.SetView(GetInnerDimensions().Height, _innerListHeight);
        }

        public void SetScrollbar(UIScrollbar scrollbar)
        {
            _scrollbar = scrollbar;
            UpdateScrollbar();
        }

        public void UpdateOrder()
        {
            _items.Sort(new Comparison<UIElement>(SortMethod));
            UpdateScrollbar();
        }

        public int SortMethod(UIElement item1, UIElement item2)
        {
            return item1.CompareTo(item2);
        }

        public override List<SnapPoint> GetSnapPoints()
        {
            List<SnapPoint> list = new List<SnapPoint>();
            SnapPoint item;
            if (GetSnapPoint(out item))
            {
                list.Add(item);
            }
            foreach (UIElement current in _items)
            {
                list.AddRange(current.GetSnapPoints());
            }
            return list;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (_scrollbar != null)
            {
                _innerList.Top.Set(-_scrollbar.GetValue(), 0f);
            }
            Recalculate();
        }
    }
}