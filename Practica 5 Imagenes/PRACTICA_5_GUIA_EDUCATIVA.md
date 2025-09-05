# Práctica 5: Manejo Avanzado de Imágenes en .NET MAUI

## ?? Objetivos de Aprendizaje

Esta práctica demuestra las **mejores prácticas** para el manejo de imágenes en aplicaciones móviles, incluyendo:

### Conceptos Clave
1. **FFImageLoading.Maui** - Librería avanzada para cache de imágenes
2. **Transformaciones Visuales** - Redimensionado, efectos y optimización
3. **Cache Management** - Control de memoria y rendimiento
4. **Comparación Image vs CachedImage** - Cuándo usar cada uno
5. **Optimización en CollectionView** - Rendimiento en listas grandes
6. **Arquitectura MVVM con DI** - Buenas prácticas acumuladas

## ??? Arquitectura del Proyecto

```
Practica 5 Imagenes/
??? ?? Models/
?   ??? Photo.cs                    # Modelo de datos con metadatos de imagen
??? ?? Services/
?   ??? IPhotoService.cs           # Interfaz del servicio (para testing)
?   ??? PhotoService.cs            # HTTP service con Lorem Picsum API
??? ?? ViewModels/
?   ??? BaseViewModel.cs           # Base con INotifyPropertyChanged
?   ??? MainViewModel.cs           # Lógica de la galería y cache
??? ?? Converters/
?   ??? ValueConverters.cs         # Converters para UI (string?bool, filesize)
??? ?? Views/
    ??? MainPage.xaml              # UI con CachedImage y comparación
    ??? MainPage.xaml.cs           # Code-behind mínimo con DI
```

## ?? Características Implementadas

### 1. FFImageLoading vs Image Nativo

| Característica | CachedImage (FFImageLoading) | Image Nativo |
|----------------|------------------------------|---------------|
| **Cache automático** | ? Memoria + Disco | ? Solo memoria básica |
| **Transformaciones** | ? Resize, blur, rounded | ? Limitado |
| **Placeholder/Error** | ? Configurable | ? Manual |
| **Retry automático** | ? Con backoff | ? Manual |
| **Animaciones** | ? Fade-in, etc. | ? Básico |
| **Performance en listas** | ? Optimizado | ?? Puede ser lento |
| **Tamaño del bundle** | ?? +2MB aprox | ? Incluido en framework |
| **Uso recomendado** | Listas, imágenes remotas | Iconos, recursos locales |

### 2. Configuración de Cache

```csharp
// En MainViewModel - Configuración automática via FFImageLoading
<ffimageloading:CachedImage 
    Source="{Binding ThumbnailUrl}"
    CacheDuration="30"              // 30 días en cache
    RetryCount="3"                  // 3 reintentos en error
    FadeAnimationEnabled="True"     // Animación suave
    DownsampleToViewSize="True">    // Optimización de memoria
```

### 3. Transformaciones Visuales

```xml
<ffimageloading:CachedImage.Transformations>
    <!-- Redimensionar manteniendo proporción -->
    <ffimageloading:ResizeTransformation Width="300" Height="200" />
    
    <!-- Esquinas redondeadas (opcional) -->
    <ffimageloading:RoundedTransformation Radius="15" />
    
    <!-- Blur effect (opcional) -->
    <ffimageloading:BlurTransformation Radius="5" />
</ffimageloading:CachedImage.Transformations>
```

## ?? Gestión de Cache

### Operaciones Disponibles
- **Limpiar Cache**: `ImageService.Instance.InvalidateCacheAsync()`
- **Toggle Cache**: Habilitar/Deshabilitar por usuario
- **Info Cache**: Estadísticas de uso y estado

### Configuración Recomendada
```csharp
// En MauiProgram.cs se configura automáticamente:
builder.UseFFImageLoading();

// Configuración por defecto óptima:
// - Cache en memoria: Habilitado
// - Cache en disco: 30 días
// - Retry: 3 intentos con delay
// - Animaciones: Fade-in suave
```

## ?? API Utilizada

