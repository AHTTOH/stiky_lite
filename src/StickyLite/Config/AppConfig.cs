using System.Drawing;

namespace StickyLite.Config
{
    /// <summary>
    /// 애플리케이션 설정 모델
    /// </summary>
    public class AppConfig
    {
        // 창 위치 및 크기
        public int WindowX { get; set; } = 100;
        public int WindowY { get; set; } = 100;
        public int WindowWidth { get; set; } = 300;
        public int WindowHeight { get; set; } = 200;

        // 창 상태
        public bool TopMost { get; set; } = true;
        public double Opacity { get; set; } = 0.9;

        // 폰트 설정
        public string FontFamily { get; set; } = "맑은 고딕";
        public float FontSize { get; set; } = 10.0f;
        public bool FontBold { get; set; } = false;

        // 색상 설정
        public string BackgroundColor { get; set; } = "#FFFFE0"; // 연노랑
        public string TextColor { get; set; } = "#000000"; // 검정
        public int ColorTheme { get; set; } = 0; // 색상 테마 인덱스

        // 자동저장 설정
        public int AutoSaveDelayMs { get; set; } = 1200; // 1.2초

        /// <summary>
        /// 기본 설정으로 초기화
        /// </summary>
        public static AppConfig CreateDefault()
        {
            return new AppConfig();
        }

        /// <summary>
        /// 색상 문자열을 Color로 변환
        /// </summary>
        public Color GetBackgroundColor()
        {
            try
            {
                return ColorTranslator.FromHtml(BackgroundColor);
            }
            catch
            {
                return Color.FromArgb(255, 255, 224); // 연노랑 기본값
            }
        }

        /// <summary>
        /// 색상 문자열을 Color로 변환
        /// </summary>
        public Color GetTextColor()
        {
            try
            {
                return ColorTranslator.FromHtml(TextColor);
            }
            catch
            {
                return Color.Black; // 검정 기본값
            }
        }

        /// <summary>
        /// Font 객체 생성
        /// </summary>
        public Font GetFont()
        {
            try
            {
                var fontStyle = FontBold ? FontStyle.Bold : FontStyle.Regular;
                return new Font(FontFamily, FontSize, fontStyle);
            }
            catch
            {
                return new Font("맑은 고딕", 10.0f, FontStyle.Regular);
            }
        }

        /// <summary>
        /// 파스텔 색상 테마 배열
        /// </summary>
        public static readonly string[] PastelColors = new string[]
        {
            "#FFFFE0", // 연노랑 (기본)
            "#E6E6FA", // 라벤더 (연보라)
            "#F0FFF0", // 허니듀 (연초록)
            "#E0F6FF", // 스카이블루 (연파랑)
            "#FFE4E1", // 미스트로즈 (연분홍)
            "#F5F5DC", // 베이지
            "#FFF8DC", // 코른실크 (연크림)
            "#E6F3FF", // 베이비블루 (연하늘색)
            "#F0E68C", // 카키 (연갈색)
            "#DDA0DD"  // 플럼 (연자주)
        };

        /// <summary>
        /// 현재 테마에 따른 배경색 반환
        /// </summary>
        public Color GetThemeBackgroundColor()
        {
            try
            {
                if (ColorTheme >= 0 && ColorTheme < PastelColors.Length)
                {
                    return ColorTranslator.FromHtml(PastelColors[ColorTheme]);
                }
                return ColorTranslator.FromHtml(PastelColors[0]);
            }
            catch
            {
                return Color.FromArgb(255, 255, 224); // 연노랑 기본값
            }
        }

        /// <summary>
        /// 다음 색상 테마로 변경
        /// </summary>
        public void NextColorTheme()
        {
            ColorTheme = (ColorTheme + 1) % PastelColors.Length;
            BackgroundColor = PastelColors[ColorTheme];
        }

        /// <summary>
        /// 특정 색상 테마로 설정
        /// </summary>
        public void SetColorTheme(int themeIndex)
        {
            if (themeIndex >= 0 && themeIndex < PastelColors.Length)
            {
                ColorTheme = themeIndex;
                BackgroundColor = PastelColors[themeIndex];
            }
        }
    }
}
