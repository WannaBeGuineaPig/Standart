CREATE TABLE `category_item` (
  `id` int NOT NULL AUTO_INCREMENT,
  `category` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
);

CREATE TABLE `item` (
  `articul` varchar(45) NOT NULL,
  `id_type` int NOT NULL,
  `unit_of_measurement` enum('шт.') NOT NULL,
  `price` double NOT NULL,
  `id_supplier` int NOT NULL,
  `id_manafacturer` int NOT NULL,
  `id_category` int NOT NULL,
  `discount_percent` int DEFAULT NULL,
  `amount_in_storage` int NOT NULL,
  `description` longtext NOT NULL,
  `image` longblob,
  PRIMARY KEY (`articul`),
  KEY `id_type_idx` (`id_type`),
  KEY `id_supplier_idx` (`id_supplier`),
  KEY `id_manafacturer_idx` (`id_manafacturer`),
  KEY `id_category_idx` (`id_category`),
  CONSTRAINT `id_category` FOREIGN KEY (`id_category`) REFERENCES `category_item` (`id`),
  CONSTRAINT `id_manafacturer` FOREIGN KEY (`id_manafacturer`) REFERENCES `manafacturer` (`id`),
  CONSTRAINT `id_supplier` FOREIGN KEY (`id_supplier`) REFERENCES `supplier` (`id`),
  CONSTRAINT `id_type` FOREIGN KEY (`id_type`) REFERENCES `type_item` (`id`)
);

CREATE TABLE `manafacturer` (
  `id` int NOT NULL AUTO_INCREMENT,
  `manafacturer` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
);

CREATE TABLE `order` (
  `id` int NOT NULL AUTO_INCREMENT,
  `date_ordering` date NOT NULL,
  `date_delivery` date NOT NULL,
  `id_pickup_point` int NOT NULL,
  `id_user` int NOT NULL,
  `code_pickup` int NOT NULL,
  `status` enum('Новый', 'Завершен') NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_pickup_point_idx` (`id_pickup_point`),
  KEY `id_user_idx` (`id_user`),
  CONSTRAINT `id_pickup_point` FOREIGN KEY (`id_pickup_point`) REFERENCES `pickup_point` (`id`),
  CONSTRAINT `id_user` FOREIGN KEY (`id_user`) REFERENCES `user` (`id`)
);

CREATE TABLE `order_item` (
  `id_order` int NOT NULL AUTO_INCREMENT,
  `articul_item` varchar(100) NOT NULL,
  `amount_item` int NOT NULL,
  PRIMARY KEY (`id_order`,`articul_item`),
  KEY `articul_item_idx` (`articul_item`),
  CONSTRAINT `articul_item` FOREIGN KEY (`articul_item`) REFERENCES `item` (`articul`) ON DELETE CASCADE,
  CONSTRAINT `id_order` FOREIGN KEY (`id_order`) REFERENCES `order` (`id`) ON DELETE CASCADE
);

CREATE TABLE `pickup_point` (
  `id` int NOT NULL AUTO_INCREMENT,
  `address` longtext NOT NULL,
  PRIMARY KEY (`id`)
);

CREATE TABLE `supplier` (
  `id` int NOT NULL AUTO_INCREMENT,
  `supplier` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
);

CREATE TABLE `type_item` (
  `id` int NOT NULL AUTO_INCREMENT,
  `type` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
);

CREATE TABLE `user` (
  `id` int NOT NULL AUTO_INCREMENT,
  `role` enum('Администратор', 'Менеджер', 'Авторизированный клиент') NOT NULL,
  `last_name` varchar(100) NOT NULL,
  `first_name` varchar(100) NOT NULL,
  `midle_name` varchar(100) NOT NULL,
  `mail` varchar(255) NOT NULL,
  `password` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
);
