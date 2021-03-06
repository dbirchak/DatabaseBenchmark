﻿using DatabaseBenchmark.Charts;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WeifenLuo.WinFormsUI.Docking;

namespace DatabaseBenchmark.Frames
{
    public partial class StepFrame : DockContent
    {
        private Dictionary<int, ColumnStyle> CurrentStyles;

        public StepFrame()
        {
            InitializeComponent();
        }

        public void InitializeCharts(IEnumerable<KeyValuePair<string, Color>> lineSeries)
        {
            // Bar charts.
            barChartSpeed.CreateSeries("Series1", "{0:N0}"); // "{#,#}"
            barChartSize.CreateSeries("Series1", "{0:0.#}");
            barChartTime.CreateSeries("Series1", "HH:mm:ss");
            barChartTime.AxisYValueType = ChartValueType.DateTime;

            //barChartCPU.CreateSeries("Series1", "{0:0.#}");
            barChartMemory.CreateSeries("Series1", "{0:0.#}");
            //barChartIO.CreateSeries("Series1", "{0:0.#}");

            barChartSpeed.Title = "Speed (rec/sec)";
            barChartSize.Title = "Size (MB)";
            barChartTime.Title = "Time (hh:mm:ss)";
            barChartCPU.Title = "CPU usage (%)";
            barChartMemory.Title = "Peak Memory (MB)";
            barChartIO.Title = "IO Data (MB/sec)";

            // Line charts.
            lineChartAverageSpeed.AxisXTitle = "Records";
            lineChartAverageSpeed.AxisYTitle = "Records/Sec";
            lineChartMomentSpeed.AxisXTitle = "Records";
            lineChartMomentSpeed.AxisYTitle = "Records/Sec";
            //lineChartAverageCPU.AxisXTitle = "Records";
            //lineChartAverageCPU.AxisYTitle = "Percent (%)";
            lineChartMomentMemory.AxisXTitle = "Records";
            lineChartMomentMemory.AxisYTitle = "MB";
            //lineChartAverageIO.AxisXTitle = "Records";
            //lineChartAverageIO.AxisYTitle = "MB/Sec";

            foreach (var item in lineSeries)
            {
                lineChartAverageSpeed.CreateSeries(item.Key, item.Value);
                lineChartMomentSpeed.CreateSeries(item.Key, item.Value);
                //lineChartAverageCPU.CreateSeries(item.Key, item.Value);
                lineChartMomentMemory.CreateSeries(item.Key, item.Value);
                //lineChartAverageIO.CreateSeries(item.Key, item.Value);
            }
        }

        public void ClearCharts()
        {
            lineChartAverageSpeed.Clear();
            lineChartMomentSpeed.Clear();
            //lineChartAverageCPU.Clear();
            lineChartMomentMemory.Clear();
            //lineChartAverageIO.Clear();

            barChartSpeed.Clear();
            barChartTime.Clear();
            barChartSize.Clear();
            //barChartCPU.Clear();
            barChartMemory.Clear();
            //barChartIO.Clear();
        }

        #region Add points to LineChart

        public void AddAverageSpeed(string series, IEnumerable<KeyValuePair<long, double>> data)
        {
            foreach (var item in data)
                lineChartAverageSpeed.AddPoint(series, item.Key, item.Value);
        }

        public void AddMomentSpeed(string series, IEnumerable<KeyValuePair<long, double>> data)
        {
            foreach (var item in data)
                lineChartMomentSpeed.AddPoint(series, item.Key, item.Value);
        }

        public void AddAverageCpuUsage(string series, IEnumerable<KeyValuePair<long, double>> data)
        {
            //foreach (var item in data)
            //    lineChartAverageCPU.AddPoint(series, item.Key, item.Value);
        }

        public void AddPeakMemoryUsage(string series, IEnumerable<KeyValuePair<long, double>> data)
        {
            foreach (var item in data)
                lineChartMomentMemory.AddPoint(series, item.Key, item.Value / (1024.0 * 1024.0));
        }

        public void AddAverageIO(string series, IEnumerable<KeyValuePair<long, double>> data)
        {
            //foreach (var item in data)
            //    lineChartAverageIO.AddPoint(series, item.Key, item.Value / (1024.0 * 1024.0));
        }

        #endregion

        #region Add points to BarChart

        public void AddAverageSpeedToBar(string label, object y, Color color)
        {
            barChartSpeed.AddPoint(label, y, color);
        }

        public void AddSizeToBar(string label, object y, Color color)
        {
            barChartSize.AddPoint(label, y, color);
        }

        public void AddTimeToBar(string label, object y, Color color)
        {
            barChartTime.AddPoint(label, y, color);
        }

        public void AddCpuUsageToBar(string label, object y, Color color)
        {
            barChartCPU.AddPoint(label, y, color);
        }

