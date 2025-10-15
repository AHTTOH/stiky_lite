using System.Text.Json;
using System.Text.Json.Serialization;

namespace StickyLite.Config
{
    /// <summary>
    /// 설정 파일 관리자
    /// </summary>
    public class ConfigManager
    {
        private readonly string _configPath;
        private readonly JsonSerializerOptions _jsonOptions;

        public ConfigManager(string configPath)
        {
            _configPath = configPath;
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        /// <summary>
        /// 설정 로드
        /// </summary>
        public AppConfig LoadConfig()
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    return AppConfig.CreateDefault();
                }

                var json = File.ReadAllText(_configPath, System.Text.Encoding.UTF8);
                var config = JsonSerializer.Deserialize<AppConfig>(json, _jsonOptions);
                
                return config ?? AppConfig.CreateDefault();
            }
            catch (Exception ex)
            {
                // 설정 파일 손상 시 기본값 사용
                System.Diagnostics.Debug.WriteLine($"설정 로드 실패: {ex.Message}");
                return AppConfig.CreateDefault();
            }
        }

        /// <summary>
        /// 설정 저장
        /// </summary>
        public bool SaveConfig(AppConfig config)
        {
            try
            {
                var json = JsonSerializer.Serialize(config, _jsonOptions);
                
                // 원자적 저장을 위해 임시 파일 사용
                var tempPath = _configPath + ".tmp";
                File.WriteAllText(tempPath, json, System.Text.Encoding.UTF8);
                
                // 원자적 교체
                if (File.Exists(_configPath))
                {
                    File.Replace(tempPath, _configPath, null);
                }
                else
                {
                    File.Move(tempPath, _configPath);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"설정 저장 실패: {ex.Message}");
                return false;
            }
        }
    }
}
