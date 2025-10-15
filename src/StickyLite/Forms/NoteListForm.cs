using System.Windows.Forms;
using StickyLite.Core;

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
            this.TopMost = true;

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
    }
}
