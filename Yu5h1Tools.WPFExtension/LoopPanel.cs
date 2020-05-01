using Math = System.Math;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Yu5h1Tools.WPFControls
{
    public class LoopPanel<DataType,T> where T : FrameworkElement
    {
        public DataType[] datas;
        public Orientation ScrollerOrientation = Orientation.Horizontal;
        public int DisplayCountlimit = 5;

        public Grid owner;
        public List<T> elements = new List<T>();
        public double Width { get { return owner.ActualWidth; } }
        public double Height { get { return owner.ActualHeight; } }
        public bool freezed;

        public Size elementSize
        {
            get
            {
                return new Size(
                    (ScrollerOrientation == Orientation.Horizontal ? Width / DisplayCountlimit : Width * 0.5),
                    (ScrollerOrientation == Orientation.Vertical ? Height / DisplayCountlimit : Height * 0.5));
            }
        }
        Point ScrollPoint;
        Point PreviouseScrollPoint;
        Point mousePoint { get { return Mouse.GetPosition(owner); } }
        double elementInterval { get { return ScrollerOrientation == Orientation.Horizontal ? elementSize.Width : elementSize.Height; } }
        double elementCenterPosition { get { return (ScrollerOrientation == Orientation.Horizontal ? elementSize.Height : elementSize.Width) * 0.5; } }
        double curStartPos;
        int FirstElementIndex;
        int ScrollerTotalLength;
        System.Action<LoopPanel<DataType, T>, int, T> updateElementMethod;

        Point dragStart;
        
        public LoopPanel(   Grid grid,DataType[] args,
                            System.Func<LoopPanel<DataType, T>, T> initalizeElement,
                            System.Action<LoopPanel<DataType, T>, int, T> updateElement,
                            Orientation scrollerOrientation = Orientation.Horizontal)
        {
            ScrollerOrientation = scrollerOrientation;
            owner = grid;
            owner.MouseLeftButtonDown += OnMouseLeftButtonDown;
            owner.MouseMove += OnMouseMove;
            owner.MouseLeftButtonUp += OnMouseLeftButtonUp;
            owner.MouseWheel += OnMouseWheel;
            owner.SizeChanged += OnSizeChanged;
            updateElementMethod = updateElement;
            datas = args;
            elements.Clear();
            ScrollerTotalLength = (int)(args.Length * elementInterval);
            int totalCount = args.Length > DisplayCountlimit ? DisplayCountlimit + 1 : args.Length;
            grid.ClipToBounds = true;
            for (int i = 0; i < datas.Length; i++)
            {
                var ele = initalizeElement(this);                
                ele.HorizontalAlignment = HorizontalAlignment.Left;
                ele.VerticalAlignment = VerticalAlignment.Top;
                grid.Children.Add(ele);
                elements.Add(ele);
                updateElement(this, i, ele);
            }
            SetSize();
        }
        void SetSize()
        {
            ScrollerTotalLength = (int)(datas.Length * elementInterval);
            for (int i = 0; i < elements.Count; i++)
            {                
                var ele = elements[i];
                ele.Width = elementSize.Width;
                ele.Height = elementSize.Height;
            }
            DockElements();
        }
        void DockElements()
        {
            double curPos = ScrollerOrientation == Orientation.Horizontal ? ScrollPoint.X : ScrollPoint.Y;
            bool Inverse = curPos <= 0;
            curPos = Math.Abs(curPos) % ScrollerTotalLength;
            curPos = Inverse ? curPos : ScrollerTotalLength - curPos;
            curStartPos = -curPos % elementInterval;
            FirstElementIndex = (int)(curPos / ScrollerTotalLength * datas.Length) % datas.Length;

            for (int i = 0; i < elements.Count; i++)
            {
                var ele = elements[i];
                var curInterval = curStartPos + (i * elementInterval);                
                ele.Margin = new Thickness(
                    ScrollerOrientation == Orientation.Horizontal ? curInterval : elementCenterPosition,
                    ScrollerOrientation == Orientation.Vertical ? curInterval : elementCenterPosition, 0, 0);
                updateElementMethod(this, (FirstElementIndex + i) % datas.Length, ele);
            }
        }
        public void OnMouseLeftButtonDown(object sender,MouseButtonEventArgs e) {
            if (owner.IsMouseDirectlyOver && !freezed )
            {
                dragStart = mousePoint;
                PreviouseScrollPoint = ScrollPoint;
                owner.CaptureMouse();
            }
        }
        public void OnMouseMove(object sender, MouseEventArgs e) {
            if (owner.IsMouseDirectlyOver && owner.IsMouseCaptureWithin && !freezed)
            {
                var horizontalDragDistance = mousePoint.X - dragStart.X;
                ScrollPoint.X = PreviouseScrollPoint.X + (mousePoint.X - dragStart.X);
                ScrollPoint.Y = PreviouseScrollPoint.Y + (mousePoint.Y - dragStart.Y);
                DockElements();
            }
        }
        public void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (!freezed) {
                owner.ReleaseMouseCapture();
            }
        }
        void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!freezed)
            {
                double f = e.Delta >= 0 ? 1 : -1;
                if (ScrollerOrientation == Orientation.Horizontal) ScrollPoint.X += (f * elementInterval);
                else ScrollPoint.Y += (f * elementInterval);
                DockElements();
            }
        }
        public void OnSizeChanged(object sender,System.EventArgs e) {
            SetSize();
        }
    }
}
