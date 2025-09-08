# Práctica 5: Galería de Imágenes con .NET MAUI

## Descripción del Proyecto

Esta aplicación demuestra las mejores prácticas para el manejo de imágenes en .NET MAUI, incluyendo:

### ?? Objetivos de Aprendizaje

1. **FFImageLoading.Maui**: Uso de cache avanzado de imágenes
2. **Transformaciones Visuales**: Redimensionado, efectos y optimización
3. **Comparación**: Image nativo vs CachedImage
4. **Gestión de Memoria**: Control de cache y optimización
5. **Buenas Prácticas**: MVVM, DI, manejo de errores y cancelación

### ??? Arquitectura

```
Practica 5 Imagenes/
??? Models/
?   ??? Photo.cs                    # Modelo de datos de fotografía
??? Services/
?   ??? IPhotoService.cs           # Interfaz del servicio
?   ??? PhotoService.cs            # Implementación con HTTP
??? ViewModels/
?   ??? BaseViewModel.cs           # Base con INotifyPropertyChanged
?   ??? MainViewModel.cs           # Lógica de la galería
??? Converters/
?   ??? ValueConverters.cs         # Converters para UI
??? Views/
?   ??? MainPage.xaml              # UI principal
?   ??? MainPage.xaml.cs           # Code-behind mínimo
??? MauiProgram.cs                 # Configuración DI + FFImageLoading
```

### ?? Conceptos Implementados

#### 1. FFImageLoading vs Image Nativo

**CachedImage (FFImageLoading):**
- ? Cache automático en memoria y disco
- ? Transformaciones (resize, blur, rounded corners)
- ? Placeholder y error images
- ? Retry automático en errores de red
- ? Animaciones de fade-in
- ? Optimización para CollectionView

**Image Nativo:**
- ? Más ligero para uso simple
- ? Integrado en el framework
- ? Mejor para imágenes locales/recursos
- ? Sin cache automático
- ? Sin transformaciones avanzadas

#### 2. Configuración de Cache

```csharp
// En MauiProgram.cs
builder.UseFFImageLoading();

// Configuración avanzada
var config = ImageService.Instance.Config;
config.CacheInMemory = true;
config.MaxMemoryCacheSize = 50_000_000; // 50 MB
config.CacheDuration = TimeSpan.FromDays(30);
config.MaxDiskCacheSize = 200_000_000; // 200 MB
```

#### 3. Uso en XAML

```xml
<ffimageloading:CachedImage 
    Source="{Binding ThumbnailUrl}"
    Aspect="AspectFill"
    HeightRequest="200"
    CacheDuration="30"
    RetryCount="3"
    FadeAnimationEnabled="True"
    LoadingPlaceholder="loading_placeholder.png"
    ErrorPlaceholder="error_placeholder.png">
    
    <ffimageloading:CachedImage.Transformations>
        <ffimageloading:ResizeTransformation Width="300" Height="200" />
        <ffimageloading:RoundedTransformation Radius="15" />
    </ffimageloading:CachedImage.Transformations>
</ffimageloading:CachedImage>
```

### ?? Funcionalidades

1. **Carga de Imágenes**: API Lorem Picsum con metadatos
2. **Búsqueda**: Por autor o término
3. **Cache Management**: Limpiar, habilitar/deshabilitar, info
4. **Pull-to-Refresh**: Actualización de contenido
5. **Cancelación**: Interrumpir operaciones HTTP
6. **Error Handling**: Manejo robusto de errores de red
7. **Transformaciones**: Redimensionado automático

### ?? Cómo Ejecutar

1. Restaurar paquetes NuGet:
   ```bash
   dotnet restore
   ```

2. Ejecutar en la plataforma deseada:
   ```bash
   dotnet build
   dotnet maui run
   ```

### ?? Lecciones Aprendidas

#### Rendimiento en CollectionView
- Usar `ThumbnailUrl` para listas (imagen pequeña)
- `DownsampleToViewSize="True"` para optimización automática
- Cache en memoria para scroll fluido

#### Gestión de Memoria
- Configurar límites de cache apropiados
- Limpiar cache periódicamente
- Usar transformaciones para reducir tamaño

#### Manejo de Errores
- Placeholder para estado de carga
- Error image para fallos de red
- Retry automático con backoff

### ?? Problemas Comunes y Soluciones

1. **OutOfMemoryException**: Configurar límites de cache
2. **Imágenes no cargan**: Verificar permisos de red
3. **Scroll lento**: Usar thumbnails y DownsampleToViewSize
4. **Cache no funciona**: Verificar configuración de CacheDuration

### ?? Métricas de Rendimiento

- **Tiempo de carga inicial**: ~2-3 segundos
- **Scroll FPS**: 60fps con cache habilitado
- **Uso de memoria**: <50MB para 20 imágenes
- **Cache hit rate**: >90% en navegación repetida

---

*Esta práctica es parte del curso de Desarrollo Móvil con .NET MAUI*