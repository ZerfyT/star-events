-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               11.8.2-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             12.10.0.7000
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for star_events
DROP DATABASE IF EXISTS `star_events`;
CREATE DATABASE IF NOT EXISTS `star_events` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci */;
USE `star_events`;

-- Dumping data for table star_events.aspnetroleclaims: ~0 rows (approximately)

-- Dumping data for table star_events.aspnetroles: ~3 rows (approximately)
INSERT INTO `aspnetroles` (`Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`) VALUES
	('7c2d984e-58ac-419c-8c26-02567523e81c', 'Customer', 'CUSTOMER', NULL),
	('c3166805-e1a1-4f3a-bf1f-4b297c7dbea5', 'Admin', 'ADMIN', NULL),
	('d6764eda-713e-4a19-8504-fb4b5e4cf931', 'EventOrganizer', 'EVENTORGANIZER', NULL);

-- Dumping data for table star_events.aspnetuserclaims: ~0 rows (approximately)

-- Dumping data for table star_events.aspnetuserlogins: ~0 rows (approximately)

-- Dumping data for table star_events.aspnetuserroles: ~5 rows (approximately)
INSERT INTO `aspnetuserroles` (`UserId`, `RoleId`) VALUES
	('24a60955-a1f0-4aec-8874-66a42ffd7620', '7c2d984e-58ac-419c-8c26-02567523e81c'),
	('bfc3c2f0-83f5-4a41-a476-e253868abbf5', '7c2d984e-58ac-419c-8c26-02567523e81c'),
	('214b8e62-60a3-4bb8-908a-f91e7d9b0294', 'c3166805-e1a1-4f3a-bf1f-4b297c7dbea5'),
	('3b2f6a0f-3dd1-474a-b4ab-1419879b88dc', 'd6764eda-713e-4a19-8504-fb4b5e4cf931'),
	('f10b2c50-f7c8-4ea3-b58a-46de0f674da5', 'd6764eda-713e-4a19-8504-fb4b5e4cf931');

