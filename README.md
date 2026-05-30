# API Inteligente de Tareas y Análisis

API REST para gestionar tareas internas, aplicar filtros, consumir una API externa y realizar análisis de sentimiento con ML.NET.

---

## Stack tecnológico

| Tecnología | Uso |
|---|---|
| ASP.NET Core Web API .NET 8 | Framework principal |
| Entity Framework Core | ORM para acceso a datos |
| SQLite | Base de datos local |
| ML.NET | Análisis de sentimiento |
| Swagger / Swashbuckle | Documentación interactiva |
| Git / GitHub | Control de versiones |

---

## Ejecutar localmente

### Requisitos previos
- .NET 8 SDK instalado
- Git instalado

### Pasos

```bash
# 1. Clonar el repositorio
git clone <url-del-repositorio>

# 2. Entrar a la carpeta del proyecto
cd ApiInteligenteTareas

# 3. Restaurar paquetes NuGet
dotnet restore

# 4. Aplicar migraciones y crear la base de datos
dotnet ef database update

# 5. Ejecutar la API
dotnet run
```

### Abrir Swagger

Una vez iniciada la aplicación, abrir en el navegador:

```
https://localhost:<puerto>/swagger
```

> El puerto exacto se muestra en la consola al ejecutar `dotnet run`.

---

## Endpoints

### CRUD de tareas

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/tareas` | Obtener todas las tareas |
| GET | `/api/tareas/{id}` | Obtener tarea por ID |
| POST | `/api/tareas` | Crear nueva tarea |
| PUT | `/api/tareas/{id}` | Actualizar tarea existente |
| DELETE | `/api/tareas/{id}` | Eliminar tarea |

### Filtros (query string opcionales en GET /api/tareas)

| Ejemplo | Descripción |
|---|---|
| `GET /api/tareas?estado=Pendiente` | Filtrar por estado |
| `GET /api/tareas?prioridad=Alta` | Filtrar por prioridad |
| `GET /api/tareas?fechaInicio=2026-05-01&fechaFin=2026-05-31` | Filtrar por rango de fecha de vencimiento |

Los filtros son opcionales y se pueden combinar entre sí.

Valores válidos para `estado`: `Pendiente`, `EnProceso`, `Completada`

Valores válidos para `prioridad`: `Baja`, `Media`, `Alta`

### API externa (JSONPlaceholder)

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/tareas-externas` | Obtener todos los todos externos |
| GET | `/api/tareas-externas/{id}` | Obtener todo externo por ID |

### ML.NET — Análisis de sentimiento

| Método | Ruta | Descripción |
|---|---|---|
| POST | `/api/ml/sentimiento` | Analizar sentimiento de un comentario |

---

## Ejemplos de uso

### Crear tarea — `POST /api/tareas`

> **Nota — valores de enums:** los campos `estado` y `prioridad` se envían como números enteros.
>
> | Valor | Estado | | Valor | Prioridad |
> |---|---|---|---|---|
> | `0` | Pendiente | | `0` | Baja |
> | `1` | EnProceso | | `1` | Media |
> | `2` | Completada | | `2` | Alta |

**Request:**
```json
{
  "titulo": "Entregar informe final",
  "descripcion": "Informe del proyecto universitario",
  "estado": 0,
  "prioridad": 2,
  "fechaVencimiento": "2026-06-15T00:00:00"
}
```

**Response `201 Created`:**
```json
{
  "id": 1,
  "titulo": "Entregar informe final",
  "descripcion": "Informe del proyecto universitario",
  "estado": "Pendiente",
  "prioridad": "Alta",
  "fechaCreacion": "2026-05-29T10:30:00",
  "fechaVencimiento": "2026-06-15T00:00:00"
}
```

---

### Actualizar tarea — `PUT /api/tareas/{id}`

**Request:**
```json
{
  "titulo": "Entregar informe final revisado",
  "descripcion": "Versión corregida del informe",
  "estado": 1,
  "prioridad": 2,
  "fechaVencimiento": "2026-06-20T00:00:00"
}
```

**Response `200 OK`:** devuelve la tarea con los datos actualizados. `FechaCreacion` no se modifica.

---

### Analizar sentimiento — `POST /api/ml/sentimiento`

**Request:**
```json
{
  "comentario": "La tarea fue completada correctamente y el sistema funciona bien"
}
```

**Response `200 OK`:**
```json
{
  "comentario": "La tarea fue completada correctamente y el sistema funciona bien",
  "sentimiento": "Positivo"
}
```

---

### API externa — `GET /api/tareas-externas/{id}`

Se consume `https://jsonplaceholder.typicode.com/todos` y la respuesta original se mapea a un DTO propio:

**Response `200 OK`:**
```json
{
  "externalId": 1,
  "titulo": "delectus aut autem",
  "completado": false
}
```

Si el ID no existe, devuelve `404`. Si la API externa no responde, devuelve `503` con un mensaje descriptivo.

---

## Modelo de ML.NET

El análisis de sentimiento utiliza un modelo de clasificación binaria entrenado al iniciar la aplicación.

- **Dataset:** `ML/sentimiento-dataset.csv` — 20 frases etiquetadas sobre tareas y sistema (10 positivas, 10 negativas)
- **Featurización:** `FeaturizeText` convierte el texto del comentario en vectores numéricos
- **Algoritmo:** `SdcaLogisticRegression` para clasificación binaria
- **Resultado:** `Positivo` si el modelo predice `true`, `Negativo` si predice `false`

El servicio se registra como `Singleton`, por lo que el entrenamiento ocurre una sola vez al arrancar la API.

---

## Control de versiones

El proyecto fue desarrollado por partes, cada funcionalidad en su propia rama:

| Rama | Contenido |
|---|---|
| `feature/api-tareas` | Modelos, DTOs, DbContext, CRUD completo |
| `feature/filtros-tareas` | Filtros por estado, prioridad y fechas en GET /api/tareas |
| `feature/api-externa-todos` | Consumo de JSONPlaceholder y mapeo a DTO propio |
| `feature/mlnet-basico` | Análisis de sentimiento con ML.NET |
| `feature/readme-documentacion` | Documentación del proyecto |
