# ?? MauiAppsDamII - Desarrollo de Aplicaciones Móviles II

## ?? **Workspace de Prácticas con .NET MAUI**

### ??? **Estructura de Proyectos**

| Práctica | Proyecto | Descripción | Estado |
|----------|----------|-------------|--------|
| **Práctica 1** | `MauiFirstApp` | Primera aplicación MAUI básica | ? Completado |
| **Práctica 2** | `MauiApp2MVVM` | Introducción al patrón MVVM | ? Completado |
| **Práctica 3** | `Practica3MVVM` | MVVM avanzado con tareas | ? Completado |
| **HTTP MAUI** | `HTTPmaui` | Consumo de APIs HTTP | ? Completado |
| **Práctica 4** | `Practica4sinDI` | MVVM sin Dependency Injection | ? Completado |
| **Práctica 5** | `Practica 5 Imagenes` | Manejo de imágenes y recursos | ? Completado |
| **Práctica 6** | `Practica6Shell` | **Navegación Shell avanzada** | ? Completado |
| **Práctica 7** | `QuizApp` | **Aplicación de Quiz interactiva** | ? Completado |

---

## ?? **Tecnologías Utilizadas**

- **Framework**: .NET 9 MAUI
- **Lenguaje**: C# 13.0
- **Patrón**: MVVM (Model-View-ViewModel)
- **Navegación**: Shell Navigation
- **DI**: Microsoft Dependency Injection
- **UI**: XAML + Data Binding
- **Plataformas**: Android, iOS, Windows, macOS

---

## ?? **Últimos Proyectos Destacados**

### ?? **Práctica 6: Navegación Shell**
- **Shell Navigation** completa
- **Tabs** + **Flyout Menu**
- **Navegación modal** y con parámetros
- **Arquitectura MVVM** con DI
- **Rutas personalizadas**

**Características:**
- ? 7 páginas interconectadas
- ? Navegación por pestañas
- ? Menú lateral deslizable
- ? Paso de datos entre páginas
- ? Configuración modal

### ?? **Práctica 7: Quiz Interactivo**
- **Sistema de preguntas y respuestas**
- **28 preguntas** en 5 categorías
- **Puntuación automática** y cronómetro
- **Historial de resultados**
- **Interfaz responsive**

**Características:**
- ? Quiz por categorías específicas
- ? Quiz aleatorio mezclado
- ? Sistema de calificación (90%=Excelente, 70%=Muy Bien, etc.)
- ? Feedback inmediato
- ? Arquitectura escalable

**Categorías incluidas:**
- ?? **Programación**: MVVM, .NET MAUI, C# (8 preguntas)
- ?? **Ciencias**: Física, Química, Biología (6 preguntas)  
- ??? **Historia**: Eventos históricos (5 preguntas)
- ?? **Geografía**: Países y capitales (5 preguntas)
- ? **Deportes**: Olimpiadas y reglas (4 preguntas)

---

## ?? **Configuración del Workspace**

### **Solución Principal**
```
DesarrolloMovilesDos.sln
??? HTTPmaui/
??? Practica 1/ (MauiFirstApp)
??? Practica 2/ (MauiApp2MVVM)  
??? Practica 3/ (Practica3MVVM)
??? Practica 5 Imagenes/
??? Practica 7/QuizApp/ ? NUEVO
??? Practica4sinDI/
??? Practica6Shell/ ? NAVEGACIÓN
```

### **Compilación**
```bash
# Compilar toda la solución
dotnet build DesarrolloMovilesDos.sln

# Compilar proyecto específico (Android)
dotnet build "Practica 7/QuizApp/QuizApp.csproj" -f net9.0-android

# Listar proyectos en la solución
dotnet sln DesarrolloMovilesDos.sln list
```

---

## ?? **Conceptos Demostrados**

### **Arquitectura y Patrones**
- ? **MVVM Pattern** completo
- ? **Dependency Injection** con Microsoft.Extensions
- ? **CommunityToolkit.Mvvm** para commands y properties
- ? **Separation of Concerns**
- ? **Async/Await** patterns

### **Navegación MAUI**
- ? **Shell Navigation** avanzada
- ? **Tab Navigation** (pestañas inferiores)
- ? **Flyout Navigation** (menú lateral)
- ? **Modal Navigation** (overlay)
- ? **Parametrized Navigation** (paso de datos)
- ? **Route Registration** (rutas personalizadas)

### **UI/UX**
- ? **Data Binding** bidireccional
- ? **Compiled Bindings** (x:DataType)
- ? **Commands** para interacciones
- ? **Converters** y **Triggers**
- ? **Responsive Design**
- ? **Theming** y estilos globales

### **Gestión de Datos**
- ? **HTTP Client** para APIs
- ? **In-Memory Storage** para desarrollo
- ? **Data Models** estructurados
- ? **Service Pattern** para lógica de negocio
- ? **Result Tracking** y persistencia

---

## ?? **Estado del Workspace**

### ? **Completamente Funcional**
- Todos los proyectos compilan exitosamente
- Navegación Shell implementada y probada
- Sistema de Quiz completamente funcional
- Arquitectura MVVM en todos los proyectos
- Dependency Injection configurada
- Sin caracteres especiales problemáticos

### ?? **Listo para**
- Deploy en dispositivos físicos
- Extensión con nuevas funcionalidades
- Integración con APIs reales
- Añadir bases de datos locales
- Implementar push notifications

---

## ?? **Recursos de Aprendizaje**

Cada proyecto incluye:
- ?? **README.md** con documentación específica
- ?? **Comentarios explicativos** en el código
- ?? **Ejemplos prácticos** de implementación
- ?? **Mejores prácticas** de .NET MAUI

**Este workspace demuestra una progresión completa desde aplicaciones básicas hasta sistemas complejos de navegación y quiz interactivos en .NET MAUI.**