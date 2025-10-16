using System.Text;

namespace StickyLite.Logging
{
    /// <summary>
    /// 간단한 파일 로거 (회전 기능 포함)
    /// </summary>
    public class SimpleLogger
    {
        private readonly string _logPath;
        private readonly object _lockObject = new object();
        private const long MaxLogSizeBytes = 256 * 1024; // 256KB
        private const int MaxBackupFiles = 2;

        public SimpleLogger(string logPath)
        {
            _logPath = logPath;
        }

        /// <summary>
        /// 정보 로그
        /// </summary>
        public void Info(string message)
        {
            WriteLog("INFO", message);
        }

        /// <summary>
        /// 경고 로그
        /// </summary>
        public void Warning(string message)
        {
            WriteLog("WARN", message);
        }

        /// <summary>
        /// 오류 로그
        /// </summary>
        public void Error(string message)
        {
            WriteLog("ERROR", message);
        }

        /// <summary>
        /// 예외 로그
        /// </summary>
        public void Error(string message, Exception ex)
        {
            var fullMessage = $"{message}: {ex.Message}";
            if (ex.InnerException != null)
            {
                fullMessage += $" (Inner: {ex.InnerException.Message})";
            }
            WriteLog("ERROR", fullMessage);
        }

        /// <summary>
        /// 디버그 로그
        /// </summary>
        public void Debug(string message)
        {
            WriteLog("DEBUG", message);
        }

        /// <summary>
        /// 로그 쓰기 (스레드 안전)
        /// </summary>
        private void WriteLog(string level, string message)
        {
            lock (_lockObject)
            {
                try
                {
                    var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}{Environment.NewLine}";
                    
                    // 로그 파일 크기 체크 및 회전
                    RotateLogIfNeeded();
                    
                    // 로그 쓰기
                    File.AppendAllText(_logPath, logEntry, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    // 로그 쓰기 실패 시 디버그 출력으로 대체
                    System.Diagnostics.Debug.WriteLine($"로그 쓰기 실패: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 로그 파일 회전 (크기 초과 시)
        /// </summary>
        private void RotateLogIfNeeded()
        {
            try
            {
                if (!File.Exists(_logPath))
                {
                    return;
                }

                var fileInfo = new FileInfo(_logPath);
                if (fileInfo.Length <= MaxLogSizeBytes)
                {
                    return;
                }

                // 기존 백업 파일들을 한 칸씩 뒤로 이동
                for (int i = MaxBackupFiles - 1; i >= 1; i--)
                {
                    var oldBackup = $"{_logPath}.{i}";
                    var newBackup = $"{_logPath}.{i + 1}";
                    
                    if (File.Exists(oldBackup))
                    {
                        if (File.Exists(newBackup))
                        {
                            File.Delete(newBackup);
                        }
                        File.Move(oldBackup, newBackup);
                    }
                }

                // 현재 로그를 .1로 이동
                var firstBackup = $"{_logPath}.1";
                if (File.Exists(firstBackup))
                {
                    File.Delete(firstBackup);
                }
                File.Move(_logPath, firstBackup);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"로그 회전 실패: {ex.Message}");
            }
        }

        /// <summary>
        /// 로그 파일 삭제 (정리용)
        /// </summary>
        public void ClearLogs()
        {
            lock (_lockObject)
            {
                try
                {
                    if (File.Exists(_logPath))
                    {
                        File.Delete(_logPath);
                    }

                    for (int i = 1; i <= MaxBackupFiles; i++)
                    {
                        var backupPath = $"{_logPath}.{i}";
                        if (File.Exists(backupPath))
                        {
                            File.Delete(backupPath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"로그 정리 실패: {ex.Message}");
                }
            }
        }
    }
}
