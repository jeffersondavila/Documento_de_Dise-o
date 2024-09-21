# Camera Review
---
## Problema a resolver (Overview)
La empresa "RandomCameraReviews" necesita un sistema que permita que fotografos profesionales suban "reviews" de Camaras fotograficas, para que cualquier persona desde cualquier parte del mundo pueda buscar los reviews y comprarlas a travez de su portal. La empresa cuenta con un equipo de developers especializado en frontEnd que realizara un portal para que los editores suban los "reviews" y los usuarios puedan verlos, y han solicitado que tu como especista en Backend, les proporciones un sistema, incluyendo API que permita realizar lo siguiente:

* Subir reviews de Camaras fotograficas
* Obtener el contenido de los reviews para mostrarlo en vistas del portal en sus versiones web y mobile.
* Manejo de usuarios para editores (no incluye visitantes que leen los reviews)

### Alcance (Scope)



### Casos de uso

Descripción...
* Como editor me gustaria poder subir una review de una camara
* Como editor me gustaria poder subir un reviwe de un lente para camara
* Como usuario no registrado me gustaria poder leer una review

#### Out of Scope (Casos de uso No Soportados)
Descripción...

* Como usuario no registrado me gustaria poder subir una review de una camara
---
## Infraestructura

### Diagrama caso de uso

### Arquitectura

### Diseño a Bajo Nivel para el Sistema de Reviews

1. Subir Review (Web API)
**Componente:** Web API para recibir la reseña del usuario.

- **Implementación:**
  - Utiliza **ASP.NET Core Web API** como el punto de entrada para la subida de reseñas.
  - Crea un endpoint POST en la API para recibir los datos de la reseña. Este endpoint recibe los datos en el cuerpo de la solicitud.
  - El API valida los datos de entrada (asegura que la reseña tenga el formato correcto, por ejemplo, título, puntuación, autor, etc.).

- **Tecnología:**
  - **ASP.NET Core Web API** para el servicio de subida de reseñas.
  - Se puede usar **Azure API Management** para gestionar mejor las solicitudes, agregar autenticación y limitar la tasa de acceso (rate limiting).

2. Función Serverless (Azure Function)
**Componente:** Función que procesa la reseña y la guarda en la base de datos NoSQL.

- **Implementación:**
  - Crea una **Azure Function** en C# que se active cuando la API envíe la reseña. Esta función puede ser del tipo HTTP Trigger o conectada a una cola si deseas desacoplar más el sistema.
  - La función procesará la reseña y la almacenará en una base de datos NoSQL (**MongoDB** en este caso).

- **Tecnología:**
  - **Azure Functions** con **MongoDB** para almacenar los datos de forma escalable.

3. Base de Datos NoSQL (MongoDB)
**Componente:** Almacenar las reseñas en una base de datos NoSQL para accesos rápidos.

- **Implementación:**
  - Utiliza **MongoDB** como la base de datos NoSQL, debido a su capacidad para escalar horizontalmente y su modelo flexible de almacenamiento de documentos JSON.
  - Cada reseña se almacena como un documento JSON, y las consultas rápidas pueden basarse en índices predefinidos (por ejemplo, `author`, `date`, etc.).

- **Tecnología:**
  - **MongoDB** como base de datos NoSQL.

4. Proceso para Obtener Información e Inyectar a BD (SQL Server)
**Componente:** Proceso que toma las reseñas de **MongoDB** y las transforma para **SQL Server**.

- **Implementación:**
  - Puedes crear una **Azure Function** adicional o un **Azure Logic App** que periódicamente obtenga las reseñas de MongoDB y las inyecte en SQL Server.
  - Este proceso puede consultar las reseñas de MongoDB, transformarlas según el esquema SQL, y luego insertarlas en SQL Server.

- **Tecnología:**
  - **Azure Functions** o **Azure Logic Apps** para el proceso de sincronización.
  - **Entity Framework Core** o **Dapper** para interactuar con **SQL Server**.

5. Base de Datos SQL (SQL Server)
**Componente:** Almacenar las reseñas para análisis estructurado en una base de datos relacional.

- **Implementación:**
  - Crea una base de datos **SQL Server** con un esquema para almacenar las reseñas de forma estructurada.
  - Los datos en SQL Server se utilizan para generar reportes, análisis y posiblemente generar recomendaciones basadas en las reseñas.

- **Tecnología:**
  - **SQL Server** como base de datos relacional para reportes y análisis.

### Plan de prueba

### Modelo de datos
Poner diseño de entidades, Jsons, tablas, diagramas entidad relación, etc..

### Integracion continua


---
## Limitaciones
Lista de limitaciones conocidas. Puede ser en formato de lista.
Ej.
* Llamada al API que permite subir un review, no excede los limites de latencia x
* Llamada al API que permite obtener un review, no excede los limites de latencia x
* No se soporta mas de X llamadas por segundo
---
## Costo
Descripción/Análisis de costos
Ejemplo:
Contenplando 1000 usuarios diarios, que visitan recurrentemente cada hora:
* 1000 llamadas diarias a serverless functions. $XX.XX
* 1000 read/write units diarias a X Database on-demand. $XX.XX
Total: $xx.xx (al mes/dia/año)
