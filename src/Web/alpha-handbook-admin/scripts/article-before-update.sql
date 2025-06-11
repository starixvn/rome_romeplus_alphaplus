USE alpha_centraldoc;
DROP TRIGGER IF EXISTS article_before_update;

USE alpha_centraldoc;
DELIMITER $$
CREATE TRIGGER article_before_update
BEFORE UPDATE ON articles
FOR EACH ROW
BEGIN
  IF (new.slug IS NULL) THEN
	  SET new.slug = REPLACE(LOWER(new.title), ' ', '-');
	  SET new.slug = REPLACE(LOWER(new.slug), '/', '-');
  END IF;
  IF (new.info_in_english IS NULL) THEN
	  SET new.info_in_english = CONCAT("[", new.locale, "] ", new.name);
  END IF;

  IF (old.published_at IS NOT NULL AND new.published_at IS NULL) THEN
    SET new.published_count = 1;
  END IF;
END$$   
DELIMITER ; 
