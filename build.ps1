# StickyLite 빌드 스크립트
# 두 가지 타겟으로 빌드: net6.0-windows (self-contained) 및 net48 (x86)

param(
    [switch]$Clean,
    [switch]$SkipBuild,
    [switch]$SkipZip,
    [switch]$SkipHash
)

$ErrorActionPreference = "Stop"

# 경로 설정
$ProjectPath = "src\StickyLite\StickyLite.csproj"
$BuildDir = "build"
$HashesDir = "hashes"

# 빌드 타겟 설정
$BuildTargets = @(
    @{
        Framework = "net9.0-windows"
        Runtime = "win-x64"
        SelfContained = $true
        OutputDir = "StickyLite_Portable_net9_x64"
        ZipName = "StickyLite_Portable_net9_x64.zip"
    }
)

Write-Host "=== StickyLite 빌드 시작 ===" -ForegroundColor Green

# 정리
if ($Clean) {
    Write-Host "빌드 디렉토리 정리 중..." -ForegroundColor Yellow
    if (Test-Path $BuildDir) {
        Remove-Item $BuildDir -Recurse -Force
    }
    if (Test-Path $HashesDir) {
        Remove-Item $HashesDir -Recurse -Force
    }
}

# 디렉토리 생성
New-Item -ItemType Directory -Path $BuildDir -Force | Out-Null
New-Item -ItemType Directory -Path $HashesDir -Force | Out-Null

# 빌드 실행
if (-not $SkipBuild) {
    foreach ($target in $BuildTargets) {
        Write-Host "`n빌드 중: $($target.Framework) ($($target.Runtime))" -ForegroundColor Cyan
        
        $outputPath = Join-Path $BuildDir $target.OutputDir
        $publishArgs = @(
            "publish"
            $ProjectPath
            "-c", "Release"
            "-f", $target.Framework
            "-o", $outputPath
        )
        
        if ($target.SelfContained) {
            $publishArgs += @("-r", $target.Runtime, "--self-contained", "true")
        } else {
            $publishArgs += @("-r", $target.Runtime)
        }
        
        try {
            & dotnet @publishArgs
            if ($LASTEXITCODE -ne 0) {
                throw "빌드 실패: $($target.Framework)"
            }
            Write-Host "빌드 완료: $($target.OutputDir)" -ForegroundColor Green
        }
        catch {
            Write-Error "빌드 실패: $($target.Framework) - $_"
            exit 1
        }
    }
}

# ZIP 압축
if (-not $SkipZip) {
    Write-Host "`nZIP 압축 중..." -ForegroundColor Cyan
    
    foreach ($target in $BuildTargets) {
        $sourceDir = Join-Path $BuildDir $target.OutputDir
        $zipPath = Join-Path $BuildDir $target.ZipName
        
        if (Test-Path $zipPath) {
            Remove-Item $zipPath -Force
        }
        
        try {
            Compress-Archive -Path "$sourceDir\*" -DestinationPath $zipPath -Force
            Write-Host "압축 완료: $($target.ZipName)" -ForegroundColor Green
        }
        catch {
            Write-Error "압축 실패: $($target.ZipName) - $_"
            exit 1
        }
    }
}

# SHA256 해시 계산
if (-not $SkipHash) {
    Write-Host "`nSHA256 해시 계산 중..." -ForegroundColor Cyan
    
    $hashFile = Join-Path $HashesDir "sha256.txt"
    $hashContent = @()
    $hashContent += "# StickyLite SHA256 해시"
    $hashContent += "# 생성일시: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
    $hashContent += ""
    
    # 실행 파일 해시
    foreach ($target in $BuildTargets) {
        $exePath = Join-Path $BuildDir (Join-Path $target.OutputDir "StickyLite.exe")
        if (Test-Path $exePath) {
            $hash = Get-FileHash $exePath -Algorithm SHA256
            $hashContent += "$($hash.Hash)  $($target.OutputDir)\StickyLite.exe"
            Write-Host "EXE 해시: $($target.OutputDir)\StickyLite.exe" -ForegroundColor Green
        }
    }
    
    $hashContent += ""
    
    # ZIP 파일 해시
    foreach ($target in $BuildTargets) {
        $zipPath = Join-Path $BuildDir $target.ZipName
        if (Test-Path $zipPath) {
            $hash = Get-FileHash $zipPath -Algorithm SHA256
            $hashContent += "$($hash.Hash)  $($target.ZipName)"
            Write-Host "ZIP 해시: $($target.ZipName)" -ForegroundColor Green
        }
    }
    
    try {
        $hashContent | Out-File -FilePath $hashFile -Encoding UTF8
        Write-Host "해시 파일 생성: $hashFile" -ForegroundColor Green
    }
    catch {
        Write-Error "해시 파일 생성 실패: $_"
        exit 1
    }
}

# 빌드 결과 요약
Write-Host "`n=== 빌드 완료 ===" -ForegroundColor Green
Write-Host "빌드 디렉토리: $BuildDir" -ForegroundColor White
Write-Host "해시 파일: $HashesDir\sha256.txt" -ForegroundColor White

Write-Host "`n생성된 파일:" -ForegroundColor Yellow
foreach ($target in $BuildTargets) {
    $outputDir = Join-Path $BuildDir $target.OutputDir
    $zipFile = Join-Path $BuildDir $target.ZipName
    
    if (Test-Path $outputDir) {
        Write-Host "  - $($target.OutputDir)\" -ForegroundColor White
    }
    if (Test-Path $zipFile) {
        Write-Host "  - $($target.ZipName)" -ForegroundColor White
    }
}

Write-Host "`n사용법:" -ForegroundColor Yellow
Write-Host "  1. ZIP 파일을 원하는 위치에 압축 해제" -ForegroundColor White
Write-Host "  2. StickyLite.exe 더블클릭으로 실행" -ForegroundColor White
Write-Host "  3. 해시 확인: Get-FileHash StickyLite.exe -Algorithm SHA256" -ForegroundColor White
