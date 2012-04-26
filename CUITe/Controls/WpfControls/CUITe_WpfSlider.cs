﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;

namespace CUITe.Controls.WpfControls
{
    /// <summary>
    /// Wrapper class for WpfSlider
    /// </summary>
    public class CUITe_WpfSlider : CUITe_WpfControl<WpfSlider>
    {
        public CUITe_WpfSlider() : base() { }
        public CUITe_WpfSlider(string sSearchParameters) : base(sSearchParameters) { }

        public double LargeChange
        {
            get { return this.UnWrap().LargeChange; }
        }

        public double MaximumPosition
        {
            get { return this.UnWrap().MaximumPosition; }
        }

        public double MinimumPosition
        {
            get { return this.UnWrap().MinimumPosition; }
        }

        public double Position
        {
            get { return this.UnWrap().Position; }
            set { this.UnWrap().Position = value; }
        }

        public string PositionAsString
        {
            get { return this.UnWrap().PositionAsString; }
            set { this.UnWrap().PositionAsString = value; }
        }

        public double SmallChange
        {
            get { return this.UnWrap().SmallChange; }
        }
    }
}