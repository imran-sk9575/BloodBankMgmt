CREATE DATABASE IF NOT EXISTS bloodbank;
USE bloodbank;

DROP TABLE IF EXISTS donors;
CREATE TABLE IF NOT EXISTS donors (
  id int(11) NOT NULL AUTO_INCREMENT,
  username varchar(100) NOT NULL,
  password varchar(100) NOT NULL,
  email varchar(100) NOT NULL,
  create_at timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (id)
);
COMMIT;