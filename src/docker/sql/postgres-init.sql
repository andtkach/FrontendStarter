CREATE DATABASE jiwebapicatalogdb;

\c jiwebapicatalogdb;

CREATE TABLE execlog (
	id serial PRIMARY KEY,
	event_at TIMESTAMPTZ DEFAULT Now(),
	title VARCHAR ( 255 ) NOT NULL
);

INSERT INTO execlog (title) VALUES ('JI WebAPI Data database cresated');



CREATE DATABASE jiwebapiidentitydb;

\c jiwebapiidentitydb;

CREATE TABLE execlog (
	id serial PRIMARY KEY,
	event_at TIMESTAMPTZ DEFAULT Now(),
	title VARCHAR ( 255 ) NOT NULL
);

INSERT INTO execlog (title) VALUES ('JI WebAPI Identity database cresated');



CREATE DATABASE jiwebapimessagedb;

\c jiwebapimessagedb;

CREATE TABLE execlog (
	id serial PRIMARY KEY,
	event_at TIMESTAMPTZ DEFAULT Now(),
	title VARCHAR ( 255 ) NOT NULL
);

INSERT INTO execlog (title) VALUES ('JI WebAPI Message database cresated');



