# Smart Agriculture
---
## Problema a resolver (Overview)

Debido al creciente cambio climatico y a la escasez de recursos naturales, la agricultura enfrenta desafíos significativos para mantener la calidad y productividad de las cosechas. Estos factores han aumentado la complejidad del manejo de cultivos, haciendo necesario un control preciso y automatizado que optimice el uso de recursos como el agua, los nutrientes y la energía, mientras se maximiza la calidad y rendimiento de los productos agrícolas. El problema se centra en la necesidad de desarrollar un sistema inteligente que permita automatizar el monitoreo y control de las plantaciones, adaptándose en tiempo real a las condiciones cambiantes y mejorando tanto la sostenibilidad como la eficienta del proceso agrícola.

### Alcance (Scope)

1. **Monitoreo en Tiempo Real:**
   - **Descripción:** Implementar sensores IoT en las plantaciones para recopilar datos en tiempo real sobre condiciones ambientales críticas como temperatura, humedad, luz, y pH del suelo.
   - **Funcionalidad de la IA:** Análisis de datos en tiempo real para detectar patrones, anomalías, o condiciones adversas que puedan afectar la salud de los vegetales, permitiendo intervenciones rápidas y precisas.
   - **Impacto:** Mejora la capacidad de respuesta ante cambios ambientales y optimiza el cuidado de los cultivos.

2. **Asistente Virtual de Cultivo:**
   - **Descripción:** Desarrollar una aplicación móvil o web que sirva como asistente virtual para agricultores y jardineros, brindando recomendaciones personalizadas basadas en los datos recopilados por los sensores.
   - **Funcionalidad de la IA:** Sugerencias automáticas sobre riego, fertilización, poda, y momentos óptimos para la cosecha, adaptadas a las necesidades específicas de cada cultivo.
   - **Impacto:** Facilita la gestión diaria de las plantaciones y optimiza el manejo de los cultivos.

3. **Predicción de Plagas y Enfermedades:**
   - **Descripción:** Entrenar modelos de IA para predecir la aparición de plagas o enfermedades mediante el análisis de imágenes y datos ambientales.
   - **Funcionalidad de la IA:** Implementación de un sistema de alerta temprana que notifique a los usuarios sobre posibles riesgos y sugiera medidas preventivas.
   - **Impacto:** Reduce las pérdidas por plagas y enfermedades y mejora la calidad de los cultivos.

4. **Optimización del Uso de Recursos:**
   - **Descripción:** Desarrollar un sistema que optimice el uso de recursos como agua y fertilizantes, ajustando automáticamente los sistemas de riego y dosificación.
   - **Funcionalidad de la IA:** Algoritmos que analicen las necesidades precisas de las plantas y ajusten los recursos de manera eficiente.
   - **Impacto:** Reduce el desperdicio de recursos, disminuye costos operativos y mejora la sostenibilidad agrícola.

5. **Análisis Predictivo de Rendimiento:**
   - **Descripción:** Utilizar datos históricos y actuales para predecir el rendimiento de los cultivos en diferentes escenarios.
   - **Funcionalidad de la IA:** Modelos predictivos que permiten a los agricultores tomar decisiones informadas sobre las mejores prácticas de cultivo para maximizar la producción.
   - **Impacto:** Mejora la planificación agrícola y optimiza el rendimiento de las cosechas.

6. **Mercado y Comercio Justo:**
   - **Descripción:** Crear una plataforma que conecte a los productores con los consumidores, facilitando la comercialización de los productos agrícolas.
   - **Funcionalidad de la IA:** Análisis de tendencias de mercado para sugerir precios justos y oportunidades de venta.
   - **Impacto:** Mejora la rentabilidad para los agricultores y asegura un comercio justo para los consumidores.


### Casos de Uso

1. **Caso de Uso: Monitoreo de Condiciones Ambientales**
   - **Descripción:** El sistema monitorea las condiciones ambientales en tiempo real utilizando sensores IoT distribuidos en la plantación.
   - **Actores:** Sensores IoT, Sistema de IA, Agricultor
   - **Flujo Principal:**
     1. Los sensores IoT recopilan datos sobre temperatura, humedad, luz, y pH del suelo.
     2. El sistema de IA analiza los datos en tiempo real.
     3. Si se detectan anomalías o condiciones adversas, el sistema genera una alerta.
     4. El agricultor recibe la alerta y las recomendaciones para tomar acciones correctivas.
   - **Precondiciones:** Los sensores IoT deben estar instalados y funcionando correctamente.
   - **Postcondiciones:** Los datos recopilados se almacenan para análisis histórico.

