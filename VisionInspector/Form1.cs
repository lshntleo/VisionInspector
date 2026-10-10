using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace VisionInspector;

public partial class Form1 : Form
{
    private readonly InspectionRepository _repo = new();

    private TcpInspectionServer? _server;

    private string? _imagePath;

    public Form1()
    {
        InitializeComponent();
        numMinArea.Maximum = 100000;
        numMinArea.Value = 50;
        numMaxCount.Value = 3;
        btnInspect.Enabled = false;
        RefreshHistory();
        _server = new TcpInspectionServer(HandleCommand);
        _server.Start(9000);
    }

    // 1단계: 이미지 열기
    private void btnOpen_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Filter = "이미지|*.png;*.jpg;*.jpeg;*.bmp"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        _imagePath = dlg.FileName;
        picOriginal.Image?.Dispose();
        picOriginal.Image = new Bitmap(_imagePath);
        picResult.Image = null;
        lblResult.Text = "-";
        lblResult.ForeColor = Color.Black;
        btnInspect.Enabled = true;
    }

    private void btnInspect_Click(object sender, EventArgs e)
    {
        if (_imagePath == null) return;
        RunInspection(_imagePath);
    }

    // 버튼, TCP 공용: 반드시 UI 스레드에서 호출
    private (bool ok, int count, long ms) RunInspection(string path)
    {
        var (ok, count, ms) = Inspect(path);
        ShowResult(ok, ms);
        lblTime.Text = $"검사 시간: {ms} ms / 검출: {count}개";
        _repo.Insert(Path.GetFileName(path), ok, count, ms);
        RefreshHistory();
        return (ok, count, ms);
    }

    // TCP 스레드에서 호출됨 -> UI 작업은 Invoke로 UI 스레드에 넘김
    private string HandleCommand(string cmd)
    {
        var parts = cmd.Split(' ', 2);
        if (!parts[0].Equals("START", StringComparison.OrdinalIgnoreCase))
            return "ERROR,UNKNOWN_COMMAND";

        string? argPath = parts.Length > 1 ? parts[1].Trim().Trim('"') : null;

        return (string)Invoke(new Func<string>(() =>
        {
            string? path = argPath ?? _imagePath;
            if (string.IsNullOrEmpty(path)) return "ERROR,NO_IMAGE";
            if (!File.Exists(path)) return "ERROR,FILE_NOT_FOUND";

            _imagePath = path;
            picOriginal.Image?.Dispose();
            picOriginal.Image = new Bitmap(path);
            btnInspect.Enabled = true;

            var (ok, count, ms) = RunInspection(path);
            return $"{(ok ? "OK" : "NG")},{count},{ms}";
        }));
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _server?.Stop();
        base.OnFormClosing(e);
    }

    // 2단계: 검사 로직 (나중에 TCP에서도 재사용)
    public (bool ok, int count, long ms) Inspect(string path)
    {
        var sw = Stopwatch.StartNew();

        using var src = CvInvoke.Imread(path, ImreadModes.ColorBgr); ;
        using var gray = new Mat();
        using var blur = new Mat();
        using var bin = new Mat();

        CvInvoke.CvtColor(src, gray, ColorConversion.Bgr2Gray);
        CvInvoke.GaussianBlur(gray, blur, new Size(5, 5), 0);
        // Otsu + 반전: 어두운 물체를 흰색(전경)으로
        CvInvoke.Threshold(blur, bin, 0, 255,
            ThresholdType.BinaryInv | ThresholdType.Otsu);

        using var contours = new VectorOfVectorOfPoint();
        using var hier = new Mat();
        CvInvoke.FindContours(bin, contours, hier,
            RetrType.External, ChainApproxMethod.ChainApproxSimple);

        int minArea = (int)numMinArea.Value;
        int count = 0;
        using var result = src.Clone();
        for (int i = 0; i < contours.Size; i++)
        {
            double area = CvInvoke.ContourArea(contours[i]);
            if (area < minArea) continue;   // 면적 필터
            count++;
            var rect = CvInvoke.BoundingRectangle(contours[i]);
            CvInvoke.Rectangle(result, rect, new MCvScalar(0, 0, 255), 2);
        }

        // 판정: 검출 개수가 허용치 이하면 OK
        bool ok = count <= (int)numMaxCount.Value;

        sw.Stop();
        picResult.Image?.Dispose();
        picResult.Image = result.ToBitmap();
        return (ok, count, sw.ElapsedMilliseconds);
    }

    private void RefreshHistory()
    {
        dgvHistory.DataSource = _repo.GetAll();
        if (dgvHistory.Columns.Count == 0) return;
        dgvHistory.Columns["Id"]!.HeaderText = "번호";
        dgvHistory.Columns["InspectedAt"]!.HeaderText = "검사 시각";
        dgvHistory.Columns["FileName"]!.HeaderText = "파일명";
        dgvHistory.Columns["Result"]!.HeaderText = "판정";
        dgvHistory.Columns["DefectCount"]!.HeaderText = "검출 개수";
        dgvHistory.Columns["ElapsedMs"]!.HeaderText = "소요(ms)";
    }

    private void ShowResult(bool ok, long ms)
    {
        lblResult.Text = ok ? "OK" : "NG";
        lblResult.ForeColor = ok ? Color.Green : Color.Red;
        lblTime.Text = $"검사 시간: {ms} ms";
    }
}