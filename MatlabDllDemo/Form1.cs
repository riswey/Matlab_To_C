using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MatlabDllDemo
{
    public partial class Form1 : Form
    {
        [DllImport("GenSig.dll")]
        public static extern void GenSig(double[] time_vals_data, int[] time_vals_size, double[] signal_data, int[] signal_size);
        [DllImport("GenSig.dll")]
        public static extern void GenSig_initialize();
        [DllImport("GenSig.dll")]
        public static extern void GenSig_terminate();

        [DllImport("CalcSpec.dll")]
        public static extern void CalcSpec(double[] signal_, double[] spectrum);
        [DllImport("CalcSpec.dll")]
        public static extern void CalcSpec_initialize();
        [DllImport("CalcSpec.dll")]
        public static extern void CalcSpec_terminate();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetSignal(out double[] signal_data);

            chart1.Legends.Clear();
            chart2.Legends.Clear();

            chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            for (int i=0;i<2001;i++) {
                chart1.Series["Series1"].Points.AddXY(i * 0.01, signal_data[i]);
            }
            //chart title  
            chart1.Titles.Add("Signal");

            GetSpectrum(signal_data, out double[] spectrum);
            chart2.Series["Series1"].ChartType = SeriesChartType.Line;
            for (int i = 0; i < 1001; i++)
            {
                chart2.Series["Series1"].Points.AddXY((double)i / 20, spectrum[i]);
            }
            //chart title  
            chart2.Titles.Add("Spectrum");

        }


        static void GetSignal(out double[] signal_data)
        {
            //Declare and Initialse
            int[] time_vals_size = new int[1] { 2001 };
            double[] time_vals_data = new double[2001];
            {
                double counter = 0.0;
                for (int i = 0; i < 2001; i++)
                {
                    time_vals_data[i] = counter;
                    counter += 0.01;
                }
            }

            //Result array
            signal_data = new double[2001];

            int[] signal_size = new int[2]; //what does this do?

            //Only needs be called once
            GenSig_initialize();
            //Make function calls freely
            GenSig(time_vals_data, time_vals_size, signal_data, signal_size);
            //Only needs be called once
            GenSig_terminate();
        }

        static void GetSpectrum(double[] signal_data, out double[] spectrum)
        {
            spectrum = new double[2001];
            CalcSpec_initialize();
            CalcSpec(signal_data, spectrum);
            CalcSpec_terminate();
        }

    }
}