2. **Caso de Uso: Asistente Virtual de Cultivo**
   - **Descripción:** El asistente virtual proporciona recomendaciones personalizadas al agricultor para la gestión diaria de la plantación.
   - **Actores:** Sistema de IA, Aplicación Móvil/Web, Agricultor
   - **Flujo Principal:**
     1. El agricultor ingresa a la aplicación móvil/web.
     2. La IA analiza los datos recientes de la plantación.
     3. El asistente virtual sugiere acciones como riego, fertilización, poda, y cosecha.
     4. El agricultor sigue las recomendaciones o ajusta las acciones según sea necesario.
   - **Precondiciones:** El agricultor debe tener acceso a la aplicación y los datos deben estar actualizados.
   - **Postcondiciones:** Las acciones sugeridas son registradas para futuros análisis.

3. **Caso de Uso: Predicción de Plagas y Enfermedades**
   - **Descripción:** El sistema predice la aparición de plagas o enfermedades y notifica al agricultor con antelación.
   - **Actores:** Sistema de IA, Agricultor
   - **Flujo Principal:**
     1. El sistema de IA analiza imágenes de las plantas y datos ambientales.
     2. La IA identifica patrones que indican la posible aparición de plagas o enfermedades.
     3. El sistema genera una alerta temprana y notifica al agricultor.
     4. El agricultor recibe la alerta y toma medidas preventivas sugeridas por la IA.
   - **Precondiciones:** Las imágenes y datos ambientales deben ser precisos y actuales.
   - **Postcondiciones:** Las alertas y acciones tomadas se registran para mejorar la precisión futura.

4. **Caso de Uso: Optimización del Uso de Recursos**
   - **Descripción:** El sistema ajusta automáticamente el riego y la fertilización para optimizar el uso de recursos.
   - **Actores:** Sistema de IA, Sistema de Riego y Fertilización, Agricultor
   - **Flujo Principal:**
     1. La IA analiza las necesidades de las plantas basándose en los datos ambientales y de crecimiento.
     2. El sistema ajusta el riego y la dosificación de fertilizantes de acuerdo con las recomendaciones de la IA.
     3. El agricultor recibe un informe sobre las acciones tomadas y los recursos utilizados.
   - **Precondiciones:** Los sistemas de riego y fertilización deben estar conectados y ser controlables por la IA.
   - **Postcondiciones:** Se genera un reporte de uso de recursos, y los ajustes se aplican automáticamente.

5. **Caso de Uso: Análisis Predictivo de Rendimiento**
   - **Descripción:** El sistema predice el rendimiento de los cultivos bajo diferentes escenarios, ayudando al agricultor a planificar las actividades.
   - **Actores:** Sistema de IA, Agricultor
   - **Flujo Principal:**
     1. La IA analiza datos históricos y actuales sobre la plantación.
     2. Se simulan diferentes escenarios de crecimiento y se predice el rendimiento potencial.
     3. El sistema proporciona recomendaciones sobre la mejor estrategia de manejo.
     4. El agricultor utiliza esta información para tomar decisiones sobre las próximas etapas de cultivo.
   - **Precondiciones:** Deben existir suficientes datos históricos y actuales para realizar análisis precisos.
   - **Postcondiciones:** Las predicciones y recomendaciones son almacenadas para referencia futura.

6. **Caso de Uso: Comercio y Mercado**
   - **Descripción:** La plataforma conecta a los productores con consumidores y sugiere precios justos basados en análisis de mercado.
   - **Actores:** Sistema de IA, Agricultor, Consumidor
   - **Flujo Principal:**
     1. La IA analiza las tendencias de mercado y datos de producción.
     2. El sistema sugiere precios de venta justos y posibles oportunidades de comercialización.
     3. El agricultor publica sus productos en la plataforma con los precios sugeridos.
     4. Los consumidores compran los productos directamente a los agricultores.
   - **Precondiciones:** La plataforma debe estar en funcionamiento y conectada a bases de datos de mercado.
   - **Postcondiciones:** Las transacciones realizadas y los datos de mercado se registran para análisis futuro.


#### Out of Scope (Casos de uso No Soportados)
1. **Caso de Uso: Capacitación y Entrenamiento**
   - **Descripción:** El sistema ofrecería contenido educativo personalizado para mejorar las habilidades y conocimientos de los agricultores.
   - **Motivo para No Ser Soportado:** Se ha decidido no incluir este caso de uso en el alcance actual del proyecto, posiblemente para enfocarse en otros aspectos más críticos del sistema.
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
* Llamadas del API tienen latencia X
* No se soporta mas de X llamadas por segundo
---
## Costo
Descripción/Análisis de costos
Ejemplo:
"Considerando N usuarios diarios, M llamadas a X servicio/baseDatos/etc"
* 1000 llamadas diarias a serverless functions. $XX.XX
* 1000 read/write units diarias a X Database on-demand. $XX.XX
Total: $xx.xx (al mes/dia/año)
