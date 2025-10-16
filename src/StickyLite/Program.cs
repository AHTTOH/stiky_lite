using System.Diagnostics;
using System.Runtime.InteropServices;
using StickyLite.Resources;

namespace StickyLite
{
    /// <summary>
    /// StickyLite 메인 진입점
    /// </summary>
    internal static class Program
    {
        #region Win32 API
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        private const int SW_RESTORE = 9;
        #endregion

        private static readonly string MutexName = "StickyLite_SingleInstance_Mutex";
        private static Mutex? _singleInstanceMutex;

        /// <summary>
        /// 애플리케이션의 메인 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // 아이콘 생성 모드 확인
            if (args.Length > 0 && args[0] == "--generate-icon")
            {
                try
                {
                    var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Resources", "app.ico");
                    IconGenerator.CreateIconFile(iconPath);
                    Console.WriteLine($"아이콘 파일이 생성되었습니다: {iconPath}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"아이콘 생성 실패: {ex.Message}");
                    return;
                }
            }

            // 단일 인스턴스 체크
            if (!CheckSingleInstance())
            {
                return; // 이미 실행 중인 인스턴스가 있음
            }

            try
            {
                // High DPI 설정
                Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // 전역 예외 처리 설정
                SetupGlobalExceptionHandling();

                // 첫 번째 노트 생성
                var firstNote = new MainForm();
                firstNote.Show();

                // 메시지 루프 시작 (특정 폼에 종속되지 않음)
                Application.Run();
            }
            catch (Exception ex)
            {
                // 치명적 오류 처리
                HandleFatalError(ex);
            }
            finally
            {
                // 리소스 정리
                Cleanup();
            }
        }

        /// <summary>
        /// 단일 인스턴스 체크
        /// </summary>
        private static bool CheckSingleInstance()
        {
            try
            {
                _singleInstanceMutex = new Mutex(true, MutexName, out bool createdNew);

                if (!createdNew)
                {
                    // 이미 실행 중인 인스턴스가 있음
                    ActivateExistingInstance();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // Mutex 생성 실패 시에도 계속 진행 (권한 문제 등)
                Debug.WriteLine($"Mutex 생성 실패: {ex.Message}");
                return true;
            }
        }

        /// <summary>
        /// 기존 인스턴스 활성화
        /// </summary>
        private static void ActivateExistingInstance()
        {
            try
            {
                var currentProcess = Process.GetCurrentProcess();
                var processes = Process.GetProcessesByName(currentProcess.ProcessName);

                foreach (var process in processes)
                {
                    if (process.Id != currentProcess.Id && process.MainWindowHandle != IntPtr.Zero)
                    {
                        // 창이 최소화되어 있으면 복원
                        if (IsIconic(process.MainWindowHandle))
                        {
                            ShowWindow(process.MainWindowHandle, SW_RESTORE);
                        }

                        // 창을 앞으로 가져오기
                        SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"기존 인스턴스 활성화 실패: {ex.Message}");
            }
        }

        /// <summary>
        /// 전역 예외 처리 설정
        /// </summary>
        private static void SetupGlobalExceptionHandling()
        {
            // UI 스레드 예외 처리
            Application.ThreadException += (sender, e) =>
            {
                HandleException("UI 스레드 예외", e.Exception);
            };

            // 비UI 스레드 예외 처리
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                HandleException("비UI 스레드 예외", e.ExceptionObject as Exception);
            };
        }

        /// <summary>
        /// 예외 처리
        /// </summary>
        private static void HandleException(string source, Exception? ex)
        {
            try
            {
                var message = ex?.Message ?? "알 수 없는 오류";
                Debug.WriteLine($"[{source}] {message}");
                
                if (ex != null)
                {
                    Debug.WriteLine($"스택 트레이스: {ex.StackTrace}");
                }

                // 로그 파일에 기록 시도 (PathManager가 초기화되지 않았을 수 있음)
                try
                {
                    var logPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) ?? "", "logs", "app.log");
                    var logDir = Path.GetDirectoryName(logPath);
                    if (logDir != null && !Directory.Exists(logDir))
                    {
                        Directory.CreateDirectory(logDir);
                    }
                    
                    var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [ERROR] [{source}] {message}{Environment.NewLine}";
                    File.AppendAllText(logPath, logEntry);
                }
                catch
                {
                    // 로그 기록 실패는 무시
                }
            }
            catch
            {
                // 예외 처리 중 예외 발생 시 무시
            }
        }

        /// <summary>
        /// 치명적 오류 처리
        /// </summary>
        private static void HandleFatalError(Exception ex)
        {
            try
            {
                var message = $"치명적 오류가 발생했습니다:\n\n{ex.Message}\n\n애플리케이션이 종료됩니다.";
                
                MessageBox.Show(message, "StickyLite 오류", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                // MessageBox도 실패하면 무시
            }
        }

        /// <summary>
        /// 리소스 정리
        /// </summary>
        private static void Cleanup()
        {
            try
            {
                _singleInstanceMutex?.ReleaseMutex();
                _singleInstanceMutex?.Dispose();
            }
            catch
            {
                // 정리 실패는 무시
            }
        }
    }
}