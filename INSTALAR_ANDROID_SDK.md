# Instalación de Android SDK para .NET MAUI - Guía Paso a Paso

## 📱 ¿Por qué necesitas el Android SDK?

Para compilar aplicaciones .NET MAUI para Android, necesitas tener instalado el Android SDK (Software Development Kit). Este contiene todas las herramientas, APIs y librerías necesarias para construir, probar y depurar aplicaciones Android.

---

## ✅ Opción 1: Instalación Rápida con Visual Studio (RECOMENDADO)

Esta es la forma más sencilla y recomendada:

### Pasos:

1. **Abre Visual Studio 2022**

2. **Ve a Tools → Get Tools and Features** (Herramientas → Obtener herramientas y características)

3. **En la pestaña "Workloads"**, asegúrate de tener marcado:
   - ✅ **.NET Multi-platform App UI development** (Desarrollo de aplicaciones de interfaz de usuario multiplataforma de .NET)

4. **En la pestaña "Individual components"**, busca y marca:
   - ✅ **Android SDK setup (API level 35)** o superior
   - ✅ **Android SDK build-tools**
   - ✅ **Android Emulator**

5. **Haz clic en "Modify"** o "Install" y espera a que se complete la instalación (puede tardar 10-30 minutos dependiendo de tu conexión)

6. **Reinicia Visual Studio** después de la instalación

---

## ✅ Opción 2: Instalación Manual con Command Line Tools

Si prefieres instalar manualmente o no usas Visual Studio:

### Paso 1: Descargar Android Command Line Tools

1. Ve a: https://developer.android.com/studio#command-line-tools-only
2. Descarga: **Command Line Tools for Windows** (aproximadamente 150 MB)
3. Guarda el archivo ZIP en tu carpeta de descargas

### Paso 2: Extraer y Configurar

1. **Crea la carpeta del SDK**:
   ```
   C:\Users\TU_USUARIO\AppData\Local\Android\Sdk
   ```

2. **Crea la subcarpeta para las herramientas**:
   ```
   C:\Users\TU_USUARIO\AppData\Local\Android\Sdk\cmdline-tools
   ```

3. **Extrae el ZIP descargado** en la carpeta `cmdline-tools`

4. **Renombra** la carpeta extraída `cmdline-tools` a `latest`
   - Ruta final: `C:\Users\TU_USUARIO\AppData\Local\Android\Sdk\cmdline-tools\latest`

### Paso 3: Configurar Variables de Entorno

1. **Abre "Editar las variables de entorno del sistema"**:
   - Presiona `Win + R`
   - Escribe: `sysdm.cpl`
   - Ve a la pestaña "Opciones avanzadas"
   - Haz clic en "Variables de entorno"

2. **En "Variables de usuario"**, haz clic en "Nueva" y agrega:
   - **Nombre**: `ANDROID_HOME`
   - **Valor**: `C:\Users\TU_USUARIO\AppData\Local\Android\Sdk`

3. **Agrega otra variable**:
   - **Nombre**: `ANDROID_SDK_ROOT`
   - **Valor**: `C:\Users\TU_USUARIO\AppData\Local\Android\Sdk`

4. **Edita la variable "Path"**:
   - Selecciona "Path" y haz clic en "Editar"
   - Haz clic en "Nuevo" y agrega:
     - `%ANDROID_HOME%\platform-tools`
     - `%ANDROID_HOME%\cmdline-tools\latest\bin`

5. **Haz clic en "Aceptar"** en todas las ventanas

### Paso 4: Instalar Componentes del SDK

1. **Abre PowerShell o CMD como Administrador**

2. **Navega a la carpeta de tu proyecto**:
   ```powershell
   cd C:\Users\TU_USUARIO\source\repos\Cout-Nico-F\MauiAppsDamII
   ```

3. **Ejecuta los siguientes comandos uno por uno**:

   ```powershell
   # Aceptar licencias
   sdkmanager --licenses
   # (Escribe 'y' y presiona Enter para cada licencia)

   # Instalar platform para Android 35
   sdkmanager "platforms;android-35"

   # Instalar build-tools
   sdkmanager "build-tools;35.0.0"

   # Instalar platform-tools
   sdkmanager "platform-tools"

   # Instalar emulador (opcional, pero recomendado)
   sdkmanager "emulator"

   # Instalar system image para el emulador (opcional)
   sdkmanager "system-images;android-35;google_apis;x86_64"
   ```

4. **Espera a que se completen todas las descargas** (puede tardar 10-20 minutos)

### Paso 5: Verificar la Instalación

1. **Cierra y vuelve a abrir PowerShell** (para cargar las nuevas variables de entorno)

2. **Ejecuta**:
   ```powershell
   sdkmanager --list
   ```
   
