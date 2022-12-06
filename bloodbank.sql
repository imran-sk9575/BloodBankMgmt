CREATE DATABASE bloodbank;
USE bloodbank;

CREATE TABLE IF NOT EXISTS centerdetails (
  id int(11) NOT NULL AUTO_INCREMENT,
  Center_Name varchar(100) NOT NULL,
  Address varchar(100) NOT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4;

INSERT INTO centerdetails (id, Center_Name, Address) VALUES
(1, 'AMERICAN LEGION POST 147', 'Clovis Vet Memorial, 808 4th St\r\nClovis, California 93612 United States'),
(2, 'CHURCH OF JESUS CHRIST LATTER DAY SAINTS', 'LDS Church, 1880 Gettysburg Avenue\r\nClovis, California 93611 United States'),
(3, 'FRESYES REALTY', 'Fresyes-Woodward, 30 River Park Place West\r\nFresno, California 93720 United States'),
(4, 'GRACE COMMUNITY CHURCH', 'Grace Comm Church, 17755 Road 26\r\nMadera, California 93638 United States'),
(5, 'AMERICAN LEGION', 'Veterans Hall, 1900 W Olive Ave\r\nPorterville, California 93257 United States'),
(6, 'PORTERVILLE ELKS LODGE', 'Porterville Elks Ldg, 386 North Main Street\r\nPorterville, California 93257 United States'),
(7, 'CALIFORNIA STATE UNIVERSITY FRESNO BLOOD DRIVE', 'Memorial Garden, 5284 N Jackson\r\nFresno, California 93740 United States'),
(8, 'CALIFORNIA STATE UNIVERSITY FRESNO BLOOD DRIVE', 'Univ Dining Hall, 5152 N. Barton Ave.\r\nFresno, California 93740 United States'),
(9, 'FUTURE FORD OF CLOVIS', 'Future Ford, 920 W. Shaw Ave.\r\nClovis, California 93612 United States');


CREATE TABLE IF NOT EXISTS donerdetails (
  id int(11) NOT NULL AUTO_INCREMENT,
  DonerId int(11) NOT NULL,
  CenterId int(11) NOT NULL,
  DonateDate date NOT NULL,
  Unit varchar(100) NOT NULL,
  Reason varchar(250) NOT NULL,
  Supervisor varchar(250) NOT NULL,
  BloodGrop varchar(100) NOT NULL,
  Status varchar(50) DEFAULT NULL,
  PRIMARY KEY (id),
  KEY fk (DonerId),
  KEY fk2 (CenterId)
);


DROP TABLE IF EXISTS donors;
CREATE TABLE IF NOT EXISTS donors (
  id int(11) NOT NULL AUTO_INCREMENT,
  username varchar(100) NOT NULL,
  password varchar(100) NOT NULL,
  email varchar(100) NOT NULL,
  create_at timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  ROLE varchar(50) DEFAULT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;


INSERT INTO donors (id, username, password, email, create_at, ROLE) VALUES
(1, 'Mark', '123456', 'mark.k@gmail.com', '2022-11-26 14:48:12', 'User');
COMMIT;