        public void AddMemoryUsageToBar(string label, object y, Color color)
        {
            barChartMemory.AddPoint(label, y, color);
        }

        public void AddIoUsageToBar(string label, object y, Color color)
        {
            barChartIO.AddPoint(label, y, color);
        }

        #endregion

        public List<BarChart> GetSelectedBarCharts()
        {
            List<BarChart> allBarCharts = new List<BarChart>();

            foreach (Control item in LayoutPanel.Controls)
            {
                if (LayoutPanel.ColumnStyles[LayoutPanel.GetColumn(item)].SizeType == SizeType.Percent)
                    allBarCharts.Add(item as BarChart);
            }

            return allBarCharts;
        }

        public List<BarChart> GetSummaryBarCharts()
        {
            List<BarChart> barCharts = new List<BarChart>();

            CurrentStyles = new Dictionary<int, ColumnStyle>();
            CurrentStyles[0] = LayoutPanel.ColumnStyles[0];
            CurrentStyles[2] = LayoutPanel.ColumnStyles[2];

            LayoutPanel.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 18);
            LayoutPanel.ColumnStyles[2] = new ColumnStyle(SizeType.Percent, 18);

            barChartSpeed.Invalidate();
            barChartSize.Invalidate();

            barCharts.Add(barChartSpeed);
            barCharts.Add(barChartSize);

            return barCharts;
        }

        public List<BarChart> GetAllBarCharts()
        {
            CurrentStyles = new Dictionary<int, ColumnStyle>();
            CurrentStyles[0] = LayoutPanel.ColumnStyles[0];
            CurrentStyles[1] = LayoutPanel.ColumnStyles[1];
            CurrentStyles[2] = LayoutPanel.ColumnStyles[2];
           //CurrentStyles[3] = LayoutPanel.ColumnStyles[3];
            CurrentStyles[4] = LayoutPanel.ColumnStyles[4];
           // CurrentStyles[5] = LayoutPanel.ColumnStyles[5];

            LayoutPanel.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 18);
            LayoutPanel.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 18);
            LayoutPanel.ColumnStyles[2] = new ColumnStyle(SizeType.Percent, 18);
            //LayoutPanel.ColumnStyles[3] = new ColumnStyle(SizeType.Percent, 18);
            LayoutPanel.ColumnStyles[4] = new ColumnStyle(SizeType.Percent, 18);
            //LayoutPanel.ColumnStyles[5] = new ColumnStyle(SizeType.Percent, 18);

            List<BarChart> barCharts = new List<BarChart>();
            barCharts.Add(barChartSpeed);
            barCharts.Add(barChartSize);
            barCharts.Add(barChartTime);
            barCharts.Add(barChartMemory);

            return barCharts;
        }

        public void ResetColumnStyle()
        {
            foreach (var item in CurrentStyles)
                LayoutPanel.ColumnStyles[item.Key] = new ColumnStyle(item.Value.SizeType, item.Value.Width);
        }

        public List<ChartSettings> GetLineChartSettings()
        {
            List<ChartSettings> settings = new List<ChartSettings>();

            settings.Add(lineChartAverageSpeed.Settings);
            settings.Add(lineChartMomentSpeed.Settings);
            //settings.Add(lineChartAverageCPU.Settings);
            settings.Add(lineChartMomentMemory.Settings);
            //settings.Add(lineChartAverageIO.Settings);

            return settings;
        }

        public void SetSettings(List<ChartSettings> settings)
        {
            lineChartAverageSpeed.Settings = settings[0];
            lineChartMomentSpeed.Settings = settings[1];
            //lineChartAverageCPU.Settings = settings[2];
            lineChartMomentMemory.Settings = settings[2];
            //lineChartAverageIO.Settings = settings[4];
        }

        public void ShowBarChart(int columnNumber, bool visible)
        {
            if (visible)
                LayoutPanel.ColumnStyles[columnNumber] = new ColumnStyle(SizeType.Percent, 18);
            else
                LayoutPanel.ColumnStyles[columnNumber] = new ColumnStyle(SizeType.Absolute, 0);
        }

        public LegendPossition SelectedChartPosition
        {
            get { return ((LineChart)tabControlCharts.SelectedTab.Controls[0]).GetLegendPosition(); }
            set { ((LineChart)tabControlCharts.SelectedTab.Controls[0]).SetLegenedPosition(value); }
        }

        public bool SelectedChartIsLogarithmic
        {
            get { return ((LineChart)tabControlCharts.SelectedTab.Controls[0]).IsLogarithmic; }
            set { ((LineChart)tabControlCharts.SelectedTab.Controls[0]).IsLogarithmic = value; }
        }

        public bool SelectedChartLegendIsVisible
        {
            get { return ((LineChart)tabControlCharts.SelectedTab.Controls[0]).LegendVisible; }
            set { ((LineChart)tabControlCharts.SelectedTab.Controls[0]).LegendVisible = value; }
        }
    }
}
