namespace StickyLite.Models
{
    /// <summary>
    /// 노트 메타데이터 모델
    /// </summary>
    public class NoteMetadata
    {
        /// <summary>
        /// 노트 고유 ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 노트 제목 (첫 줄 또는 사용자 지정)
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 생성일시
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 마지막 수정일시
        /// </summary>
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 노트 미리보기 (첫 100자)
        /// </summary>
        public string Preview { get; set; } = string.Empty;

        /// <summary>
        /// 노트 크기 (바이트)
        /// </summary>
        public long Size { get; set; } = 0;

        /// <summary>
        /// 창 위치 X
        /// </summary>
        public int WindowX { get; set; } = 100;

        /// <summary>
        /// 창 위치 Y
        /// </summary>
        public int WindowY { get; set; } = 100;

        /// <summary>
        /// 창 너비
        /// </summary>
        public int WindowWidth { get; set; } = 300;

        /// <summary>
        /// 창 높이
        /// </summary>
        public int WindowHeight { get; set; } = 200;

        /// <summary>
        /// 항상 위 여부
        /// </summary>
        public bool TopMost { get; set; } = true;

        /// <summary>
        /// 투명도 (0.7 ~ 1.0)
        /// </summary>
        public double Opacity { get; set; } = 0.9;

        /// <summary>
        /// 폰트 크기
        /// </summary>
        public float FontSize { get; set; } = 10.0f;

        /// <summary>
        /// 노트 내용 업데이트
        /// </summary>
        public void UpdateContent(string content)
        {
            ModifiedAt = DateTime.Now;
            Size = System.Text.Encoding.UTF8.GetByteCount(content);
            
            // 제목 추출 (첫 줄)
            var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Title = lines.Length > 0 ? lines[0].Trim() : "새 노트";
            
            // 미리보기 생성 (첫 100자)
            Preview = content.Length > 100 ? content.Substring(0, 100) + "..." : content;
        }

        /// <summary>
        /// 창 위치 업데이트
        /// </summary>
        public void UpdateWindowState(int x, int y, int width, int height, bool topMost, double opacity)
        {
            WindowX = x;
            WindowY = y;
            WindowWidth = width;
            WindowHeight = height;
            TopMost = topMost;
            Opacity = opacity;
            ModifiedAt = DateTime.Now;
        }
    }
}
