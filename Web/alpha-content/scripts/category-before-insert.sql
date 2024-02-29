USE alpha_centraldoc;
DROP TRIGGER IF EXISTS category_before_insert;

USE alpha_centraldoc;
DELIMITER $$
CREATE TRIGGER category_before_insert
BEFORE INSERT ON categories
FOR EACH ROW
BEGIN
  IF (new.slug is null) THEN
	  SET new.slug = REPLACE(LOWER(new.name), ' ', '-');
	  SET new.slug = REPLACE(LOWER(new.name), '/', '-');
  END IF;
END$$   
DELIMITER ; 