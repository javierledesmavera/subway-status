
# subway-status

## Armado de ambiente
- Subway.Status.Api
	- Al compilar la solución se restaurarán los paquetes de Nuget
	- Setear este proyecto como proyecto de inicio
	- Correr la aplicación con el IIS Express
	- Las migrations se van a ejecutar al correr la aplicación (si el entorno es Desarrollo). Sino, abrir el Package Manager Console, seleccionar el proyecto Subway.Status.Repository y ejecutar el comando Update-Database.
	
- <span>Subway.Status.Web<span>
	- Abrir una terminal en la carpeta raíz de este proyecto
	- Ejecutar npm install
	- Ejecutar ng serve
	- Browsear la url localhost:4200

## Arquitectura de la aplicación
- Subway.Status.Business
	- Se definen las clases que contienen lógica de negocio, conexión a integraciones y mapeos a DTOs.
- Subway.Status.Domain
	- Se definen los DTOs e interfaces comunes a toda la aplicación.
- Subway.Status.Integration
	- Se definen las clases que se integran a servicios externos y sus entidades.
- Subway.Status.Repository
	- Se definen las clases de Repositorio para acceder a la base de datos y sus entidades.
- Subway.Status.Api
	- Se definen los controllers con los endpoints a exponer por la API.
- <span>Subway.Status.Web<span>
	- Se encuentran los componentes para las vistas, modelos y servicios.

## Tecnologías utilizadas
- .Net Core 3.1
	- EF Core
	- Refit
	- AutoMapper
- Angular 8
	- NgBootstrap
- SQL Server Express