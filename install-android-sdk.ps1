# Script para instalar Android SDK para .NET MAUI
# Este script descarga e instala el Android SDK necesario para compilar aplicaciones MAUI

Write-Host "=== Instalador de Android SDK para .NET MAUI ===" -ForegroundColor Green
Write-Host ""

# Definir rutas
$androidSdkRoot = "$env:LOCALAPPDATA\Android\Sdk"
$commandLineToolsUrl = "https://dl.google.com/android/repository/commandlinetools-win-11076708_latest.zip"
$tempZip = "$env:TEMP\android-commandlinetools.zip"
$cmdlineToolsPath = "$androidSdkRoot\cmdline-tools"
$latestPath = "$cmdlineToolsPath\latest"

Write-Host "1. Creando directorio para Android SDK..." -ForegroundColor Yellow
New-Item -ItemType Directory -Force -Path $androidSdkRoot | Out-Null
New-Item -ItemType Directory -Force -Path $cmdlineToolsPath | Out-Null

Write-Host "2. Descargando Android Command Line Tools..." -ForegroundColor Yellow
Write-Host "   Esto puede tardar unos minutos..." -ForegroundColor Gray
try {
    Invoke-WebRequest -Uri $commandLineToolsUrl -OutFile $tempZip -UseBasicParsing
    Write-Host "   ✓ Descarga completada" -ForegroundColor Green
} catch {
    Write-Host "   ✗ Error al descargar: $_" -ForegroundColor Red
    exit 1
}

Write-Host "3. Extrayendo Command Line Tools..." -ForegroundColor Yellow
try {
    Expand-Archive -Path $tempZip -DestinationPath $cmdlineToolsPath -Force
    
    # Renombrar la carpeta 'cmdline-tools' a 'latest' si existe
    $extractedPath = "$cmdlineToolsPath\cmdline-tools"
    if (Test-Path $extractedPath) {
        if (Test-Path $latestPath) {
            Remove-Item -Path $latestPath -Recurse -Force
        }
        Move-Item -Path $extractedPath -Destination $latestPath
    }
    
    Write-Host "   ✓ Extracción completada" -ForegroundColor Green
} catch {
    Write-Host "   ✗ Error al extraer: $_" -ForegroundColor Red
    exit 1
}

# Limpiar archivo temporal
Remove-Item -Path $tempZip -Force

Write-Host "4. Configurando variables de entorno..." -ForegroundColor Yellow
[System.Environment]::SetEnvironmentVariable("ANDROID_HOME", $androidSdkRoot, [System.EnvironmentVariableTarget]::User)
[System.Environment]::SetEnvironmentVariable("ANDROID_SDK_ROOT", $androidSdkRoot, [System.EnvironmentVariableTarget]::User)

# Actualizar PATH
$currentPath = [System.Environment]::GetEnvironmentVariable("Path", [System.EnvironmentVariableTarget]::User)
$platformToolsPath = "$androidSdkRoot\platform-tools"
$cmdlineToolsBinPath = "$latestPath\bin"

if ($currentPath -notlike "*$platformToolsPath*") {
    [System.Environment]::SetEnvironmentVariable("Path", "$currentPath;$platformToolsPath;$cmdlineToolsBinPath", [System.EnvironmentVariableTarget]::User)
}

# Actualizar variables de entorno en la sesión actual
$env:ANDROID_HOME = $androidSdkRoot
$env:ANDROID_SDK_ROOT = $androidSdkRoot
$env:Path += ";$platformToolsPath;$cmdlineToolsBinPath"

Write-Host "   ✓ Variables de entorno configuradas" -ForegroundColor Green

Write-Host "5. Instalando componentes necesarios del SDK..." -ForegroundColor Yellow
Write-Host "   Esto puede tardar varios minutos. Por favor, espera..." -ForegroundColor Gray

$sdkmanager = "$latestPath\bin\sdkmanager.bat"

# Aceptar licencias automáticamente
Write-Host "   Aceptando licencias..." -ForegroundColor Gray
$licenseInput = "y`ny`ny`ny`ny`ny`ny`n"
$licenseInput | & $sdkmanager --licenses 2>&1 | Out-Null

# Instalar componentes necesarios
Write-Host "   Instalando Android SDK Platform 35..." -ForegroundColor Gray
& $sdkmanager "platforms;android-35" 2>&1 | Out-Null

Write-Host "   Instalando Android SDK Build-Tools 35..." -ForegroundColor Gray
& $sdkmanager "build-tools;35.0.0" 2>&1 | Out-Null

Write-Host "   Instalando Platform Tools..." -ForegroundColor Gray
& $sdkmanager "platform-tools" 2>&1 | Out-Null

Write-Host "   Instalando Emulator..." -ForegroundColor Gray
& $sdkmanager "emulator" 2>&1 | Out-Null

Write-Host "   Instalando System Images..." -ForegroundColor Gray
& $sdkmanager "system-images;android-35;google_apis;x86_64" 2>&1 | Out-Null

Write-Host "   Componentes instalados correctamente" -ForegroundColor Green

Write-Host ""
Write-Host "=== Instalación Completada ===" -ForegroundColor Green
Write-Host ""
Write-Host "Android SDK instalado en: $androidSdkRoot" -ForegroundColor Cyan
Write-Host ""
Write-Host "⚠️  IMPORTANTE: Cierra y vuelve a abrir tu terminal/PowerShell" -ForegroundColor Yellow
Write-Host "   para que las variables de entorno se actualicen correctamente." -ForegroundColor Yellow
Write-Host ""
Write-Host "Después, puedes compilar tu proyecto con:" -ForegroundColor White
Write-Host '  dotnet build "Practica 1\MauiFirstApp.csproj" -f net9.0-android35.0' -ForegroundColor Cyan
Write-Host ""
