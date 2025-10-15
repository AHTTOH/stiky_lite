using System.Runtime.InteropServices;
using StickyLite.Config;
using StickyLite.Storage;
using StickyLite.Logging;
using StickyLite.Core;
using StickyLite.Forms;

namespace StickyLite
{
    /// <summary>
    /// 메인 폼 - 스티키노트 UI
    /// </summary>
    public partial class MainForm : Form
    {
        #region Win32 API
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTBOTTOM = 15;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private const int WM_NCLBUTTONDOWN = 0x00A1;
        #endregion

        private readonly PathManager _pathManager;
        private readonly ConfigManager _configManager;
        private readonly NoteStorage _noteStorage;
        private readonly SimpleLogger _logger;
        private AppConfig _config = null!;
        private readonly string _noteId;

        /// <summary>
        /// 노트 제목 (public 속성)
        /// </summary>
        public string NoteTitle => lblTitle.Text;

        /// <summary>
        /// 노트 내용 미리보기 (첫 줄)
        /// </summary>
        public string NotePreview => txtNote.Text.Split('\n').FirstOrDefault() ?? "";

        public MainForm(string noteId = null)
        {
            _noteId = noteId ?? "default";
            InitializeComponent();
            
            // 의존성 주입
            _pathManager = new PathManager();
            _configManager = new ConfigManager(_pathManager.ConfigPath);
            _noteStorage = new NoteStorage(_pathManager.GetNotePath(_noteId));
            _logger = new SimpleLogger(_pathManager.LogPath);

            // 초기화
            InitializeApp();
            
            // NoteManager에 등록
            NoteManager.AddNote(_noteId, this);
        }

        /// <summary>
        /// 애플리케이션 초기화
        /// </summary>
        private void InitializeApp()
        {
            try
            {
                // 경로 정보 로깅
                _logger.Info(_pathManager.GetPathInfo());

                // 설정 로드
                _config = _configManager.LoadConfig();
                ApplyConfig();

                // 노트 내용 로드
                var noteContent = _noteStorage.LoadNote();
                txtNote.Text = noteContent;

                _logger.Info("StickyLite 초기화 완료");
            }
            catch (Exception ex)
            {
                _logger.Error("초기화 실패", ex);
                // 기본 설정으로 계속 진행
                _config = AppConfig.CreateDefault();
                ApplyConfig();
            }
        }

        /// <summary>
        /// 설정 적용
        /// </summary>
        private void ApplyConfig()
        {
            // 창 위치 및 크기
            this.Location = new Point(_config.WindowX, _config.WindowY);
            this.Size = new Size(_config.WindowWidth, _config.WindowHeight);
            this.TopMost = _config.TopMost;
            this.Opacity = _config.Opacity;

            // 폰트 설정
            txtNote.Font = _config.GetFont();
            txtNote.ForeColor = _config.GetTextColor();
            
            // 색상 테마 적용
            ApplyColorTheme();

            // 컨텍스트 메뉴 상태 업데이트
            menuTopMost.Checked = _config.TopMost;
            trackOpacity.Value = (int)(_config.Opacity * 100);

            // 자동저장 타이머 설정
            autoSaveTimer.Interval = _config.AutoSaveDelayMs;
        }

