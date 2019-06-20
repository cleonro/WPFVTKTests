using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Kitware.VTK;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            m_timer = new System.Timers.Timer(1000 / 100);
            m_timer.Elapsed += this.OnTimer;
        }

        private void File_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (m_timer.Enabled)
            {
                m_timer.Stop();
                this.startMenu.Header = "Start";
            }
            else
            {
                m_timer.Start();
                this.startMenu.Header = "Stop";
            }
        }

        private void InitializeVTK()
        {
            if(m_vtkInitialized)
            {
                return;
            }
            m_vtkInitialized = true;

            //vtk version
            string vtkVersion = Kitware.VTK.vtkVersion.GetVTKVersion();
            vtkVersion = "VTK " + vtkVersion;
            Console.WriteLine(vtkVersion);

            this.label.Content = vtkVersion;

            //vtk scene
            m_scene = new SceneVTK();
            m_scene.Initialize(wfh);

            //add axes
            vtkRenderer r = m_scene.Renderer();
            vtkAxesActor axa = SceneVTK.CreateAxesActor(50.0, 0.1, false);
            r.AddActor(axa);

            //add sphere actor
            m_sa = SceneVTK.CreateSphereActor(10.0);
            r.AddActor(m_sa);

            m_sa.SetPosition(25.0, 25.0, 25.0);

            //start scene continuous render
            m_scene.StartContinuousRender();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            InitializeVTK();
        }
  
        private void OnTimer(Object source, System.Timers.ElapsedEventArgs e)
        {
            UpdateGr u = new UpdateGr(UpdateVTK);
            wfh.Child.Invoke(u);
        }

        private void UpdateVTK()
        {
            string cs = this.label1.Content.ToString();
            int ci = int.Parse(cs);
            ci += 1;
            cs = ci.ToString();
            this.label1.Content = cs;

            
            m_elt += m_step;
            double x = 0.0 + 25.0 * Math.Cos(m_elt);
            double y = 25.0 + 25.0 * Math.Sin(m_elt);
            double z = 25.0 * Math.Cos(0.1 * m_elt);

            m_sa.SetPosition(x, y, z);

            //m_scene.UpdateScene();
        }

        private delegate void UpdateGr();

        private System.Timers.Timer m_timer;
        private vtkActor m_sa;

        private double m_elt = 0.0;
        private double m_step = Math.PI / 100.0;

        private void Window_Closed(object sender, EventArgs e)
        {
            m_scene.StopContinuousRender();
            m_timer.Dispose();
        }

        private SceneVTK m_scene;
        private bool m_vtkInitialized = false;
    }
}
