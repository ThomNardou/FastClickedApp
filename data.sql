DROP DATABASE IF EXISTS db_clickGame;

CREATE DATABASE db_clickGame;

USE db_clickGame;

CREATE TABLE `t_joueur` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`name` VARCHAR(255) CHARACTER SET utf8 COLLATE utf8_general_ci,
	`score` INT,
	PRIMARY KEY (`id`)
);