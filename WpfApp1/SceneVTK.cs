using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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
            if(m_timer.Enabled && m_countFPS == 0)
            {
                m_stopWatch.Start();
            }
            this.RenderWindow.Render();
            if (m_timer.Enabled)
            {
                ++m_countFPS;
                if (m_countFPS == m_countFPSMax)
                {
                    m_stopWatch.Stop();
                    long elapsedTime = m_stopWatch.ElapsedMilliseconds;
                    m_fps = 1000.0 * (m_countFPS + 0.0) / (elapsedTime + 0.0);
                    string s = (int)m_fps + " FPS";
                    m_fpsText.SetInput(s);
                    m_countFPS = 0;
                    m_stopWatch.Reset();
                }
            }
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
            m_fpsText.SetVisibility(1);
        }

        public void StopContinuousRender()
        {
            m_timer.Stop();
            m_fpsText.SetVisibility(0);
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

            //add fps text actor
            m_fpsText = new vtkTextActor();
            m_fpsText.GetTextProperty().SetFontSize(12);
            m_fpsText.GetTextProperty().SetColor(0.3, 1.0, 0.3);
            m_fpsText.SetPosition2(5, 15);
            m_fpsText.SetInput(" FPS");
            m_renderer.AddActor2D(m_fpsText);
            m_fpsText.SetVisibility(0);
        }

        private void InitializeTimer()
        {
            m_stopWatch = new Stopwatch();
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

        int m_countFPS = 0;
        int m_countFPSMax = 60;
        double m_fps = 0;
        Stopwatch m_stopWatch;
        vtkTextActor m_fpsText;
    }
}
