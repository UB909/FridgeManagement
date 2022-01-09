
-- phpMyAdmin SQL Dump
-- version 5.0.4deb2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Erstellungszeit: 14. Nov 2021 um 17:28
-- Server-Version: 10.5.12-MariaDB-0+deb11u1
-- PHP-Version: 7.4.25

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

DROP TABLE IF EXISTS entries_history;
DROP TABLE IF EXISTS entries;
DROP TABLE IF EXISTS items_history;
DROP TABLE IF EXISTS items;
DROP TABLE IF EXISTS categories_history;
DROP TABLE IF EXISTS categories;
DROP TABLE IF EXISTS locations_history;
DROP TABLE IF EXISTS locations;


/*-----------------------------------------------------------------------------------------------*/
/* Locations                                                                                     */
/*-----------------------------------------------------------------------------------------------*/
CREATE TABLE locations (
  id int(11) NOT NULL AUTO_INCREMENT,
  name text NOT NULL,
  PRIMARY KEY(id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


CREATE TABLE locations_history (
  internal_id int(11) UNIQUE NOT NULL AUTO_INCREMENT COMMENT 'internal use only',
  date timestamp(3) UNIQUE NOT NULL DEFAULT current_timestamp() COMMENT 'timestamp when the location with the given id was modified',
  type tinyint(4) NOT NULL COMMENT '0: Added\r\n1: Modified\r\n2: Deleted',
  handled BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'true if this entry was included into locations',
  id int(11) NOT NULL COMMENT 'ID of location which was modified',
  name text DEFAULT NULL COMMENT 'null or new/modified name'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


/*-----------------------------------------------------------------------------------------------*/
/* Categories                                                                                    */
/*-----------------------------------------------------------------------------------------------*/
CREATE TABLE categories (
  id int(11) NOT NULL AUTO_INCREMENT,
  name text NOT NULL,
  PRIMARY KEY(id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


CREATE TABLE categories_history (
  internal_id int(11) UNIQUE NOT NULL AUTO_INCREMENT COMMENT 'internal use only',
  date timestamp(3) UNIQUE NOT NULL DEFAULT current_timestamp() COMMENT 'timestamp when the category with the given id was modified',
  type tinyint(4) NOT NULL COMMENT '0: Added\r\n1: Modified\r\n2: Deleted',
  handled BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'true if this entry was included into categories',
  id int(11) NOT NULL COMMENT 'ID of category which was modified',
  name text DEFAULT NULL COMMENT 'null or new/modified name'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


/*-----------------------------------------------------------------------------------------------*/
/* Items                                                                                         */
/*-----------------------------------------------------------------------------------------------*/
CREATE TABLE items (
  id int(11) NOT NULL AUTO_INCREMENT,
  name text NOT NULL,
  category_id int(11) NOT NULL,
  image_size int(11) DEFAULT NULL,
  image longblob DEFAULT NULL,
  PRIMARY KEY(id),
  KEY(category_id),
  CONSTRAINT constr_category FOREIGN KEY (category_id) REFERENCES categories (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


CREATE TABLE items_history (
  internal_id int(11) UNIQUE NOT NULL AUTO_INCREMENT COMMENT 'internal use only',
  date timestamp(3) UNIQUE NOT NULL DEFAULT current_timestamp() COMMENT 'timestamp when the item with the given id was modified',
  type tinyint(4) NOT NULL COMMENT '0: Added\r\n1: Modified\r\n2: Deleted',
  handled BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'true if this entry was included into items',
  id int(11) NOT NULL COMMENT 'ID of item which was modified',
  name text DEFAULT NULL COMMENT 'null or new/modified name',
  image_size int(11) DEFAULT NULL COMMENT 'null or new/modified image size',
  image longblob DEFAULT NULL COMMENT 'null or new/modified image'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


/*-----------------------------------------------------------------------------------------------*/
/* Entries                                                                                       */
/*-----------------------------------------------------------------------------------------------*/
CREATE TABLE entries (
  id int(11) NOT NULL AUTO_INCREMENT,
  item_id int(11) NOT NULL,
  location_id int(11) NOT NULL,
  number_of_items int(11) NOT NULL,
  PRIMARY KEY(id),
  KEY(item_id),
  KEY(location_id),
  CONSTRAINT constr_item FOREIGN KEY (item_id) REFERENCES items (id),
  CONSTRAINT constr_location FOREIGN KEY (location_id) REFERENCES locations (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


CREATE TABLE entries_history (
  internal_id int(11) UNIQUE NOT NULL AUTO_INCREMENT COMMENT 'internal use only',
  date timestamp(3) UNIQUE NOT NULL DEFAULT current_timestamp() COMMENT 'timestamp when the item with the given id was modified',
  type tinyint(4) NOT NULL COMMENT '0: Added\r\n2: Deleted\r\n3: Set Value\r\n4: Increase number of items\r\n5: Decrease number of items',
  handled BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'true if this entry was included into entries',
  id int(11) NOT NULL COMMENT 'ID of entry which was modified',
  item_id int(11) DEFAULT NULL COMMENT 'null or new/modified item_id',
  location_id int(11) DEFAULT NULL COMMENT 'null or new/modified location_id',
  number_of_items int(11) DEFAULT NULL COMMENT 'null or new/modified number_of_items'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
