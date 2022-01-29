-- phpMyAdmin SQL Dump
-- version 4.9.5
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Jan 22, 2022 at 11:50 PM
-- Server version: 5.7.24
-- PHP Version: 7.4.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `minesweeper`
--

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `ID` int(11) NOT NULL COMMENT 'The ID of the user',
  `FIRSTNAME` varchar(100) NOT NULL COMMENT 'The user''s first name',
  `LASTNAME` varchar(100) NOT NULL COMMENT 'The user''s last name',
  `SEX` int(11) NOT NULL COMMENT 'The user''s sex',
  `AGE` int(11) NOT NULL COMMENT 'The user''s age',
  `STATE` varchar(100) NOT NULL COMMENT 'The user''s state',
  `EMAIL` varchar(200) NOT NULL COMMENT 'The user''s email',
  `USERNAME` varchar(200) NOT NULL COMMENT 'The user''s username',
  `PASSWORD` varchar(200) NOT NULL COMMENT 'The user''s password'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`ID`, `FIRSTNAME`, `LASTNAME`, `SEX`, `AGE`, `STATE`, `EMAIL`, `USERNAME`, `PASSWORD`) VALUES
(1, 'Michael', 'Duisenberg', 1, 24, 'California', 'MDuisenber1@my.gcu.edu', 'MrDuise', 'ZacIsCoolerThanMe!101');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT 'The ID of the user', AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
