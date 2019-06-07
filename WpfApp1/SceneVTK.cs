using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kitware.VTK;

namespace WpfApp1
{
    class SceneVTK : RenderWindowControl
    {
        public static vtkActor CreateSphereActor(double radius)
        {
            vtkActor a = new vtkActor();

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

        public static vtkAxesActor CreateAxesActor(double length, double coneRadius, bool showLabels)
        {
            vtkAxesActor axa = new vtkAxesActor();

            axa.SetTotalLength(length, length, length);
            axa.SetConeRadius(coneRadius);
            int isl = showLabels ? 1 : 0;
            axa.SetAxisLabels(isl);

            axa.GetXAxisCaptionActor2D().GetTextActor().SetTextScaleMode((int)Kitware.VTK.vtkTextActor.TEXT_SCALE_MODE_NONE_WrapperEnum.TEXT_SCALE_MODE_NONE);
            axa.GetXAxisCaptionActor2D().GetTextActor().GetTextProperty().SetFontSize(32);
            axa.GetYAxisCaptionActor2D().GetTextActor().SetTextScaleMode((int)Kitware.VTK.vtkTextActor.TEXT_SCALE_MODE_NONE_WrapperEnum.TEXT_SCALE_MODE_NONE);
            axa.GetYAxisCaptionActor2D().GetTextActor().GetTextProperty().SetFontSize(32);
            axa.GetZAxisCaptionActor2D().GetTextActor().SetTextScaleMode((int)Kitware.VTK.vtkTextActor.TEXT_SCALE_MODE_NONE_WrapperEnum.TEXT_SCALE_MODE_NONE);
            axa.GetZAxisCaptionActor2D().GetTextActor().GetTextProperty().SetFontSize(32);

            return axa;
        }

        public SceneVTK()
        {

        }

       ~SceneVTK()
        {

        }

        public vtkRenderer Renderer()
        {
            return m_renderer;
        }

        public void UpdateScene()
        {
            this.RenderWindow.Render();
        }

        public void Initialize(System.Windows.Forms.Integration.WindowsFormsHost wfh)
        {
            InitializeVTK(wfh);
            InitializeTimer();
        }

        public void StartContinuousRender(int desiredFPS = 60)
        {
            m_timer.Interval = 1000.0 / desiredFPS;
            m_timer.Start();
        }

        public void StopContinuousRender()
        {
            m_timer.Stop();
        }

        public bool IsContinuousRendering()
        {
            return m_timer.Enabled;
        }

        private void InitializeVTK(System.Windows.Forms.Integration.WindowsFormsHost wfh)
        {
            this.AddTestActors = false;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            wfh.Child = this;
            wfh.Visibility = System.Windows.Visibility.Visible;
            vtkRendererCollection rs = this.RenderWindow.GetRenderers();
            Kitware.VTK.vtkRenderer r = rs.GetFirstRenderer();
            r.SetBackground(0.1, 0.3, 0.7);
            r.SetBackground2(0.7, 0.8, 1.0);
            r.SetGradientBackground(true);

            m_renderer = r;
        }

        private void InitializeTimer()
        {
            m_timer = new System.Timers.Timer(1000.0 / 60);
            m_timer.Elapsed += OnTimer;
        }

        private void OnTimer(Object source, System.Timers.ElapsedEventArgs e)
        {
            UpdateSceneFunction u = new UpdateSceneFunction(UpdateScene);
            this.Invoke(u);
        }

        private vtkRenderer m_renderer;
        private System.Timers.Timer m_timer;
        private delegate void UpdateSceneFunction();
    }
}
