using System.Drawing;
using StickyLite.Config;

namespace StickyLite.Forms
{
    /// <summary>
    /// 색상 프리셋 선택 다이얼로그
    /// </summary>
    public partial class ColorPresetDialog : Form
    {
        private TableLayoutPanel colorPanel;
        private Button btnOK;
        private Button btnCancel;
        private Label lblTitle;
        private int selectedColorIndex = -1;

        public int SelectedColorIndex => selectedColorIndex;

        public ColorPresetDialog()
        {
            InitializeComponent();
            LoadColorPresets();
        }

        private void InitializeComponent()
        {
            this.Text = "색상 프리셋 선택";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Icon = CreateStickyNoteIcon();

            // 제목 라벨
            lblTitle = new Label
            {
                Text = "원하는 색상을 선택하세요:",
                Font = new Font("맑은 고딕", 10F, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(350, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // 색상 패널 (5x2 그리드)
            colorPanel = new TableLayoutPanel
            {
                Location = new Point(20, 60),
                Size = new Size(350, 120),
                ColumnCount = 5,
                RowCount = 2,
                BackColor = SystemColors.Control
            };

            // 그리드 설정
            for (int i = 0; i < 5; i++)
            {
                colorPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            }
            for (int i = 0; i < 2; i++)
            {
                colorPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            }

            // 버튼들
            btnOK = new Button
            {
                Text = "확인",
                Size = new Size(80, 30),
                Location = new Point(220, 200),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += BtnOK_Click;

            btnCancel = new Button
            {
                Text = "취소",
                Size = new Size(80, 30),
                Location = new Point(310, 200),
                DialogResult = DialogResult.Cancel
            };

            // 컨트롤 추가
            this.Controls.Add(lblTitle);
            this.Controls.Add(colorPanel);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);

            // 포커스 설정
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }

        private void LoadColorPresets()
        {
            var colorNames = new string[]
            {
                "연노랑", "라벤더", "허니듀", "스카이블루", "미스트로즈",
                "베이지", "코른실크", "베이비블루", "카키", "플럼"
            };

            for (int i = 0; i < AppConfig.PastelColors.Length; i++)
            {
                var colorButton = new Button
                {
                    Text = colorNames[i],
                    Font = new Font("맑은 고딕", 8F),
                    FlatStyle = FlatStyle.Flat,
                    Tag = i,
                    Margin = new Padding(2)
                };

                // 색상 설정
                var color = ColorTranslator.FromHtml(AppConfig.PastelColors[i]);
                colorButton.BackColor = color;
                colorButton.ForeColor = GetContrastColor(color);
                colorButton.FlatAppearance.BorderSize = 2;
                colorButton.FlatAppearance.BorderColor = Color.Gray;

                // 클릭 이벤트
                colorButton.Click += ColorButton_Click;

                // 그리드 위치 계산
                int row = i / 5;
                int col = i % 5;
                colorPanel.Controls.Add(colorButton, col, row);
            }
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is int index)
            {
                // 이전 선택 해제
                foreach (Control control in colorPanel.Controls)
                {
                    if (control is Button btn)
                    {
                        btn.FlatAppearance.BorderColor = Color.Gray;
                        btn.FlatAppearance.BorderSize = 2;
                    }
                }

                // 새 선택 표시
                button.FlatAppearance.BorderColor = Color.Black;
                button.FlatAppearance.BorderSize = 3;
                selectedColorIndex = index;
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (selectedColorIndex == -1)
            {
                MessageBox.Show("색상을 선택해주세요.", "알림", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        /// <summary>
        /// 배경색에 대비되는 텍스트 색상 반환
        /// </summary>
        private Color GetContrastColor(Color backgroundColor)
        {
            // 밝기 계산 (0.299*R + 0.587*G + 0.114*B)
            var brightness = (backgroundColor.R * 0.299 + backgroundColor.G * 0.587 + backgroundColor.B * 0.114);
            return brightness > 128 ? Color.Black : Color.White;
        }

        /// <summary>
        /// 포스트잇 스타일 아이콘 생성
        /// </summary>
        private Icon CreateStickyNoteIcon()
        {
            try
            {
                // 32x32 비트맵 생성
                using (var bitmap = new Bitmap(32, 32))
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    // 배경을 투명하게 설정
                    graphics.Clear(Color.Transparent);
                    
                    // 포스트잇 색상
                    var stickyColor = Color.FromArgb(255, 230, 109); // #FFE66D
                    var shadowColor = Color.FromArgb(200, 180, 80);
                    var foldColor = Color.FromArgb(220, 200, 90);
                    var borderColor = Color.FromArgb(180, 160, 60);
                    var lineColor = Color.FromArgb(160, 140, 50);
                    
                    // 그림자 효과 (오른쪽 하단)
                    var shadowRect = new Rectangle(2, 2, 28, 28);
                    using (var shadowBrush = new SolidBrush(shadowColor))
                    {
                        graphics.FillRectangle(shadowBrush, shadowRect);
                    }
                    
                    // 메인 포스트잇
                    var mainRect = new Rectangle(0, 0, 28, 28);
                    using (var stickyBrush = new SolidBrush(stickyColor))
                    {
                        graphics.FillRectangle(stickyBrush, mainRect);
                    }
                    
                    // 테두리
                    using (var borderPen = new Pen(borderColor, 1))
                    {
                        graphics.DrawRectangle(borderPen, mainRect);
                    }
                    
                    // 상단 접힌 부분 (작은 삼각형)
                    var foldPoints = new Point[]
                    {
                        new Point(20, 0),
                        new Point(28, 0),
                        new Point(28, 8)
                    };
                    using (var foldBrush = new SolidBrush(foldColor))
                    {
                        graphics.FillPolygon(foldBrush, foldPoints);
                    }
                    using (var foldPen = new Pen(borderColor, 1))
                    {
                        graphics.DrawPolygon(foldPen, foldPoints);
                    }
                    
                    // 간단한 라인 (텍스트 효과)
                    using (var linePen = new Pen(lineColor, 1))
                    {
                        graphics.DrawLine(linePen, 4, 8, 24, 8);
                        graphics.DrawLine(linePen, 4, 12, 20, 12);
                        graphics.DrawLine(linePen, 4, 16, 22, 16);
                        graphics.DrawLine(linePen, 4, 20, 18, 20);
                    }
                    
                    // 비트맵을 아이콘으로 변환
                    var hIcon = bitmap.GetHicon();
                    return Icon.FromHandle(hIcon);
                }
            }
            catch
            {
                // 아이콘 생성 실패 시 기본 아이콘 반환
                return SystemIcons.Application;
            }
        }
    }
}
