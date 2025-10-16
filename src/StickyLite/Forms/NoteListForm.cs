using System.Windows.Forms;
using StickyLite.Core;
using System.Drawing;
using System.Drawing.Imaging;

namespace StickyLite.Forms
{
    /// <summary>
    /// 노트 목록 폼
    /// </summary>
    public partial class NoteListForm : Form
    {
        private ListView listView;
        private Button btnClose;
        private Button btnRefresh;

        public NoteListForm()
        {
            InitializeComponent();
            LoadNotes();
        }

        private void InitializeComponent()
        {
            this.Text = "노트 목록";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Icon = CreateStickyNoteIcon();

            // ListView 초기화
            listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };

            listView.Columns.Add("제목", 150);
            listView.Columns.Add("내용 미리보기", 150);
            listView.Columns.Add("위치", 80);
            listView.Columns.Add("상태", 60);

            // 버튼들
            btnRefresh = new Button
            {
                Text = "새로고침",
                Size = new Size(80, 30),
                Location = new Point(10, 10)
            };
            btnRefresh.Click += BtnRefresh_Click;

            btnClose = new Button
            {
                Text = "닫기",
                Size = new Size(80, 30),
                Location = new Point(310, 10)
            };
            btnClose.Click += BtnClose_Click;

            // 패널
            var panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50
            };
            panel.Controls.Add(btnRefresh);
            panel.Controls.Add(btnClose);

            // 컨트롤 추가
            this.Controls.Add(listView);
            this.Controls.Add(panel);

            // 이벤트
            listView.DoubleClick += ListView_DoubleClick;
        }

        private void LoadNotes()
        {
            listView.Items.Clear();

            var notes = NoteManager.GetAllNotes();
            foreach (var kvp in notes)
            {
                var noteId = kvp.Key;
                var note = kvp.Value;

                var item = new ListViewItem(note.NoteTitle);
                var preview = note.NotePreview.Length > 30 ? note.NotePreview.Substring(0, 30) + "..." : note.NotePreview;
                item.SubItems.Add(preview);
                item.SubItems.Add($"({note.Location.X}, {note.Location.Y})");
                item.SubItems.Add(note.TopMost ? "항상 위" : "일반");
                item.Tag = note;

                listView.Items.Add(item);
            }

            // 노트가 없으면 메시지 표시
            if (listView.Items.Count == 0)
            {
                var item = new ListViewItem("활성 노트가 없습니다.");
                item.SubItems.Add("");
                item.SubItems.Add("");
                item.SubItems.Add("");
                listView.Items.Add(item);
            }
        }

        private void ListView_DoubleClick(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                var selectedItem = listView.SelectedItems[0];
                if (selectedItem.Tag is MainForm note)
                {
                    note.BringToFront();
                    note.Activate();
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadNotes();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    
                    // 포스트잇 노란색 배경 (#FFE66D)
                    var stickyColor = Color.FromArgb(255, 230, 109);
                    var shadowColor = Color.FromArgb(200, 180, 80);
                    
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
                    using (var borderPen = new Pen(Color.FromArgb(180, 160, 60), 1))
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
                    using (var foldBrush = new SolidBrush(Color.FromArgb(220, 200, 90)))
                    {
                        graphics.FillPolygon(foldBrush, foldPoints);
                    }
                    using (var foldPen = new Pen(Color.FromArgb(180, 160, 60), 1))
                    {
                        graphics.DrawPolygon(foldPen, foldPoints);
                    }
                    
                    // 간단한 라인 (텍스트 효과)
                    using (var linePen = new Pen(Color.FromArgb(160, 140, 50), 1))
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
