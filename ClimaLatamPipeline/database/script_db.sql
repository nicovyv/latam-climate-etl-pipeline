USE master
GO

CREATE DATABASE climaLatamDB;
GO

USE climaLatamDB;
GO

CREATE TABLE dim_pais(
	IdPais VARCHAR (3),
	Nombre VARCHAR (100) NOT NULL,
	CONSTRAINT PK_PAIS PRIMARY KEY (IdPais)
);

GO

CREATE TABLE dim_indicador(
	IdIndicador VARCHAR(50),
	Nombre VARCHAR(200) NOT NULL,
	CONSTRAINT PK_INDICADOR PRIMARY KEY (IdIndicador)
);

GO

CREATE TABLE fact_metrica(
	IdFact INT IDENTITY(1,1),
	IdPais VARCHAR(3) NOT NULL,
	IdIndicador VARCHAR(50) NOT NULL,
	Anio INT NOT NULL,
	Valor Decimal (18, 4) NULL,
	CONSTRAINT PK_METRICA PRIMARY KEY (IdFact),
	CONSTRAINT FK_METRICA_PAIS FOREIGN KEY (IdPais) REFERENCES dim_pais (IdPais),
	CONSTRAINT FK_METRICA_INDICADOR FOREIGN KEY (IdIndicador) REFERENCES dim_indicador (IdIndicador),
	CONSTRAINT UQ_METRICA UNIQUE (IdPais, IdIndicador, Anio)
);

GO

INSERT INTO Dim_Indicador (IdIndicador, Nombre) VALUES 
('EN.GHG.CO2.PC.CE.AR5', 'Emisiones de CO2 (toneladas per cápita)'),
('EG.FEC.RNEW.ZS', 'Consumo de energía renovable (% del total)'),
('AG.LND.FRST.ZS', 'Área selvática/boscosa (% del área de tierra)'),
('EN.ATM.PM25.MC.M3', 'Exposición a contaminación del aire (PM2.5)');
GO

INSERT INTO dim_pais (IdPais, Nombre) 
VALUES 
('ARG', 'Argentina'),
('BRA', 'Brasil'),
('CHL', 'Chile'),
('COL', 'Colombia'),
('MEX', 'México');