CREATE DATABASE PersonalBlog;
GO

USE PersonalBlog;
GO

CREATE TABLE EstadoUsuario (
    CodigoEstadoUsuario INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Estado VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE EstadoBlog (
    CodigoEstadoBlog INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Estado VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Usuario (
    CodigoUsuario INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(200) NOT NULL,
    Correo VARCHAR(200) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    CodigoEstadoUsuario INT NOT NULL,
    FOREIGN KEY (CodigoEstadoUsuario) REFERENCES EstadoUsuario(CodigoEstadoUsuario)
);

CREATE TABLE Blog (
    CodigoBlog INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Titulo VARCHAR(200) NOT NULL,
    Contenido TEXT NOT NULL,
    FechaCreacion SMALLDATETIME NOT NULL DEFAULT GETDATE(),
    FechaModificacion SMALLDATETIME NULL,
    CodigoUsuario INT NOT NULL,
    CodigoEstadoBlog INT NOT NULL,
    FOREIGN KEY (CodigoUsuario) REFERENCES Usuario(CodigoUsuario),
    FOREIGN KEY (CodigoEstadoBlog) REFERENCES EstadoBlog(CodigoEstadoBlog)
);

INSERT INTO EstadoUsuario (Estado) VALUES ('Activo');
INSERT INTO EstadoUsuario (Estado) VALUES ('Inactivo');

INSERT INTO EstadoBlog (Estado) VALUES ('Publicado');
INSERT INTO EstadoBlog (Estado) VALUES ('Borrador');

ALTER TABLE Usuario
ADD 
    FechaUltimoAcceso SMALLDATETIME NULL,
    TokenRecuperacion VARCHAR(255) NULL;