-- Dumping data for table star_events.aspnetusers: ~5 rows (approximately)
INSERT INTO `aspnetusers` (`Id`, `FirstName`, `LastName`, `ContactNo`, `Address`, `LoyaltyPoints`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEnd`, `LockoutEnabled`, `AccessFailedCount`) VALUES
	('214b8e62-60a3-4bb8-908a-f91e7d9b0294', 'Admin', 'User', '+94 11 234 5678', NULL, 0, 'admin@starevents.lk', 'ADMIN@STAREVENTS.LK', 'admin@starevents.lk', 'ADMIN@STAREVENTS.LK', 1, 'AQAAAAIAAYagAAAAEF2iZfKrF/rOHQi9tT/VLT6fzexCKDsILtToYjKN7hAzbbzI85nH/bTHMowKUvHHVg==', 'RMBFYCZVFVMJ7XIJ6BXIBAFFYWRXJJLO', '6316d1ef-3ca7-4052-8a4e-aeffeca6cb1f', NULL, 0, 0, NULL, 1, 0),
	('24a60955-a1f0-4aec-8874-66a42ffd7620', 'Jane', 'Smith', '+94 77 123 4568', NULL, 0, 'customer02@starevents.lk', 'CUSTOMER02@STAREVENTS.LK', 'customer02@starevents.lk', 'CUSTOMER02@STAREVENTS.LK', 1, 'AQAAAAIAAYagAAAAEKo+p2VeovdeYnxnEUyLHyY43/RwmlUnk2fGRA+fDkYCmOVWsOsBrl3yzGnN8d8Sxw==', '3LKAYYFLCVL2KE7WFA755Z2O7S3YM4R4', 'f2ea1766-b061-44dd-9360-623996b2a372', NULL, 0, 0, NULL, 1, 0),
	('3b2f6a0f-3dd1-474a-b4ab-1419879b88dc', 'Sarah', 'Johnson', '+94 11 234 5680', NULL, 0, 'organizer2@starevents.lk', 'ORGANIZER2@STAREVENTS.LK', 'organizer2@starevents.lk', 'ORGANIZER2@STAREVENTS.LK', 1, 'AQAAAAIAAYagAAAAEPmT5JMz5k6kMzdz2hakj2rlacwLTjQfYvSsIDKyOW8ZFFVB/aNUfvf4POwtb3a7Sg==', 'R3HBQ6RUIUWDYW6H7GUUKS3RXVJQI5CH', 'e048738c-28fe-4d85-8f0e-8cc3cc8d5952', NULL, 0, 0, NULL, 1, 0),
	('bfc3c2f0-83f5-4a41-a476-e253868abbf5', 'John', 'Doe', '+94 77 123 4567', NULL, 170, 'customer@starevents.lk', 'CUSTOMER@STAREVENTS.LK', 'customer@starevents.lk', 'CUSTOMER@STAREVENTS.LK', 1, 'AQAAAAIAAYagAAAAEPGiW4UCjfWJPivY4jl77/Nf58gSPZotSYIOy7B0efH5p0Zx4+8WcOXsKWPhuBtfsw==', '2GIIJF2RINCDVBW6LKSMFBLETNMXZ2M2', '55b836f3-75b5-4242-b645-a374ed7ecfcf', NULL, 0, 0, NULL, 1, 0),
	('f10b2c50-f7c8-4ea3-b58a-46de0f674da5', 'Event', 'Organizer', '+94 11 234 5679', NULL, 0, 'organizer@starevents.lk', 'ORGANIZER@STAREVENTS.LK', 'organizer@starevents.lk', 'ORGANIZER@STAREVENTS.LK', 1, 'AQAAAAIAAYagAAAAEElx0Ve36j1CnYncMF9nusc5w2ABKYvro6E/snfmTVcLJfera4ZrGvCMUjUsFmTZyA==', 'IUXGWE5ZNLU3W3WIXWC5N2S4HBW7DSS5', '254faf18-f7d9-49d6-afdf-c3a5363aced3', NULL, 0, 0, NULL, 1, 0);

-- Dumping data for table star_events.aspnetusertokens: ~0 rows (approximately)

-- Dumping data for table star_events.bookings: ~69 rows (approximately)
INSERT INTO `bookings` (`BookingID`, `BookingDateTime`, `TotalAmount`, `Status`, `UserID`, `PromotionID`, `EventID`) VALUES
	(1, '2025-08-17 19:00:00.000000', 12.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 1),
	(2, '2025-08-10 19:00:00.000000', 36.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 1),
	(3, '2025-08-08 19:00:00.000000', 30.60, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 1, 1),
	(4, '2025-08-18 19:00:00.000000', 36.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 1),
	(5, '2025-08-02 19:00:00.000000', 63.75, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 1, 1),
	(6, '2025-07-28 19:00:00.000000', 54.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 1),
	(7, '2025-08-01 19:00:00.000000', 18.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 1),
	(8, '2025-08-08 19:00:00.000000', 25.00, 'Pending', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 1),
	(9, '2025-07-30 19:00:00.000000', 50.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 1),
	(10, '2025-07-23 19:00:00.000000', 0.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 2, 1),
	(11, '2025-07-27 19:00:00.000000', 50.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 1),
	(12, '2025-08-17 18:00:00.000000', 30.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 2),
	(13, '2025-09-02 18:00:00.000000', 21.00, 'Pending', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 2),
	(14, '2025-08-30 18:00:00.000000', 0.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 2, 2),
	(15, '2025-08-25 18:00:00.000000', 21.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 2),
	(16, '2025-08-22 18:00:00.000000', 14.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 2),
	(17, '2025-08-27 18:00:00.000000', 0.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 2, 2),
	(18, '2025-08-22 18:00:00.000000', 14.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 2),
	(19, '2025-08-18 18:00:00.000000', 7.00, 'Pending', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 2),
	(20, '2025-09-08 16:00:00.000000', 0.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 2, 3),
	(21, '2025-09-05 16:00:00.000000', 6.80, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 1, 3),
	(22, '2025-08-11 16:00:00.000000', 24.00, 'Pending', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 3),
	(23, '2025-08-28 16:00:00.000000', 13.60, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 1, 3),
	(24, '2025-08-17 16:00:00.000000', 40.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 3),
	(25, '2025-09-08 16:00:00.000000', 16.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 3),
	(26, '2025-09-03 16:00:00.000000', 60.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 3),
	(27, '2025-08-26 16:00:00.000000', 24.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 3),
	(28, '2025-08-16 16:00:00.000000', 16.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 3),
	(29, '2025-09-02 16:00:00.000000', 8.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 3),
	(30, '2025-08-21 16:00:00.000000', 24.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 3),
	(31, '2025-08-11 16:00:00.000000', 16.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 3),
	(32, '2025-08-20 20:00:00.000000', 15.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 4),
	(33, '2025-08-18 20:00:00.000000', 66.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 4),
	(34, '2025-08-29 20:00:00.000000', 0.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 2, 4),
	(35, '2025-09-13 20:00:00.000000', 18.70, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 1, 4),
	(36, '2025-09-10 20:00:00.000000', 22.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 4),
	(37, '2025-09-13 20:00:00.000000', 0.00, 'Pending', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 2, 4),
	(38, '2025-08-28 20:00:00.000000', 66.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 4),
	(39, '2025-08-21 20:00:00.000000', 45.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 4),
	(40, '2025-09-03 20:00:00.000000', 0.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 2, 4),
	(41, '2025-08-27 20:00:00.000000', 15.00, 'Pending', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 4),
	(42, '2025-08-19 20:00:00.000000', 45.00, 'Pending', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 4),
	(43, '2025-08-21 20:00:00.000000', 66.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 4),
	(44, '2025-09-18 20:03:21.464503', 30.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 5),
	(45, '2025-09-18 20:03:21.464506', 36.00, 'Pending', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 5),
	(46, '2025-09-18 20:03:21.464506', 60.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 5),
	(47, '2025-09-16 20:03:21.464506', 30.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 5),
	(48, '2025-09-13 20:03:21.464508', 0.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 2, 5),
	(49, '2025-09-12 20:03:21.464509', 60.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 5),
	(50, '2025-09-11 20:03:21.464510', 18.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 6),
	(51, '2025-09-15 20:03:21.464510', 18.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 6),
	(52, '2025-09-14 20:03:21.464510', 36.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 6),
	(53, '2025-09-10 20:03:21.464511', 30.60, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 1, 6),
	(54, '2025-09-15 20:03:21.464511', 18.00, 'Pending', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 6),
	(55, '2025-09-16 20:03:21.464512', 20.40, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 1, 6),
	(56, '2025-09-16 20:03:21.464512', 50.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 6),
	(57, '2025-09-17 20:03:21.464512', 12.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 6),
	(58, '2025-09-12 20:03:21.464512', 12.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 6),
	(59, '2025-09-11 20:03:21.464512', 25.00, 'Pending', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 6),
	(60, '2025-09-16 20:03:21.473761', 36.00, 'Pending', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 7),
	(61, '2025-09-15 20:03:21.473763', 30.00, 'Pending', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 7),
	(62, '2025-09-17 20:03:21.473765', 0.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 2, 7),
	(63, '2025-09-18 20:03:21.473765', 30.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 7),
	(64, '2025-09-16 20:03:21.473765', 0.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 2, 7),
	(65, '2025-09-17 20:03:21.473767', 0.00, 'Pending', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 2, 8),
	(66, '2025-09-16 20:03:21.473768', 5.95, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 1, 8),
	(67, '2025-09-15 20:03:21.473769', 18.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 9),
	(68, '2025-09-18 20:03:21.473769', 0.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', 2, 9),
	(69, '2025-09-18 20:03:21.473773', 24.00, 'Confirmed', 'bfc3c2f0-83f5-4a41-a476-e253868abbf5', NULL, 9);

-- Dumping data for table star_events.categories: ~5 rows (approximately)
INSERT INTO `categories` (`CategoryID`, `Name`, `Description`) VALUES
	(1, 'Live Concert', 'Live musical performances by artists and bands.'),
	(2, 'Theatre & Drama', 'Stage plays, dramas, and theatrical performances.'),
	(3, 'Cultural Festival', 'Events celebrating culture, heritage, and traditions.'),
	(4, 'Stand-up Comedy', 'Live comedy shows featuring stand-up comedians.'),
	(5, 'Workshop & Seminar', 'Educational workshops and professional seminars.');

-- Dumping data for table star_events.events: ~11 rows (approximately)
INSERT INTO `events` (`EventID`, `LocationID`, `CategoryID`, `OrganizerID`, `Title`, `Description`, `StartDateTime`, `EndDateTime`, `Status`, `ImagePaths`) VALUES
	(1, 1, 1, 'f10b2c50-f7c8-4ea3-b58a-46de0f674da5', 'Symphony of Strings - Live in Colombo', 'A magical evening featuring the nation\'s top classical and contemporary artists. Experience a fusion of sounds that will captivate your soul.', '2025-08-20 19:00:00.000000', '2025-08-20 22:00:00.000000', 'Active', '["/uploads/events/eb3a379d-856a-4c99-bf41-8cb7642c70d5_austin-neill-hgO1wFPXl3I-unsplash.jpg"]'),
	(2, 4, 2, 'f10b2c50-f7c8-4ea3-b58a-46de0f674da5', 'Colombo International Theatre Festival', 'Showcasing the best of local and international drama. A week-long festival of captivating performances, workshops, and artist talks.', '2025-09-04 18:00:00.000000', '2025-09-04 21:00:00.000000', 'Active', '["/uploads/events/a1e5a609-4c79-4488-ac45-fb5ad8ce2968_gettyimages-1484041369-612x612.jpg"]'),
	(3, 3, 3, 'f10b2c50-f7c8-4ea3-b58a-46de0f674da5', 'Sri Lankan Cultural Heritage Festival', 'Celebrate the rich cultural heritage of Sri Lanka with traditional dance, music, and art exhibitions.', '2025-09-09 16:00:00.000000', '2025-09-09 20:00:00.000000', 'Active', '["/uploads/events/327e6e10-01bf-450c-b276-a38aa4a7acb1_pavol-svantner-y2QgFpCPxvE-unsplash.jpg"]'),
	(4, 2, 4, 'f10b2c50-f7c8-4ea3-b58a-46de0f674da5', 'Comedy Night with Local Stars', 'An evening of laughter with Sri Lanka\'s top comedians. Prepare for a night of non-stop entertainment.', '2025-09-14 20:00:00.000000', '2025-09-14 23:00:00.000000', 'Active', '["/uploads/events/03a18eb2-eae3-4a35-98e0-5c9b12b2ae55_gettyimages-873359588-612x612.jpg"]'),
	(5, 2, 5, 'f10b2c50-f7c8-4ea3-b58a-46de0f674da5', 'Digital Marketing Workshop', 'Learn the latest digital marketing strategies and tools from industry experts. Perfect for entrepreneurs and marketing professionals.', '2025-09-21 09:00:00.000000', '2025-09-21 17:00:00.000000', 'Active', '["/uploads/events/2abdaa27-5aab-4765-a1d2-0d56014168b9_Hippop.jpg"]'),
	(6, 1, 1, 'f10b2c50-f7c8-4ea3-b58a-46de0f674da5', 'Jazz Under the Stars', 'An intimate jazz performance under the beautiful Colombo sky. Featuring international and local jazz artists.', '2025-09-26 19:00:00.000000', '2025-09-26 22:00:00.000000', 'Active', '["/uploads/events/3f63860a-2bcb-4db9-a524-1d338d6afd05_download.jpg"]'),
	(7, 2, 5, 'f10b2c50-f7c8-4ea3-b58a-46de0f674da5', 'Tech Innovation Summit 2024', 'Join industry leaders and innovators for a comprehensive look at the future of technology in Sri Lanka.', '2025-10-04 08:00:00.000000', '2025-10-04 18:00:00.000000', 'Active', '["/uploads/events/a45a389d-aa54-4290-97a2-05e00a3e2880_images (1).jpg"]'),
	(8, 3, 2, 'f10b2c50-f7c8-4ea3-b58a-46de0f674da5', 'Shakespeare in the Park', 'Experience the magic of Shakespeare\'s timeless plays in an outdoor setting. Perfect for families and literature lovers.', '2025-10-14 18:00:00.000000', '2025-10-14 21:00:00.000000', 'Active', '["/uploads/events/add075a2-5ff4-445d-8912-595a880e8cca_andre-benz-Jb7TLs6fW_I-unsplash.jpg"]'),
	(9, 3, 1, 'f10b2c50-f7c8-4ea3-b58a-46de0f674da5', 'Rock the City Concert', 'The biggest rock concert of the year featuring international and local rock bands. Don\'t miss this epic musical experience.', '2025-10-24 19:00:00.000000', '2025-10-24 23:00:00.000000', 'Active', '["/uploads/events/c5b4fd29-2783-4e73-8464-b8b5ab1cd605_actionvance-eXVd7gDPO9A-unsplash.jpg"]'),
	(10, 4, 4, 'f10b2c50-f7c8-4ea3-b58a-46de0f674da5', 'Stand-up Comedy Championship', 'Witness the funniest comedians compete for the title of Sri Lanka\'s best stand-up comedian.', '2025-11-03 20:00:00.000000', '2025-11-03 23:00:00.000000', 'Active', '["/uploads/events/1b9f998b-11d7-4d0d-b3d3-ea7e90e4eb7d_1192499-2699x1511-desktop-hd-jimmy-carr-wallpaper-photo.jpg"]'),
	(11, 1, 1, 'f10b2c50-f7c8-4ea3-b58a-46de0f674da5', 'Cancelled Event Example', 'This event was cancelled due to unforeseen circumstances.', '2025-10-09 19:00:00.000000', '2025-10-09 22:00:00.000000', 'Cancelled', '["/uploads/events/b58199b3-9759-453c-a231-fb97da07b189_EDM.jpg"]');

-- Dumping data for table star_events.locations: ~4 rows (approximately)
INSERT INTO `locations` (`LocationID`, `Name`, `Address`, `City`, `Capacity`) VALUES
	(1, 'Nelum Pokuna Theatre', '110 Ananda Coomaraswamy Mawatha', 'Colombo', 1288),
	(2, 'BMICH Main Hall', 'Bauddhaloka Mawatha', 'Colombo', 1600),
	(3, 'Galle Face Green', 'Galle Road, Colombo', 'Colombo', 25000),
	(4, 'Lionel Wendt Art Centre', '18 Guildford Crescent', 'Colombo', 622);

-- Dumping data for table star_events.payments: ~60 rows (approximately)
INSERT INTO `payments` (`PaymentID`, `PaymentGatewayTransactionID`, `AmountPaid`, `PaymentMethod`, `PaymentStatus`, `PaymentDateTime`, `BookingID`) VALUES
	(1, 'TXN_47DF8DE1', 12.00, 'PayPal', 'Completed', '2025-08-17 19:21:00.000000', 1),
	(2, 'TXN_F22080D2', 36.00, 'Digital Wallet', 'Completed', '2025-08-10 19:10:00.000000', 2),
	(3, 'TXN_79F697AB', 30.60, 'Credit Card', 'Completed', '2025-08-08 19:13:00.000000', 3),
	(4, 'TXN_96BE110E', 36.00, 'Credit Card', 'Completed', '2025-08-18 19:27:00.000000', 4),
	(5, 'TXN_A547D598', 63.75, 'Debit Card', 'Completed', '2025-08-02 19:11:00.000000', 5),
	(6, 'TXN_56D005C4', 54.00, 'PayPal', 'Completed', '2025-07-28 19:09:00.000000', 6),
	(7, 'TXN_5D24916D', 50.00, 'Bank Transfer', 'Completed', '2025-07-30 19:05:00.000000', 9),
	(8, 'TXN_D64FDE3C', 0.00, 'Digital Wallet', 'Completed', '2025-07-23 19:15:00.000000', 10),
	(9, 'TXN_E4CEE192', 50.00, 'Debit Card', 'Completed', '2025-07-27 19:16:00.000000', 11),
	(10, 'TXN_A834A4B4', 30.00, 'Bank Transfer', 'Completed', '2025-08-17 18:25:00.000000', 12),
	(11, 'TXN_B4BEFF52', 21.00, 'Credit Card', 'Completed', '2025-09-02 18:27:00.000000', 13),
	(12, 'TXN_3A2FA630', 0.00, 'Digital Wallet', 'Completed', '2025-08-30 18:28:00.000000', 14),
	(13, 'TXN_28D142E7', 21.00, 'Credit Card', 'Completed', '2025-08-25 18:26:00.000000', 15),
	(14, 'TXN_8E4702C2', 14.00, 'Bank Transfer', 'Completed', '2025-08-22 18:26:00.000000', 16),
	(15, 'TXN_86383CC7', 0.00, 'PayPal', 'Completed', '2025-08-27 18:26:00.000000', 17),
	(16, 'TXN_406AF3CF', 14.00, 'Bank Transfer', 'Completed', '2025-08-22 18:02:00.000000', 18),
	(17, 'TXN_26C4508F', 0.00, 'Digital Wallet', 'Completed', '2025-09-08 16:06:00.000000', 20),
	(18, 'TXN_E4B63CB0', 6.80, 'Debit Card', 'Completed', '2025-09-05 16:23:00.000000', 21),
	(19, 'TXN_AD64F5E7', 24.00, 'Bank Transfer', 'Pending', '2025-08-11 16:20:00.000000', 22),
	(20, 'TXN_FB986478', 13.60, 'Digital Wallet', 'Completed', '2025-08-28 16:27:00.000000', 23),
	(21, 'TXN_682FFD50', 40.00, 'Digital Wallet', 'Completed', '2025-08-17 16:12:00.000000', 24),
	(22, 'TXN_5630731A', 60.00, 'Credit Card', 'Completed', '2025-09-03 16:05:00.000000', 26),
	(23, 'TXN_EAB2C028', 24.00, 'Debit Card', 'Completed', '2025-08-26 16:02:00.000000', 27),
	(24, 'TXN_52EE2103', 16.00, 'Debit Card', 'Completed', '2025-08-16 16:03:00.000000', 28),
	(25, 'TXN_E37FCC54', 8.00, 'Credit Card', 'Completed', '2025-09-02 16:26:00.000000', 29),
	(26, 'TXN_C6F5CC86', 24.00, 'Digital Wallet', 'Completed', '2025-08-21 16:21:00.000000', 30),
	(27, 'TXN_ECF05CB9', 16.00, 'Digital Wallet', 'Completed', '2025-08-11 16:27:00.000000', 31),
	(28, 'TXN_825EECA8', 66.00, 'Digital Wallet', 'Completed', '2025-08-18 20:05:00.000000', 33),
	(29, 'TXN_0332ABC4', 0.00, 'PayPal', 'Completed', '2025-08-29 20:13:00.000000', 34),
	(30, 'TXN_11E2F8F4', 18.70, 'Digital Wallet', 'Completed', '2025-09-13 20:20:00.000000', 35),
	(31, 'TXN_11CDFA10', 22.00, 'Bank Transfer', 'Completed', '2025-09-10 20:11:00.000000', 36),
	(32, 'TXN_8E92414A', 66.00, 'Credit Card', 'Completed', '2025-08-28 20:27:00.000000', 38),
	(33, 'TXN_572B3510', 45.00, 'Debit Card', 'Completed', '2025-08-21 20:22:00.000000', 39),
	(34, 'TXN_8838CECD', 0.00, 'Digital Wallet', 'Completed', '2025-09-03 20:02:00.000000', 40),
	(35, 'TXN_0F195D42', 15.00, 'Digital Wallet', 'Pending', '2025-08-27 20:09:00.000000', 41),
	(36, 'TXN_C05B4B9F', 45.00, 'Credit Card', 'Pending', '2025-08-19 20:19:00.000000', 42),
	(37, 'TXN_D080E308', 66.00, 'PayPal', 'Completed', '2025-08-21 20:13:00.000000', 43),
	(38, 'TXN_6B65B10C', 30.00, 'PayPal', 'Completed', '2025-09-18 20:31:21.464503', 44),
	(39, 'TXN_88F281F3', 36.00, 'Debit Card', 'Pending', '2025-09-18 20:13:21.464506', 45),
	(40, 'TXN_C1537240', 60.00, 'Credit Card', 'Completed', '2025-09-18 20:21:21.464506', 46),
	(41, 'TXN_22A75019', 30.00, 'Debit Card', 'Completed', '2025-09-16 20:21:21.464506', 47),
	(42, 'TXN_16310405', 0.00, 'Digital Wallet', 'Completed', '2025-09-13 20:32:21.464508', 48),
	(43, 'TXN_51356A80', 60.00, 'Credit Card', 'Completed', '2025-09-12 20:32:21.464509', 49),
	(44, 'TXN_650881B2', 18.00, 'PayPal', 'Completed', '2025-09-15 20:20:21.464510', 51),
	(45, 'TXN_C0DE1343', 36.00, 'Bank Transfer', 'Completed', '2025-09-14 20:31:21.464510', 52),
	(46, 'TXN_6FA2B19A', 30.60, 'Debit Card', 'Completed', '2025-09-10 20:17:21.464511', 53),
	(47, 'TXN_FEC4C501', 18.00, 'Digital Wallet', 'Pending', '2025-09-15 20:04:21.464511', 54),
	(48, 'TXN_98053692', 20.40, 'Credit Card', 'Completed', '2025-09-16 20:14:21.464512', 55),
	(49, 'TXN_6737561E', 50.00, 'Digital Wallet', 'Completed', '2025-09-16 20:12:21.464512', 56),
	(50, 'TXN_428856E8', 12.00, 'Digital Wallet', 'Completed', '2025-09-17 20:27:21.464512', 57),
	(51, 'TXN_3B0E370C', 12.00, 'Credit Card', 'Completed', '2025-09-12 20:19:21.464512', 58),
	(52, 'TXN_124B5783', 36.00, 'Digital Wallet', 'Pending', '2025-09-16 20:08:21.473761', 60),
	(53, 'TXN_5B644809', 30.00, 'Digital Wallet', 'Pending', '2025-09-15 20:20:21.473763', 61),
	(54, 'TXN_47D49D6E', 0.00, 'Credit Card', 'Completed', '2025-09-17 20:27:21.473765', 62),
	(55, 'TXN_18E30062', 30.00, 'Digital Wallet', 'Completed', '2025-09-18 20:09:21.473765', 63),
	(56, 'TXN_AFF20D2C', 0.00, 'Bank Transfer', 'Completed', '2025-09-16 20:32:21.473765', 64),
	(57, 'TXN_F2ABC3D1', 0.00, 'Bank Transfer', 'Completed', '2025-09-17 20:19:21.473767', 65),
	(58, 'TXN_A8F0368E', 18.00, 'PayPal', 'Completed', '2025-09-15 20:19:21.473769', 67),
	(59, 'TXN_A64EFAB6', 0.00, 'Bank Transfer', 'Completed', '2025-09-18 20:16:21.473769', 68),
	(60, 'TXN_6CC4B42E', 24.00, 'Bank Transfer', 'Completed', '2025-09-18 20:13:21.473773', 69);

-- Dumping data for table star_events.promotions: ~2 rows (approximately)
INSERT INTO `promotions` (`PromotionID`, `EventID`, `PromoCode`, `DiscountType`, `DiscountValue`, `StartDate`, `EndDate`, `IsActive`) VALUES
	(1, NULL, 'EARLYBIRD15', 'Percentage', 15.000000000000000000000000000000, '2025-09-19 20:03:21.306161', '2025-10-19 20:03:21.306203', 1),
	(2, NULL, 'SAVE500', 'FixedAmount', 500.000000000000000000000000000000, '2025-09-19 20:03:21.306279', '2025-11-19 20:03:21.306279', 1);

-- Dumping data for table star_events.tickets: ~80 rows (approximately)
INSERT INTO `tickets` (`TicketID`, `QRCodeValue`, `IsScanned`, `ScannedAt`, `BookingID`, `TicketTypeID`) VALUES
	(1, 'QR_F1E9850E82DB', 1, '2025-09-04 18:00:00.000000', 1, 5),
	(2, 'QR_F239F14E4236', 1, '2025-08-20 19:00:00.000000', 2, 1),
	(3, 'QR_8412523BD125', 1, '2025-08-20 19:00:00.000000', 3, 1),
	(4, 'QR_19DB31A1792A', 1, '2025-08-20 17:00:00.000000', 4, 1),
	(5, 'QR_86B03CA40BA8', 1, '2025-08-20 20:00:00.000000', 5, 1),
	(6, 'QR_3271B12DD060', 0, NULL, 5, 1),
	(7, 'QR_CB39EE962083', 1, '2025-08-20 19:00:00.000000', 6, 1),
	(8, 'QR_0234FB6A5E80', 1, '2025-08-20 20:00:00.000000', 6, 1),
	(9, 'QR_FB23FEEC0AD6', 1, '2025-09-04 17:00:00.000000', 7, 5),
	(10, 'QR_8BC57C066817', 0, NULL, 8, 1),
	(11, 'QR_2D6B93ADD1AC', 1, '2025-08-20 19:00:00.000000', 9, 1),
	(12, 'QR_7F75CCD68B2B', 1, '2025-08-20 20:00:00.000000', 9, 1),
	(13, 'QR_C3DA54F55604', 1, '2025-08-20 17:00:00.000000', 10, 3),
	(14, 'QR_5275CC616179', 1, '2025-08-20 20:00:00.000000', 11, 1),
	(15, 'QR_7DB415B3A30A', 1, '2025-08-20 19:00:00.000000', 11, 1),
	(16, 'QR_118622599557', 1, '2025-08-20 18:00:00.000000', 12, 1),
	(17, 'QR_FFCB6C087C64', 0, NULL, 13, 1),
	(18, 'QR_06ACEE6C13CA', 1, '2025-08-20 18:00:00.000000', 14, 3),
	(19, 'QR_BBB4973A2532', 1, '2025-08-20 19:00:00.000000', 15, 1),
	(20, 'QR_5018945AA3E4', 0, NULL, 16, 5),
	(21, 'QR_9C98B3E241E9', 1, '2025-08-20 18:00:00.000000', 17, 3),
	(22, 'QR_24B165835E09', 1, '2025-09-04 19:00:00.000000', 18, 5),
	(23, 'QR_543AD05FB412', 0, NULL, 19, 6),
	(24, 'QR_32066B6B88AA', 1, '2025-08-20 17:00:00.000000', 20, 3),
	(25, 'QR_62CF6DF78687', 1, '2025-09-04 16:00:00.000000', 21, 6),
	(26, 'QR_D001D385E94A', 0, NULL, 22, 1),
	(27, 'QR_3CD847FD82F5', 1, '2025-09-04 17:00:00.000000', 23, 5),
	(28, 'QR_014DFCECC56B', 1, '2025-08-20 20:00:00.000000', 24, 1),
	(29, 'QR_BE629F0CE0EB', 1, '2025-09-04 17:00:00.000000', 25, 5),
	(30, 'QR_32235ABE397F', 0, NULL, 26, 1),
	(31, 'QR_BF8ED46AD843', 1, '2025-08-20 19:00:00.000000', 26, 1),
	(32, 'QR_368DCE6304DB', 1, '2025-08-20 20:00:00.000000', 27, 1),
	(33, 'QR_E8DED47E6070', 1, '2025-09-04 18:00:00.000000', 28, 5),
	(34, 'QR_D106EC954336', 1, '2025-08-20 20:00:00.000000', 29, 3),
	(35, 'QR_5CB772F5876F', 1, '2025-08-20 19:00:00.000000', 30, 1),
	(36, 'QR_41686A30C757', 1, '2025-09-04 18:00:00.000000', 31, 5),
	(37, 'QR_00DAC8762023', 1, '2025-09-04 16:00:00.000000', 32, 5),
	(38, 'QR_6476107F2E2F', 1, '2025-08-20 19:00:00.000000', 33, 1),
	(39, 'QR_AB4DD03E0B71', 1, '2025-08-20 17:00:00.000000', 33, 1),
	(40, 'QR_32E0BA32F35C', 1, '2025-08-20 20:00:00.000000', 34, 3),
	(41, 'QR_6FF3F78E508B', 1, '2025-09-04 16:00:00.000000', 35, 5),
	(42, 'QR_EEB708AFACFF', 1, '2025-08-20 20:00:00.000000', 36, 1),
	(43, 'QR_8E2506826C00', 0, NULL, 37, 3),
	(44, 'QR_EAFD0B826CD5', 1, '2025-08-20 20:00:00.000000', 38, 1),
	(45, 'QR_C12BC41F4BB4', 0, NULL, 38, 1),
	(46, 'QR_0FCF382AEE0F', 1, '2025-08-20 20:00:00.000000', 39, 1),
	(47, 'QR_78A0BF5553DC', 0, NULL, 40, 3),
	(48, 'QR_1B06CD1996F7', 0, NULL, 41, 5),
	(49, 'QR_D57F47AE4463', 0, NULL, 42, 1),
	(50, 'QR_2932E390DB16', 1, '2025-08-20 18:00:00.000000', 43, 1),
	(51, 'QR_7A128AF8A3FA', 1, '2025-08-20 18:00:00.000000', 43, 1),
	(52, 'QR_EF52F0B01221', 1, '2025-08-20 19:00:00.000000', 44, 1),
	(53, 'QR_6DC61DDFC033', 0, NULL, 45, 1),
	(54, 'QR_2BAE8CCFB3CF', 1, '2025-08-20 19:00:00.000000', 46, 1),
	(55, 'QR_C8897E4ECA41', 1, '2025-08-20 20:00:00.000000', 46, 1),
	(56, 'QR_FBA52359E8C5', 1, '2025-08-20 20:00:00.000000', 47, 1),
	(57, 'QR_1403E02D9D32', 0, NULL, 48, 3),
	(58, 'QR_81B9523490BC', 0, NULL, 49, 1),
	(59, 'QR_4AD61D142EF0', 1, '2025-08-20 18:00:00.000000', 49, 1),
	(60, 'QR_AA30D3203A36', 1, '2025-09-04 18:00:00.000000', 50, 5),
	(61, 'QR_A91908FC64F9', 0, NULL, 51, 5),
	(62, 'QR_1BD44A91DE4D', 1, '2025-08-20 18:00:00.000000', 52, 1),
	(63, 'QR_086FDE6BF02D', 1, '2025-08-20 20:00:00.000000', 53, 1),
	(64, 'QR_0597C2FF6B5D', 0, NULL, 54, 5),
	(65, 'QR_CAB6EF5EE86D', 1, '2025-08-20 20:00:00.000000', 55, 1),
	(66, 'QR_1DC8CFAD9EF0', 1, '2025-08-20 19:00:00.000000', 56, 1),
	(67, 'QR_44E27B423703', 1, '2025-08-20 18:00:00.000000', 56, 1),
	(68, 'QR_448EAB6D06B3', 0, NULL, 57, 5),
	(69, 'QR_323066BF83BD', 1, '2025-09-04 18:00:00.000000', 58, 5),
	(70, 'QR_FF3F3C7CD98C', 0, NULL, 59, 1),
	(71, 'QR_937794EF606D', 0, NULL, 60, 1),
	(72, 'QR_A872DBE34623', 0, NULL, 61, 1),
	(73, 'QR_C2E7989A9FF1', 1, '2025-08-20 18:00:00.000000', 62, 3),
	(74, 'QR_A8CA8CBB5F5E', 1, '2025-08-20 20:00:00.000000', 63, 1),
	(75, 'QR_BC5D14DFC76F', 1, '2025-08-20 20:00:00.000000', 64, 3),
	(76, 'QR_00D49D1A3518', 0, NULL, 65, 3),
	(77, 'QR_81751CC5F8D5', 0, NULL, 66, 6),
	(78, 'QR_3643D6CFAEC5', 0, NULL, 67, 5),
	(79, 'QR_E38F5878DF2C', 1, '2025-08-20 17:00:00.000000', 68, 3),
	(80, 'QR_6520F2B86D28', 1, '2025-08-20 18:00:00.000000', 69, 1);

-- Dumping data for table star_events.tickettypes: ~28 rows (approximately)
INSERT INTO `tickettypes` (`TicketTypeID`, `EventID`, `Name`, `Price`, `TotalQuantity`, `AvailableQuantity`) VALUES
	(1, 1, 'VIP Box', 25.000000000000000000000000000000, 50, 50),
	(2, 1, 'Premium Seating', 18.000000000000000000000000000000, 200, 200),
	(3, 1, 'General Admission', 12.000000000000000000000000000000, 500, 500),
	(4, 2, 'Front Row', 15.000000000000000000000000000000, 100, 100),
	(5, 2, 'Standard Seating', 10.000000000000000000000000000000, 300, 300),
	(6, 2, 'Balcony', 7.000000000000000000000000000000, 200, 200),
	(7, 3, 'Premium Pass', 20.000000000000000000000000000000, 100, 100),
	(8, 3, 'Standard Pass', 8.000000000000000000000000000000, 1000, 1000),
	(9, 4, 'VIP Seating', 22.000000000000000000000000000000, 50, 50),
	(10, 4, 'Regular Seating', 15.000000000000000000000000000000, 400, 400),
	(11, 5, 'Full Day Pass', 30.000000000000000000000000000000, 200, 200),
	(12, 5, 'Half Day Pass', 18.000000000000000000000000000000, 300, 300),
	(13, 6, 'VIP Box', 25.000000000000000000000000000000, 50, 50),
	(14, 6, 'Premium Seating', 18.000000000000000000000000000000, 200, 200),
	(15, 6, 'General Admission', 12.000000000000000000000000000000, 500, 500),
	(16, 7, 'Full Day Pass', 30.000000000000000000000000000000, 200, 200),
	(17, 7, 'Half Day Pass', 18.000000000000000000000000000000, 300, 300),
	(18, 8, 'Front Row', 15.000000000000000000000000000000, 100, 100),
	(19, 8, 'Standard Seating', 10.000000000000000000000000000000, 300, 300),
	(20, 8, 'Balcony', 7.000000000000000000000000000000, 200, 200),
	(21, 9, 'VIP Box', 25.000000000000000000000000000000, 50, 50),
	(22, 9, 'Premium Seating', 18.000000000000000000000000000000, 200, 200),
	(23, 9, 'General Admission', 12.000000000000000000000000000000, 500, 500),
	(24, 10, 'VIP Seating', 22.000000000000000000000000000000, 50, 50),
	(25, 10, 'Regular Seating', 15.000000000000000000000000000000, 400, 400),
	(26, 11, 'VIP Box', 25.000000000000000000000000000000, 50, 50),
	(27, 11, 'Premium Seating', 18.000000000000000000000000000000, 200, 200),
	(28, 11, 'General Admission', 12.000000000000000000000000000000, 500, 500);

-- Dumping data for table star_events.__efmigrationshistory: ~5 rows (approximately)
INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
	('20250913151550_update migrations v1', '8.0.19'),
	('20250914034306_add multiple event image paths', '8.0.19'),
	('20250914035854_add multiple event image paths-2', '8.0.19'),
	('20250916140108_update booking and ticket tables', '8.0.19'),
	('20250919143251_add event relation to booking', '8.0.19');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
