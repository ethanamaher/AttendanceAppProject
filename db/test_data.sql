-- MySQL dump 10.13  Distrib 8.0.40, for macos14 (arm64)
--
-- Host: localhost    Database: attendance_db
-- ------------------------------------------------------
-- Server version	8.0.40

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping data for table `Attendance_Instance`
--

LOCK TABLES `Attendance_Instance` WRITE;
/*!40000 ALTER TABLE `Attendance_Instance` DISABLE KEYS */;
INSERT INTO `Attendance_Instance` VALUES ('d94dfd14-f96d-11ef-ac73-c5cdb5259684','2021384756','d94db2a0-f96d-11ef-ac73-c5cdb5259684','192.168.1.2',0,0,'2025-02-01 15:10:00'),('d94e062e-f96d-11ef-ac73-c5cdb5259684','2021837465','d94db2a0-f96d-11ef-ac73-c5cdb5259684','192.168.1.3',1,0,'2025-02-01 15:45:00'),('d94e06f6-f96d-11ef-ac73-c5cdb5259684','2022948576','d94dbaac-f96d-11ef-ac73-c5cdb5259684','192.168.1.4',0,0,'2025-02-02 17:05:00');
/*!40000 ALTER TABLE `Attendance_Instance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Class`
--

LOCK TABLES `Class` WRITE;
/*!40000 ALTER TABLE `Class` DISABLE KEYS */;
INSERT INTO `Class` VALUES ('d94db2a0-f96d-11ef-ac73-c5cdb5259684','1996847561','CS','1200','Introduction to Computer Science','2025-01-15','2025-05-15','09:00:00','10:30:00'),('d94dbaac-f96d-11ef-ac73-c5cdb5259684','2000738290','CS','3345','Data Structures and Algorithms','2025-01-15','2025-05-15','11:00:00','12:30:00');
/*!40000 ALTER TABLE `Class` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Class_Schedule`
--

LOCK TABLES `Class_Schedule` WRITE;
/*!40000 ALTER TABLE `Class_Schedule` DISABLE KEYS */;
INSERT INTO `Class_Schedule` VALUES ('d94dd776-f96d-11ef-ac73-c5cdb5259684','d94db2a0-f96d-11ef-ac73-c5cdb5259684','Monday'),('d94de284-f96d-11ef-ac73-c5cdb5259684','d94db2a0-f96d-11ef-ac73-c5cdb5259684','Wednesday'),('d94de392-f96d-11ef-ac73-c5cdb5259684','d94dbaac-f96d-11ef-ac73-c5cdb5259684','Tuesday'),('d94de432-f96d-11ef-ac73-c5cdb5259684','d94dbaac-f96d-11ef-ac73-c5cdb5259684','Thursday');
/*!40000 ALTER TABLE `Class_Schedule` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Passwords`
--

LOCK TABLES `Passwords` WRITE;
/*!40000 ALTER TABLE `Passwords` DISABLE KEYS */;
INSERT INTO `Passwords` VALUES ('d94e43aa-f96d-11ef-ac73-c5cdb5259684','d94db2a0-f96d-11ef-ac73-c5cdb5259684','2025-02-01','library'),('d94e4c42-f96d-11ef-ac73-c5cdb5259684','d94dbaac-f96d-11ef-ac73-c5cdb5259684','2025-02-02','dictionary');
/*!40000 ALTER TABLE `Passwords` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Professor`
--

LOCK TABLES `Professor` WRITE;
/*!40000 ALTER TABLE `Professor` DISABLE KEYS */;
INSERT INTO `Professor` VALUES ('1996847561','Emily','Brown'),('2000738290','James','Wilson');
/*!40000 ALTER TABLE `Professor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Quiz_Instance`
--

LOCK TABLES `Quiz_Instance` WRITE;
/*!40000 ALTER TABLE `Quiz_Instance` DISABLE KEYS */;
INSERT INTO `Quiz_Instance` VALUES ('d94e24d8-f96d-11ef-ac73-c5cdb5259684','d94db2a0-f96d-11ef-ac73-c5cdb5259684','2025-02-10 15:00:00','2025-02-10 15:30:00'),('d94e2eb0-f96d-11ef-ac73-c5cdb5259684','d94dbaac-f96d-11ef-ac73-c5cdb5259684','2025-02-15 17:00:00','2025-02-15 17:30:00');
/*!40000 ALTER TABLE `Quiz_Instance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Quiz_Questions`
--

LOCK TABLES `Quiz_Questions` WRITE;
/*!40000 ALTER TABLE `Quiz_Questions` DISABLE KEYS */;
INSERT INTO `Quiz_Questions` VALUES ('d94e6588-f96d-11ef-ac73-c5cdb5259684','d94e24d8-f96d-11ef-ac73-c5cdb5259684','A','What is 2+2?','4','3','5','6'),('d94e7050-f96d-11ef-ac73-c5cdb5259684','d94e2eb0-f96d-11ef-ac73-c5cdb5259684','B','What is the time complexity of binary search?','O(n)','O(log n)','O(1)','O(n log n)');
/*!40000 ALTER TABLE `Quiz_Questions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Quiz_Responses`
--

LOCK TABLES `Quiz_Responses` WRITE;
/*!40000 ALTER TABLE `Quiz_Responses` DISABLE KEYS */;
INSERT INTO `Quiz_Responses` VALUES ('d94e994a-f96d-11ef-ac73-c5cdb5259684','2021384756','d94e6588-f96d-11ef-ac73-c5cdb5259684','d94e24d8-f96d-11ef-ac73-c5cdb5259684'),('d94ea3c2-f96d-11ef-ac73-c5cdb5259684','2021837465','d94e7050-f96d-11ef-ac73-c5cdb5259684','d94e2eb0-f96d-11ef-ac73-c5cdb5259684');
/*!40000 ALTER TABLE `Quiz_Responses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Student`
--

LOCK TABLES `Student` WRITE;
/*!40000 ALTER TABLE `Student` DISABLE KEYS */;
INSERT INTO `Student` VALUES ('2021384756','Alice','Johnson','AJJ493827'),('2021837465','Bob','Smith','BMS726491'),('2022948576','Charlie','Davis','CDX385920');
/*!40000 ALTER TABLE `Student` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-03-06 15:26:16