**Lorem Picsum** (https://picsum.photos/)
- API gratuita de imágenes placeholder
- Metadatos incluidos (autor, dimensiones)
- URLs optimizadas por tamaño
- Perfecta para demos y prototipos

```
Endpoints utilizados:
GET /v2/list?page=1&limit=20     # Lista de fotos con metadatos
GET /id/{id}/info                # Info específica de una foto
GET /id/{id}/300/200            # Thumbnail optimizada
GET /id/{id}/800/600            # Imagen completa
```

## ?? Optimizaciones de Rendimiento

### 1. En CollectionView
```csharp
// Usar ThumbnailUrl para listas (300x200 vs 800x600)
public string ThumbnailUrl { get; set; }  // Para listas
public string Url { get; set; }           // Para vista detalle
```

### 2. Downsample Automático
```xml
<!-- Reduce automáticamente la imagen al tamaño del contenedor -->
<ffimageloading:CachedImage DownsampleToViewSize="True" />
```

### 3. Cache Inteligente
- **Memoria**: Para scroll fluido (50MB máximo)
- **Disco**: Para sesiones futuras (30 días, 200MB)
- **Limpieza automática**: Cada 7 días

## ?? UX/UI Patterns

### 1. Estados Visuales
- **Loading**: ActivityIndicator + BackgroundColor gris
- **Error**: Placeholder con mensaje informativo
- **Success**: Fade-in animation suave
- **Empty**: EmptyView con iconos y texto guía

### 2. Pull-to-Refresh
```xml
<RefreshView IsRefreshing="{Binding IsRefreshing}"
             Command="{Binding RefreshCommand}">
    <CollectionView ... />
</RefreshView>
```

### 3. Búsqueda en Tiempo Real
- Entry con binding bidireccional
- SearchCommand con validación
- Cancel support para operaciones lentas

## ?? Testing y Debugging

### Logs Útiles
```csharp
#if DEBUG
builder.Logging.AddDebug();  // Para ver requests HTTP
#endif
```

### Métricas de Performance
- **Scroll FPS**: >60fps con cache
- **Memoria**: <50MB para 20 imágenes
- **Tiempo de carga inicial**: 2-3 segundos
- **Cache hit rate**: >90% en uso repetido

## ?? Buenas Prácticas Aplicadas

### 1. Arquitectura
- ? **MVVM** con separación clara de responsabilidades
- ? **Dependency Injection** para testing y flexibilidad
- ? **Interfaces** para abstraer servicios
- ? **Commands** en lugar de eventos
- ? **Async/Await** con manejo de cancelación

### 2. HTTP Best Practices
- ? **IHttpClientFactory** para pool de conexiones
- ? **CancellationToken** en todos los métodos async
- ? **Retry automático** con backoff exponencial
- ? **Timeout configurado** (30-45 segundos)
- ? **User-Agent** personalizado

### 3. Error Handling
- ? **Try/catch específicos** por tipo de error
- ? **Mensajes user-friendly** (no stack traces)
- ? **Estados visuales** para loading/error/empty
- ? **Logging** estructurado para debugging

### 4. Memory Management
- ? **Dispose** de CancellationTokenSource
- ? **Límites de cache** configurados
- ? **Downsample** automático
- ? **Limpieza periódica** de cache

## ?? Conceptos Avanzados Demostrados

1. **Value Converters** - Para transformar datos en UI
2. **Resource Dictionaries** - Para reutilizar estilos
3. **Data Templates** - Para personalizar ItemTemplates  
4. **Binding Converters** - StringToBool, FileSizeFormatter
5. **Command Parameters** - Para commands complejos
6. **ObservableCollection** - Para listas reactivas
7. **RefreshView** - Para pull-to-refresh nativo

---

## ?? Próximos Pasos

Esta práctica sienta las bases para:
- **Navegación avanzada** (Shell, NavigationPage)
- **Offline-first apps** (SQLite + cache)
- **Sincronización** (online/offline)
- **Performance profiling** (memory, CPU)
- **Testing unitario** (mocking HTTP calls)

*Esta es la Práctica 5 del curso de Desarrollo Móvil con .NET MAUI*