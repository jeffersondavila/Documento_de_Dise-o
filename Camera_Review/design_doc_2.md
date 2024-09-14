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
## Arquitectura

### Diagramas
poner diagramas de secuencia, uml, etc

### Modelo de datos
Poner diseño de entidades, Jsons, tablas, diagramas entidad relación, etc..
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