        /// <summary>
        /// Alt+드래그로 창 이동 처리 및 테두리 리사이즈 처리
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCHITTEST)
            {
                var pos = new Point(m.LParam.ToInt32() & 0xFFFF, m.LParam.ToInt32() >> 16);
                pos = PointToClient(pos);

                // Alt 키가 눌려있고 텍스트박스 영역을 클릭한 경우
                if (Control.ModifierKeys == Keys.Alt && txtNote.ClientRectangle.Contains(pos))
                {
                    m.Result = new IntPtr(HTCAPTION);
                    return;
                }

                // 테두리 리사이즈 감지
                var resizeHandle = GetResizeHandle(pos);
                if (resizeHandle != HTCLIENT)
                {
                    m.Result = new IntPtr(resizeHandle);
                    return;
                }
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// 마우스 위치에 따른 리사이즈 핸들 반환
        /// </summary>
        private int GetResizeHandle(Point pos)
        {
            const int resizeBorder = 8;
            var clientSize = this.ClientSize;

            // 모서리 감지 (우선순위 높음)
            if (pos.X <= resizeBorder && pos.Y <= resizeBorder)
                return HTTOPLEFT;
            if (pos.X >= clientSize.Width - resizeBorder && pos.Y <= resizeBorder)
                return HTTOPRIGHT;
            if (pos.X <= resizeBorder && pos.Y >= clientSize.Height - resizeBorder)
                return HTBOTTOMLEFT;
            if (pos.X >= clientSize.Width - resizeBorder && pos.Y >= clientSize.Height - resizeBorder)
                return HTBOTTOMRIGHT;

            // 가장자리 감지
            if (pos.X <= resizeBorder)
                return HTLEFT;
            if (pos.X >= clientSize.Width - resizeBorder)
                return HTRIGHT;
            if (pos.Y <= resizeBorder)
                return HTTOP;
            if (pos.Y >= clientSize.Height - resizeBorder)
                return HTBOTTOM;

            return HTCLIENT;
        }

        /// <summary>
        /// 키보드 단축키 처리
        /// </summary>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.T:
                        ToggleTopMost();
                        e.Handled = true;
                        break;
                    case Keys.S:
                        SaveNote();
                        e.Handled = true;
                        break;
                    case Keys.Q:
                        Close();
                        e.Handled = true;
                        break;
                    case Keys.C:
                        ChangeColorTheme();
                        e.Handled = true;
                        break;
                }
            }
        }

        /// <summary>
        /// 항상 위 토글
        /// </summary>
        private void ToggleTopMost()
        {
            _config.TopMost = !_config.TopMost;
            this.TopMost = _config.TopMost;
            menuTopMost.Checked = _config.TopMost;
            
            // 압정 버튼 아이콘 업데이트
            btnPin.Text = _config.TopMost ? "📌" : "📍";
            
            _logger.Info($"항상 위 토글: {_config.TopMost}");
        }

        /// <summary>
        /// 노트 저장
        /// </summary>
        private void SaveNote()
        {
            try
            {
                var success = _noteStorage.SaveNote(txtNote.Text);
                if (success)
                {
                    _logger.Info("노트 저장 완료");
                }
                else
                {
                    _logger.Error("노트 저장 실패");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("노트 저장 중 오류", ex);
            }
        }

        /// <summary>
        /// 설정 저장
        /// </summary>
        private void SaveConfig()
        {
            try
            {
                // 현재 창 상태 저장
                _config.WindowX = this.Location.X;
                _config.WindowY = this.Location.Y;
                _config.WindowWidth = this.Size.Width;
                _config.WindowHeight = this.Size.Height;
                _config.TopMost = this.TopMost;
                _config.Opacity = this.Opacity;

                // 폰트 설정 저장
                _config.FontFamily = txtNote.Font.FontFamily.Name;
                _config.FontSize = txtNote.Font.Size;
                _config.FontBold = txtNote.Font.Bold;
                
                // 색상 테마는 이미 _config에 저장되어 있음

                var success = _configManager.SaveConfig(_config);
                if (success)
                {
                    _logger.Info("설정 저장 완료");
                }
                else
                {
                    _logger.Error("설정 저장 실패");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("설정 저장 중 오류", ex);
            }
        }

        /// <summary>
        /// 텍스트 변경 시 자동저장 타이머 시작
        /// </summary>
        private void txtNote_TextChanged(object sender, EventArgs e)
        {
            autoSaveTimer.Stop();
            autoSaveTimer.Start();
        }

        /// <summary>
        /// 자동저장 타이머 이벤트
        /// </summary>
        private void autoSaveTimer_Tick(object sender, EventArgs e)
        {
            autoSaveTimer.Stop();
            SaveNote();
        }

        /// <summary>
        /// 폰트 크기 증가
        /// </summary>
        private void menuFontIncrease_Click(object? sender, EventArgs e)
        {
            var newSize = Math.Min(txtNote.Font.Size + 1, 24);
            txtNote.Font = new Font(txtNote.Font.FontFamily, newSize, txtNote.Font.Style);
            _logger.Info($"폰트 크기 증가: {newSize}");
        }

        /// <summary>
        /// 폰트 크기 감소
        /// </summary>
        private void menuFontDecrease_Click(object? sender, EventArgs e)
        {
            var newSize = Math.Max(txtNote.Font.Size - 1, 6);
            txtNote.Font = new Font(txtNote.Font.FontFamily, newSize, txtNote.Font.Style);
            _logger.Info($"폰트 크기 감소: {newSize}");
        }

        /// <summary>
        /// 투명도 변경
        /// </summary>
        private void trackOpacity_ValueChanged(object? sender, EventArgs e)
        {
            var opacity = trackOpacity.Value / 100.0;
            this.Opacity = opacity;
            _logger.Info($"투명도 변경: {opacity:F2}");
        }

        /// <summary>
        /// 항상 위 메뉴 클릭
        /// </summary>
        private void menuTopMost_Click(object? sender, EventArgs e)
        {
            ToggleTopMost();
        }

        /// <summary>
        /// 저장 메뉴 클릭
        /// </summary>
        private void menuSave_Click(object? sender, EventArgs e)
        {
            SaveNote();
        }

        /// <summary>
        /// 종료 메뉴 클릭
        /// </summary>
        private void menuExit_Click(object? sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 압정 버튼 클릭 (TopMost 토글)
        /// </summary>
        private void btnPin_Click(object sender, EventArgs e)
        {
            ToggleTopMost();
        }

        /// <summary>
        /// 새 노트 버튼 클릭
        /// </summary>
        private void btnNewNote_Click(object sender, EventArgs e)
        {
            CreateNewNote();
        }

        /// <summary>
        /// 노트 목록 버튼 클릭
        /// </summary>
        private void btnNoteList_Click(object sender, EventArgs e)
        {
            ShowNoteList();
        }

        /// <summary>
        /// 닫기 버튼 클릭
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 투명도 슬라이더 값 변경
        /// </summary>
        private void opacitySlider_ValueChanged(object sender, EventArgs e)
        {
            var opacity = opacitySlider.Value / 100.0;
            this.Opacity = opacity;
            _logger.Info($"투명도 변경: {opacity:F2}");
        }

        /// <summary>
        /// 제목바 드래그로 창 이동
        /// </summary>
        private void pnlTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, (IntPtr)HTCAPTION, IntPtr.Zero);
            }
        }

        /// <summary>
        /// 제목 라벨 드래그로 창 이동
        /// </summary>
        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, (IntPtr)HTCAPTION, IntPtr.Zero);
            }
        }

        /// <summary>
        /// 새 노트 생성
        /// </summary>
        private void CreateNewNote()
        {
            try
            {
                var newNoteId = Guid.NewGuid().ToString();
                var newNote = new MainForm(newNoteId);
                
                // 새 노트 위치를 현재 노트에서 약간 오프셋
                var offset = NoteManager.NoteCount * 20; // 노트 개수에 따라 20px씩 오프셋
                newNote.Location = new Point(this.Location.X + offset, this.Location.Y + offset);
                
                newNote.Show();
                _logger.Info($"새 노트 생성: {newNoteId} at ({newNote.Location.X}, {newNote.Location.Y})");
            }
            catch (Exception ex)
            {
                _logger.Error("새 노트 생성 실패", ex);
            }
        }

        /// <summary>
        /// 제목 더블클릭으로 편집
        /// </summary>
        private void lblTitle_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var newTitle = Microsoft.VisualBasic.Interaction.InputBox(
                    "노트 제목을 입력하세요:", 
                    "제목 편집", 
                    lblTitle.Text);
                
                if (!string.IsNullOrEmpty(newTitle))
                {
                    lblTitle.Text = newTitle;
                    _logger.Info($"제목 변경: {newTitle}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("제목 편집 실패", ex);
            }
        }

        /// <summary>
        /// 노트 목록 표시
        /// </summary>
        private void ShowNoteList()
        {
            try
            {
                var noteListForm = new NoteListForm();
                noteListForm.ShowDialog(this);
                _logger.Info("노트 목록 표시");
            }
            catch (Exception ex)
            {
                _logger.Error("노트 목록 표시 실패", ex);
            }
        }

        /// <summary>
        /// 색상 테마 변경
        /// </summary>
        private void ChangeColorTheme()
        {
            try
            {
                _config.NextColorTheme();
                ApplyColorTheme();
                _logger.Info($"색상 테마 변경: {_config.ColorTheme}");
            }
            catch (Exception ex)
            {
                _logger.Error("색상 테마 변경 실패", ex);
            }
        }

        /// <summary>
        /// 색상 테마 적용
        /// </summary>
        private void ApplyColorTheme()
        {
            try
            {
                var backgroundColor = _config.GetThemeBackgroundColor();
                this.BackColor = backgroundColor;
                txtNote.BackColor = backgroundColor;
                pnlTitleBar.BackColor = backgroundColor;
                opacitySlider.BackColor = backgroundColor;
                _logger.Info($"색상 테마 적용: {_config.ColorTheme}");
            }
            catch (Exception ex)
            {
                _logger.Error("색상 테마 적용 실패", ex);
            }
        }

        /// <summary>
        /// 색상 변경 메뉴 클릭
        /// </summary>
        private void menuColorChange_Click(object? sender, EventArgs e)
        {
            ChangeColorTheme();
        }

        /// <summary>
        /// 폼 종료 시 최종 저장
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                // 자동저장 타이머 중지
                autoSaveTimer.Stop();

                // 최종 저장
                SaveNote();
                SaveConfig();

                _logger.Info("StickyLite 종료");
            }
            catch (Exception ex)
            {
                _logger.Error("종료 중 오류", ex);
            }
            finally
            {
                // NoteManager에서 제거
                NoteManager.RemoveNote(_noteId);
            }

            base.OnFormClosing(e);
        }
    }
}
