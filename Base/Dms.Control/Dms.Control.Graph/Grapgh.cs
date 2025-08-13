using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace Dms.Control.Graph
{    
    public partial class Grapgh : UserControl
    {
        #region Field
        private List<LineItem> m_LineItems = new List<LineItem>();
        #endregion


        #region Properties
        [Category("DMS : Setting")]
        public string Title
        {
            get { return graphControl1.GraphPane.Title.Text; }
            set { graphControl1.GraphPane.Title.Text = value; }
        }
        [Category("DMS : Setting")]
        public string XAxisTitle
        {
            get { return graphControl1.GraphPane.XAxis.Title.Text; }
            set { graphControl1.GraphPane.XAxis.Title.Text = value; }
        }
        [Category("DMS : Setting")]
        public AxisType XAxisType
        {
            get { return graphControl1.GraphPane.XAxis.Type; }
            set { graphControl1.GraphPane.XAxis.Type = value; }
        }

        [Category("DMS : Setting")]
        public bool XAxisMajorGridVisible
        {
            get { return graphControl1.GraphPane.XAxis.MajorGrid.IsVisible; }
            set { graphControl1.GraphPane.XAxis.MajorGrid.IsVisible = value; }
        }
        [Category("DMS : Setting")]
        public string YAxisTitle
        {
            get { return graphControl1.GraphPane.YAxis.Title.Text; }
            set { graphControl1.GraphPane.YAxis.Title.Text = value; }
        }
        [Category("DMS : Setting")]
        public AxisType YAxisType
        {
            get { return graphControl1.GraphPane.YAxis.Type; }
            set { graphControl1.GraphPane.YAxis.Type = value; }
        }
        [Category("DMS : Setting")]
        public bool YAxisMajorGridVisible
        {
            get { return graphControl1.GraphPane.YAxis.MajorGrid.IsVisible; }
            set { graphControl1.GraphPane.YAxis.MajorGrid.IsVisible = value; }
        }

        [Browsable(false)]
        public GraphPane Pane
        {
            get { return graphControl1.GraphPane; }
        }
        
        #endregion

        public Grapgh()
        {
            InitializeComponent();            
        }

        public void AddCurve(string name, Color color, SymbolType symbolType)
        {
            RollingPointPairList list = new RollingPointPairList(1200);

            // Initially, a curve is added with no data points (list is empty)
            // Color is blue, and there will be no symbols
            LineItem curve = graphControl1.GraphPane.AddCurve(name, list, color, symbolType);
            
            m_LineItems.Add(curve);
        }

        public void SetXAxisScale(double min, double max, double minorStep, double majorStep)
        {
            graphControl1.GraphPane.XAxis.Scale.Min = min;
            graphControl1.GraphPane.XAxis.Scale.Max = max;
            graphControl1.GraphPane.XAxis.Scale.MinorStep = minorStep;
            graphControl1.GraphPane.XAxis.Scale.MajorStep = majorStep;
        }

        public void SetYAxisScale(double min, double max, double minorStep, double majorStep)
        {
            graphControl1.GraphPane.YAxis.Scale.Min = min;
            graphControl1.GraphPane.YAxis.Scale.Max = max;
            graphControl1.GraphPane.YAxis.Scale.MinorStep = minorStep;
            graphControl1.GraphPane.YAxis.Scale.MajorStep = majorStep;
        }

        public void Display(string name, XDate x, double y)
        {
            if (graphControl1.GraphPane.CurveList.Count <= 0) return;

            foreach (LineItem curve in graphControl1.GraphPane.CurveList)
            {
                if (name == curve.Label.Text)
                {
                    IPointListEdit list = curve.Points as IPointListEdit;

                    if (list == null) return;

                    list.Add(x, y);
                }
            }
        }

        public void Display(string name, double x, double y)
        {
            if (graphControl1.GraphPane.CurveList.Count <= 0) return;
                       
            foreach (LineItem curve in graphControl1.GraphPane.CurveList)
            {
                if (name == curve.Label.Text)
                {
                    IPointListEdit list = curve.Points as IPointListEdit;

                    if (list == null) return;

                    list.Add(x, y);
                }
            }                     
        }
    }
}
