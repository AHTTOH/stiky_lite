using System.Text;
using System.Windows.Forms;

namespace StickyLite.Storage
{
    /// <summary>
    /// 노트 파일 저장/로드 관리
    /// </summary>
    public class NoteStorage
    {
        private readonly string _notePath;
        private const int MaxFileSizeBytes = 1024 * 1024; // 1MB
        private const int MaxRetryCount = 3;

        public NoteStorage(string notePath)
        {
            _notePath = notePath;
        }

        /// <summary>
        /// 노트 내용 로드
        /// </summary>
        public string LoadNote()
        {
            try
            {
                if (!File.Exists(_notePath))
                {
                    return string.Empty;
                }

                // 파일 크기 체크
                var fileInfo = new FileInfo(_notePath);
                if (fileInfo.Length > MaxFileSizeBytes)
                {
                    System.Diagnostics.Debug.WriteLine($"경고: 노트 파일이 1MB를 초과합니다 ({fileInfo.Length} bytes)");
                }

                return File.ReadAllText(_notePath, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"노트 로드 실패: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// 노트 내용 저장 (원자적 저장)
        /// </summary>
        public bool SaveNote(string content)
        {
            if (content == null)
            {
                content = string.Empty;
            }

            // 내용 크기 체크
            var contentBytes = Encoding.UTF8.GetByteCount(content);
            if (contentBytes > MaxFileSizeBytes)
            {
                System.Diagnostics.Debug.WriteLine($"경고: 노트 내용이 1MB를 초과합니다 ({contentBytes} bytes)");
            }

            var tempPath = _notePath + ".tmp";
            var retryCount = 0;

            while (retryCount < MaxRetryCount)
            {
                try
                {
                    // 임시 파일에 쓰기
                    File.WriteAllText(tempPath, content, Encoding.UTF8);

                    // 원자적 교체
                    if (File.Exists(_notePath))
                    {
                        File.Replace(tempPath, _notePath, null);
                    }
                    else
                    {
                        File.Move(tempPath, _notePath);
                    }

                    return true;
                }
                catch (IOException ex) when (retryCount < MaxRetryCount - 1)
                {
                    // 파일 잠금 등으로 인한 IOException은 재시도
                    retryCount++;
                    System.Diagnostics.Debug.WriteLine($"저장 재시도 {retryCount}/{MaxRetryCount}: {ex.Message}");
                    
                    // 잠시 대기 후 재시도
                    Thread.Sleep(100 * retryCount);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"노트 저장 실패: {ex.Message}");
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// 노트 파일 존재 여부
        /// </summary>
        public bool NoteExists()
        {
            return File.Exists(_notePath);
        }

        /// <summary>
        /// 노트 파일 크기 (바이트)
        /// </summary>
        public long GetNoteSize()
        {
            try
            {
                if (File.Exists(_notePath))
                {
                    return new FileInfo(_notePath).Length;
                }
            }
            catch
            {
                // 파일 접근 실패 시 0 반환
            }
            return 0;
        }

        /// <summary>
        /// RTF 형식으로 노트 저장
        /// </summary>
        public bool SaveNoteAsRtf(RichTextBox richTextBox)
        {
            try
            {
                var rtfPath = Path.ChangeExtension(_notePath, ".rtf");
                var tempPath = rtfPath + ".tmp";

                // RTF 내용을 임시 파일에 저장
                richTextBox.SaveFile(tempPath, RichTextBoxStreamType.RichText);

                // 파일 크기 확인
                var fileInfo = new FileInfo(tempPath);
                if (fileInfo.Length > MaxFileSizeBytes)
                {
                    File.Delete(tempPath);
                    return false;
                }

                // 원자적 교체
                if (File.Exists(rtfPath))
                {
                    File.Delete(rtfPath);
                }
                File.Move(tempPath, rtfPath);

                // 기존 .txt 파일이 있다면 삭제 (형식 변경)
                var txtPath = Path.ChangeExtension(_notePath, ".txt");
                if (File.Exists(txtPath))
                {
                    File.Delete(txtPath);
                }

                return true;
            }
            catch (Exception ex)
            {
                // 임시 파일 정리
                var tempPath = Path.ChangeExtension(_notePath, ".rtf.tmp");
                if (File.Exists(tempPath))
                {
                    try { File.Delete(tempPath); } catch { }
                }
                throw new Exception($"RTF 저장 실패: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// RTF 형식으로 노트 로드
        /// </summary>
        public void LoadNoteToRtf(RichTextBox richTextBox)
        {
            try
            {
                var rtfPath = Path.ChangeExtension(_notePath, ".rtf");
                var txtPath = Path.ChangeExtension(_notePath, ".txt");

                if (File.Exists(rtfPath))
                {
                    // RTF 파일이 있으면 RTF로 로드
                    richTextBox.LoadFile(rtfPath, RichTextBoxStreamType.RichText);
                }
                else if (File.Exists(txtPath))
                {
                    // RTF 파일이 없고 TXT 파일이 있으면 TXT로 로드
                    var content = File.ReadAllText(txtPath, Encoding.UTF8);
                    richTextBox.Text = content;
                }
                else
                {
                    // 파일이 없으면 빈 텍스트
                    richTextBox.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"RTF 로드 실패: {ex.Message}", ex);
            }
        }
    }
}
