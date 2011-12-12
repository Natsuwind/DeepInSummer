using com.google.zxing;
using com.google.zxing.client.result;
using com.google.zxing.common;
using com.google.zxing.datamatrix;
using com.google.zxing.oned;
using com.google.zxing.qrcode;
using com.shopsavvy.scankit.Helpers;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
namespace com.shopsavvy.scankit
{
    [TemplateVisualState(GroupName = "CodeType", Name = "QRCode"), TemplatePart(Name = "PART_VideoTransform", Type = typeof(CompositeTransform)), TemplatePart(Name = "PART_VideoBrush", Type = typeof(VideoBrush)), TemplateVisualState(GroupName = "CodeType", Name = "OneDCode"), TemplateVisualState(GroupName = "ScanningStatus", Name = "Scanning"), TemplateVisualState(GroupName = "ScanningStatus", Name = "NotScanning"), TemplateVisualState(GroupName = "CodeType", Name = "TwoDCode"), TemplatePart(Name = "PART_CamRect", Type = typeof(Rectangle))]
    public class BarcodeScanner : Control
    {
        private const string CameraRectangle = "PART_CamRect";
        private const string CameraBrush = "PART_VideoBrush";
        private const string CameraTransform = "PART_VideoTransform";
        private Rectangle _cameraRectangle;
        private VideoBrush _cameraVideoBrush;
        private CompositeTransform _cameraTransform;
        private PhotoCamera _cam;
        private static ManualResetEvent _captureEvent;
        private static ManualResetEvent _pauseFramesEvent = new ManualResetEvent(true);
        private Thread _YFramesThread;
        private bool _pumpYFrames;
        private int _frameCounterForFocus;
        private TimeSpan _scanTime;
        private CodeType _scanType;
        private Dictionary<object, object> _hintDictionary;
        private Reader _currentReader;
        private bool _isScanning;
        private bool _isCameraOn;
        private Result _resultCache;
        public static readonly DependencyProperty CameraOrientationProperty = DependencyProperty.Register("CameraOrientation", typeof(PageOrientation), typeof(BarcodeScanner), new PropertyMetadata(1, new PropertyChangedCallback(BarcodeScanner.OnCameraOrientationChanged)));
        public static readonly DependencyProperty VibrateOnSuccessProperty = DependencyProperty.Register("VibrateOnSuccess", typeof(bool), typeof(BarcodeScanner), new PropertyMetadata(true));
        public static readonly DependencyProperty VibrationTimeProperty = DependencyProperty.Register("VibrationTime", typeof(TimeSpan), typeof(BarcodeScanner), new PropertyMetadata(new TimeSpan(0, 0, 0, 0, 300), new PropertyChangedCallback(BarcodeScanner.OnVibrationTimeChanged)));
        public static readonly DependencyProperty IsScanningProperty = DependencyProperty.Register("IsScanning", typeof(bool), typeof(BarcodeScanner), new PropertyMetadata(true, new PropertyChangedCallback(BarcodeScanner.OnIsScanningChanged)));
        public static readonly DependencyProperty ScanTypeProperty = DependencyProperty.Register("ScanType", typeof(CodeType), typeof(BarcodeScanner), new PropertyMetadata(CodeType.QRCode, new PropertyChangedCallback(BarcodeScanner.OnScanTypeChanged)));
        public static readonly DependencyProperty UseAutoFocusProperty = DependencyProperty.Register("UseAutoFocus", typeof(bool), typeof(BarcodeScanner), new PropertyMetadata(true, new PropertyChangedCallback(BarcodeScanner.OnUseAutoFocusChanged)));
        public static readonly DependencyProperty FramesPerFocusAttemptProperty = DependencyProperty.Register("FramesPerFocusAttempt", typeof(int), typeof(BarcodeScanner), new PropertyMetadata(70, new PropertyChangedCallback(BarcodeScanner.OnFramesPerFocusAttemptChanged)));
        public static readonly DependencyProperty StopOnSuccessProperty = DependencyProperty.Register("StopOnSuccess", typeof(bool), typeof(BarcodeScanner), new PropertyMetadata(true, new PropertyChangedCallback(BarcodeScanner.OnStopOnSuccessChanged)));
        public static readonly DependencyProperty SampleDateTimeProperty = DependencyProperty.Register("SampleDateTime", typeof(DateTime), typeof(BarcodeScanner), new PropertyMetadata(DateTime.Now));
        public static readonly DependencyProperty IsCameraOnProperty = DependencyProperty.Register("IsCameraOn", typeof(bool), typeof(BarcodeScanner), new PropertyMetadata(true));
        public event EventHandler<ScanTypeChangedEventArgs> ScanTypeChanged;
        public event EventHandler<ScanSuccessfulEventArgs> OnScanSuccessful;
        public event EventHandler<CameraInitializedEventArgs> CameraInitializeChanged;
        public bool IsCameraInitialized
        {
            get;
            private set;
        }
        public PageOrientation CameraOrientation
        {
            get
            {
                return (PageOrientation)base.GetValue(BarcodeScanner.CameraOrientationProperty);
            }
            set
            {
                base.SetValue(BarcodeScanner.CameraOrientationProperty, value);
            }
        }
        public bool VibrateOnSuccess
        {
            get
            {
                return (bool)base.GetValue(BarcodeScanner.VibrateOnSuccessProperty);
            }
            set
            {
                base.SetValue(BarcodeScanner.VibrateOnSuccessProperty, value);
            }
        }
        public TimeSpan VibrationTime
        {
            get
            {
                return (TimeSpan)base.GetValue(BarcodeScanner.VibrationTimeProperty);
            }
            set
            {
                base.SetValue(BarcodeScanner.VibrationTimeProperty, value);
            }
        }
        public bool IsScanning
        {
            get
            {
                return (bool)base.GetValue(BarcodeScanner.IsScanningProperty);
            }
            set
            {
                base.SetValue(BarcodeScanner.IsScanningProperty, value);
            }
        }
        public CodeType ScanType
        {
            get
            {
                return (CodeType)base.GetValue(BarcodeScanner.ScanTypeProperty);
            }
            set
            {
                base.SetValue(BarcodeScanner.ScanTypeProperty, value);
            }
        }
        public bool UseAutoFocus
        {
            get
            {
                return (bool)base.GetValue(BarcodeScanner.UseAutoFocusProperty);
            }
            set
            {
                base.SetValue(BarcodeScanner.UseAutoFocusProperty, value);
            }
        }
        public int FramesPerFocusAttempt
        {
            get
            {
                return (int)base.GetValue(BarcodeScanner.FramesPerFocusAttemptProperty);
            }
            set
            {
                base.SetValue(BarcodeScanner.FramesPerFocusAttemptProperty, value);
            }
        }
        public bool StopOnSuccess
        {
            get
            {
                return (bool)base.GetValue(BarcodeScanner.StopOnSuccessProperty);
            }
            set
            {
                base.SetValue(BarcodeScanner.StopOnSuccessProperty, value);
            }
        }
        public DateTime SampleDateTime
        {
            get
            {
                return (DateTime)base.GetValue(BarcodeScanner.SampleDateTimeProperty);
            }
            set
            {
                base.SetValue(BarcodeScanner.SampleDateTimeProperty, value);
            }
        }
        public bool IsCameraOn
        {
            get
            {
                return (bool)base.GetValue(BarcodeScanner.IsCameraOnProperty);
            }
            set
            {
                base.SetValue(BarcodeScanner.IsCameraOnProperty, value);
            }
        }
        public BarcodeScanner()
        {
            base.DefaultStyleKey = typeof(BarcodeScanner);
            BarcodeScanner._captureEvent = new ManualResetEvent(true);
            this.IsCameraInitialized = false;
            base.Loaded += new RoutedEventHandler(this.BarcodeScanner_Loaded);
            base.LayoutUpdated += new EventHandler(this.BarcodeScanner_LayoutUpdated);
        }
        private void BarcodeScanner_LayoutUpdated(object sender, EventArgs e)
        {
            this._cam = new PhotoCamera();
            this._cam.Initialized += new EventHandler<CameraOperationCompletedEventArgs>(this._cam_Initialized);
            if (this._cameraRectangle != null && this._cameraVideoBrush != null)
            {
                CameraVideoBrushExtensions.SetSource(this._cameraVideoBrush, this._cam);
            }
            if (this._YFramesThread == null || !this._YFramesThread.IsAlive)
            {
                this._pumpYFrames = true;
                this._YFramesThread = new Thread(new ThreadStart(this.PumpYFrames));
                this._YFramesThread.Start();
            }
        }
        private void BarcodeScanner_Loaded(object sender, RoutedEventArgs e)
        {
            this.IsCameraInitialized = false;
            if (this._cam == null)
            {
                this._cam = new PhotoCamera();
                this._cam.Initialized += new EventHandler<CameraOperationCompletedEventArgs>(this._cam_Initialized);
                if (this._cameraRectangle != null && this._cam != null && this._cameraVideoBrush != null)
                {
                    CameraVideoBrushExtensions.SetSource(this._cameraVideoBrush, this._cam);
                }
            }
            if (this._YFramesThread == null || !this._YFramesThread.IsAlive)
            {
                this._pumpYFrames = true;
                this._YFramesThread = new Thread(new ThreadStart(this.PumpYFrames));
                this._YFramesThread.Start();
            }
            CompositionTarget.Rendering += new EventHandler(this.CompositionTarget_Rendering);
        }
        public void RestartCamera()
        {
            this._cam.Initialized -= new EventHandler<CameraOperationCompletedEventArgs>(this._cam_Initialized);
            CompositionTarget.Rendering -= new EventHandler(this.CompositionTarget_Rendering);
            this._cam = null;
            this._YFramesThread = null;
            this._cam = new PhotoCamera();
            this._cam.Initialized += new EventHandler<CameraOperationCompletedEventArgs>(this._cam_Initialized);
            if (this._cameraRectangle != null && this._cameraVideoBrush != null)
            {
                CameraVideoBrushExtensions.SetSource(this._cameraVideoBrush, this._cam);
            }
            this._pumpYFrames = true;
            this._YFramesThread = new Thread(new ThreadStart(this.PumpYFrames));
            this._YFramesThread.Start();
            CompositionTarget.Rendering += new EventHandler(this.CompositionTarget_Rendering);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this._cam = new PhotoCamera();
            this._cam.Initialized += new EventHandler<CameraOperationCompletedEventArgs>(this._cam_Initialized);
            this._cameraRectangle = (base.GetTemplateChild("PART_CamRect") as Rectangle);
            if (this._cameraRectangle != null && this._cam != null)
            {
                this._cameraVideoBrush = (base.GetTemplateChild("PART_VideoBrush") as VideoBrush);
                this._cameraTransform = (base.GetTemplateChild("PART_VideoTransform") as CompositeTransform);
                if (this._cameraVideoBrush != null)
                {
                    CameraVideoBrushExtensions.SetSource(this._cameraVideoBrush, this._cam);
                }
            }
            this._hintDictionary = new Dictionary<object, object>();
            this._currentReader = this.GetReader(this.ScanType);
            this._scanType = this.ScanType;
            if (this.ScanType == CodeType.QRCode)
            {
                VisualStateManager.GoToState(this, "QRCode", true);
            }
            else
            {
                if (this.ScanType == CodeType.TwoDCodes || this.ScanType == CodeType.DataMatrix)
                {
                    VisualStateManager.GoToState(this, "TwoDCode", true);
                }
                else
                {
                    VisualStateManager.GoToState(this, "OneDCode", true);
                }
            }
            if (this.IsScanning)
            {
                VisualStateManager.GoToState(this, "Scanning", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "NotScanning", true);
            }
            if (this.CameraOrientation == PageOrientation.Portrait || this.CameraOrientation == PageOrientation.PortraitDown || this.CameraOrientation == PageOrientation.PortraitUp)
            {
                this._cameraTransform.Rotation = 90.0;
            }
            else
            {
                if (this.CameraOrientation == PageOrientation.LandscapeLeft)
                {
                    this._cameraTransform.Rotation = 0.0;
                }
                else
                {
                    this._cameraTransform.Rotation = 180.0;
                }
            }
            if (this.IsScanning)
            {
                this._isScanning = true;
            }
            else
            {
                this._isScanning = false;
            }
            CompositionTarget.Rendering += new EventHandler(this.CompositionTarget_Rendering);
        }
        private void PumpYFrames()
        {
            byte[] array = new byte[307200];
            bool flag = false;
            while (this._pumpYFrames)
            {
                BarcodeScanner._pauseFramesEvent.WaitOne();
                if (this._isScanning)
                {
                    try
                    {
                        this._cam.GetPreviewBufferY(array);
                        flag = true;
                    }
                    catch
                    {
                        flag = false;
                    }
                    if (flag)
                    {
                        DateTime now = DateTime.Now;
                        RGBLuminanceSource rGBLuminanceSource = new RGBLuminanceSource(array, 640, 480, false);
                        if (this._scanType == CodeType.OneDCodes || this._scanType == CodeType.UPC || this._scanType == CodeType.ITF || this._scanType == CodeType.EAN || this._scanType == CodeType.Code128 || this._scanType == CodeType.Code39)
                        {
                            rGBLuminanceSource.rotateCounterClockwise();
                        }
                        HybridBinarizer binarizer = new HybridBinarizer(rGBLuminanceSource);
                        BinaryBitmap image = new BinaryBitmap(binarizer);
                        Reader reader = this.GetReader(this._scanType);
                        try
                        {
                            Result results = reader.decode(image, this._hintDictionary);
                            TimeSpan timeSpent = DateTime.Now.Subtract(now);
                            Deployment.Current.Dispatcher.BeginInvoke(delegate
                            {
                                this.ProcessScan(results, timeSpent, this.ScanType);
                                if (this.StopOnSuccess)
                                {
                                    this.IsScanning = false;
                                }
                            }
                            );
                            BarcodeScanner._pauseFramesEvent.Set();
                        }
                        catch
                        {
                            BarcodeScanner._pauseFramesEvent.Set();
                        }
                    }
                }
            }
        }
        private void ProcessScan(Result successfulScan, TimeSpan scanTime, CodeType scanType)
        {
            bool flag = true;
            if (this._resultCache == null)
            {
                flag = true;
            }
            else
            {
                if (this._resultCache.Text.Length == successfulScan.Text.Length && this._resultCache.Text.Substring(1, 10) == successfulScan.Text.Substring(1, 10))
                {
                    flag = false;
                }
            }
            if (flag)
            {
                this._resultCache = successfulScan;
                if (scanType == CodeType.QRCode)
                {
                    ParsedResult scanObject = ResultParser.parseResult(successfulScan);
                    this.RaiseOnScanSuccessful(scanType, successfulScan.Text, scanObject, scanTime);
                }
                else
                {
                    this.RaiseOnScanSuccessful(scanType, successfulScan.Text, successfulScan.Text, scanTime);
                }
                if (this.VibrateOnSuccess)
                {
                    VibrateController @default = VibrateController.Default;
                    @default.Start(this.VibrationTime);
                }
            }
        }
        private Reader GetReader(CodeType aCodeType)
        {
            this._hintDictionary = new Dictionary<object, object>();
            switch (aCodeType)
            {
                case CodeType.OneDCodes:
                    {
                        return new MultiFormatOneDReader(this._hintDictionary);
                    }
                case CodeType.TwoDCodes:
                    {
                        List<BarcodeFormat> list = new List<BarcodeFormat>();
                        list.Add(BarcodeFormat.QR_CODE);
                        list.Add(BarcodeFormat.DATAMATRIX);
                        this._hintDictionary.Add(DecodeHintType.POSSIBLE_FORMATS, list);
                        return new MultiFormatReader
                        {
                            Hints = this._hintDictionary
                        };
                    }
                case CodeType.UPC:
                    {
                        return new MultiFormatUPCEANReader(this._hintDictionary);
                    }
                case CodeType.QRCode:
                    {
                        return new QRCodeReader();
                    }
                case CodeType.EAN:
                    {
                        return new MultiFormatUPCEANReader(this._hintDictionary);
                    }
                case CodeType.Code128:
                    {
                        return new Code128Reader();
                    }
                case CodeType.Code39:
                    {
                        return new Code39Reader();
                    }
                case CodeType.ITF:
                    {
                        return new ITFReader();
                    }
                case CodeType.DataMatrix:
                    {
                        return new DataMatrixReader();
                    }
                default:
                    {
                        return new MultiFormatUPCEANReader(this._hintDictionary);
                    }
            }
        }
        private void _cam_Initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            if (e.Succeeded)
            {
                this.RaiseCameraInitializeChanged(true);
                this.IsCameraInitialized = true;
                this._pumpYFrames = true;
                return;
            }
            this.RaiseCameraInitializeChanged(false);
            this.IsCameraInitialized = false;
        }
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (this.IsCameraInitialized)
            {
                if (this._frameCounterForFocus > this.FramesPerFocusAttempt && this.UseAutoFocus)
                {
                    try
                    {
                        if (this._cam.IsFocusSupported)
                        {
                            this._cam.Focus();
                        }
                    }
                    catch
                    {
                        this.IsCameraInitialized = false;
                    }
                    this._frameCounterForFocus = 0;
                }
                else
                {
                    this._frameCounterForFocus++;
                }
                if (this._YFramesThread != null && !this._YFramesThread.IsAlive)
                {
                    this._YFramesThread.Start();
                }
            }
        }
        private static void OnCameraOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BarcodeScanner)d).OnCameraOrientationChanged(e);
        }
        protected virtual void OnCameraOrientationChanged(DependencyPropertyChangedEventArgs e)
        {
            PageOrientation pageOrientation = (PageOrientation)e.NewValue;
            if (pageOrientation == PageOrientation.Portrait || pageOrientation == PageOrientation.PortraitDown || pageOrientation == PageOrientation.PortraitUp)
            {
                this._cameraTransform.Rotation = 90.0;
                return;
            }
            if (pageOrientation == PageOrientation.LandscapeLeft)
            {
                this._cameraTransform.Rotation = 0.0;
                return;
            }
            this._cameraTransform.Rotation = 180.0;
        }
        private static void OnVibrationTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BarcodeScanner)d).OnVibrationTimeChanged(e);
        }
        protected virtual void OnVibrationTimeChanged(DependencyPropertyChangedEventArgs e)
        {
        }
        private static void OnIsScanningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BarcodeScanner)d).OnIsScanningChanged(e);
        }
        protected virtual void OnIsScanningChanged(DependencyPropertyChangedEventArgs e)
        {
            bool flag = (bool)e.NewValue;
            this._isScanning = flag;
            if (flag)
            {
                VisualStateManager.GoToState(this, "Scanning", true);
                return;
            }
            VisualStateManager.GoToState(this, "NotScanning", true);
        }
        private static void OnScanTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BarcodeScanner)d).OnScanTypeChanged(e);
        }
        protected virtual void OnScanTypeChanged(DependencyPropertyChangedEventArgs e)
        {
            CodeType codeType = (CodeType)e.NewValue;
            this._currentReader = this.GetReader(codeType);
            this._scanType = codeType;
            this.RaiseOnScanTypeChanged(codeType);
            if (codeType == CodeType.QRCode)
            {
                VisualStateManager.GoToState(this, "QRCode", true);
                return;
            }
            if (codeType == CodeType.TwoDCodes || codeType == CodeType.DataMatrix)
            {
                VisualStateManager.GoToState(this, "TwoDCode", true);
                return;
            }
            VisualStateManager.GoToState(this, "OneDCode", true);
        }
        private static void OnUseAutoFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BarcodeScanner)d).OnUseAutoFocusChanged(e);
        }
        protected virtual void OnUseAutoFocusChanged(DependencyPropertyChangedEventArgs e)
        {
        }
        private static void OnFramesPerFocusAttemptChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BarcodeScanner)d).OnFramesPerFocusAttemptChanged(e);
        }
        protected virtual void OnFramesPerFocusAttemptChanged(DependencyPropertyChangedEventArgs e)
        {
        }
        private static void OnStopOnSuccessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BarcodeScanner)d).OnStopOnSuccessChanged(e);
        }
        protected virtual void OnStopOnSuccessChanged(DependencyPropertyChangedEventArgs e)
        {
        }
        public void FocusCamera()
        {
            try
            {
                if (this._cam.IsFocusSupported)
                {
                    this._cam.Focus();
                }
            }
            catch
            {
            }
        }
        public void FocusCameraAtPoint(double x, double y)
        {
            try
            {
                if (this._cam.IsFocusAtPointSupported)
                {
                    this._cam.FocusAtPoint(x, y);
                }
                else
                {
                    if (this._cam.IsFocusSupported)
                    {
                        this._cam.Focus();
                    }
                }
            }
            catch
            {
            }
        }
        public void TurnCameraOff()
        {
            if (this._cam != null)
            {
                this._cam = null;
                this._pumpYFrames = false;
                this._YFramesThread = null;
                this.IsCameraOn = false;
            }
        }
        public void TurnCameraOn()
        {
            this._cam = new PhotoCamera();
            this._cam.Initialized += new EventHandler<CameraOperationCompletedEventArgs>(this._cam_Initialized);
            if (this._cameraRectangle != null && this._cameraVideoBrush != null)
            {
                CameraVideoBrushExtensions.SetSource(this._cameraVideoBrush, this._cam);
            }
            this._YFramesThread = null;
            this._pumpYFrames = true;
            this._YFramesThread = new Thread(new ThreadStart(this.PumpYFrames));
            this._YFramesThread.Start();
        }
        private void RaiseOnScanTypeChanged(CodeType newCodeType)
        {
            if (this.ScanTypeChanged != null)
            {
                this.ScanTypeChanged.Invoke(this, new ScanTypeChangedEventArgs(newCodeType));
            }
        }
        private void RaiseOnScanSuccessful(CodeType scanType, string rawScanResult, object scanObject, TimeSpan scanTime)
        {
            bool isScanTimeCalculated = true;
            if (this.OnScanSuccessful != null)
            {
                this.OnScanSuccessful.Invoke(this, new ScanSuccessfulEventArgs(scanType, rawScanResult, scanObject, isScanTimeCalculated, scanTime));
            }
        }
        private void RaiseCameraInitializeChanged(bool isCamInitialized)
        {
            if (this.CameraInitializeChanged != null)
            {
                this.CameraInitializeChanged.Invoke(this, new CameraInitializedEventArgs(isCamInitialized));
            }
        }
    }
}
