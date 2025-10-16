using System.IO;

namespace StickyLite.Storage
{
    /// <summary>
    /// 경로 관리자 - 포터블 경로와 폴백 경로 처리
    /// </summary>
    public class PathManager
    {
        private readonly string _basePath;
        private readonly string _fallbackPath;
        private readonly string _dataPath;
        private readonly string _logsPath;
        private bool _usingFallback = false;

        public PathManager()
        {
            // 실행 파일이 있는 디렉토리
            _basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? "";
            
            // 포터블 경로
            _dataPath = Path.Combine(_basePath, "data");
            _logsPath = Path.Combine(_basePath, "logs");
            
            // 폴백 경로 (읽기 전용 환경용)
            _fallbackPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "StickyLite");
        }

        /// <summary>
        /// 데이터 디렉토리 경로 (읽기/쓰기 가능한 경로)
        /// </summary>
        public string DataPath
        {
            get
            {
                EnsureDirectoryExists(_dataPath);
                return _dataPath;
            }
        }

        /// <summary>
        /// 로그 디렉토리 경로 (읽기/쓰기 가능한 경로)
        /// </summary>
        public string LogsPath
        {
            get
            {
                EnsureDirectoryExists(_logsPath);
                return _logsPath;
            }
        }

        /// <summary>
        /// 설정 파일 경로
        /// </summary>
        public string ConfigPath => Path.Combine(DataPath, "config.json");

        /// <summary>
        /// 노트 파일 경로 (기본)
        /// </summary>
        public string NotePath => Path.Combine(DataPath, "note.txt");

        /// <summary>
        /// 특정 노트 ID의 파일 경로
        /// </summary>
        public string GetNotePath(string noteId)
        {
            if (noteId == "default")
            {
                return NotePath; // 기존 호환성
            }
            
            var notesDir = Path.Combine(DataPath, "notes");
            EnsureDirectoryExists(notesDir);
            return Path.Combine(notesDir, $"{noteId}.txt");
        }

        /// <summary>
        /// 로그 파일 경로
        /// </summary>
        public string LogPath => Path.Combine(LogsPath, "app.log");

        /// <summary>
        /// 폴백 사용 여부
        /// </summary>
        public bool IsUsingFallback => _usingFallback;

        /// <summary>
        /// 디렉토리 존재 확인 및 생성, 권한 문제 시 폴백 경로 사용
        /// </summary>
        private void EnsureDirectoryExists(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // 쓰기 권한 테스트
                var testFile = Path.Combine(path, "test_write.tmp");
                File.WriteAllText(testFile, "test");
                File.Delete(testFile);
            }
            catch (Exception)
            {
                // 포터블 경로에 쓰기 권한이 없으면 폴백 경로 사용
                if (!_usingFallback && path != _fallbackPath)
                {
                    _usingFallback = true;
                    EnsureDirectoryExists(_fallbackPath);
                }
                else
                {
                    throw; // 폴백 경로도 실패하면 예외 발생
                }
            }
        }

        /// <summary>
        /// 경로 정보를 로그용 문자열로 반환
        /// </summary>
        public string GetPathInfo()
        {
            if (_usingFallback)
            {
                return $"폴백 경로 사용: {_fallbackPath} (원본: {_basePath})";
            }
            else
            {
                return $"포터블 경로 사용: {_basePath}";
            }
        }
    }
}
