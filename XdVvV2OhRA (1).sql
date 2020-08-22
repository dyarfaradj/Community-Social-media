-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Oct 06, 2019 at 02:49 PM
-- Server version: 8.0.13-4
-- PHP Version: 7.2.19-0ubuntu0.18.04.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `XdVvV2OhRA`
--

-- --------------------------------------------------------

--
-- Table structure for table `billing`
--

CREATE TABLE `billing` (
  `bill_no` int(11) NOT NULL,
  `uid` int(11) NOT NULL,
  `total_amt` int(11) NOT NULL,
  `packed` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `billing`
--

INSERT INTO `billing` (`bill_no`, `uid`, `total_amt`, `packed`) VALUES
(1, 2, 66700, 0),
(2, 2, 66700, 0),
(3, 2, 66700, 0),
(4, 2, 66700, 0),
(5, 2, 66700, 1),
(6, 1, 266800, 0),
(7, 4, 65000, 0),
(8, 1, 0, 0),
(9, 1, 0, 0),
(10, 4, 0, 0),
(11, 4, 100000, 0),
(12, 4, 165000, 0),
(13, 4, 0, 0),
(14, 1, 45300, 0),
(15, 1, 0, 0),
(16, 1, 900, 0),
(17, 12, 165000, 0),
(18, 4, 165000, 0),
(19, 4, 45000, 0),
(20, 12, 1500, 0),
(21, 1, 0, 0),
(22, 2, 2, 1),
(23, 2, 2, 1),
(24, 2, 2, 0);

-- --------------------------------------------------------

--
-- Table structure for table `cart`
--

CREATE TABLE `cart` (
  `cart_id` int(11) NOT NULL,
  `uid` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL,
  `price` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `cart`
--

INSERT INTO `cart` (`cart_id`, `uid`, `product_id`, `quantity`, `price`) VALUES
(1, 1, 1, 1, 65000),
(2, 1, 1, 1, 65000),
(3, 1, 1, 1, 65000),
(4, 1, 1, 1, 65000),
(5, 1, 1, 1, 65000),
(6, 1, 1, 1, 65000),
(7, 1, 1, 1, 65000),
(8, 1, 1, 1, 65000),
(28, 14, 7, 1, 300);

-- --------------------------------------------------------

--
-- Table structure for table `category`
--

CREATE TABLE `category` (
  `cat_id` int(11) NOT NULL,
  `cat_name` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `category`
--

INSERT INTO `category` (`cat_id`, `cat_name`) VALUES
(1, 'electronics'),
(2, 'books'),
(3, 'Mens'),
(4, 'Womens'),
(7, 'testtestsdf'),
(10, 'Cars');

-- --------------------------------------------------------

--
-- Table structure for table `order_inv`
--

CREATE TABLE `order_inv` (
  `serial_id` int(11) NOT NULL,
  `bill_no` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL,
  `total_prod_price` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `order_inv`
--

INSERT INTO `order_inv` (`serial_id`, `bill_no`, `product_id`, `quantity`, `total_prod_price`) VALUES
(1, 5, 1, 1, 65000),
(2, 5, 8, 1, 900),
(3, 5, 6, 1, 800),
(4, 6, 3, 1, 165000),
(5, 6, 1, 1, 65000),
(6, 6, 7, 1, 300),
(7, 6, 5, 1, 1500),
(8, 6, 2, 1, 35000),
(9, 7, 1, 1, 65000),
(10, 11, 2, 1, 35000),
(11, 11, 1, 1, 65000),
(12, 12, 3, 1, 165000),
(13, 14, 4, 1, 45000),
(14, 14, 7, 1, 300),
(15, 16, 8, 1, 900),
(16, 17, 3, 1, 165000),
(17, 18, 3, 1, 165000),
(18, 19, 4, 1, 45000),
(19, 20, 5, 1, 1500),
(20, 22, 2, 4, 140000),
(21, 22, 1, 2, 130000),
(22, 22, 3, 3, 495000),
(23, 23, 3, 1, 165000),
(24, 23, 6, 1, 800);

-- --------------------------------------------------------

--
-- Table structure for table `product`
--

CREATE TABLE `product` (
  `product_id` int(11) NOT NULL,
  `cat_id` int(11) DEFAULT NULL,
  `product_name` varchar(20) DEFAULT NULL,
  `price` int(11) NOT NULL,
  `stock` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `product`
--

INSERT INTO `product` (`product_id`, `cat_id`, `product_name`, `price`, `stock`) VALUES
(1, 1, 'Apple Iphone 6', 65000, 0),
(2, 1, 'Sony S23X', 35000, 34),
(3, 2, 'Alienware X', 165001, 17),
(4, 1, 'Dell', 45000, 28),
(5, 2, 'Corporate Financial ', 1500, 17),
(6, 2, 'The Brain', 800, 19),
(7, 2, 'Things To Do', 300, 19),
(8, 2, 'Five Little Duckling', 900, 20),
(10, 2, 'BMW X5', 978, 56);

-- --------------------------------------------------------

--
-- Table structure for table `user_reg`
--

CREATE TABLE `user_reg` (
  `uid` int(11) NOT NULL,
  `role` varchar(10) NOT NULL DEFAULT 'customer',
  `name` varchar(20) DEFAULT NULL,
  `address` varchar(20) DEFAULT NULL,
  `mobile_no` varchar(20) DEFAULT NULL,
  `email` varchar(20) NOT NULL,
  `user_name` varchar(20) DEFAULT NULL,
  `passwd` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `user_reg`
--

INSERT INTO `user_reg` (`uid`, `role`, `name`, `address`, `mobile_no`, `email`, `user_name`, `passwd`) VALUES
(1, 'customer', 'Arpitha', 'Bangalore', '9876543212', '', 'arpitha', '1234'),
(2, 'admin', 'Django Mysk', 'Dhule', '9916254764', '', 'dj', '123'),
(3, 'customer', 'Mohan', 'Pune', '9403755793', '', 'ayyio', '2134'),
(4, 'customer', 'Ganesh', 'Nasik', '9876543212', '', 'ganesh', '12345'),
(10, 'customer', 'Sarita', 'Mumbai', '1234284432', '', 'Ranu', '1234'),
(11, 'customer', 'Ankur', 'Gaya', '9834568323', '', 'ankur', 'qwer'),
(12, 'customer', 'Asok', 'Than`', '8432941021', '', 'asok', 'patel'),
(13, 'customer', 'Nadhasmdasd', '07213123', '07213123@es.se', '', 'kamal', 'teeest'),
(14, 'customer', 'Charlie Karlsson', 'malmgatan 5', '32423432', 'test@test.se', 'test', 'test');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `billing`
--
ALTER TABLE `billing`
  ADD PRIMARY KEY (`bill_no`),
  ADD KEY `uid` (`uid`);

--
-- Indexes for table `cart`
--
ALTER TABLE `cart`
  ADD PRIMARY KEY (`cart_id`),
  ADD KEY `uid` (`uid`),
  ADD KEY `product_id` (`product_id`);

--
-- Indexes for table `category`
--
ALTER TABLE `category`
  ADD PRIMARY KEY (`cat_id`);

--
-- Indexes for table `order_inv`
--
ALTER TABLE `order_inv`
  ADD PRIMARY KEY (`serial_id`),
  ADD KEY `bill_no` (`bill_no`),
  ADD KEY `product_id` (`product_id`);

--
-- Indexes for table `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`product_id`),
  ADD KEY `cat_id` (`cat_id`);

--
-- Indexes for table `user_reg`
--
ALTER TABLE `user_reg`
  ADD PRIMARY KEY (`uid`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `billing`
--
ALTER TABLE `billing`
  MODIFY `bill_no` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

--
-- AUTO_INCREMENT for table `cart`
--
ALTER TABLE `cart`
  MODIFY `cart_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT for table `category`
--
ALTER TABLE `category`
  MODIFY `cat_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `order_inv`
--
ALTER TABLE `order_inv`
  MODIFY `serial_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

--
-- AUTO_INCREMENT for table `product`
--
ALTER TABLE `product`
  MODIFY `product_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `user_reg`
--
ALTER TABLE `user_reg`
  MODIFY `uid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `billing`
--
ALTER TABLE `billing`
  ADD CONSTRAINT `billing_ibfk_1` FOREIGN KEY (`uid`) REFERENCES `user_reg` (`uid`);

--
-- Constraints for table `cart`
--
ALTER TABLE `cart`
  ADD CONSTRAINT `cart_ibfk_1` FOREIGN KEY (`uid`) REFERENCES `user_reg` (`uid`),
  ADD CONSTRAINT `cart_ibfk_2` FOREIGN KEY (`product_id`) REFERENCES `product` (`product_id`);

--
-- Constraints for table `order_inv`
--
ALTER TABLE `order_inv`
  ADD CONSTRAINT `order_inv_ibfk_1` FOREIGN KEY (`bill_no`) REFERENCES `billing` (`bill_no`),
  ADD CONSTRAINT `order_inv_ibfk_2` FOREIGN KEY (`product_id`) REFERENCES `product` (`product_id`);

--
-- Constraints for table `product`
--
ALTER TABLE `product`
  ADD CONSTRAINT `product_ibfk_1` FOREIGN KEY (`cat_id`) REFERENCES `category` (`cat_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
