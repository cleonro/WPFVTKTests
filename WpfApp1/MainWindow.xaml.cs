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

            m_timer = new System.Timers.Timer(1000 / 10);
            m_timer.Elapsed += this.OnTimer;
        }

        private void File_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Menu clicked!");
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
            Kitware.VTK.RenderWindowControl rw = new Kitware.VTK.RenderWindowControl();
            rw.AddTestActors = false;
            rw.Dock = System.Windows.Forms.DockStyle.Fill;
            wfh.Child = rw;
            wfh.Visibility = System.Windows.Visibility.Visible;
            Kitware.VTK.vtkRendererCollection rs = rw.RenderWindow.GetRenderers();
            int rsc = rs.GetNumberOfItems();
            Console.WriteLine(rsc + " renderers");
            Kitware.VTK.vtkRenderer r = rs.GetFirstRenderer();
            r.SetBackground(0.1, 0.3, 0.7);
            r.SetBackground2(0.7, 0.8, 1.0);
            r.SetGradientBackground(true);

            Kitware.VTK.vtkAxesActor axa = new Kitware.VTK.vtkAxesActor();
            r.AddActor(axa);

            string vtkVersion = Kitware.VTK.vtkVersion.GetVTKVersion();
            vtkVersion = "VTK " + vtkVersion;
            Console.WriteLine(vtkVersion);

            this.label.Content = vtkVersion;

            axa.SetTotalLength(50.0, 50.0, 50.0);
            axa.SetConeRadius(0.1);
            //axa.SetAxisLabels(0);

            axa.GetXAxisCaptionActor2D().GetTextActor().SetTextScaleMode((int)Kitware.VTK.vtkTextActor.TEXT_SCALE_MODE_NONE_WrapperEnum.TEXT_SCALE_MODE_NONE);
            axa.GetXAxisCaptionActor2D().GetTextActor().GetTextProperty().SetFontSize(32);
            axa.GetYAxisCaptionActor2D().GetTextActor().SetTextScaleMode((int)Kitware.VTK.vtkTextActor.TEXT_SCALE_MODE_NONE_WrapperEnum.TEXT_SCALE_MODE_NONE);
            axa.GetYAxisCaptionActor2D().GetTextActor().GetTextProperty().SetFontSize(32);
            axa.GetZAxisCaptionActor2D().GetTextActor().SetTextScaleMode((int)Kitware.VTK.vtkTextActor.TEXT_SCALE_MODE_NONE_WrapperEnum.TEXT_SCALE_MODE_NONE);
            axa.GetZAxisCaptionActor2D().GetTextActor().GetTextProperty().SetFontSize(32);

            m_sa = this.CreateSphereActor(10.0);
            r.AddActor(m_sa);

            m_sa.SetPosition(25.0, 25.0, 25.0);
        }

        private vtkActor CreateSphereActor(double radius)
        {
            Kitware.VTK.vtkActor a = new Kitware.VTK.vtkActor();

            vtkSphereSource sphereSource3D = new vtkSphereSource();
            sphereSource3D.SetCenter(0.0, 0.0, 0.0);
            sphereSource3D.SetRadius(radius);
            sphereSource3D.SetThetaResolution(10);
            sphereSource3D.SetPhiResolution(10);

            vtkPolyDataMapper sphereMapper3D = vtkPolyDataMapper.New();
            sphereMapper3D.SetInputConnection(sphereSource3D.GetOutputPort());
            a.SetMapper(sphereMapper3D);
            a.GetProperty().SetColor(0.95, 0.5, 0.3);
            a.GetProperty().SetOpacity(0.5);

            return a;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            InitializeVTK();
        }

        private RenderWindowControl RenderWindow()
        {
            RenderWindowControl rw = (RenderWindowControl)wfh.Child;
            return rw;
        }

        private vtkRenderer Renderer()
        {
            RenderWindowControl rw = (RenderWindowControl)wfh.Child;
            vtkRendererCollection rs = rw.RenderWindow.GetRenderers();
            vtkRenderer r = rs.GetFirstRenderer();

            return r;
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

            //RenderWindowControl rw = this.RenderWindow();
            //rw.Refresh();

            //vtkRenderer r = this.Renderer();
            //r.GetRenderWindow().Render();
            this.RenderWindow().RenderWindow.Render();
        }

        private delegate void UpdateGr();

        private System.Timers.Timer m_timer;
        private vtkActor m_sa;

        private double m_elt = 0.0;
        private double m_step = Math.PI / 100.0;

        private void Window_Closed(object sender, EventArgs e)
        {
            m_timer.Dispose();
        }
    }
}
