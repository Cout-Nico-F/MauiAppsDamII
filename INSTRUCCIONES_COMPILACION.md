# Instrucciones de Compilación - Proyectos MAUI

## 📋 Resumen de la Situación

Los proyectos han sido **actualizados para .NET 9** y están listos para compilar. Se realizaron las siguientes correcciones:

### ✅ Correcciones Aplicadas:

1. **Versiones de plataforma especificadas**:
   - Cambio de `net9.0-android` → `net9.0-android35.0`
   - Cambio de `net9.0-ios` → `net9.0-ios18.0`
   - Cambio de `net9.0-maccatalyst` → `net9.0-maccatalyst18.0`

2. **Versión de Microsoft.Maui.Controls fijada**:
   - Se reemplazó `$(MauiVersion)` por `9.0.0` en todos los proyectos

3. **Workloads de MAUI instaladas**:
   - Se instalaron las workloads necesarias para .NET MAUI

## 🚀 Compilación Exitosa

### Para compilar todo el proyecto MauiFirstApp (Practica 1):

```powershell
# Solo para Windows (recomendado para desarrollo inicial)
dotnet build "Practica 1\MauiFirstApp.csproj" -f net9.0-windows10.0.19041.0
```

### Para compilar otros proyectos:

```powershell
# Practica 2
dotnet build "Practica 2\MauiApp2MVVM.csproj" -f net9.0-windows10.0.19041.0

# Practica 3
dotnet build "Practica 3\Practica3MVVM.csproj" -f net9.0-windows10.0.19041.0

# Practica 4
dotnet build "Practica4sinDI\Practica4sinDI.csproj" -f net9.0-windows10.0.19041.0

# Practica 5
dotnet build "Practica 5 Imagenes\Practica 5 Imagenes.csproj" -f net9.0-windows10.0.19041.0

# Practica 6
dotnet build "Practica6Shell\Practica6Shell.csproj" -f net9.0-windows10.0.19041.0

# Practica 7
dotnet build "Practica 7\QuizApp\QuizApp.csproj" -f net9.0-windows10.0.19041.0

# Practica 9
dotnet build "Practica 9\Practica 9.csproj" -f net9.0-windows10.0.19041.0

# HTTPmaui
dotnet build "HTTPmaui\HTTPmaui.csproj" -f net9.0-windows10.0.19041.0

# Persistencia1
dotnet build "Persistencia1\Persistencia1.csproj" -f net9.0-windows10.0.19041.0
```

## 📱 Compilación para Otras Plataformas

### Android
Para compilar para Android, se necesita:
1. **Android SDK** instalado
2. Ejecutar: 
   ```powershell
   dotnet build "Practica 1\MauiFirstApp.csproj" -f net9.0-android35.0
   ```

### iOS / MacCatalyst
Para compilar para iOS o Mac, se necesita:
1. **Un Mac con Xcode** instalado
2. Conexión a un Mac mediante Visual Studio
3. Ejecutar:
   ```powershell
   dotnet build "Practica 1\MauiFirstApp.csproj" -f net9.0-ios18.0
   dotnet build "Practica 1\MauiFirstApp.csproj" -f net9.0-maccatalyst18.0
   ```

## 🔧 Solución de Problemas

### Error: "The Android SDK directory could not be found"
- **Causa**: No tienes Android SDK instalado
- **Solución**: Instala Android SDK siguiendo: https://aka.ms/dotnet-android-install-sdk
- **O compila solo para Windows** usando el parámetro `-f net9.0-windows10.0.19041.0`

### Error: "NETSDK1147: workloads must be installed"
- **Causa**: Faltan workloads de MAUI
- **Solución**: Ejecuta:
  ```powershell
  dotnet workload install maui
  ```

### Error: "MSB4018: GetLatestMSVCVersion task failed"
- **Causa**: Problema con Visual Studio SDK (ruta incorrecta)
- **Solución**: Este error solo afecta a algunas plataformas. Compila para Windows específicamente.

## 💡 Recomendaciones para tus Alumnos

1. **Para comenzar**: Compilen solo para **Windows** (es más rápido y no requiere SDKs adicionales)
2. **Android**: Instalen Android SDK solo si necesitan probar en dispositivos Android
3. **iOS/Mac**: Solo necesario si tienen un Mac disponible
4. **Visual Studio**: Usen Visual Studio 2022 (versión 17.8 o superior) para mejor integración
5. **Línea de comandos**: Las compilaciones desde la terminal son más rápidas para desarrollo

## 📝 Cambios Realizados en los Archivos .csproj

Todos los archivos `.csproj` fueron actualizados con:

```xml
<!-- Antes (NO FUNCIONA en .NET 9) -->
<TargetFrameworks>net9.0-android;net9.0-ios;net9.0-maccatalyst</TargetFrameworks>
<PackageReference Include="Microsoft.Maui.Controls" Version="$(MauiVersion)" />

<!-- Después (CORRECTO para .NET 9) -->
<TargetFrameworks>net9.0-android35.0;net9.0-ios18.0;net9.0-maccatalyst18.0</TargetFrameworks>
<PackageReference Include="Microsoft.Maui.Controls" Version="9.0.0" />
```

## 🎓 Para el Profesor

Estos proyectos fueron creados en un cuatrimestre anterior y **requieren actualizaciones** cuando se usan en nuevas versiones de .NET. Los cambios principales fueron:

1. Especificar versiones de plataforma (obligatorio en .NET 9)
2. Especificar versiones de paquetes NuGet explícitamente
3. Instalar workloads actualizadas

**Recomendación**: Mantén un proyecto "template" actualizado cada semestre para que los alumnos partan de una base funcional.

---

## ✅ Estado Actual

- ✅ Todos los proyectos compilan correctamente para **Windows**
- ⚠️ Android requiere Android SDK
- ⚠️ iOS/Mac requieren hardware Mac
- ✅ Todos los archivos .csproj están actualizados
- ✅ Workloads de MAUI instaladas
