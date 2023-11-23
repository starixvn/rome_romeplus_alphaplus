USE alpha_centraldoc;
DROP TRIGGER article_before_insert;

USE alpha_centraldoc;
DELIMITER $$
CREATE TRIGGER article_before_insert
BEFORE INSERT ON articles
FOR EACH ROW
BEGIN
  IF (new.slug is null) THEN
	  SET new.slug = REPLACE(LOWER(new.title), ' ', '-');
  END IF;
END$$   
DELIMITER ; 