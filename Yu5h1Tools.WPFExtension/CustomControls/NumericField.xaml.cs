using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Cursors = System.Windows.Input.Cursors;
namespace Yu5h1Tools.WPFExtension
{
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericField : UserControl
    {
        [Category("Common Properties")]
        public string header
        {
            get { return label.Content.ToString(); }
            set
            {
                label.Content = value;
                OnHeaderChanged();
            }
        }
        private bool _UpDownButton = true;
        [Category("Common Properties")]
        public bool UpDownButton{
            get => _UpDownButton;
            set {
                if (_UpDownButton != value) {
                    _UpDownButton = value;
                    updown_btn.Visibility = _UpDownButton ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        double previouseValue;
        [Category("Common Properties")]
        public double value {
            get
            {
                double.TryParse(value_textbox.Text, out double result);
                return result;
            }
            set
            {                
                value_textbox.Text = value.ToString(ValueFormat);                
            }
        }
        public delegate void NumericFieldValueChanged(NumericField sender,double delta);
        public NumericFieldValueChanged NumericFieldValueChangedEvent;

        public event NumericFieldValueChanged ValueChanged
        {
            add { NumericFieldValueChangedEvent += value; }
            remove { NumericFieldValueChangedEvent -= value; }
        }


        [Category("Common Properties")]
        public string ValueFormat { get; set; } = "0.0";
        
        [Category("Common Properties")]
        public double LabelWidth
        {
            get { return label.MinWidth; }
            set
            {
                label.MinWidth = value;
            }
        }
        [Category("Common Properties")]
        public double FieldWidth
        {
            get { return value_textbox.Width; }
            set
            {
                value_textbox.Width = value;
                double udbwidth = 16;
                if (FieldWidth < 66) {
                    if (FieldWidth > 50) {
                        udbwidth = 16 + FieldWidth - 50;
                    }
                }else { udbwidth = 32; }
                updown_btn.Width = udbwidth;
            }
        }
        [Browsable(false)]
        public Point previouseDragPoint { get; set; }        
        public NumericField()
        {
            InitializeComponent();
            DataObject.AddPastingHandler(value_textbox, value_textbox_pasting);            
            label.Cursor = new Cursor(new System.IO.MemoryStream(Yu5h1Tools.WPFExtension.Resources.numericArror));

            MouseEventUtility.AddMouseDragEvent(this,
                () => { previouseValue = value; return true; }, (v) =>
            {
                ModifyValue(v.X,false);
                //if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                //    value = previouseValue+ (v.X * 0.1);
                //else
                //    value = previouseValue + v.X;
                //value_textbox.CaretIndex = value_textbox.Text.Length;
            }, () =>
            {
                previouseValue = value;
            }, (IsCancel) =>
            {
                if (IsCancel) value = previouseValue;
                else {
                    value = 0;
                    previouseValue = value;
                }
                
            });
        }
        protected override void OnQueryCursor(QueryCursorEventArgs e)
        {
            label.Cursor = e.Cursor;
            base.OnQueryCursor(e);
        }
        bool IsDigits(string txt)
        {
            return double.TryParse(txt,out double val);
        }
        private void OnHeaderChanged()
        {
        }
        private void value_textbox_pasting(object sender, DataObjectPastingEventArgs e)
        {
            bool cancelPast = false;
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                if (!IsDigits((string)e.DataObject.GetData(typeof(String)))) cancelPast = true;
            } else cancelPast = true;
            if (cancelPast) {
                e.CancelCommand();
                System.Media.SystemSounds.Beep.Play();
            } 
            
        }
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            
            base.OnRenderSizeChanged(sizeInfo);
        }
        void ComputMathSyntax() {
            try
            {
                if (value_textbox.Text == String.Empty)
                {
                    value_textbox.Text = "0";
                } else {
                    if (value_textbox.Text[0] == '.') value_textbox.Text = "0" + value_textbox.Text;
                    value_textbox.Text = new System.Data.DataTable().Compute(value_textbox.Text, null).ToString();                    
                    previouseValue = double.Parse(value_textbox.Text);
                    value_textbox.CaretIndex = value_textbox.Text.Length;
                }
            }
            catch (Exception)
            {
                if (previouseValue == 0) value_textbox.Text = "0";
                else value_textbox.Text = previouseValue.ToString(ValueFormat);
                
                value_textbox.CaretIndex = value_textbox.Text.Length;
            }
        }
        private void value_textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            ComputMathSyntax();
        }

        private void value_textbox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ComputMathSyntax();
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            ModifyValue(e.Delta > 0 ? 1 : -1);
            base.OnMouseWheel(e);
        }
        private void value_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                value_textbox.Text = previouseValue.ToString(ValueFormat);
                Keyboard.ClearFocus();
            }else if (e.Key == Key.Enter) {
                ComputMathSyntax();
            }
        }
        void ModifyValue( double amount,bool confirm = true) {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                value = previouseValue + (amount * 10); 
            else if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                value = previouseValue + (amount * 0.1);
            else
                value = previouseValue + amount;
            value_textbox.CaretIndex = value_textbox.Text.Length;
            if (confirm) previouseValue = value;
        }
        private void scrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (updown_btn.Value != 0)
            {
                double delta = -updown_btn.Value;
                updown_btn.Value = 0;
                ModifyValue(delta);
            }
            
        }

        private void value_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void value_textbox_TextInput(object sender, TextCompositionEventArgs e)
        {
            
        }
        
        private void value_textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char chr = e.Text[0];
            //bool SymbolOverlapping = char.IsSymbol(value_textbox.Text[value_textbox.Text.Length - 1]) && char.IsSymbol(chr);
            if ((!char.IsControl(chr) && char.IsLetter(chr)))
            {
                e.Handled = true;
                System.Media.SystemSounds.Beep.Play();
            }            
        }
    }
}
