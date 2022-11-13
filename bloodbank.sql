-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 12, 2022 at 11:21 PM
-- Server version: 10.4.25-MariaDB
-- PHP Version: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bloodbank`
--
CREATE DATABASE IF NOT EXISTS `bloodbank` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `bloodbank`;

-- --------------------------------------------------------

--
-- Table structure for table `centerdetails`
--

DROP TABLE IF EXISTS `centerdetails`;
CREATE TABLE IF NOT EXISTS `centerdetails` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `Center_Name` varchar(100) NOT NULL,
  `Address` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `centerdetails`
--

INSERT INTO `centerdetails` (`id`, `Center_Name`, `Address`) VALUES
(1, 'AMERICAN LEGION POST 147', 'Clovis Vet Memorial, 808 4th St\r\nClovis, California 93612 United States'),
(2, 'CHURCH OF JESUS CHRIST LATTER DAY SAINTS', 'LDS Church, 1880 Gettysburg Avenue\r\nClovis, California 93611 United States'),
(3, 'FRESYES REALTY', 'Fresyes-Woodward, 30 River Park Place West\r\nFresno, California 93720 United States'),
(4, 'GRACE COMMUNITY CHURCH', 'Grace Comm Church, 17755 Road 26\r\nMadera, California 93638 United States'),
(5, 'AMERICAN LEGION', 'Veterans Hall, 1900 W Olive Ave\r\nPorterville, California 93257 United States'),
(6, 'PORTERVILLE ELKS LODGE', 'Porterville Elks Ldg, 386 North Main Street\r\nPorterville, California 93257 United States'),
(7, 'CALIFORNIA STATE UNIVERSITY FRESNO BLOOD DRIVE', 'Memorial Garden, 5284 N Jackson\r\nFresno, California 93740 United States'),
(8, 'CALIFORNIA STATE UNIVERSITY FRESNO BLOOD DRIVE', 'Univ Dining Hall, 5152 N. Barton Ave.\r\nFresno, California 93740 United States'),
(9, 'CALIFORNIA STATE UNIVERSITY FRESNO BLOOD DRIVE', 'Univ Dining Hall, 5152 N. Barton Ave.\r\nFresno, California 93740 United States'),
(10, 'FUTURE FORD OF CLOVIS', 'Future Ford, 920 W. Shaw Ave.\r\nClovis, California 93612 United States');

-- --------------------------------------------------------

--
-- Table structure for table `donerdetails`
--

DROP TABLE IF EXISTS `donerdetails`;
CREATE TABLE IF NOT EXISTS `donerdetails` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `DonerId` int(11) NOT NULL,
  `CenterId` int(11) NOT NULL,
  `DonateDate` date NOT NULL,
  `Unit` varchar(100) NOT NULL,
  `Reason` varchar(250) NOT NULL,
  `Supervisor` varchar(250) NOT NULL,
  `BloodGrop` varchar(100) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk` (`DonerId`),
  KEY `fk2` (`CenterId`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `donerdetails`
--

INSERT INTO `donerdetails` (`id`, `DonerId`, `CenterId`, `DonateDate`, `Unit`, `Reason`, `Supervisor`, `BloodGrop`) VALUES
(3, 2, 3, '2022-11-25', 'Two', 'Donation', 'Dr. Roddy ', 'B+');

-- --------------------------------------------------------

--
-- Table structure for table `donors`
--

DROP TABLE IF EXISTS `donors`;
CREATE TABLE IF NOT EXISTS `donors` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(100) NOT NULL,
  `password` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `create_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `donors`
--

INSERT INTO `donors` (`id`, `username`, `password`, `email`, `create_at`) VALUES
(2, 'Mark', '123456', 'mark.k@gmail.com', '2022-11-12 22:10:05');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
