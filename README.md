# StickyLite

간단하고 가벼운 Windows용 스티키 노트 애플리케이션

## 🌟 주요 기능

- ✅ **간편한 메모 작성**: 빠르고 직관적인 텍스트 입력
- ✅ **자동 저장**: 입력 즉시 자동으로 저장
- ✅ **창 이동**: 제목바를 드래그하여 자유롭게 이동
- ✅ **창 크기 조절**: 테두리를 드래그하여 크기 조절
- ✅ **10가지 파스텔 색상**: Ctrl+C 또는 우클릭 메뉴로 색상 변경
- ✅ **제목 편집**: 제목을 더블클릭하여 편집
- ✅ **투명도 조절**: 제목바의 슬라이더로 투명도 조절
- ✅ **새 노트 생성**: ➕ 버튼으로 여러 노트 관리
- ✅ **노트 목록**: 📋 버튼으로 모든 노트 확인
- ✅ **항상 위**: 📌 버튼으로 창을 항상 최상단에 고정

## 📦 다운로드

### 포터블 버전 (추가 설치 불필요)
**파일**: [StickyLite_Portable_net9_x64.zip](./StickyLite_Portable_net9_x64.zip)
- 크기: ~46MB
- .NET 런타임 포함 (별도 설치 불필요)
- Windows 10/11 지원

## 🚀 사용 방법

1. **ZIP 파일 다운로드**
2. **압축 해제** (원하는 위치에)
3. **StickyLite.exe 실행**

그게 전부입니다! 추가 설치나 설정이 필요 없습니다.

## ⌨️ 키보드 단축키

- `Ctrl + T`: 항상 위 토글
- `Ctrl + C`: 색상 변경
- `Ctrl + S`: 수동 저장
- `Ctrl + Q`: 닫기
- `제목 더블클릭`: 제목 편집

## 🎨 색상 테마

10가지 파스텔 색상을 지원합니다:
- 연노랑 (기본)
- 라벤더 (연보라)
- 허니듀 (연초록)
- 스카이블루 (연파랑)
- 미스트로즈 (연분홍)
- 베이지
- 코른실크 (연크림)
- 베이비블루 (연하늘색)
- 카키 (연갈색)
- 플럼 (연자주)

## 🛠️ 빌드 방법

### 요구사항
- .NET 9.0 SDK
- Windows 10/11

### 빌드 명령
```powershell
# PowerShell에서 실행
.\build.ps1
```

또는 수동 빌드:
```powershell
dotnet publish src\StickyLite\StickyLite.csproj -c Release -f net9.0-windows -r win-x64 --self-contained true
```

## 📝 라이선스

이 프로젝트는 MIT 라이선스 하에 배포됩니다. 자세한 내용은 [LICENSE](LICENSE) 파일을 참조하세요.

## 🔒 보안

보안 관련 문제는 [SECURITY.md](SECURITY.md)를 참조하세요.

## 💡 기여

이슈와 풀 리퀘스트를 환영합니다!

## 📸 스크린샷

![StickyLite Screenshot](https://via.placeholder.com/600x400?text=StickyLite+Screenshot)
*(스크린샷을 추가해주세요)*

## 🙏 감사의 말

Windows Forms와 .NET 9.0을 사용하여 제작되었습니다.