3. **Deberías ver** una lista de paquetes instalados incluyendo:
   - `platforms;android-35`
   - `build-tools;35.0.0`
   - `platform-tools`

---

## ✅ Opción 3: Instalación Automática con PowerShell

Si tienes experiencia con PowerShell, puedes usar este script automatizado:

```powershell
# Ejecuta esto en PowerShell (como Administrador)
$androidSdkRoot = "$env:LOCALAPPDATA\Android\Sdk"
$cmdlineToolsUrl = "https://dl.google.com/android/repository/commandlinetools-win-11076708_latest.zip"
$tempZip = "$env:TEMP\android-cmdlinetools.zip"

# Crear directorios
New-Item -ItemType Directory -Force -Path $androidSdkRoot\cmdline-tools | Out-Null

# Descargar
Write-Host "Descargando Android Command Line Tools..."
Invoke-WebRequest -Uri $cmdlineToolsUrl -OutFile $tempZip

# Extraer
Write-Host "Extrayendo archivos..."
Expand-Archive -Path $tempZip -DestinationPath $androidSdkRoot\cmdline-tools -Force
Move-Item "$androidSdkRoot\cmdline-tools\cmdline-tools" "$androidSdkRoot\cmdline-tools\latest" -Force

# Configurar variables de entorno
[Environment]::SetEnvironmentVariable("ANDROID_HOME", $androidSdkRoot, "User")
[Environment]::SetEnvironmentVariable("ANDROID_SDK_ROOT", $androidSdkRoot, "User")
$path = [Environment]::GetEnvironmentVariable("Path", "User")
[Environment]::SetEnvironmentVariable("Path", "$path;$androidSdkRoot\platform-tools;$androidSdkRoot\cmdline-tools\latest\bin", "User")

# Actualizar variables en sesión actual
$env:ANDROID_HOME = $androidSdkRoot
$env:ANDROID_SDK_ROOT = $androidSdkRoot
$env:Path += ";$androidSdkRoot\platform-tools;$androidSdkRoot\cmdline-tools\latest\bin"

Write-Host "Instalando componentes del SDK..."
$sdkmanager = "$androidSdkRoot\cmdline-tools\latest\bin\sdkmanager.bat"

# Aceptar licencias y instalar
echo "y" | & $sdkmanager --licenses
& $sdkmanager "platforms;android-35"
& $sdkmanager "build-tools;35.0.0"
& $sdkmanager "platform-tools"
& $sdkmanager "emulator"
& $sdkmanager "system-images;android-35;google_apis;x86_64"

Write-Host "Instalación completada!" -ForegroundColor Green
Write-Host "Reinicia tu terminal para aplicar los cambios."
```

---

## 🧪 Probar la Compilación para Android

Una vez instalado el SDK, prueba compilar tu proyecto:

```powershell
# Compila para Android
dotnet build "Practica 1\MauiFirstApp.csproj" -f net9.0-android35.0

# Si todo funciona, deberías ver:
# "Build succeeded"
```

---

## ⚠️ Problemas Comunes

### Error: "The Android SDK directory could not be found"
**Solución**: 
- Verifica que las variables de entorno estén configuradas correctamente
- Reinicia Visual Studio / PowerShell después de configurar las variables
- Ejecuta: `echo $env:ANDROID_HOME` para verificar la variable

### Error: "sdkmanager: command not found"
**Solución**:
- Cierra y vuelve a abrir PowerShell/CMD
- Verifica que la ruta esté en la variable PATH
- Ejecuta el sdkmanager con la ruta completa:
  ```
  C:\Users\TU_USUARIO\AppData\Local\Android\Sdk\cmdline-tools\latest\bin\sdkmanager.bat --list
  ```

### Error: "License not accepted"
**Solución**:
```powershell
sdkmanager --licenses
# Escribe 'y' para cada licencia
```

### La compilación es muy lenta
**Es normal**: La primera compilación para Android puede tardar 5-10 minutos porque:
- Descarga dependencias de Xamarin.Android
- Compila recursos de Android
- Optimiza el código

Las compilaciones siguientes serán mucho más rápidas (30-60 segundos).

---

## 📚 Recursos Adicionales

- **Documentación oficial de .NET MAUI para Android**: https://learn.microsoft.com/dotnet/maui/android/
- **Android Developer Site**: https://developer.android.com/
- **Troubleshooting**: https://aka.ms/dotnet-android-install-sdk

---

## 💡 Recomendación

Para tus alumnos, la **Opción 1 (Visual Studio)** es la más recomendada porque:
- ✅ Es más sencilla
- ✅ Gestiona las actualizaciones automáticamente
- ✅ Incluye herramientas visuales para gestionar emuladores
- ✅ Menor probabilidad de errores de configuración

---

**Última actualización**: 2025 - Para .NET 9 y Android SDK 35
