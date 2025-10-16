# StickyLite - 포터블 스티키노트

**설치 없이 바로 실행되는 윈도우 스티키노트 앱**

## 🚀 빠른 시작

1. **압축 해제**: `StickyLite_Portable_net9_x64.zip` 또는 `StickyLite_Portable_net48_x86.zip`을 원하는 위치에 압축 해제
2. **실행**: `StickyLite.exe` 더블클릭
3. **완료**: 즉시 스티키노트 창이 나타납니다!

## ✨ 주요 기능

- **항상 위**: 기본적으로 다른 창 위에 표시
- **포터블**: 설치 없이 실행, 레지스트리 변경 없음
- **자동저장**: 입력 후 1.2초 뒤 자동 저장
- **Alt+드래그**: Alt 키를 누른 상태로 창 이동
- **크기 조절**: 창 모서리를 드래그하여 크기 조절
- **투명도 조절**: 우클릭 메뉴에서 투명도 조절 (70%~100%)
- **폰트 크기**: 우클릭 메뉴에서 폰트 크기 조절

## ⌨️ 단축키

| 단축키 | 기능 |
|--------|------|
| `Ctrl + T` | 항상 위 토글 |
| `Ctrl + S` | 즉시 저장 |
| `Ctrl + Q` | 저장 후 종료 |

## 📁 데이터 저장 위치

### 기본 경로 (포터블)
```
StickyLite.exe가 있는 폴더/
├── data/
│   ├── note.txt      # 노트 내용
│   └── config.json   # 설정 (위치, 크기, 폰트 등)
└── logs/
    └── app.log       # 로그 파일
```

### 폴백 경로 (읽기 전용 환경)
읽기 전용 폴더에서 실행 시 자동으로 다음 경로로 폴백:
```
%LOCALAPPDATA%\StickyLite\
├── data/
│   ├── note.txt
│   └── config.json
└── logs/
    └── app.log
```

## 🔒 보안 및 정책

### AppLocker/WDAC 환경에서의 사용

**해시 허용 방법**:
1. `hashes/sha256.txt` 파일에서 해당 exe의 SHA256 해시 확인
2. IT 관리자에게 다음 정보 전달:

```
프로그램: StickyLite Portable
경로: [압축 해제한 폴더]\StickyLite.exe
해시: [sha256.txt에서 확인한 해시값]
권한: asInvoker (관리자 권한 불필요)
```

**보안 특징**:
- ✅ 네트워크 접근 없음
- ✅ 레지스트리 변경 없음
- ✅ 서비스/작업 스케줄러 생성 없음
- ✅ 자식 프로세스 생성 없음
- ✅ 관리자 권한 불필요 (asInvoker)

## 🗑️ 제거 방법

**완전 제거**: 압축 해제한 폴더를 통째로 삭제하면 끝!

- 포터블 경로 사용 시: 폴더 삭제만으로 완전 제거
- 폴백 경로 사용 시: `%LOCALAPPDATA%\StickyLite\` 폴더도 삭제

## 🔧 빌드 정보

### 두 가지 빌드 제공

1. **net9.0-windows (x64)**: 
   - .NET 9 런타임 포함 (self-contained)
   - 최신 Windows에서 최적 성능
   - 파일 크기: ~50MB

2. **net48 (x86)**:
   - .NET Framework 4.8 의존성
   - Windows 7+ 호환
   - 파일 크기: ~5MB

### 해시 확인

```powershell
# 실행 파일 해시 확인
Get-FileHash StickyLite.exe -Algorithm SHA256

# ZIP 파일 해시 확인  
Get-FileHash StickyLite_Portable_net9_x64.zip -Algorithm SHA256
```

## 🐛 문제 해결

### 실행이 안 될 때
1. **Windows Defender**: 실시간 보호에서 예외 추가
2. **바이러스 백신**: StickyLite.exe를 신뢰할 수 있는 프로그램으로 추가
3. **권한**: 폴더에 쓰기 권한이 있는지 확인

### 데이터가 저장되지 않을 때
- 자동으로 `%LOCALAPPDATA%\StickyLite\`로 폴백됩니다
- 로그 파일(`logs/app.log`)에서 경로 정보 확인 가능

### 창이 보이지 않을 때
- 작업 표시줄에서 StickyLite 아이콘 확인
- `Ctrl + T`로 항상 위 모드 토글
- 다른 모니터에 창이 있을 수 있음

## 📄 라이선스

MIT License - 자유롭게 사용, 수정, 배포 가능

## 🔄 업데이트

- 자동 업데이트 기능 없음
- 새 버전은 수동으로 다운로드하여 교체
- 기존 데이터는 그대로 유지됨

## 🔨 빌드 방법

### 요구사항
- .NET 9.0 SDK 또는 Visual Studio 2022
- Windows 10/11

### 빌드 명령어
```powershell
# PowerShell 실행 정책 설정 (필요시)
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

# 빌드 스크립트 실행
.\build.ps1

# 또는 수동 빌드
dotnet publish "src\StickyLite\StickyLite.csproj" -c Release -f net9.0-windows -r win-x64 --self-contained true -o "build\StickyLite_Portable_net9_x64"
dotnet publish "src\StickyLite\StickyLite.csproj" -c Release -f net48 -r win-x86 -o "build\StickyLite_Portable_net48_x86"
```

### Visual Studio에서 빌드
1. `src\StickyLite.sln` 열기
2. 솔루션 구성: Release
3. 솔루션 플랫폼: Any CPU
4. 빌드 → 솔루션 다시 빌드
