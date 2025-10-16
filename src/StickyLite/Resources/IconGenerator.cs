using System.Drawing;
using System.Drawing.Imaging;

namespace StickyLite.Resources
{
    /// <summary>
    /// 포스트잇 스타일 아이콘 생성기
    /// </summary>
    public static class IconGenerator
    {
        /// <summary>
        /// 포스트잇 스타일 아이콘을 ICO 파일로 생성
        /// </summary>
        public static void CreateIconFile(string filePath)
        {
            try
            {
                // 여러 크기의 아이콘을 포함한 ICO 파일 생성
                var iconSizes = new int[] { 16, 24, 32, 48, 64, 128, 256 };
                var iconImages = new List<Bitmap>();

                foreach (var size in iconSizes)
                {
                    var bitmap = CreateStickyNoteBitmap(size);
                    iconImages.Add(bitmap);
                }

                // ICO 파일로 저장
                SaveAsIcon(iconImages, filePath);

                // 메모리 정리
                foreach (var bitmap in iconImages)
                {
                    bitmap.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"아이콘 생성 실패: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 지정된 크기의 포스트잇 비트맵 생성
        /// </summary>
        private static Bitmap CreateStickyNoteBitmap(int size)
        {
            var bitmap = new Bitmap(size, size);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                // 고품질 렌더링 설정
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // 배경을 투명하게 설정
                graphics.Clear(Color.Transparent);

                // 포스트잇 색상
                var stickyColor = Color.FromArgb(255, 230, 109); // #FFE66D
                var shadowColor = Color.FromArgb(200, 180, 80);
                var foldColor = Color.FromArgb(220, 200, 90);
                var borderColor = Color.FromArgb(180, 160, 60);
                var lineColor = Color.FromArgb(160, 140, 50);

                // 크기에 따른 비율 계산
                var scale = size / 32.0f;
                var shadowOffset = Math.Max(1, (int)(2 * scale));
                var borderWidth = Math.Max(1, (int)(1 * scale));
                var foldSize = Math.Max(4, (int)(8 * scale));

                // 그림자 효과 (오른쪽 하단)
                var shadowRect = new Rectangle(
                    shadowOffset, 
                    shadowOffset, 
                    size - shadowOffset, 
                    size - shadowOffset
                );
                using (var shadowBrush = new SolidBrush(shadowColor))
                {
                    graphics.FillRectangle(shadowBrush, shadowRect);
                }

                // 메인 포스트잇
                var mainRect = new Rectangle(0, 0, size - shadowOffset, size - shadowOffset);
                using (var stickyBrush = new SolidBrush(stickyColor))
                {
                    graphics.FillRectangle(stickyBrush, mainRect);
                }

                // 테두리
                using (var borderPen = new Pen(borderColor, borderWidth))
                {
                    graphics.DrawRectangle(borderPen, mainRect);
                }

                // 상단 접힌 부분 (작은 삼각형)
                var foldPoints = new Point[]
                {
                    new Point(size - foldSize - shadowOffset, 0),
                    new Point(size - shadowOffset, 0),
                    new Point(size - shadowOffset, foldSize)
                };
                using (var foldBrush = new SolidBrush(foldColor))
                {
                    graphics.FillPolygon(foldBrush, foldPoints);
                }
                using (var foldPen = new Pen(borderColor, borderWidth))
                {
                    graphics.DrawPolygon(foldPen, foldPoints);
                }

                // 텍스트 라인 효과 (크기에 따라 조정)
                if (size >= 16)
                {
                    var lineWidth = Math.Max(1, (int)(1 * scale));
                    var lineSpacing = Math.Max(2, (int)(4 * scale));
                    var startX = Math.Max(2, (int)(4 * scale));
                    var endX = size - Math.Max(4, (int)(8 * scale)) - shadowOffset;
                    var startY = Math.Max(4, (int)(8 * scale));

                    using (var linePen = new Pen(lineColor, lineWidth))
                    {
                        for (int i = 0; i < 4 && startY + i * lineSpacing < size - shadowOffset - 4; i++)
                        {
                            var y = startY + i * lineSpacing;
                            var currentEndX = endX - i * Math.Max(1, (int)(2 * scale)); // 점점 짧아지는 라인
                            graphics.DrawLine(linePen, startX, y, currentEndX, y);
                        }
                    }
                }
            }

            return bitmap;
        }

        /// <summary>
        /// 여러 비트맵을 ICO 파일로 저장
        /// </summary>
        private static void SaveAsIcon(List<Bitmap> bitmaps, string filePath)
        {
            // ICO 파일 헤더 작성
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            using (var writer = new BinaryWriter(fileStream))
            {
                // ICO 파일 헤더
                writer.Write((short)0); // Reserved (0)
                writer.Write((short)1); // Type (1 = icon)
                writer.Write((short)bitmaps.Count); // Count

                // 각 이미지의 헤더 정보 계산
                var imageHeaders = new List<byte[]>();
                var imageData = new List<byte[]>();
                var offset = 6 + (bitmaps.Count * 16); // 헤더 + 이미지 헤더들

                foreach (var bitmap in bitmaps)
                {
                    var size = bitmap.Width;
                    var pngData = BitmapToPngBytes(bitmap);
                    
                    // 이미지 헤더 (16 bytes)
                    var header = new byte[16];
                    header[0] = (byte)size; // Width
                    header[1] = (byte)size; // Height
                    header[2] = 0; // Color palette
                    header[3] = 0; // Reserved
                    header[4] = 1; // Color planes
                    header[5] = 0;
                    header[6] = 32; // Bits per pixel
                    header[7] = 0;
                    BitConverter.GetBytes(pngData.Length).CopyTo(header, 8); // Image size
                    BitConverter.GetBytes(offset).CopyTo(header, 12); // Image offset
                    
                    imageHeaders.Add(header);
                    imageData.Add(pngData);
                    offset += pngData.Length;
                }

                // 헤더들 쓰기
                foreach (var header in imageHeaders)
                {
                    writer.Write(header);
                }

                // 이미지 데이터 쓰기
                foreach (var data in imageData)
                {
                    writer.Write(data);
                }
            }
        }

        /// <summary>
        /// 비트맵을 PNG 바이트 배열로 변환
        /// </summary>
        private static byte[] BitmapToPngBytes(Bitmap bitmap)
        {
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
    }
}
