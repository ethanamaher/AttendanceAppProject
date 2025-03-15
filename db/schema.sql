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
-- Table structure for table `Attendance_Instance`
--

DROP TABLE IF EXISTS `Attendance_Instance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Attendance_Instance` (
  `Attendance_id` char(36) NOT NULL,
  `Student_id` char(10) NOT NULL,
  `Class_id` char(36) NOT NULL,
  `Ip_address` varchar(45) DEFAULT NULL,
  `Is_late` tinyint(1) DEFAULT NULL,
  `Excused_absence` tinyint(1) DEFAULT NULL,
  `Date_time` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Attendance_id`),
  KEY `fk_attendanceInstance_studentId` (`Student_id`),
  KEY `fk_attendanceInstance_classId` (`Class_id`),
  CONSTRAINT `fk_attendanceInstance_classId` FOREIGN KEY (`Class_id`) REFERENCES `Class` (`Class_id`),
  CONSTRAINT `fk_attendanceInstance_studentId` FOREIGN KEY (`Student_id`) REFERENCES `Student` (`Utd_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Class`
--

DROP TABLE IF EXISTS `Class`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Class` (
  `Class_id` char(36) NOT NULL,
  `Prof_utd_id` char(10) NOT NULL,
  `Class_prefix` varchar(4) DEFAULT NULL,
  `Class_number` char(8) DEFAULT NULL,
  `Class_name` varchar(100) DEFAULT NULL,
  `Start_date` date DEFAULT NULL,
  `End_date` date DEFAULT NULL,
  `Start_time` time DEFAULT NULL,
  `End_time` time DEFAULT NULL,
  PRIMARY KEY (`Class_id`),
  KEY `fk_class_profId` (`Prof_utd_id`),
  CONSTRAINT `fk_class_profId` FOREIGN KEY (`Prof_utd_id`) REFERENCES `Professor` (`Utd_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Class_Schedule`
--

DROP TABLE IF EXISTS `Class_Schedule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Class_Schedule` (
  `Class_schedule_id` char(36) NOT NULL,
  `Class_id` char(36) NOT NULL,
  `Day_of_Week` enum('Monday','Tuesday','Wednesday','Thursday','Friday','Saturday','Sunday') DEFAULT NULL,
  PRIMARY KEY (`Class_schedule_id`),
  KEY `fk_classSchedule_classId` (`Class_id`),
  CONSTRAINT `fk_classSchedule_classId` FOREIGN KEY (`Class_id`) REFERENCES `Class` (`Class_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Passwords`
--

DROP TABLE IF EXISTS `Passwords`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Passwords` (
  `Password_id` char(36) NOT NULL,
  `Class_id` char(36) NOT NULL,
  `Date_assigned` date DEFAULT NULL,
  `Password_text` varchar(100) NOT NULL,
  PRIMARY KEY (`Password_id`),
  KEY `fk_passwords_classId` (`Class_id`),
  CONSTRAINT `fk_passwords_classId` FOREIGN KEY (`Class_id`) REFERENCES `Class` (`Class_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Professor`
--

DROP TABLE IF EXISTS `Professor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Professor` (
  `Utd_id` char(10) NOT NULL,
  `First_name` varchar(50) NOT NULL,
  `Last_name` varchar(50) NOT NULL,
  PRIMARY KEY (`Utd_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Quiz_Instance`
--

DROP TABLE IF EXISTS `Quiz_Instance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Quiz_Instance` (
  `Quiz_id` char(36) NOT NULL,
  `Class_id` char(36) NOT NULL,
  `Start_time` timestamp NULL DEFAULT NULL,
  `End_time` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Quiz_id`),
  KEY `fk_quizInstance_classId` (`Class_id`),
  CONSTRAINT `fk_quizInstance_classId` FOREIGN KEY (`Class_id`) REFERENCES `Class` (`Class_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Quiz_Questions`
--

DROP TABLE IF EXISTS `Quiz_Questions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Quiz_Questions` (
  `Question_id` char(36) NOT NULL,
  `Quiz_id` char(36) NOT NULL,
  `Correct_answer` char(1) NOT NULL,
  `Question_text` varchar(500) NOT NULL,
  `Answer_a` varchar(255) NOT NULL,
  `Answer_b` varchar(255) NOT NULL,
  `Answer_c` varchar(255) DEFAULT NULL,
  `Answer_d` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Question_id`),
  KEY `fk_quizQuestions_quizId` (`Quiz_id`),
  CONSTRAINT `fk_quizQuestions_quizId` FOREIGN KEY (`Quiz_id`) REFERENCES `Quiz_Instance` (`Quiz_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Quiz_Responses`
--

DROP TABLE IF EXISTS `Quiz_Responses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Quiz_Responses` (
  `Response_id` char(36) NOT NULL,
  `Student_id` char(10) NOT NULL,
  `Quiz_question_id` char(36) NOT NULL,
  `Quiz_instance_id` char(36) NOT NULL,
  PRIMARY KEY (`Response_id`),
  KEY `fk_quizResponses_studentId` (`Student_id`),
  KEY `fk_quizResponses_quizQuestionId` (`Quiz_question_id`),
  KEY `fk_quizResponses_quizInstanceid` (`Quiz_instance_id`),
  CONSTRAINT `fk_quizResponses_quizInstanceid` FOREIGN KEY (`Quiz_instance_id`) REFERENCES `Quiz_Instance` (`Quiz_id`),
  CONSTRAINT `fk_quizResponses_quizQuestionId` FOREIGN KEY (`Quiz_question_id`) REFERENCES `Quiz_Questions` (`Question_id`),
  CONSTRAINT `fk_quizResponses_studentId` FOREIGN KEY (`Student_id`) REFERENCES `Student` (`Utd_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Student`
--

DROP TABLE IF EXISTS `Student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Student` (
  `Utd_id` char(10) NOT NULL,
  `First_name` varchar(50) NOT NULL,
  `Last_name` varchar(50) NOT NULL,
  `Username` char(9) NOT NULL,
  PRIMARY KEY (`Utd_id`),
  UNIQUE KEY `Username` (`Username`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-03-04 21:00:35
