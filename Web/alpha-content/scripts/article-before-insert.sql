USE alpha_centraldoc;
DROP TRIGGER IF EXISTS article_before_insert;

USE alpha_centraldoc;
DELIMITER $$
CREATE TRIGGER article_before_insert
BEFORE INSERT ON articles
FOR EACH ROW
BEGIN
  IF (new.slug is null) THEN
	  SET new.slug = REPLACE(LOWER(new.title), ' ', '-');
	  SET new.slug = REPLACE(LOWER(new.slug), '/', '-');
  END IF;
END$$   
DELIMITER ; 