USE alpha_centraldoc;
DROP TRIGGER article_before_update;

USE alpha_centraldoc;
DELIMITER $$
CREATE TRIGGER article_before_update
BEFORE UPDATE ON articles
FOR EACH ROW
BEGIN
  IF (new.slug is null) THEN
	  SET new.slug = REPLACE(LOWER(new.title), ' ', '-');
  END IF;
END$$   
DELIMITER ; 